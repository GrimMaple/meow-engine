using System;

namespace Meow.Framework.Util
{
    using Graphics;
    using System.IO;

    /// <summary>
    /// MTX Image format loader
    /// </summary>
    public class MTX
    {
        /// <summary>
        /// Format key
        /// </summary>
        private const int KEY = 0x3158544D;

        /// <summary>
        /// Read MTX image from file
        /// </summary>
        /// <param name="file">File path</param>
        /// <returns>New image</returns>
        public static Image FromFile(string file)
        {
            byte[] data = File.ReadAllBytes(file);
            return FromMemory(new MemoryStream(data));
        }

        /// <summary>
        /// Reads MTX image from memory stream
        /// </summary>
        /// <param name="stream">Stream to read from</param>
        /// <returns>New image</returns>
        public static Image FromMemory(MemoryStream stream)
        {
            BinaryReader br = new BinaryReader(stream);
            if (br.ReadInt32() != KEY)
                throw new Exception();

            byte flags = br.ReadByte();
            int bytes = flags & 3;
            bool compress = (flags & 4) != 0;
            bytes++;
            short w = br.ReadInt16();
            short h = br.ReadInt16();
            int length = bytes * w * h;
            byte[] data;
            if (!compress)
             data = br.ReadBytes(length);
            else
            {
                RLEReader reader = new RLEReader(br.BaseStream);
                reader.ElementLength = bytes;
                data = reader.ReadToEnd();
            }
            Image ret = new Image(data, w, h, "", (ImageFormat)bytes);
            return ret;
        }

        /// <summary>
        /// Get image info
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns>Image info</returns>
        public static ImageInfo GetInfo(string path)
        {
            ImageInfo nfo;
            BinaryReader br = new BinaryReader(File.OpenRead(path));
            byte[] data = br.ReadBytes(9);
            if (BitConverter.ToInt32(data, 0) != KEY)
                throw new Exception();
            nfo.bpp = (data[4] & 3) + 1;
            nfo.name = Path.GetFileNameWithoutExtension(path);
            nfo.width = BitConverter.ToInt16(data, 5);
            nfo.height = BitConverter.ToInt16(data, 7);
            nfo.fileSize = (int)br.BaseStream.Length;
            nfo.filePath = path;
            nfo.imageFormat = ImageType.MTX;
            br.Close();
            return nfo;
        }
    }
}
