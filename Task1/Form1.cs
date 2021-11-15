using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Task1
{
    public partial class Form1 : Form
    {
        private const string label1Text = "Выбирете угол поворота";
        private const string label2Text = "Выбирете XY точки поворота";

        private Graphics g;
        private Pen p;

        private int _x1, _y1, _x2, _y2;

        private double _angle;

        private double _lineLenght;

        private string[] _lines;

        public Form1()
        {
            InitializeComponent();

            label1.Text = label1Text;
            label2.Text = label2Text;

        }

        private string[] WriteComboBoxItems()
        {
            var lines = new List<string>();

            var xCount = _x2 - _x1;

            var yCount = _y2 - _y1;

            var count = xCount > yCount ? xCount : yCount;

            int x = _x1, y = _y1;

            for (int i = 0; i < count; i++)
            {
                lines.Add($"{x} {y}");

                if (x <= _x2)
                {
                    x++;
                }

                if (y <= _y2)
                {
                    y++;
                }
            }

            return _lines = lines.ToArray();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearForm();

            g = Graphics.FromHwnd(pictureBox1.Handle);

            g.Clear(Color.White);

            p = new Pen(Color.Red);

            _x1 = _x2 = pictureBox1.Width / 2;

            _y1 = pictureBox1.Height / 2 - 25;

            _y2 = _y1 + 75;

            g.DrawLine(p, _x1, _y1, _x2, _y2);

            comboBox1.Items.AddRange(WriteComboBoxItems());

            _lineLenght = Math.Sqrt( (_x2 - _x1) * (_x2 - _x1) + (_y2 - _y1) * (_y2 - _y1));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
        }

        private void button2_Click(object sender, EventArgs e) =>
            Close();

        private void ClearForm()
        {
            comboBox1.Items.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (g is null )
            {
                MessageBox.Show("Не создана линия", "Ошибка");

                return;
            }

            var angleResult = double.TryParse(textBox1.Text, out _angle);

            if (!angleResult)
            {
                MessageBox.Show("Угол не корректен", "Ошибка");

                return;
            }

            int selectIndex = comboBox1.SelectedIndex;

            if (selectIndex == -1)
            {
                MessageBox.Show("Не выбрана точка", "Ошибка");

                return;
            }

            var str = _lines[selectIndex];

            var xy = str.Split(' ');

            int x = int.Parse(xy[0]);

            int y = int.Parse(xy[1]);

            int newX1 = _x2 - x;
            int newY1 = _y2 - y;

            int newX2 = x - _x1;
            int newY2 = y - _y1;


            float X1 = (float)(newX1 * Math.Cos(ToRad(_angle)) - newY1 * Math.Sin(ToRad(_angle))) + x;

            float Y1 = (float)(newX1 * Math.Sin(ToRad(_angle)) + newY1 * Math.Cos(ToRad(_angle))) + y;

            float X2 = (float)(newX2 * Math.Cos(ToRad(_angle + 180)) - newY2 * Math.Sin(ToRad(_angle + 180))) + x;

            float Y2 = (float)(newX2 * Math.Sin(ToRad(_angle + 180)) + newY2 * Math.Cos(ToRad(_angle + 180))) + y;

            g.DrawLine(p, x, y, X1, Y1);

            g.DrawLine(p, x, y, X2, Y2);
        }

        private double ToRad(double degr) =>
            degr * Math.PI / 180;
    }
}
