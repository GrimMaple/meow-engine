using System;
using System.Runtime.InteropServices;

namespace Meow.Core.Util
{
	public class Shader
	{
        [DllImport("meow")]
        private static extern uint renderer_create_shader([MarshalAs(UnmanagedType.LPStr)]
                                                          string source,
                                                          uint type);
        internal uint id;

		protected Shader(string source, uint type)
		{
            id = renderer_create_shader(source, type);
		}
	}
}

