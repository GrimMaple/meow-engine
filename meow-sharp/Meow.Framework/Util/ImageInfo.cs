using System;

namespace Meow.Framework.Util
{
    /// <summary>
    /// Image type
    /// </summary>
    public enum ImageType
    {
        /// <summary>
        /// Meow TeXture
        /// </summary>
        MTX = 0,

        /// <summary>
        /// Tagra image format
        /// </summary>
        TGA = 1
    }

    /// <summary>
    /// Provides basic info about an image
    /// </summary>
    public struct ImageInfo : IComparable
    {
        /// <summary>
        /// Image width
        /// </summary>
        public int width;

        /// <summary>
        /// Image height
        /// </summary>
        public int height;

        /// <summary>
        /// Image name
        /// </summary>
        public string name;

        /// <summary>
        /// Color depth as bits per pixel
        /// </summary>
        public int bpp;

        /// <summary>
        /// Image file size
        /// </summary>
        public int fileSize;

        /// <summary>
        /// Image file path
        /// </summary>
        public string filePath;

        /// <summary>
        /// Image file format
        /// </summary>
        public ImageType imageFormat;

        /// <summary>
        /// Compare two images
        /// </summary>
        /// <param name="compare">Image to compare to</param>
        /// <returns></returns>
        public int CompareTo(object compare)
        {
            if (compare is ImageInfo)
            {
                ImageInfo c = (ImageInfo)compare;
                if ((width * height) == (c.width * c.height))
                    return name.CompareTo(c.name);
                return (width * height) < (c.width * c.height) ? -1 : 1;
            }

            throw new ArgumentException();
        }
    }
}
