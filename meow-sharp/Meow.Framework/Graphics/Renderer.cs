namespace Meow.Framework.Graphics
{
    /// <summary>
    /// Represents a factory that writes to Renderer
    /// </summary>
    public class Renderer : IArraysParser
    {
        public void Put(DrawObject obj)
        {
            Core.Renderer.SetTexture(obj.textureObj);
            Core.Renderer.DrawArray(obj.vertex, obj.texture, obj.color, obj.elemCount, Core.DrawMethod.Triangles);
        }

        public void UseShader(ShadingProgram program)
        {
            Core.Renderer.UseShader(program);
        }

        public void Translate(float x, float y)
        {
            Core.Renderer.Translate(x, y);
        }

        public void Rotate(float angle)
        {
            Core.Renderer.Rotate(angle);
        }

        public void PushMatrix()
        {
            Core.Renderer.PushMatrix();
        }

        public void PopMatrix()
        {
            Core.Renderer.PopMatrix();
        }

        public void ResetMatrix()
        {
            Core.Renderer.ResetMatrix();
        }
    }
}
