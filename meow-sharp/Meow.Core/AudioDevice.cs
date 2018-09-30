using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Meow.Core
{
    /// <summary>
    /// Low-level abstracion of audio device
    /// </summary>
    public abstract class AudioDevice : IDisposable
    {
        [DllImport("meow.dll")]
        private static extern IntPtr audio_open(int samples, uint bps, uint channels, uint blockAlign, uint avgBps);

        [DllImport("meow.dll")]
        private static extern void audio_close(IntPtr handle);

        [DllImport("meow.dll")]
        private static extern void audio_write(IntPtr handle, [MarshalAs(UnmanagedType.LPArray)] byte[] data, uint size);

        [DllImport("meow.dll")]
        private static extern void audio_stop(IntPtr handle);

        [DllImport("meow")]
        private static extern void audio_subscribe(IntPtr handle, AudioCallback callback);

        private delegate void AudioCallback();

        private AudioCallback callback;

        private IntPtr handle = IntPtr.Zero;


        private void CallbackFun()
        {
            Console.WriteLine("Callback fired");
            NextRequested();
        }

        protected abstract void NextRequested();

        /// <summary>
        /// Create a new low-level audio device
        /// </summary>
        /// <param name="numSamples">Number of sasmples per second</param>
        /// <param name="bitsPerSample">Amount of bits per sample</param>
        /// <param name="numChannels">Amount of channels</param>
        /// <param name="blockAlign">Block alighn</param>
        /// <param name="avgBps">Average bytes per second</param>
        public AudioDevice(int numSamples, uint bitsPerSample, uint numChannels, uint blockAlign, uint avgBps)
        {
            handle = audio_open(numSamples, bitsPerSample, numChannels, blockAlign, avgBps);
            callback = CallbackFun;
            audio_subscribe(handle, callback);
        }

        public void Dispose()
        {
            audio_close(handle);
            GC.SuppressFinalize(this);
        }

        ~AudioDevice()
        {
            if(handle != IntPtr.Zero)
                audio_close(handle);
        }

        /// <summary>
        /// Write raw audio data to device
        /// </summary>
        /// <param name="data">Data to write</param>
        protected void Write(byte[] data, int length)
        {
            audio_write(handle, data, (uint)length);
        }

        protected void Write(IntPtr data, int Length)
        {

        }

        /// <summary>
        /// Stop current device from playing immediately
        /// </summary>
        public void Stop()
        {
            audio_stop(handle);
        }
    }
}
