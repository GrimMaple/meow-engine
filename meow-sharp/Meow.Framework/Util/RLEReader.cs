using System;
using System.Collections.Generic;
using System.IO;

namespace Meow.Framework.Util
{
    /// <summary>
    /// Provides RLE Compression reader
    /// </summary>
    public sealed class RLEReader
    {
        Stream basicStream;

        /// <summary>
        /// Element length in bytes
        /// </summary>
        public int ElementLength { get; set; }

        private byte[] ReadCompressed(int length, out int read)
        {
            byte[] buffer = new byte[length * ElementLength];

            byte[] element = new byte[ElementLength];
            int readFromStream = basicStream.Read(element, 0, ElementLength);
            if (readFromStream != ElementLength)
                throw new IOException("Tried to read element but it was too short to match ElementLength");

            read = length * ElementLength;

            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = element[i % ElementLength];
            }

            return buffer;
        }

        private byte[] ReadUncompressed(int length, out int read)
        {
            byte[] buffer = new byte[ElementLength * length];
            int readFromStream = basicStream.Read(buffer, 0, ElementLength * length);
            if (readFromStream != ElementLength * length)
                throw new IOException(String.Format("Tried to read {0} uncompressed elements but there weren't enough of them", length));
            read = readFromStream;
            return buffer;
        }

        /// <summary>
        /// Create new Decompressor from stream
        /// </summary>
        /// <param name="stream"></param>
        public RLEReader(Stream stream)
        {
            basicStream = stream;
        }

        /// <summary>
        /// Read one chunck from stream
        /// </summary>
        /// <param name="read">Amount of bytes reads</param>
        /// <returns>Array of read bytes</returns>
        public byte[] ReadChunk(out int read)
        {
            byte[] buffer = new byte[2];
            int readFromStream = basicStream.Read(buffer, 0, 2);
            read = 0;

            if (readFromStream != 2)
                throw new IOException("Tried to read chunk description but it was too short");

            short id = BitConverter.ToInt16(buffer, 0);
            if (id == -1)
                return null;

            return (id < 0) ? ReadCompressed(-id, out read) : ReadUncompressed(id, out read);
        }

        /// <summary>
        /// Decompress file to the very end
        /// </summary>
        /// <returns>Resulting bytes</returns>
        public byte[] ReadToEnd()
        {
            List<byte> ret = new List<byte>();
            byte[] tmp = null;
            do
            {
                int read;
                tmp = ReadChunk(out read);
                if (tmp != null)
                    ret.AddRange(tmp);
            } while (tmp != null);

            return ret.ToArray();
        }
    }
}
