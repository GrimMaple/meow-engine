namespace Meow.Framework.Graphics
{
    /// <summary>
    /// High-level representation of device texture.
    /// Creates, supports lifecycle and destroys textures properly
    /// </summary>
    public class Texture : Core.Util.Resource.Texture
    {
        /// <summary>
        /// Texture width
        /// </summary>
        public int Width
        {
            get
            {
                return (int)width;
            }
        }

        /// <summary>
        /// Texture height
        /// </summary>
        public int Height
        {
            get
            {
                return (int)height;
            }
        }

        /// <summary>
        /// Create new texture from an <see cref="Image"/> 
        /// </summary>
        /// <param name="image">Image to create from</param>
        public Texture(Image image) : base(image)
        {

        }

        /// <summary>
        /// Create a <see cref="Sprite"/> from this texture
        /// </summary>
        /// <returns>New sprite</returns>
        public Sprite ToSprite()
        {
            return new Sprite(Width, Height, this, new float[] { 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f });
        }

        /// <summary>
        /// Create a <see cref="Sprite"/> from this image 
        /// </summary>
        /// <param name="rect">Sprite region</param>
        /// <returns>New sprite</returns>
        public Sprite ToSprite(Rectangle rect)
        {
            return ToSprite(rect.TopLeft, rect.Width, rect.Height);
        }

        /// <summary>
        /// Create a <see cref="Sprite"/> from this image 
        /// </summary>
        /// <param name="at">Sprite region top-left point</param>
        /// <param name="w">Sprite width</param>
        /// <param name="h">Sprite height</param>
        /// <returns></returns>
        public Sprite ToSprite(Point at, int w, int h)
        {
            float fx = (float)at.X / width;
            float fy = (float)at.Y / height;
            float dx = (float)w / width;
            float dy = (float)h / height;
            return new Sprite(w, h, this, new float[] { fx, fy, fx, fy + dy, fx + dx, fy + dy, fx + dx, fy });
        }
    }
}
