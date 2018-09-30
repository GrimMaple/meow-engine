using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meow.Framework.Audio
{
    public class WaveAudio : IAudio
    {
        private const int groupId = 0x46464952; // RIFF
        private const int riffType = 0x45564157; // WAVE
        private const int chunkID = 0x20746D66; // "fmt "
        private const int dataChunkID = 0x61746164; // "data"
        private int readElements;
        private bool eof = false;

        BinaryReader br;

        private AudioFormat format;

        private List<WaveData> datas;

        public AudioFormat AudioFormat
        {
            get
            {
                return format;
            }
        }

        public WaveData CurrentData
        {
            get
            {
                return datas[0];
            }
        }

        public WaveAudio(string path)
        {
            StreamReader sr = new StreamReader(path);
            br = new BinaryReader(sr.BaseStream);

            if (br.ReadInt32() != groupId)
                throw new FileLoadException("Not a wave audio");
            br.ReadUInt32();
            if (br.ReadInt32() != riffType)
                throw new FileLoadException("Not a RIFF audio");

            if (br.ReadInt32() != chunkID)
                throw new FileLoadException("Shit happens");
            if (br.ReadUInt32() != 16)
                throw new FileLoadException("Shit happens");
            if (br.ReadUInt16() != 1)
                throw new FileLoadException("Only support M$ audio");

            format = new AudioFormat(br.ReadUInt16(), (int)br.ReadUInt32(), (int)br.ReadUInt32(), br.ReadUInt16(), br.ReadUInt16());

            readElements = format.AvgBytesPerSec / 10;
            datas = new List<WaveData>();
            try
            {
                while(br.ReadInt32() == dataChunkID)
                {
                    uint size = br.ReadUInt32();
                    datas.Add(new WaveData(br.ReadBytes(100000), 100000));
                }
            }
            catch
            {

            }
        }

        public WaveData Next()
        {
            if (eof)
                return null;
            byte[] data = new byte[readElements];
            int read = br.Read(data, 0, readElements);
            if(read < readElements)
            {
                eof = true;
            }
            return new WaveData(data, read);
        }
    }
}
