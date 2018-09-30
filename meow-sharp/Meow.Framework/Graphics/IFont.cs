namespace Meow.Framework.Graphics
{
    public interface IFont
    {
        int StringHeight(string text, int maxWidth = 0);
        void Draw(SpriteBatch spriteBatch, PrimitiveBatch primitiveBatch, string text, Point at, float scale, Color color);
        int StringLength(string text);
    }
}
