using System;
using System.Runtime.InteropServices;

namespace Meow.Core.Util.Resource
{
    public abstract class Texture : UnmanagedResource
    {
        [DllImport("meow.dll")]
        private static extern uint renderer_load_texture(IntPtr img);

        [DllImport("meow")]
        private static extern void renderer_free_texture(uint id);

        protected uint width, height;

        public uint TextureID
        {
            get
            {
                return (uint)Resource;
            }
        }

        protected Texture(Image image)
        {
            Resource = (IntPtr)renderer_load_texture(image.Resource);
            width = image.Width;
            height = image.Height;
        }

        protected override void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                renderer_free_texture((uint)Resource);
                Disposed = true;
            }
            base.Dispose(disposing);
        }
    }
}
