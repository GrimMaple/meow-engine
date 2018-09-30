using System;
using System.Linq;

namespace Meow.Framework.Graphics
{
    public sealed class SpriteFont : IFont, IDisposable
    {
        Texture font;
        Sprite[] letters;
        private int dx;
        private int dy;

        public int XSize
        {
            get
            {
                return dx;
            }
        }

        public int YSize
        {
            get
            {
                return dy;
            }
        }

        public int StringHeight(string text, int maxWidth = 0)
        {
            return YSize;
        }

        private void BuildFont()
        {
            letters = new Sprite[256];
            float height = font.Width;
            float width = font.Height;
            float ddx = width / (16);
            float ddy = height / (16);

            for(int i=0; i<16; i++)
                for(int j=0; j<16; j++)
                {
                    Sprite tmp = font.ToSprite(new Point((int)(j * ddx), (int)(i * ddx)), (int)ddx, (int)ddy);
                    letters[i * 16 + j] = tmp;
                }
        }

        public SpriteFont(Texture tex, int xsize, int ysize)
        {
            font = tex;
            dx = xsize;
            dy = ysize;

            BuildFont();
        }

        public void Draw(SpriteBatch batch, string text, int x, int y, float scale=1.0f)
        {
            Draw(batch, text, x, y, scale, Color.White);
        }

        public void Draw(SpriteBatch batch, string text, int x, int y, Color color)
        {
            Draw(batch, text, x, y, 1.0f, color);
        }

        public void Draw(SpriteBatch batch, string text, Point location, float scale, Color color)
        {
            Draw(batch, text, location.X, location.Y, scale, color);
        }

        public void Draw(SpriteBatch batch, string text, Point location, Color color)
        {
            Draw(batch, text, location.X, location.Y, 1.0f, color);
        }

        public void Draw(SpriteBatch batch, string text, int x, int y, float scale, Color color)
        {
            Draw(batch, null, text, new Point(x, y), scale, color);
        }

        public void Draw(SpriteBatch spriteBatch, PrimitiveBatch primitiveBatch, string text, Point at, float scale, Color color)
        {
            int dx = (int)(this.dx * scale);
            int x = at.X;
            int y = at.Y;
            foreach (char c in text.ToArray())
            {
                if (c == ' ')
                {
                    x += dx;
                    continue;
                }
                int index = c - 32;
                if (index < 0)
                    continue;
                if (index > 256)
                    continue;
                spriteBatch.DrawSprite(letters[index], x, y, scale, color);

                x += dx;
            }
        }

        public int StringLength(string text)
        {
            return text.Length * XSize;
        }

        public void Dispose()
        {
            font.Dispose();
        }
    }
}
