namespace Meow.Framework.Graphics
{
    /// <summary>
    /// Factory capable of bulding sprites
    /// </summary>
    public sealed class SpriteBatch
    {
		RenderTarget renderTarget;

        internal SpriteBatch(RenderTarget target)
        {
            renderTarget = target;
        }

        /// <summary>
        /// Draw a sprite
        /// </summary>
        /// <param name="s">Sprite to draw</param>
        /// <param name="x">X coordinate to draw at</param>
        /// <param name="y">Y coordinate to draw at</param>
        /// <param name="scale">Scale factor</param>
        /// <param name="color">Color to multiply with</param>
        public void DrawSprite(Sprite s, int x, int y, float scale, Color color)
        {
            renderTarget.SetTexture(s.Texture);
            renderTarget.AddPoint(x, y, 0, s.TextureCoords[0], s.TextureCoords[1], color.R, color.G, color.B, color.A);
            renderTarget.AddPoint(x, y + s.Height * scale, 0, s.TextureCoords[2], s.TextureCoords[3], color.R, color.G, color.B, color.A);
            renderTarget.AddPoint(x + s.Width * scale, y + s.Height * scale, 0, s.TextureCoords[4], s.TextureCoords[5], color.R, color.G, color.B, color.A);
            renderTarget.AddPoint(x + s.Width * scale, y + s.Height * scale, 0, s.TextureCoords[4], s.TextureCoords[5], color.R, color.G, color.B, color.A);
            renderTarget.AddPoint(x + s.Width * scale, y, 0, s.TextureCoords[6], s.TextureCoords[7], color.R, color.G, color.B, color.A);
            renderTarget.AddPoint(x, y, 0, s.TextureCoords[0], s.TextureCoords[1], color.R, color.G, color.B, color.A);
        }


        /// <summary>
        /// Draw a sprite
        /// </summary>
        /// <param name="s">Sprite to draw</param>
        /// <param name="x">X coordinate to draw at</param>
        /// <param name="y">Y coordinate to draw at</param>
        /// <param name="color">Color to multiply with</param>
        public void DrawSprite(Sprite s, int x, int y, Color color)
        {
            DrawSprite(s, x, y, 1.0f, color);
        }

        /// <summary>
        /// Draw a sprite
        /// </summary>
        /// <param name="s">Sprite to draw</param>
        /// <param name="x">X coordinate to draw at</param>
        /// <param name="y">Y coordinate to draw at</param>
        /// <param name="scale">Scale factor</param>
        public void DrawSprite(Sprite s, int x, int y, float scale=1.0f)
        {
            DrawSprite(s, x, y, scale, Color.White);
        }

        /// <summary>
        /// Draw a sprite
        /// </summary>
        /// <param name="s">Sprite to draw</param>
        /// <param name="at">Position of the sprite</param>
        /// <param name="scale">Scale factor</param>
        public void DrawSprite(Sprite s, Point at, float scale = 1.0f)
        {
            DrawSprite(s, at.X, at.Y, scale, Color.White);
        }

        /// <summary>
        /// Draw a sprite
        /// </summary>
        /// <param name="s">Sprite to draw</param>
        /// <param name="at">Position of the sprite</param>
        /// <param name="color">Color multiplier</param>
        /// <param name="scale">Scale factor</param>
        public void DrawSprite(Sprite s, Point at, Color color, float scale = 1.0f)
        {
            DrawSprite(s, at.X, at.Y, scale, color);
        }
    }
}
