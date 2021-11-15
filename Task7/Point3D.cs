namespace Task7
{
    /// <summary>
    /// Класс описывающий точку в 3D пространстве
    /// </summary>
    public class Point3D
    {
        /// <summary>
        /// координата
        /// </summary>
        public int _x { set; get; }
        /// <summary>
        /// координата
        /// </summary>
        public int _y { set; get; }
        /// <summary>
        /// координата
        /// </summary>
        public int _z { set; get; }
        /// <summary>
        /// конструктор 1
        /// </summary>
        public Point3D()
        {
            _x = 0;
            _y = 0;
            _z = 0;
        }

        /// <summary>
        /// конструктор 2
        /// </summary>
        public Point3D(int x, int y, int z)
        {
            _x = x;
            _y = y;
            _z = z;
        }
    }
}
