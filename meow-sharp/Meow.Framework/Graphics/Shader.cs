namespace Meow.Framework.Graphics
{
    public enum ShaderType : uint
    {
        Fragment = 0,
        Vertex = 1
    }

    /// <summary>
    /// High level abstraction of a Shader resource
    /// </summary>
    public class Shader : Core.Util.Shader
    {
        /// <summary>
        /// Create a shader
        /// </summary>
        /// <param name="source">Shader source</param>
        /// <param name="type">Shader type</param>
        public Shader(string source, ShaderType type) : base(source, (uint)type)
        {

        }
    }
}
