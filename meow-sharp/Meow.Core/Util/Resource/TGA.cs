using System;
using System.Runtime.InteropServices;

namespace Meow.Core.Util.Resource
{
    public static class TGA
    {
        [DllImport("meow")]
        private static extern IntPtr tga_load(string path);

        public static IntPtr LoadTGA(string path)
        {
            IntPtr ret = tga_load(path);
            if(ret.ToInt64() == 0)
                throw new Exception();
            return tga_load(path);
        }
    }
}