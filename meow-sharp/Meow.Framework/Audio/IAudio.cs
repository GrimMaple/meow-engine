using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meow.Framework.Audio
{
    public interface IAudio
    {
        WaveData Next();
    }
}
