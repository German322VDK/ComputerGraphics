using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task6
{
    public partial class Form1 : Form
    {
        // фигура
        private List<Point3D> figure_3D = new List<Point3D>();
        // pen для проекции figure_3D
        Pen pen_figure_3D = new Pen(Color.Red);

        Pen[] pen_figure_Arr = 
            { 
                new Pen(Color.Red), new Pen(Color.Red), new Pen(Color.Red), new Pen(Color.Red),
                new Pen(Color.Red), new Pen(Color.Blue), new Pen(Color.Red), new Pen(Color.Red),
                new Pen(Color.Red), new Pen(Color.Red), new Pen(Color.Red), new Pen(Color.Red),
                new Pen(Color.Red), new Pen(Color.Red), new Pen(Color.Red), new Pen(Color.Red),
                 new Pen(Color.Red)
            };
        // угол
        double angel_OXY;

        // угол
        double angel_res_OXY;

        // точка 0
        Point Point_0 = new Point(0, 0);

        public Form1()
        {
            InitializeComponent();

            // Двойная буф-я
            typeof(Control).GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance 
                | BindingFlags.SetProperty).SetValue(pictureBox1, true, null);

            // зададим точку отсчета по середине
            Point_0.X = pictureBox1.Width / 2;
            Point_0.Y = pictureBox1.Height / 2;

            // установим углы
            angel_OXY = 1.0;
            angel_res_OXY = 1.0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cube();
            Draw(figure_3D);
        }

        private void button2_Click(object sender, EventArgs e) =>
            Close();

        private void Cube()
        {
            // очистим если есть
            if (figure_3D != null)
                figure_3D.Clear();

            int start = -150;

            int pick = start + 300;

            // заполним
            figure_3D.Add(new Point3D(start, start, start));
            figure_3D.Add(new Point3D(pick, start, start));
            figure_3D.Add(new Point3D(pick, start, pick));
            figure_3D.Add(new Point3D(start, start, pick));
            figure_3D.Add(new Point3D(start, pick, pick));
            figure_3D.Add(new Point3D(start, pick, start));
            figure_3D.Add(new Point3D(start, start, pick));
            figure_3D.Add(new Point3D(start, start, start));
            figure_3D.Add(new Point3D(start, pick, start));
            figure_3D.Add(new Point3D(pick, pick, start));
            figure_3D.Add(new Point3D(pick, pick, pick));
            figure_3D.Add(new Point3D(pick, start, pick));
            figure_3D.Add(new Point3D(pick, start, start));
            figure_3D.Add(new Point3D(pick, pick, start));
            figure_3D.Add(new Point3D(start, pick, start));
            figure_3D.Add(new Point3D(start, pick, pick));
            figure_3D.Add(new Point3D(pick, pick, pick));
            figure_3D.Add(new Point3D(pick, pick, pick));
        }

        void Draw(List<Point3D> val)
        {
            // проверка наличия фигуры 3d
            if (figure_3D.Count <= 0)
                return;

            // создадим bitmap и Graphics
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics grf = Graphics.FromImage(bmp);


            // пербираем
            for (int i = 0; i < val.Count - 1; i++)
            {
                if (i == 5)
	            {
                    continue;
	            }   
                grf.DrawLine(pen_figure_Arr[i], Convert_3D_in_2D_Point(val[i]), Convert_3D_in_2D_Point(val[i + 1]));
                
            }

            // выводим 
            pictureBox1.Image = bmp;
            pictureBox1.Refresh();
            GC.Collect();
            //grf.Dispose();
            //;bmp.Dispose();

        }

        private Point Convert_3D_in_2D_Point(Point3D val)
        {
            // проицируем
            double res_x = -val._z * Math.Sin(angel_OXY) + val._x * Math.Cos(angel_OXY) + Point_0.X;
            double res_y = -(val._z * Math.Cos(angel_OXY) + val._x * Math.Sin(angel_OXY)) * Math.Sin(angel_res_OXY)
                + val._y * Math.Cos(angel_res_OXY) + Point_0.Y;

            return new Point((int)(res_x), (int)(res_y));
        }
    }
}
