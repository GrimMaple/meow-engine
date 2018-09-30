using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meow.Framework.Audio
{
    /// <summary>
    /// Describes audio format
    /// </summary>
    public class AudioFormat
    {
        /// <summary>
        /// Amount of channels
        /// </summary>
        public int Channels
        {
            get;
            private set;
        }

        /// <summary>
        /// Amount of samples per second
        /// </summary>
        public int SamplesPerSec
        {
            get;
            private set;
        }

        /// <summary>
        /// Average amount of bytes per second
        /// </summary>
        public int AvgBytesPerSec
        {
            get;
            private set;
        }

        /// <summary>
        /// BlockAlign
        /// </summary>
        public int BlockAlign
        {
            get;
            private set;
        }

        /// <summary>
        /// Amount of bits per sample
        /// </summary>
        public int BitsPerSample
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a new audio format specification
        /// </summary>
        /// <param name="ch">Number of channels</param>
        /// <param name="sps">Samples per second</param>
        /// <param name="abps">Average bytes per second</param>
        /// <param name="ba">Block align</param>
        /// <param name="bps">Bits per sample</param>
        public AudioFormat(int ch, int sps, int abps, int ba, int bps)
        {
            Channels = ch;
            SamplesPerSec = sps;
            AvgBytesPerSec = abps;
            BlockAlign = ba;
            BitsPerSample = bps;
        }
    }
}
