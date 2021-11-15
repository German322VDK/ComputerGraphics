using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Task9
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

            trackBar1.Minimum = 0;
            trackBar2.Minimum = 0;
            trackBar3.Minimum = 0;

            trackBar1.Maximum = 800;
            trackBar2.Maximum = 800;
            trackBar3.Maximum = 100;

            trackBar1.Value = 100;
            trackBar2.Value = 100;
            trackBar3.Value = 50;

        }

        #region расчеты и отрисовка

        // проекция 2D на 2D
        private Point convert_3D_in_2D_Point(Point3D val)
        {
            // проицируем
            double res_x = -val._z * Math.Sin(angel_OXY) + val._x * Math.Cos(angel_OXY) + Point_0.X;
            double res_y = -(val._z * Math.Cos(angel_OXY) + val._x * Math.Sin(angel_OXY)) * Math.Sin(angel_res_OXY) + val._y * Math.Cos(angel_res_OXY) + Point_0.Y;

            return new Point((int)(res_x), (int)(res_y));
        }

        # endregion

        #region Фигуры

        // рисуем
        void Draw(List<Point3D> val)
        {
            // проверка наличия фигуры 3d
            if (figure_3D.Count <= 0)
                return;

            // создадим bitmap и Graphics
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics grf = Graphics.FromImage(bmp);
            var pt = new Point[val.Count];

            // пербираем
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

        void FillCircle(Graphics grf)
        {
            SolidBrush br = new SolidBrush(Color.FromArgb(255, 255, 204));

            grf.FillEllipse(br, 100, 10, 50, 50);
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

            centr = pt.FirstOrDefault(el => (el.X < xMax && el.X > xMin) || (el.Y < yMax && el.Y > yMin));

            Point[] ptt1 = { centr, upleft, upright };
            Point[] ptt2 = { centr, down, upleft };
            Point[] ptt3 = { centr, down, upright };

            grf.FillPolygon(br1, ptt1);
            grf.FillPolygon(br2, ptt2);
            grf.FillPolygon(br3, ptt3);

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

        #endregion

        #region События

        private void button1_Click(object sender, EventArgs e)
        {
            Prism();
            Draw(figure_3D);
        }

        private void button2_Click(object sender, EventArgs e) =>
            Close();

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                # region горизонтальный поворт
                // вправо
                if (e.X > tmp_XX)
                {
                    if (trackBar1.Value <= trackBar1.Maximum && trackBar1.Value > 0)
                    {
                        trackBar1.Value -= 1;
                        trackBar1_Scroll(this, EventArgs.Empty);
                    }
                    else
                    {
                        if (trackBar1.Value == 0)
                            trackBar1.Value = trackBar1.Maximum;
                    }
                }

                // влево
                if (e.X < tmp_XX)
                {
                    if (trackBar1.Value < trackBar1.Maximum && trackBar1.Value >= 0)
                    {
                        trackBar1.Value += 1;
                        trackBar1_Scroll(this, EventArgs.Empty);

                        // переход через MAX
                        if (trackBar1.Value >= trackBar1.Maximum)
                            trackBar1.Value = 0;
                    }
                }

                # endregion

                # region вертикальный поворт

                // вниз
                if (e.Y > tmp_YY)
                {
                    if (trackBar2.Value < trackBar2.Maximum && trackBar2.Value >= 0)
                    {
                        trackBar2.Value += 1;
                        trackBar_res_OXY_Scroll(this, EventArgs.Empty);
                    }
                    else
                    {
                        // переход через MAX
                        if (trackBar2.Value >= trackBar2.Maximum)
                            trackBar2.Value = 0;
                    }
                }

                // вверх
                if (e.Y < tmp_YY)
                {
                    if (trackBar2.Value <= trackBar2.Maximum && trackBar2.Value > 0)
                    {
                        trackBar2.Value -= 1;
                        trackBar_res_OXY_Scroll(this, EventArgs.Empty);
                    }
                    else
                    {
                        // переход через 0
                        if (trackBar2.Value <= 0)
                            trackBar2.Value = trackBar2.Maximum;
                    }
                }

                # endregion

                // временно для хранения
                tmp_XX = e.X;
                tmp_YY = e.Y;

                return;
            }

            // пермещаем точку отсчета 0
            if (e.Button == MouseButtons.Left)
            {
                Point_0.X = e.X;
                Point_0.Y = e.Y;
                Draw(figure_3D);
            }
        }

        // поворот
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            angel_OXY = (double)(trackBar1.Value) / 100;
            Draw(figure_3D);
        }

        // поворот
        private void trackBar_res_OXY_Scroll(object sender, EventArgs e)
        {
            angel_res_OXY = (double)(trackBar2.Value) / 100;
            Draw(figure_3D);
        }

        // изменение размеров формы
        private void Form_main_Resize(object sender, EventArgs e)
        {
            Draw(figure_3D);
        }

        #endregion

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            int minX = figure_3D.Min(el => el._x);
            int maxX = figure_3D.Max(el => el._x);

            int minY = figure_3D.Min(el => el._y);
            int maxY = figure_3D.Max(el => el._y);

            int minZ = figure_3D.Min(el => el._z);
            int maxZ = figure_3D.Max(el => el._z);

            int val = trackBar3.Value - 50, valx, valy, valz;

            for (int i = 0; i < figure_3D.Count; i++)
            {
                valx = val;
                valy = val;
                valz = val;

                if (figure_3D[i]._x == minX)
                {
                    valx = val * -1;
                }
                if (figure_3D[i]._y == minX)
                {
                    valy = val * -1;
                }
                if (figure_3D[i]._z == minX)
                {
                    valz = val * -1;
                }

                var p = new Point3D(figure_3D[i]._x + valx, figure_3D[i]._y + valy, figure_3D[i]._z + valz);

                figure_3D[i] = p;
            }

            Draw(figure_3D);
        }
    }
}
