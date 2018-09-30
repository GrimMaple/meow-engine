using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Meow.Core.Util.Resource
{
    /// <summary>
    /// Low-level abstraction of an image
    /// </summary>
    public class Image : UnmanagedResource
    {
        [DllImport("meow")]
        private static extern IntPtr image_create(uint width, uint height);

        [DllImport("meow")]
        private static extern IntPtr image_create_type(uint width, uint height, uint type);

        [DllImport("meow")]
        private static extern void image_free(IntPtr image);

        [DllImport("meow")]
        private static extern uint image_get_pixel(IntPtr image, int x, int y);

		[DllImport("meow")]
		private static extern void image_set_pixel(IntPtr Image, int x, int y, uint color);

		[DllImport("meow")]
		private static extern IntPtr image_create_from_data
			(
			[MarshalAs(UnmanagedType.LPArray)]
			byte[] data,
			int width,
			int height,
			int type
			);

        [DllImport("meow")]
        private static extern uint image_get_width(IntPtr src);

        [DllImport("meow")]
        private static extern uint image_get_height(IntPtr src);

		[DllImport("meow")]
		private static extern void image_set_subimage(IntPtr dest, IntPtr source, int x, int y);

		[DllImport("meow")]
		private static extern IntPtr image_subimage(IntPtr source, int x, int y, int w, int h);

        public virtual uint Width
        {
            get
            {
                return image_get_width(Resource);
            }
        }

        public virtual uint Height
        {
            get
            {
                return image_get_height(Resource);
            }
        }

        protected uint GetPixel(int x, int y)
        {
            return image_get_pixel(Resource, x, y);
        }

		protected void SetPixel(int x, int y, uint color)
		{
			image_set_pixel(Resource, x, y, color);
		}

        protected Image(uint width, uint height, uint type = 4) : base()
        {
            Resource = image_create_type(width, height, type);
        }

		protected Image(byte[] data, int width, int height, int type)
		{
			Resource = image_create_from_data(data, width, height, type);
		}

		protected Image(Image source, int x, int y, int w, int h)
		{
			Resource = image_subimage(source.Resource, x, y, w, h);
		}

		protected void SetSubimage(Image source, int x, int y)
		{
			image_set_subimage(Resource, source.Resource, x, y);
		}

		protected Image(IntPtr handle) : base(handle)
		{
			
		}

        protected override void Dispose(bool disposing)
        {
            if(!Disposed)
            {
                image_free(Resource);
                Disposed = true;
            }
            base.Dispose(disposing);
        }
    }
}
