using System;

namespace Meow.Framework.Graphics
{
    /// <summary>
    /// High level abstraction of an image
    /// </summary>
    public class Image : Core.Util.Resource.Image
    {
        /// <summary>
        /// Image width
        /// </summary>
        public new int Width
        {
            get
            {
                return (int)base.Width;
            }
        }

        /// <summary>
        /// Image height
        /// </summary>
        public new int Height
        {
            get
            {
                return (int)base.Height;
            }
        }

        /// <summary>
        /// Get color at certain position
        /// </summary>
        /// <param name="x">Image x position</param>
        /// <param name="y">Image y position</param>
        /// <returns></returns>
        public Color this[int x, int y]
        {
            get
            {
                return new Color(GetPixel(x, y));
            }
			set
			{
				SetPixel(x, y, value.Hexcode);
			}
        }

        /// <summary>
        /// Original image name if exists
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Create new image as subimage
        /// </summary>
        /// <param name="source">Source image</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="w">Image width</param>
        /// <param name="h">Image height</param>
		private Image(Image source, int x, int y, int w, int h)
			: base(source, x, y, w, h)
		{
			
		}

        /// <summary>
        /// Creates new blank image of specified size
        /// </summary>
        /// <param name="w">Image width</param>
        /// <param name="h">Image height</param>
		/// <param name="type">Data storing type</param>  
        /// <param name="name">Name of this image</param>
        public Image(int w, int h, ImageFormat type = ImageFormat.RGBA, string name = "") 
			: base((uint)w, (uint)h, (uint)type)
        {
            Name = name;
        }

        /// <summary>
        /// Create new blank image from raw data
        /// </summary>
        /// <param name="data">Raw image data</param>
        /// <param name="w">Image width</param>
        /// <param name="h">Image height</param>
        /// <param name="name">Image name</param>
        /// <param name="type">Image storage type</param>
		public Image(byte[] data, int w, int h, string name = "", ImageFormat type = ImageFormat.RGBA)
			: base(data, w, h, (int)type)
		{
			Name = name;
		}

        /// <summary>
        /// Create new blank image of specified size
        /// </summary>
        /// <param name="width">Image width</param>
        /// <param name="height">Image height</param>
        /// <param name="name">Image name</param>
        public Image(int width, int height, string name = "") : this(width, height, ImageFormat.RGBA, name)
        {
            
        }

        /// <summary>
        /// Set subimage
        /// </summary>
        /// <param name="source">Source image</param>
        /// <param name="pt">Point at which subimage will be placed</param>
		public void SetSubimage(Image source, Point pt)
		{
			SetSubimage(source, pt.X, pt.Y);
		}

        /// <summary>
        /// Set subimage
        /// </summary>
        /// <param name="source">Source image</param>
        /// <param name="x">X position within the image</param>
        /// <param name="y">Y position within the image</param>
		public void SetSubimage(Image source, int x, int y)
		{
			base.SetSubimage(source, x, y);
		}

        /// <summary>
        /// Create a new image as subimage
        /// </summary>
        /// <param name="source">Source image</param>
        /// <param name="rect">Source rectangle</param>
        /// <returns>New Image</returns>
		public static Image Subimage(Image source, Rectangle rect)
		{
			return Subimage(source, rect.TopLeft, rect.Width, rect.Height);
		}

        /// <summary>
        /// Create a new image as subimage
        /// </summary>
        /// <param name="source">Source image</param>
        /// <param name="at">Starting point</param>
        /// <param name="w">Result width</param>
        /// <param name="h">Result height</param>
        /// <returns>New Image</returns>
		public static Image Subimage(Image source, Point at, int w, int h)
		{
            if ((at.X + w > source.Width) || (at.Y + h > source.Height))
                throw new ArgumentOutOfRangeException("Subimage is out of source bounds");
			return Subimage(source, at.X, at.Y, w, h);
		}

        /// <summary>
        /// Create a new image as subimage
        /// </summary>
        /// <param name="source">Source image</param>
        /// <param name="x">Starting x position</param>
        /// <param name="y">Starting y position</param>
        /// <param name="w">Result width</param>
        /// <param name="h">Result height</param>
        /// <returns>New Image</returns>
		public static Image Subimage(Image source, int x, int y, int w, int h)
		{
			return new Image(source, x, y, w, h);
		}

        internal Image(IntPtr handle) : base(handle)
        {

        }

        /// <summary>
        /// Rename the image
        /// </summary>
        /// <param name="newName">New image name</param>
        public void Rename(string newName)
        {
            Name = newName;
        }
    }
}
