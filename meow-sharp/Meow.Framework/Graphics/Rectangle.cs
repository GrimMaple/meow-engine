namespace Meow.Framework.Graphics
{
    /// <summary>
    /// Represents a rectangle
    /// </summary>
    public sealed class Rectangle
    {
        private int[] data;

        /// <summary>
        /// Top-left point of this rectangle
        /// </summary>
        public Point TopLeft
        {
            get
            {
                return new Point(data[0], data[1], data[2]);
            }
            private set
            {
                data[0] = value.X;
                data[1] = value.Y;
                data[2] = value.Z;
            }
        }

        /// <summary>
        /// Bottom-left point of this rectangle
        /// </summary>
        public Point BottomLeft
        {
            get
            {
                return new Point(data[3], data[4], data[5]);
            }
            private set
            {
                data[3] = value.X;
                data[4] = value.Y;
                data[5] = value.Z;
            }
        }

        /// <summary>
        /// Bottom-right point of this rectangle
        /// </summary>
        public Point BottomRight
        {
            get
            {
                return new Point(data[6], data[7], data[8]);
            }
            private set
            {
                data[6] = value.X;
                data[7] = value.Y;
                data[8] = value.Z;
            }
        }

        /// <summary>
        /// Top-right point of this rectangle
        /// </summary>
        public Point TopRight
        {
            get
            {
                return new Point(data[9], data[10], data[11]);
            }
            private set
            {
                data[9] = value.X;
                data[10] = value.Y;
                data[11] = value.Z;
            }
        }

        /// <summary>
        /// Rectangle width
        /// </summary>
        public int Width
        {
            get
            {
                return TopRight.X - TopLeft.X;
            }
        }

        /// <summary>
        /// Rectangle height
        /// </summary>
        public int Height
        {
            get
            {
                return BottomLeft.Y - TopLeft.Y;
            }
        }

        /// <summary>
        /// Rectangle data represented as an array of integers
        /// </summary>
        public int[] Array
        {
            get
            {
                return data;
            }
        }

        /// <summary>
        /// Create a new rectangle
        /// </summary>
        public Rectangle()
        {
            data = new int[3 * 4];
        }

        /// <summary>
        /// Create a new rectangle with given positions
        /// </summary>
        /// <param name="topLeft">Top-left corner</param>
        /// <param name="bottomRight">Bottom-right corner</param>
        public Rectangle(Point topLeft, Point bottomRight) : this()
        {
            TopLeft = topLeft;
            BottomLeft = new Point(topLeft.X, bottomRight.Y, topLeft.Z);
            BottomRight = bottomRight;
            TopRight = new Point(bottomRight.X, topLeft.Y, bottomRight.Z);
        }

        /// <summary>
        /// Create a new rectangle with given positions
        /// </summary>
        /// <param name="topLeft">Top-left corner</param>
        /// <param name="width">Rectangle width</param>
        /// <param name="height">Rectangle height</param>
        public Rectangle(Point topLeft, int width, int height) : this()
        {
            TopLeft = topLeft;
            int x = topLeft.X, y = topLeft.Y, w = width, h = height;
            BottomLeft = new Point(x, y + h, topLeft.Z);
            BottomRight = new Point(x + w, y + h, topLeft.Z);
            TopRight = new Point(x + w, y, topLeft.Z);
        }

        /// <summary>
        /// Check if this rectangle contains a <see cref="Point"/>
        /// </summary>
        /// <param name="point">Point to check</param>
        /// <returns>True if point is inside this rectangle</returns>
        public bool Contains(Point point)
        {
            int x = point.X, y = point.Y;
            int left = TopLeft.X;
            int right = TopRight.X;
            int bottom = BottomLeft.Y;
            int top = TopLeft.Y;
            if (x <= right && x >= left && y <= bottom && y >= top)
                return true;
            return false;
        }
    }
}
