using System;

namespace Meow.Framework.Graphics
{
    public struct DrawObject
    {
        public float[] vertex;
        public float[] color;
        public float[] texture;
        public ShadingProgram shader;
        public Texture textureObj;
        public uint elemCount;
        public IntPtr vbos;

        public void Set(IntPtr d)
        {
            vbos = d;
        }
    }
}
