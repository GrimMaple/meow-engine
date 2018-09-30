using System;
using Meow.Framework.Graphics.Util;

namespace Meow.Framework.Graphics
{
    public delegate void DrawEvt(SpriteBatch spriteBatch, PrimitiveBatch primitiveBatch);
    /// <summary>
    /// Factory that uses a static memory buffer to store objects
    /// </summary>
    public sealed class StaticObject : IDisposable
    {
        private BufferArray[] arrays;
        private IntPtr draw = IntPtr.Zero;

        /// <summary>
        /// Draw this static object to the Renderer
        /// </summary>
        public void Draw()
        {
            foreach(BufferArray obj in arrays)
            {
                obj.Draw();
            }
        }

        /// <summary>
        /// Create a new static object
        /// </summary>
        /// <param name="creation">Delegate creation function</param>
        /// <returns></returns>
        public static StaticObject Create(DrawEvt creation)
        {
            StaticObject obj = new StaticObject();
            BufferBuilder builder = new BufferBuilder();
            RenderTarget target = new RenderTarget(builder);
            SpriteBatch sBatch = new SpriteBatch(target);
            PrimitiveBatch pBatch = new PrimitiveBatch(target);
            creation(sBatch, pBatch);
            target.Flush();
            obj.arrays = builder.Arrays;
            return obj;
        }

        public void Free()
        {
            
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            foreach (BufferArray obj in arrays)
                obj.Dispose();
        }

        ~StaticObject()
        {
            Dispose(false);
        }

        private StaticObject()
        {
            
        }
    }
}
