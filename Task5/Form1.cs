using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Task5
{
    public partial class Form1 : Form
    {
        private Graphics _g;
        private Pen _p;
        private Rectangle _rectangle;
        private int _n;
        private const string label1Text = "Введите количество вершин полигона";

        private Color[] _color = { Color.Black, Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Purple };

        int x = 10, y = 50, s = 200;

        public Form1()
        {
            InitializeComponent();

            pictureBox1.BackColor = Color.White;
            label1.Text = label1Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearForm();

            var nResult = int.TryParse(textBox1.Text, out _n);

            if (!nResult || _n < 2)
            {
                MessageBox.Show("Не правильное количество вершин", "Ошибка");

                return;
            }

            _g = Graphics.FromHwnd(pictureBox1.Handle);

            _p = new Pen(Color.Black);

            _rectangle = new Rectangle(x, y, s, s);

            var points = MakeRandomPolygon(_n, _rectangle);

            _g.DrawPolygon(_p, points);


            PointF Centr = new PointF(s / 2 + x, s / 2 + y);


            int i = 0;
            foreach (var item in points)
            {
                if (i == _color.Length - 1)
                {
                    i = 0;
                }

                _p = new Pen(_color[i]);
                _g.DrawLine(_p, item, Centr);
                i++;
            }   

        }

        private void button2_Click(object sender, EventArgs e) =>
            Close();

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
    }
}
