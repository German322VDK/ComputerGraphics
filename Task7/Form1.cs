using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;

namespace Task7
{
    public partial class Form1 : Form
    {
        #region переменные

        // угол
        double angel_OXY;

        // угол
        double angel_res_OXY;

        // точка 0
        Point Point_0 = new Point(0, 0);

        // фигура
        List<Point3D> figure_3D = new List<Point3D>();

        // pen для проекции figure_3D
        Pen pen_figure_3D = new Pen(Color.Red);

        // для временного хранения при поворотах
        int tmp_XX;
        int tmp_YY;

        #endregion

        public Form1()
        {
            InitializeComponent();

            // Двойная буф-я
            typeof(Control).GetProperty("DoubleBuffered", BindingFlags.NonPublic 
                | BindingFlags.Instance | BindingFlags.SetProperty).SetValue(pictureBox1, true, null);

            // зададим точку отсчета по середине
            Point_0.X = pictureBox1.Width / 2;
            Point_0.Y = pictureBox1.Height / 2;

            // установим углы
            angel_OXY = 1.0;
            angel_res_OXY = 1.0;

        }

        // проекция 2D на 2D
        private Point convert_3D_in_2D_Point(Point3D val)
        {
            // проицируем
            double res_x = -val._z * Math.Sin(angel_OXY) + val._x * Math.Cos(angel_OXY) + Point_0.X;
            double res_y = -(val._z * Math.Cos(angel_OXY) + val._x * Math.Sin(angel_OXY)) * Math.Sin(angel_res_OXY) + val._y * Math.Cos(angel_res_OXY) + Point_0.Y;

            return new Point((int)(res_x), (int)(res_y));
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
                grf.DrawLine(pen_figure_3D, convert_3D_in_2D_Point(val[i]), convert_3D_in_2D_Point(val[i + 1]));
            }

            
            var pt = new Point[val.Count];

            for (int i = 0; i < val.Count; i++)
            {
                pt[i] = convert_3D_in_2D_Point(val[i]);
            }

            Fill(grf, pt, 1);
            Fill(grf, pt, 2);
            Fill(grf, pt, 3);

            // выводим 
            pictureBox1.Image = bmp;
            pictureBox1.Refresh();
            GC.Collect();

        }

        void Fill(Graphics grf, Point[] pt, int sw)
        {
            var gp = new GraphicsPath();
            SolidBrush br;
            Point[] ptt = new Point[3];

            switch (sw)
            {
                case 1:
                    br = new SolidBrush(Color.FromArgb(255, 46, 46));
                    ptt[0] = pt[0];
                    ptt[1] = pt[1];
                    ptt[2] = pt[2];
                    break;
                case 2:
                    br = new SolidBrush(Color.FromArgb(255, 127, 127));
                    ptt[0] = pt[0];
                    ptt[1] = pt[1];
                    ptt[2] = pt[3];
                    break;
                case 3:
                    br = new SolidBrush(Color.FromArgb(255, 204, 204));
                    ptt[0] = pt[0];
                    ptt[1] = pt[2];
                    ptt[2] = pt[3];
                    break;
                default:
                    br = new SolidBrush(Color.FromArgb(255, 204, 204));
                    break;
            }

            gp.AddPolygon(ptt);

            grf.FillPath(br, gp);
        }

        private void Prism()
        {
            // очистим если есть
            if (figure_3D != null)
                figure_3D.Clear();

            // заполним
            figure_3D.Add(new Point3D(50, 0, 0));
            figure_3D.Add(new Point3D(300, 0, 0));
            figure_3D.Add(new Point3D(0, 300, 0));
            figure_3D.Add(new Point3D(0, 0, 300));
            figure_3D.Add(new Point3D(300, 0, 0));
            figure_3D.Add(new Point3D(50, 0, 0));
            figure_3D.Add(new Point3D(0, 0, 300));
            figure_3D.Add(new Point3D(0, 300, 0));
            figure_3D.Add(new Point3D(50, 0, 0));

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Prism();

            Draw(figure_3D);

            //Fill(figure_3D);
        }

        private void button2_Click(object sender, EventArgs e) =>
            Close();

        
    }
}
