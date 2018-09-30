namespace Meow.Framework.Graphics
{
    /// <summary>
    /// Represents a 2-dimensional point
    /// </summary>
    public struct Point
    {
        /// <summary>
        /// Origin point (0; 0)
        /// </summary>
        public static Point Zero = new Point(0, 0);

        /// <summary>
        /// X coordinate
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Y coordinate
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Z coordinate
        /// </summary>
        public int Z { get; set; }

        /// <summary>
        /// Initializes a point with given coordinates
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public Point(int x, int y) : this()
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Initializes a point with given coordinates
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="z">Z coordinate</param>
        public Point(int x, int y, int z) : this(x, y)
        {
            Z = z;
        }
    }
}
