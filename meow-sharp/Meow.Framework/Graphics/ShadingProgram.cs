using System.Collections.Generic;

namespace Meow.Framework.Graphics
{
    /// <summary>
    /// High level abstraction of a Shading Program
    /// </summary>
    public class ShadingProgram : Core.Util.Resource.ShadingProgram
    {
        private Dictionary<string, uint> uniforms = new Dictionary<string, uint>();

        private uint GetUniform(string uniform)
        {
            if(!uniforms.ContainsKey(uniform))
                uniforms.Add(uniform, UniformLocation(uniform));

            return uniforms[uniform];
        }

        /// <summary>
        /// Set a uniform value
        /// </summary>
        /// <param name="uniform">Uniform name</param>
        /// <param name="value">1f value</param>
        public void SetUniform(string uniform, float value)
        {
            Uniform(GetUniform(uniform), value);
        }

        /// <summary>
        /// Set a uniform value
        /// </summary>
        /// <param name="uniform">Uniform name</param>
        /// <param name="color">(4f) Color value</param>
        public void SetUniform(string uniform, Color color)
        {
            GetUniform(uniform);

            Uniform(uniforms[uniform], color.R, color.G, color.B, color.A);
        }

        /// <summary>
        /// Set a uniform value
        /// </summary>
        /// <param name="uniform">Uniform name</param>
        /// <param name="value">1i value</param>
        public void SetUniform(string uniform, int value)
        {
            Uniform(GetUniform(uniform), value);
        }

        public void AttachTexture(string uniform, int unit, Texture texture)
        {
            AttachTexture(GetUniform(uniform), unit, texture.TextureID);
        }

        /// <summary>
        /// Create a shading program from shader resource
        /// </summary>
        /// <param name="shader">Shader resource</param>
        public ShadingProgram(Shader shader) : base(shader) { }

        /// <summary>
        /// Create a shading program from multiple shader resources
        /// </summary>
        /// <param name="shaders">Array of shader resources</param>
        public ShadingProgram(Shader[] shaders) : base(shaders) { }
    }
}
