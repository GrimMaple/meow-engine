using Meow.Framework.Graphics;
using System.IO;

namespace Meow.Framework.Util
{
    public static class TGA
    {
        public static Image Load(string path)
        {
            return new Image(Core.Util.Resource.TGA.LoadTGA(path));
        }
    }
}
