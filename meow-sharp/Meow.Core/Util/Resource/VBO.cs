using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Meow.Core.Util.Resource
{
    public class VBO : UnmanagedResource
    {
        [DllImport("meow")]
        private static extern IntPtr renderer_create_buffer_object(float[] vertex, float[] texture, float[] color, uint count);

        [DllImport("meow")]
        private static extern IntPtr renderer_cleanup_buffer_object(IntPtr obj);

        internal uint Count
        {
            get; private set;
        }

        protected VBO(float[] vertex, float[] texture, float[] color, uint count)
        {
            Resource = renderer_create_buffer_object(vertex, texture, color, count);
            Count = count;
        }

        protected override void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                renderer_cleanup_buffer_object(Resource);
                Disposed = true;
            }
            base.Dispose(disposing);
        }
    }
}
