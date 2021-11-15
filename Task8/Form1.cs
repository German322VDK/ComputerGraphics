using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;

namespace Task8
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


            var pt = new Point[val.Count];

            for (int i = 0; i < val.Count; i++)
            {
                pt[i] = convert_3D_in_2D_Point(val[i]);
            }

            FillCircle(grf);

            Fill(grf, pt);

            // выводим 
            pictureBox1.Image = bmp;
            pictureBox1.Refresh();
            GC.Collect();

        }

        void Fill(Graphics grf, Point[] pt)
        {
            var gp = new GraphicsPath();
            
            SolidBrush br1 = new SolidBrush(Color.FromArgb(255, 204, 204));
            SolidBrush br2 = new SolidBrush(Color.FromArgb(175, 127, 127));
            SolidBrush br3 = new SolidBrush(Color.FromArgb(128, 46, 46));

            Point upleft, down, upright, centr;

            int xMin, xMax = xMin = pt[0].X;

            int yMin, yMax = yMin = pt[0].Y;

            for (int i = 1; i < pt.Length; i++)
            {
                if (xMin > pt[i].X)
                {
                    xMin = pt[i].X;
                }
            }

            for (int i = 1; i < pt.Length; i++)
            {
                if (xMax < pt[i].X)
                {
                    xMax = pt[i].X;
                }
            }

            for (int i = 1; i < pt.Length; i++)
            {
                if (yMin > pt[i].Y)
                {
                    yMin = pt[i].Y;
                }
            }

            for (int i = 1; i < pt.Length; i++)
            {
                if (yMax < pt[i].Y)
                {
                    yMax = pt[i].Y;
                }
            }



            upleft = pt.FirstOrDefault(el => el.X == xMin);

            down = pt.FirstOrDefault(el => el.Y == yMax);

            upright = pt.FirstOrDefault(el => el.X == xMax);

            centr = pt.FirstOrDefault(el => (el.X < xMax && el.X > xMin) || (el.Y < yMax && el.Y > yMin) );

            Point[] ptt1 = { centr, upleft, upright };
            Point[] ptt2 = { centr, down, upleft };
            Point[] ptt3 = { centr, down, upright };

            grf.FillPolygon(br1, ptt1);
            grf.FillPolygon(br2, ptt2);
            grf.FillPolygon(br3, ptt3);

        }

        void FillCircle(Graphics grf)
        {
            SolidBrush br = new SolidBrush(Color.FromArgb(255, 255, 204));

            grf.FillEllipse(br, 100, 10, 50, 50);
        }

        private void Prism()
        {
            // очистим если есть
            if (figure_3D != null)
                figure_3D.Clear();

            // заполним
            figure_3D.Add(new Point3D(0, 0, 0));
            figure_3D.Add(new Point3D(300, 0, 0));
            figure_3D.Add(new Point3D(0, 300, 0));
            figure_3D.Add(new Point3D(0, 0, 300));

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Prism();

            Draw(figure_3D);

        }

        private void button2_Click(object sender, EventArgs e) =>
            Close();
    }
}
