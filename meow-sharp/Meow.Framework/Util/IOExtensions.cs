using System.IO;

namespace Meow.Framework.Util
{
    /// <summary>
    /// Helper class for Input-output operations
    /// </summary>
    public static class IOExtensions
    {
        /// <summary>
        /// Writes ordered array of bytes
        /// </summary>
        /// <param name="stream">Stream to write to</param>
        /// <param name="array">Array to write</param>
        public static void WriteBytesOrdered(this BinaryWriter stream, byte[] array)
        {
            for (int i = 0; i < array.Length; i++)
                stream.Write(array[i]);
        }

        /// <summary>
        /// Reads ordered bytes from stream
        /// </summary>
        /// <param name="stream">Stream to read from</param>
        /// <param name="count">Number of array elements</param>
        /// <returns></returns>
        public static byte[] ReadBytesOrdered(this BinaryReader stream, int count)
        {
            byte[] ret = new byte[count];
            for (int i = 0; i < count; i++)
                ret[i] = stream.ReadByte();

            return ret;
        }

        /// <summary>
        /// Writes byte array saving bytes order
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="array">Array to write</param>
        public static void WriteBytesOrdered(this Stream stream, byte[] array)
        {
            for (int i = 0; i < array.Length; i++)
                stream.WriteByte(array[i]);
        }

        /// <summary>
        /// Reads bytes saving order
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="count">Amount of bytes to read</param>
        /// <returns>Array of ordered read bytes</returns>
        public static byte[] ReadBytesOrdered(this Stream stream, int count)
        {
            byte[] ret = new byte[count];
            for (int i = 0; i < count; i++)
                ret[i] = (byte)stream.ReadByte();

            return ret;
        }

        /// <summary>
        /// Writes byte array saving bytes order
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="array">Array to write</param>
        public static void WriteBytesOrdered(this MemoryStream stream, byte[] array)
        {
            for (int i = 0; i < array.Length; i++)
                stream.WriteByte(array[i]);
        }

        /// <summary>
        /// Reads bytes saving order
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="count">Amount of bytes to read</param>
        /// <returns>Array of ordered read bytes</returns>
        public static byte[] ReadBytesOrdered(this MemoryStream stream, int count)
        {
            byte[] ret = new byte[count];
            for (int i = 0; i < count; i++)
                ret[i] = (byte)stream.ReadByte();

            return ret;
        }
    }
}
