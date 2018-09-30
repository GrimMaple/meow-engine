using System.Runtime.InteropServices;
using System;

namespace Meow.Core
{
	using Util.Resource;
    /// <summary>
    /// Renderer drawing mode
    /// </summary>
    public enum DrawMethod
    {
        Squares = 4,
        Triangles = 3,
        Lines = 2,
        Points = 1
    }

    /// <summary>
    /// An abstraction layer between managed Renderer and unmanaged renderer
    /// </summary>
    public class Renderer
    {

        [DllImport("meow")]
        private static extern void renderer_set_texture(uint texid);

        [DllImport("meow")]
        private static extern void renderer_draw_array([MarshalAs(UnmanagedType.LPArray)] float[] vertex, [MarshalAs(UnmanagedType.LPArray)] float[] texture, [MarshalAs(UnmanagedType.LPArray)] float[] color, uint count, uint mode);

        [DllImport("meow")]
        private static extern void renderer_draw_array_offs([MarshalAs(UnmanagedType.LPArray)] float[] vertex, [MarshalAs(UnmanagedType.LPArray)] float[] texture, [MarshalAs(UnmanagedType.LPArray)] float[] color, uint offset, uint count, uint mode);

        [DllImport("meow")]
        private static extern int renderer_get_max_texture_size();

        [DllImport("meow")]
        private static extern uint renderer_create_shader_program(string source);

        [DllImport("meow")]
        private static extern void renderer_use_program(uint shader);

        [DllImport("meow")]
        private static extern void renderer_draw_buffer_object(IntPtr obj, uint count);

        [DllImport("meow")]
        private static extern void renderer_translate(float x, float y);

        [DllImport("meow")]
        private static extern void renderer_rotate(float angle);

        [DllImport("meow")]
        private static extern void renderer_push_matrix();

        [DllImport("meow")]
        private static extern void renderer_pop_matrix();

        [DllImport("meow")]
        private static extern void renderer_reset_matrix();

        /// <summary>
        /// Get maximum hardware supported texture size
        /// </summary>
        public static int MaxTextureSize
        {
            get
            {
                return renderer_get_max_texture_size();
            }
        }

        /// <summary>
        /// Tell renderer which texture to use
        /// </summary>
        /// <param name="texture">Texture to use</param>
        public static void SetTexture(Texture texture)
        {
            if (texture == null)
            {
                renderer_set_texture(0);
                return;
            }
            renderer_set_texture(texture.TextureID);
        }

        /// <summary>
        /// Draw sequental vertices in one call
        /// </summary>
        /// <param name="vertices">Array of vertices to draw</param>
        /// <param name="textureCoords">Array of texture coordinates for vertices</param>
        /// <param name="color">Array of colors for vertices</param>
        /// <param name="count">Numer of vertices to draw</param>
        /// <param name="method">Drawing mode</param>
        public static void DrawArray(float[] vertices, float[] textureCoords, float[] color, uint count, DrawMethod method)
        {
            renderer_draw_array(vertices, textureCoords, color, count, (uint)method);
        }

        /// <summary>
        /// Draw sequental vertices in one call
        /// </summary>
        /// <param name="vertices">Array of vertices to draw</param>
        /// <param name="textureCoords">Array of texture coordinates for vertices</param>
        /// <param name="color">Array of colors for vertices</param>
        /// <param name="offset">Offset in array to start with</param>
        /// <param name="count">Numer of vertices to draw</param>
        /// <param name="method">Drawing mode</param>
        public static void DrawArray(float[] vertices, float[] textureCoords, float[] color, uint offset, uint count, DrawMethod method)
        {
            renderer_draw_array_offs(vertices, textureCoords, color, offset, count, (uint)method);
        }

        /// <summary>
        /// Set current texture to 0, thus tell renderer not to use any textures
        /// </summary>
        public static void ResetTexture()
        {
            renderer_set_texture(0);
        }

        /// <summary>
        /// Create a shading program from single source
        /// </summary>
        /// <param name="source">Fragment shader source</param>
        /// <returns>Shading program ID</returns>
        public static uint CreateShader(string source)
        {
            return renderer_create_shader_program(source);
        }

        /// <summary>
        /// Use shading program for drawing
        /// </summary>
        /// <param name="program">Shading program to use</param>
        public static void UseShader(ShadingProgram program)
        {
            renderer_use_program(program == null ? 0 : program.Id);
        }

        /// <summary>
        /// Reset shading program state so no shading program is used for drawing
        /// </summary>
        public static void UnuseShaders()
        {
            renderer_use_program(0);
        }

        public static void DrawBuffer(VBO vbo)
        {
            renderer_draw_buffer_object(vbo.Resource, vbo.Count);
        }

        public static void Rotate(float angle)
        {
            renderer_rotate(angle);
        }

        public static void Translate(float x, float y)
        {
            renderer_translate(x, y);
        }

        public static void PushMatrix()
        {
            renderer_push_matrix();
        }

        public static void PopMatrix()
        {
            renderer_pop_matrix();
        }
        
        public static void ResetMatrix()
        {
            renderer_reset_matrix();
        }
    }
}
