using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meow.Framework.Audio
{
    public class WaveData
    {
        private byte[] data;
        
        public int Length
        {
            get; private set;
        }

        public byte[] Data
        {
            get
            {
                return data;
            }
        }

        public WaveData(byte[] data, int length)
        {
            this.data = data;
            Length = length;
        }
    }
}
