using System;
using System.Drawing;
using System.Windows.Forms;

namespace Task3
{
    public partial class Form1 : Form
    {
        private Graphics _g;
        private Pen _p;
        private int _n;
        private const string label1Text = "Введите чётное количество вершин полигона";

        public Form1()
        {
            InitializeComponent();

            pictureBox1.BackColor = Color.White;
            label1.Text = label1Text;
            label2.Text = "";
        }

        private void button2_Click(object sender, EventArgs e) =>
            Close();

        private void button1_Click(object sender, EventArgs e)
        {
            ClearForm();

            var nResult = int.TryParse(textBox1.Text, out _n);

            if (!nResult || _n < 2 || _n % 2 == 1)
            {
                MessageBox.Show("Не правильное количество вершин", "Ошибка");

                return;
            }

            _g = Graphics.FromHwnd(pictureBox1.Handle);
            _p = new Pen(Color.Red);
            
            DrawPithagoras(new Point(150, 50), new Point(200, 50), _n);

            label2.Text = "дерево построено";
        }

        void DrawPithagoras(Point a, Point b, int ch)
        {
            if (ch == 0)
                return;

            Point[] quad = new Point[5];

            quad[0] = a;
            quad[1] = b;

            double angle;

            if (ch % 2 == 0)
                angle = 90;
            else
                angle = -90;

            float newX = quad[1].X - quad[0].X;
            float newY = quad[1].Y - quad[0].Y;

            double X2 = newX * Math.Cos(ToRad(angle)) - newY * Math.Sin(ToRad(angle)) + quad[1].X;

            double Y2 = newX * Math.Sin(ToRad(angle)) + newY * Math.Cos(ToRad(angle)) + quad[1].Y;

            double X3 = newX * Math.Cos(ToRad(angle)) - newY * Math.Sin(ToRad(angle)) + quad[0].X;

            double Y3 = newX * Math.Sin(ToRad(angle)) + newY * Math.Cos(ToRad(angle)) + quad[0].Y;

            quad[2] = new Point((int)X2, (int)Y2);
            quad[4] = new Point((int)X3, (int)Y3);

            double XM = Math.Abs(X3 + X2) / 2.0;

            double YM = Math.Abs(Y3 + Y2) / 2.0;

            double newX1 = XM - X3;
            double newY1 = YM - Y3;

            double X4 = newX1 * Math.Cos(ToRad(angle)) - newY1 * Math.Sin(ToRad(angle)) + XM;

            double Y4 = newX1 * Math.Sin(ToRad(angle)) + newY1 * Math.Cos(ToRad(angle)) + YM;

            quad[3] = new Point((int)X4, (int)Y4);

            _g.DrawLine(_p, quad[2], quad[4]);

            _g.DrawPolygon(_p, quad);


            DrawPithagoras(quad[2], quad[3], ch - 1);
            DrawPithagoras(quad[3], quad[4], ch - 1);
        }

        private double ToRad(double degr) =>
            degr * Math.PI / 180;

        private void ClearForm()
        {
            label2.Text = "";
            if (_g is not null)
            {
                _g.Clear(Color.White);
            }
        }
    }
}
