using System;
using System.Drawing;
using System.Windows.Forms;

namespace Task2
{
    public partial class Form1 : Form
    {
        Random _rand;
        int _size = 0;
        const int _length = 12;

        int[] _degree;
        int[] _xA, _xB, _xC, _xD, _yA, _yB, _yC, _yD;
        double[] _xDB, _xBA, _xAC, _xCD, _yDB, _yBA, _yAC, _yCD;

        double _p, _q;
        int _xStart, _yStart, _xEnd, _yEnd;
        int _xTop, _yTop, _xBottom, _yBottom;

        public Form1()
        {
            _rand = new Random();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var grpBox = pictureBox1.CreateGraphics();

            var boxPen = new Pen(Color.Navy, 3);

            grpBox.Clear(Color.White);

            _size = _rand.Next(20, 50);

            label1.Text = $"{_size}";

            InitializeRectangles(_size);

            RandDegrees(_size);

            ComputeNormals();

            Draw(boxPen);
        }

        private void button2_Click(object sender, EventArgs e) =>
            Close();

        private void button3_Click(object sender, EventArgs e)
        {
            var grpBox = pictureBox1.CreateGraphics();
            var boxPen = new Pen(Color.DeepSkyBlue, 2);
            grpBox.Clear(Color.White);
            Cut(boxPen);
        }

        void DrawTwoLines(Pen boxPen, Point A, Point B, Point C, Point D)
        {
            Graphics grpBox = pictureBox1.CreateGraphics();
            grpBox.DrawLine(boxPen, A, B);
            grpBox.DrawLine(boxPen, C, D);
        }

        private void RandDegrees(int size)
        {
            for (int i = 0; i < size - 1; i++)
            {
                _degree[i] = _rand.Next(0, 89);
            }
        }

        private void Draw(Pen boxPen)
        {
            int height = pictureBox1.Height / 2;
            int width = pictureBox1.Width / 2;
            for (int i = 1; i < _size; i++)
            {
                _xA[i - 1] = (int)(width + i * _length * Math.Cos((-_degree[i - 1] + 45) * Math.PI / 180));
                _yA[i - 1] = (int)(height + i * _length * Math.Sin((-_degree[i - 1] + 45) * Math.PI / 180));
                Point A = new Point(_xA[i - 1], _yA[i - 1]);

                _xB[i - 1] = (int)(width + i * _length * Math.Cos((-_degree[i - 1] + 135) * Math.PI / 180));
                _yB[i - 1] = (int)(height + i * _length * Math.Sin((-_degree[i - 1] + 135) * Math.PI / 180));
                Point B = new Point(_xB[i - 1], _yB[i - 1]);

                _xC[i - 1] = (int)(width + i * _length * Math.Cos((-_degree[i - 1] - 45) * Math.PI / 180));
                _yC[i - 1] = (int)(height + i * _length * Math.Sin((-_degree[i - 1] - 45) * Math.PI / 180));
                Point C = new Point(_xC[i - 1], _yC[i - 1]);

                _xD[i - 1] = (int)(width + i * _length * Math.Cos((-_degree[i - 1] - 135) * Math.PI / 180));
                _yD[i - 1] = (int)(height + i * _length * Math.Sin((-_degree[i - 1] - 135) * Math.PI / 180));
                Point D = new Point(_xD[i - 1], _yD[i - 1]);
                DrawLines(boxPen, A, C, D, B);
            }
        }
        

        private void DrawLines(Pen boxPen, Point a, Point c, Point d, Point b)
        {
            Graphics grpBox = pictureBox1.CreateGraphics();
            grpBox.DrawLine(boxPen, a, c);
            grpBox.DrawLine(boxPen, c, d);
            grpBox.DrawLine(boxPen, d, b);
            grpBox.DrawLine(boxPen, b, a);
        }

