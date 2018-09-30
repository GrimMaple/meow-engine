using System;
using System.Runtime.InteropServices;

namespace Meow.Core.Util.Resource
{
    internal delegate void UniformChanging(string uniform);

    /// <summary>
    /// Low-level abstraction of shading program
    /// </summary>
	public class ShadingProgram : UnmanagedResource
	{
		[DllImport("meow")]
		private static extern uint renderer_create_shader_program([MarshalAs(UnmanagedType.LPStr)] 
		                                                     string source);
        [DllImport("meow")]
        private static extern uint renderer_create_program(uint[] shaders, int length);

        [DllImport("meow")]
        private static extern void renderer_delete_program(uint program);

        [DllImport("meow")]
        private static extern uint renderer_uniform_location(uint program,
                                                             [MarshalAs(UnmanagedType.LPStr)]
                                                             string name);

        [DllImport("meow")]
        private static extern void ex_uniform_1f(uint location, float value);

        [DllImport("meow")]
        private static extern void ex_uniform_2f(uint location, float _0, float _1);

        [DllImport("meow")]
        private static extern void ex_uniform_3f(uint location, float _0, float _1, float _2);

        [DllImport("meow")]
        private static extern void ex_uniform_4f(uint location, float _0, float _1, float _2, float _3);

        [DllImport("meow")]
        private static extern void ex_uniform_1i(uint location, int value);

        [DllImport("meow")]
        private static extern void ex_attach_texture(uint location, int unit, uint texture);

        internal uint Id
        {
            get
            {
                return (uint)Resource;
            }
        }

        protected uint UniformLocation(string uniform)
        {
            return renderer_uniform_location((uint)Resource, uniform);
        }

        protected void Uniform(uint location, float value)
        {
            ex_uniform_1f(location, value);
        }

        protected void Uniform(uint location, float _0, float _1)
        {
            ex_uniform_2f(location, _0, _1);
        }

        protected void Uniform(uint location, float _0, float _1, float _2)
        {
            ex_uniform_3f(location, _0, _1, _2);
        }

        protected void Uniform(uint location, float _0, float _1, float _2, float _3)
        {
            ex_uniform_4f(location, _0, _1, _2, _3);
        }

        protected void Uniform(uint location, int value)
        {
            ex_uniform_1i(location, value);
        }

        protected ShadingProgram(string source)
		{
			Resource = (IntPtr)renderer_create_shader_program(source);
		}

        protected ShadingProgram(Shader shader)
        {
            Resource = (IntPtr)renderer_create_program(new uint[] { shader.id }, 1);
        }

        protected ShadingProgram(Shader[] shaders)
        {
            uint[] shadersLow = new uint[shaders.Length];
            for (int i = 0; i < shaders.Length; i++)
                shadersLow[i] = shaders[i].id;

            Resource = (IntPtr)renderer_create_program(shadersLow, shaders.Length);
        }

		protected override void Dispose(bool disposing)
		{
			if(!Disposed)
			{
                renderer_delete_program((uint)Resource);
                Disposed = true;
			}
			base.Dispose(disposing);
		}

        protected void AttachTexture(uint location, int unit, uint value)
        {
            ex_attach_texture(location, unit, value);
        }
	}
}

