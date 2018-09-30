namespace Meow.Framework.Graphics
{
    /// <summary>
    /// A factory capable of creating primitives
    /// </summary>
    public sealed class PrimitiveBatch
    {
		RenderTarget renderTarget;

		internal PrimitiveBatch(RenderTarget target)
        {
            renderTarget = target;
        }

        /// <summary>
        /// Draws a white point at given position 
        /// </summary>
        /// <param name="pt">Position to draw at</param>
        public void DrawPoint(Point pt)
        {
            DrawTriangle(pt, new Point(pt.X, pt.Y + 1), new Point(pt.X + 1, pt.Y + 1));
            DrawTriangle(new Point(pt.X + 1, pt.Y + 1), new Point(pt.X + 1, pt.Y), pt);
        }

        /// <summary>
        /// Draws a point at given position
        /// </summary>
        /// <param name="pt">Position to draw at</param>
        /// <param name="color">Color to draw with</param>
        public void DrawPoint(Point pt, Color color)
        {
            DrawTriangle(pt, new Point(pt.X, pt.Y + 1), new Point(pt.X + 1, pt.Y + 1), color);
            DrawTriangle(new Point(pt.X + 1, pt.Y + 1), new Point(pt.X + 1, pt.Y), pt, color);
        }

        /// <summary>
        /// Draws a white line between two points
        /// </summary>
        /// <param name="pt1">Start point</param>
        /// <param name="pt2">End point</param>
        public void DrawLine(Point pt1, Point pt2)
        {
            DrawLine(pt1, pt2, Color.White);
        }

        /// <summary>
        /// Draws a line between two points
        /// </summary>
        /// <param name="pt1">Start point</param>
        /// <param name="pt2">End point</param>
        /// <param name="color">Color to draw with</param>
        public void DrawLine(Point pt1, Point pt2, Color color)
        {
            Point pt3 = new Point(pt2.X + 1, pt2.Y+1);
            Point pt4 = new Point(pt1.X + 1, pt1.Y+1);
            DrawTriangle(pt1, pt2, pt3, color);
            DrawTriangle(pt3, pt4, pt1, color);
        }

        /// <summary>
        /// Draws a white triangle
        /// </summary>
        /// <param name="pt1">First point</param>
        /// <param name="pt2">Second point</param>
        /// <param name="pt3">Third point</param>
        public void DrawTriangle(Point pt1, Point pt2, Point pt3)
        {
            DrawTriangle(pt1, pt2, pt3, Color.White);
        }

        /// <summary>
        /// Draws a rectangle
        /// </summary>
        /// <param name="x">Top-left point x coordinate</param>
        /// <param name="y">Top-left point y coordinate</param>
        /// <param name="w">Rectangle width</param>
        /// <param name="h">Rectangle height</param>
        /// <param name="c">Color to draw with</param>
        public void DrawRectangle(int x, int y, int w, int h, Color c)
        {
            DrawRectangle(new Point(x, y), new Point(x + w, y + h), c);
        }

        /// <summary>
        /// Draws a rectangle
        /// </summary>
        /// <param name="r">Rectangle to draw</param>
        /// <param name="c">Color to draw with</param>
        public void DrawRectangle(Rectangle r, Color c)
        {
            DrawRectangle(r.TopLeft, r.BottomRight, c);
        }

        /// <summary>
        /// Draws a rectangle
        /// </summary>
        /// <param name="tl">Top-left point</param>
        /// <param name="br">Bottom-right point</param>
        /// <param name="color">Color to draw with</param>
        public void DrawRectangle(Point tl, Point br, Color color)
        {
            DrawTriangle(tl, new Point(tl.X, br.Y), br, color);
            DrawTriangle(tl, br, new Point(br.X, tl.Y), color);
        }

        /// <summary>
        /// Draws a triangle
        /// </summary>
        /// <param name="pt1">First point</param>
        /// <param name="pt2">Second point</param>
        /// <param name="pt3">Third point</param>
        /// <param name="c">Color to draw with</param>
        public void DrawTriangle(Point pt1, Point pt2, Point pt3, Color c)
        {
            renderTarget.SetTexture(null);
            renderTarget.AddPoint(pt1.X, pt1.Y, pt1.Z, 0, 1, c.R, c.G, c.B, c.A);
            renderTarget.AddPoint(pt2.X, pt2.Y, pt3.Z, 0, 0, c.R, c.G, c.B, c.A);
            renderTarget.AddPoint(pt3.X, pt3.Y, pt3.Z, 1, 0, c.R, c.G, c.B, c.A);
        }
    }
}
