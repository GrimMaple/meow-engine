using System.Collections.Generic;

namespace Meow.Framework.Graphics.Util
{
    class BufferBuilder : IArraysParser
    {
        List<BufferArray> arrays = new List<BufferArray>();

        public BufferArray[] Arrays
        {
            get
            {
                return arrays.ToArray();
            }
        }


        public void Put(DrawObject obj)
        {
            arrays.Add(new BufferArray(obj));
        }

        public void UseShader(ShadingProgram program)
        {
            
        }
    }
}
