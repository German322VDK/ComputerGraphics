using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task5
{
    public partial class Form1 : Form
    {
        private Graphics _g;
        private Pen[] _p;
        private Rectangle _rectangle;
        private const int _n = 5;

        private Color[] _colors = 
            { 
            Color.Black, Color.Red, Color.Green, Color.Blue, Color.DarkKhaki, Color.Orange,
            Color.Purple, Color.DarkBlue, Color.Chocolate, Color.DarkRed, Color.DarkGreen
            };

        public Form1()
        {
            InitializeComponent();

            pictureBox1.BackColor = Color.White;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearForm();

            _g = Graphics.FromHwnd(pictureBox1.Handle);

            _p = new Pen[_n];

            for (int i = 0; i < _p.Length; i++)
            {
                _p[i] = new Pen(_colors[i]);
            }

            _rectangle = new Rectangle(50, 10, 200, 200);

            var points = MakeRandomPolygon(_n, _rectangle);

            for (int i = 0; i < points.Length; i++)
            {
                if (i == points.Length - 1)
                {
                    _g.DrawLine(_p[i], points[i], points[0]);
                }
                else
                {
                    _g.DrawLine(_p[i], points[i], points[i + 1]);
                }



            }

            for (int i = 1; i < points.Length; i++)
            {
                _g.DrawLine(_p[i - 1], points[0], points[i]);

                Thread.Sleep(1000);
            }
        }

        public PointF[] MakeRandomPolygon(int num_vertices,
            Rectangle bounds)
        {
            // Выбор случайных радиусов.
            double[] radii = new double[num_vertices];
            const double min_radius = 0.5;
            const double max_radius = 1.0;

            for (int i = 0; i < num_vertices; i++)
            {
                radii[i] = NextDouble(min_radius, max_radius);
            }

            // Выбор случайных угловых весов.
            double[] angle_weights = new double[num_vertices];
            const double min_weight = 1.0;
            const double max_weight = 10.0;
            double total_weight = 0;

            for (int i = 0; i < num_vertices; i++)
            {
                angle_weights[i] = NextDouble(min_weight, max_weight);
                total_weight += angle_weights[i];
            }

            // Преобразование весов во фракции 2 * Pi радианов.
            double[] angles = new double[num_vertices];
            double to_radians = 2 * Math.PI / total_weight;

            for (int i = 0; i < num_vertices; i++)
            {
                angles[i] = angle_weights[i] * to_radians;
            }

            // Вычислить местоположения точек.
            PointF[] points = new PointF[num_vertices];
            float rx = bounds.Width / 2f;
            float ry = bounds.Height / 2f;
            float cx = bounds.MidX();
            float cy = bounds.MidY();
            double theta = 0;

            for (int i = 0; i < num_vertices; i++)
            {
                points[i] = new PointF(
                    cx + (int)(rx * radii[i] * Math.Cos(theta)),
                    cy + (int)(ry * radii[i] * Math.Sin(theta)));
                theta += angles[i];
            }

            // Вернем точки.
            return points;
        }

        public double NextDouble(double a, double b) // считаем случайное дробное число в промежутке от до
        {
            Random random = new Random();

            double x = random.NextDouble();

            return x * a + (1 - x) * b;
        }

        private void ClearForm()
        {
            if (_g is not null)
            {
                _g.Clear(Color.White);
            }
        }

        private void button2_Click(object sender, EventArgs e) =>
            Close();
    }
}
