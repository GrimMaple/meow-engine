namespace Meow.Framework.Graphics
{
    public sealed class Sprite
    {
        private float[] textureCoords;

        /// <summary>
        /// OpenGL texture identifier
        /// </summary>
        public Texture Texture
        {
            get;
            private set;
        }

        /// <summary>
        /// Sprite width
        /// </summary>
        public int Width
        {
            get;
            private set;
        }

        /// <summary>
        /// Sprite height
        /// </summary>
        public int Height
        {
            get;
            private set;
        }

        internal float[] TextureCoords
        {
            get
            {
                return textureCoords;
            }
        }

        internal Sprite(int width, int height, Texture tex, float[] texcoords)
        {
            Width = width;
            Height = height;
            Texture = tex;
            textureCoords = texcoords;
        }
    }
}
