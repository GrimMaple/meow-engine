using System;

namespace Meow.Framework.Util
{
    using Graphics;

    /// <summary>
    /// Visual debugging console
    /// </summary>
    public sealed class DebugConsole
    {
        /// <summary>
        /// Font to be used when rendering console text
        /// </summary>
        public SpriteFont Font
        {
            get; set;
        }

        /// <summary>
        /// Color of info text
        /// </summary>
        public Color InfoColor
        {
            get; set;
        } = Color.White;

        /// <summary>
        /// Color of error text
        /// </summary>
        public Color ErrorColor
        {
            get; set;
        } = new Color(255, 0, 0);

        /// <summary>
        /// Color of warning text
        /// </summary>
        public Color WarningColor
        {
            get; set;
        } = new Color(255, 255, 0);

        /// <summary>
        /// Creates a new debug console
        /// </summary>
        public DebugConsole()
        {

        }

        /// <summary>
        /// Draws the console window
        /// </summary>
        /// <param name="spriteBatch">Sprite batch</param>
        /// <param name="primitiveBatch">Primitives batch</param>
        /// <param name="w">Console window width</param>
        /// <param name="h">Console window height</param>
        public void Draw(SpriteBatch spriteBatch, PrimitiveBatch primitiveBatch, int w, int h)
        {
            if (Font == null)
                throw new NullReferenceException("Font was null");

            primitiveBatch.DrawRectangle(0, 0, w, h, new Color(0, 0, 0, 200));
            int lines = h / Font.YSize;
            int totalLines = DebugLog.SharedInstance.MessagesCount;
            lines = lines > totalLines ? totalLines : lines;
            for(int i=0; i<lines; i++)
            {
                Color color = Color.White;
                switch(DebugLog.SharedInstance[totalLines - lines + i].Type)
                {
                    case DebugMessageType.Info:
                        color = InfoColor;
                        break;
                    case DebugMessageType.Warning:
                        color = WarningColor;
                        break;
                    case DebugMessageType.Error:
                        color = ErrorColor;
                        break;
                }
                Font.Draw(spriteBatch, DebugLog.SharedInstance[totalLines - lines + i].Message, 0, i*Font.YSize, color);
            }
        }
    }
}
