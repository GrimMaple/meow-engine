using System;
using System.Collections.Generic;
using System.IO;

namespace Meow.Framework.Util
{
    /// <summary>
    /// Run-length encoding writer
    /// </summary>
    public sealed class RLEWriter
    {
        Stream baseStream;

        /// <summary>
        /// Get or set element length in bytes
        /// </summary>
        public int ElementLength { get; set; }

        private Stack<RLEElement> repeats = new Stack<RLEElement>();
        private List<RLEElement> buffer = new List<RLEElement>();
        
        private RLEElement[] RepeatsToArray()
        {
            RLEElement[] array = new RLEElement[repeats.Count];
            int count = repeats.Count;
            for (int i = 1; i <= count; i++)
            {
                array[count - i] = repeats.Pop();
            }
            repeats = new Stack<RLEElement>();
            return array;
        }

        private void FlushRepeats()
        {
            if(repeats.Count < ElementLength + 2)
            {
                buffer.AddRange(RepeatsToArray());
                return;
            }
            if (buffer.Count != 0)
                FlushBuffer();
            RLEElement elemnt = repeats.Peek();
            int toWrite = repeats.Count;
            while(toWrite > short.MaxValue)
            {
                baseStream.WriteBytesOrdered(BitConverter.GetBytes((short)(-short.MaxValue)));
                baseStream.WriteBytesOrdered(elemnt.Data);
                toWrite -= short.MaxValue;
            }
            baseStream.WriteBytesOrdered(BitConverter.GetBytes((short)(-toWrite)));
            baseStream.WriteBytesOrdered(elemnt.Data);
            repeats = new Stack<RLEElement>();
        }

        /// <summary>
        /// Flush all remaining components to stream
        /// </summary>
        private void FlushBuffer()
        {
            int toWrite = buffer.Count;
            int written = 0;
            while(toWrite > short.MaxValue)
            {
                baseStream.WriteBytesOrdered(BitConverter.GetBytes(short.MaxValue));
                for (int i = 0; i < short.MaxValue; i++)
                    baseStream.WriteBytesOrdered(buffer[written + i].Data);
                written += short.MaxValue;
            }
            baseStream.WriteBytesOrdered(BitConverter.GetBytes((short)(toWrite)));
            for (int i = 0; i < toWrite; i++)
                baseStream.WriteBytesOrdered(buffer[written + i].Data);
            buffer = new List<RLEElement>();
        }

        /// <summary>
        /// Base stream to write to
        /// </summary>
        public Stream BaseStream
        {
            get
            {
                return baseStream;
            }
        }

        
        /// <summary>
        /// Create a new RLE writer from stream
        /// </summary>
        /// <param name="stream">Reference stream</param>
        public RLEWriter(Stream stream)
        {
            baseStream = stream;
            ElementLength = 1;
        }

        /// <summary>
        /// Write element
        /// </summary>
        /// <param name="element">Element data</param>
        public void Write(byte[] element)
        {
            RLEElement elem = new RLEElement(element);
            if(repeats.Count == 0)
            {
                repeats.Push(elem);
                return;
            }
            if(repeats.Peek() == elem)
            {
                repeats.Push(elem);
                return;
            }
            FlushRepeats();
            repeats.Push(elem);
        }

        public void Write(uint element)
        {
            if (ElementLength != 4)
                throw new Exception();
            Write(BitConverter.GetBytes(element));
        }

        /// <summary>
        /// Write byte element
        /// </summary>
        /// <param name="element">Byte element</param>
        public void Write(byte element)
        {
            if (ElementLength != 1)
                throw new Exception();
            Write(new byte[] { element });
        }

        /// <summary>
        /// Write short element
        /// </summary>
        /// <param name="element">Short element</param>
        public void Write(short element)
        {
            if (ElementLength != 2)
                throw new Exception();
            Write(BitConverter.GetBytes(element));
        }

        /// <summary>
        /// Write int element
        /// </summary>
        /// <param name="element">Int element</param>
        public void Write(int element)
        {
            if (ElementLength != 4)
                throw new Exception();
            Write(BitConverter.GetBytes(element));
        }

        /// <summary>
        /// Write multiple elements
        /// </summary>
        /// <param name="elements">Elements array</param>
        public void WriteMultiple(byte[] elements)
        {
            for(int i=0; i<elements.Length; i+=ElementLength)
            {
                Write(elements.Copy(i, ElementLength));
            }
        }

        /// <summary>
        /// Flush the stream
        /// </summary>
        public void Flush()
        {
            FlushRepeats();
            if (buffer.Count != 0)
                FlushBuffer();
            baseStream.WriteBytesOrdered(BitConverter.GetBytes((short)(-1)));
            repeats = new Stack<RLEElement>();
            buffer = new List<RLEElement>();
        }
    }
}