        private void ComputeNormals()
        {
            for (int i = 0; i < _size - 1; i++)
            {
                _xDB[i] = Math.Round(Math.Cos(Math.Abs(_degree[i]) * Math.PI / 180), 2);
                _xAC[i] = -Math.Round(Math.Cos(Math.Abs(_degree[i]) * Math.PI / 180), 2);
                _xCD[i] = Math.Round(Math.Sin(Math.Abs(_degree[i]) * Math.PI / 180), 2);
                _xBA[i] = -Math.Round(Math.Sin(Math.Abs(_degree[i]) * Math.PI / 180), 2);
                _yAC[i] = Math.Round(Math.Sin(Math.Abs(_degree[i]) * Math.PI / 180), 2);
                _yDB[i] = -Math.Round(Math.Sin(Math.Abs(_degree[i]) * Math.PI / 180), 2);
                _yCD[i] = Math.Round(Math.Cos(Math.Abs(_degree[i]) * Math.PI / 180), 2);
                _yBA[i] = -Math.Round(Math.Cos(Math.Abs(_degree[i]) * Math.PI / 180), 2);
            }
        }

        private void InitializeRectangles(int size)
        {
            _degree = new int[size];
            _xA = new int[size];
            _xB = new int[size];
            _xC = new int[size];
            _xD = new int[size];
            _yA = new int[size];
            _yB = new int[size];
            _yC = new int[size];
            _yD = new int[size];
            _xDB = new double[size];
            _xBA = new double[size];
            _xAC = new double[size];
            _xCD = new double[size];
            _yDB = new double[size];
            _yBA = new double[size];
            _yAC = new double[size];
            _yCD = new double[size];
        }
        private void Cut(Pen boxPen)
        {
            double Out, In, T;

            for (int j = 0; j < _size - 1; j++)  
            {
                if (j == 0)
                {
                    Point A = new Point(_xA[j], _yA[j]);
                    Point B = new Point(_xB[j], _yB[j]);
                    Point C = new Point(_xC[j], _yC[j]);
                    Point D = new Point(_xD[j], _yD[j]);
                    DrawLines(boxPen, A, C, D, B);

                    continue;
                }

                for (int i = 0; i < 4; i++) 
                {
                    switch (i)
                    {
                        case 0:
                            { 
                                _xEnd = _xA[j]; 
                                _yEnd = _yA[j];
                                _xStart = _xB[j]; 
                                _yStart = _yB[j]; 
                            }
                            break;
                        case 1:
                            { 
                                _xEnd = _xC[j]; 
                                _yEnd = _yC[j]; 
                                _xStart = _xA[j]; 
                                _yStart = _yA[j]; 
                            }
                            break;
                        case 2:
                            { 
                                _xEnd = _xD[j]; 
                                _yEnd = _yD[j]; 
                                _xStart = _xC[j]; 
                                _yStart = _yC[j]; 
                            }
                            break;
                        case 3:
                            { 
                                _xEnd = _xB[j]; 
                                _yEnd = _yB[j]; 
                                _xStart = _xD[j];
                                _yStart = _yD[j]; 
                            }
                            break;
                    }
                    Out = 0; In = 0;

                    if (Math.Abs(_degree[j] - _degree[j - 1]) > 3)
                    {
                        int Fx = _xA[j - 1];
                        int Fy = _yA[j - 1];
                        int Dx = _xEnd - _xStart;
                        int Dy = _yEnd - _yStart;
                        double nx = _xBA[j - 1];
                        double ny = _yBA[j - 1];
                        T = ComputeT(Fx, Fy, Dx, Dy, nx, ny);
                        if (_p > 0 && _p != 0) 
                        { 
                            if (In < T && T <= 1 && T >= 0) 
                                In = T; 
                        }
                        else if (_p < 0 && _p != 0) 
                        { 
                            if (Out < T && T <= 1 && T >= 0) 
                                Out = T; 
                        }

                        Fx = _xC[j - 1];
                        Fy = _yC[j - 1];
                        nx = _xAC[j - 1];
                        ny = _yAC[j - 1];
                        T = ComputeT(Fx, Fy, Dx, Dy, nx, ny);
                        if (_p > 0 && _p != 0) 
                        { 
                            if (In < T && T <= 1 && T >= 0) 
                                In = T; 
                        }
                        else if (_p < 0 && _p != 0) 
                        { 
                            if (Out < T && T <= 1 && T >= 0) 
                                Out = T; 
                        }

                        Fx = _xD[j - 1];
                        Fy = _yD[j - 1];
                        nx = _xCD[j - 1];
                        ny = _yCD[j - 1];
                        T = ComputeT(Fx, Fy, Dx, Dy, nx, ny);
                        if (_p > 0 && _p != 0) 
                        { 
                            if (In < T && T <= 1 && T >= 0) 
                                In = T; 
                        }
                        else if (_p < 0 && _p != 0) 
                        { 
                            if (Out < T && T <= 1 && T >= 0) 
                                Out = T; 
                        }

                        Fx = _xB[j - 1];
                        Fy = _yB[j - 1];
                        nx = _xDB[j - 1];
                        ny = _yDB[j - 1];
                        T = ComputeT(Fx, Fy, Dx, Dy, nx, ny);
                        if (_p > 0 && _p != 0) 
                        { 
                            if (In < T && T <= 1 && T >= 0) 
                                In = T; 
                        }
                        else if (_p < 0 && _p != 0) 
                        { 
                            if (Out < T && T <= 1 && T >= 0) 
                                Out = T; 
                        }

                        _xTop = ComputeV(_xEnd, _xStart, Out);
                        _yTop = ComputeV(_yEnd, _yStart, Out);
                        _xBottom = ComputeV(_xEnd, _xStart, In);
                        _yBottom = ComputeV(_yEnd, _yStart, In);
                    }

                    if (In == 0 || Out == 0 || ComputeDl(_xB[j], _xBottom, _yBottom, _yB[j]) < (_length + _length / 3))
                    {
                        Point A = new Point(_xA[j], _yA[j]);
                        Point B = new Point(_xB[j], _yB[j]);
                        Point C = new Point(_xC[j], _yC[j]);
                        Point D = new Point(_xD[j], _yD[j]);
                        DrawLines(boxPen, A, C, D, B);
                    }
                    else switch (i)
                        {
                            case 0:
                                {
                                    Point B = new Point(_xB[j], _yB[j]);
                                    Point N = new Point(_xBottom, _yBottom);
                                    Point V = new Point(_xTop, _yTop);
                                    Point A = new Point(_xA[j], _yA[j]);
                                    DrawTwoLines(boxPen, B, N, V, A);
                                }
                                break;
                            case 1:
                                {
                                    Point C = new Point(_xC[j], _yC[j]);
                                    Point N = new Point(_xBottom, _yBottom);
                                    Point V = new Point(_xTop, _yTop);
                                    Point A = new Point(_xA[j], _yA[j]);
                                    DrawTwoLines(boxPen, C, V, N, A);
                                }
                                break;
                            case 2:
                                {
                                    Point D = new Point(_xD[j], _yD[j]);
                                    Point N = new Point(_xBottom, _yBottom);
                                    Point V = new Point(_xTop, _yTop);
                                    Point C = new Point(_xC[j], _yC[j]);
                                    DrawTwoLines(boxPen, C, N, V, D);
                                }
                                break;
                            case 3:
                                {
                                    Point D = new Point(_xD[j], _yD[j]);
                                    Point N = new Point(_xBottom, _yBottom);
                                    Point V = new Point(_xTop, _yTop);
                                    Point B = new Point(_xB[j], _yB[j]);
                                    DrawTwoLines(boxPen, B, V, N, D);
                                }
                                break;
                        }
                }
            }
        }
        
        double ComputeT(int Fx, int Fy, int Dx, int Dy, double nx, double ny)
        {
            int Wx = _xStart - Fx;
            int Wy = _yStart - Fy;
            _p = (Dx * nx + Dy * ny);
            _q = (Wx * nx + Wy * ny);
            return -(_q) / (_p);
        }

        int ComputeV(int end, int start, double shag) =>
            (int)(start + (end - (double)start) * ((double)shag));

        int ComputeDl(int x2, int x1, int y2, int y1) =>
            (int)Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));

    }
}
