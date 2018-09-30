using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meow.Framework.Audio
{
    /// <summary>
    /// High level abstraction of sound device
    /// </summary>
    public sealed class SoundPlayer : Meow.Core.AudioDevice
    {

        private IAudio currentAudio;
        
        /// <summary>
        /// Create a new sound player of desired format
        /// </summary>
        /// <param name="fmt">Desired audio format</param>
        public SoundPlayer(AudioFormat fmt) : base(fmt.SamplesPerSec, (uint)fmt.BitsPerSample, (uint)fmt.Channels, (uint)fmt.BlockAlign, (uint)fmt.AvgBytesPerSec)
        {
        }

        protected override void NextRequested()
        {
            if(currentAudio != null)
                PlayNext();
        }

        public void Play(IAudio audio)
        {
            currentAudio = audio;
            PlayNext();
        }

        private void PlayNext()
        {
            WaveData data = currentAudio.Next();
            Write(data.Data, data.Length);
        }
    }
}
