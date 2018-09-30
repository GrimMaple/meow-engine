namespace Meow.Framework.Graphics
{
    class BufferArray : Core.Util.Resource.VBO
    {
        private Texture texture;
        private ShadingProgram program;
         
        internal BufferArray(DrawObject obj) : base(obj.vertex, obj.texture, obj.color, obj.elemCount)
        {
            texture = obj.textureObj;
            program = obj.shader;
        }

        internal void Draw()
        {
            Core.Renderer.SetTexture(texture);
            Core.Renderer.UseShader(program);
            Core.Renderer.DrawBuffer(this);
        }
    }
}
