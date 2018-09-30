using System;

namespace Meow.Framework.Graphics
{
    public class RenderTarget
	{
		private float[] vertices;
		private float[] colors;
		private float[] textureCoords;

		private Texture texture;
		private ShadingProgram program;
        private IArraysParser arraysParser;

		private int capacity = 10000;
		private uint count = 0;

		private enum States
		{
			None,
			Triangle
		}

		private States state = States.None;

		internal RenderTarget(IArraysParser parser)
		{
            arraysParser = parser;
			RebuildArrays();
		}

		private void RebuildArrays()
		{
			float[] nVertices = new float[capacity * 3];
			float[] nColors = new float[capacity * 4];
			float[] nTextures = new float[capacity * 2];

			if(vertices == null)
				goto setArrays;

			int prevLength = vertices.Length / 3;

			for(int i=0; i<prevLength; i++)
			{
				int offs = i * 3;
				nVertices[offs] = vertices[offs];
				nVertices[offs + 1] = vertices[offs + 1];
				nVertices[offs + 2] = vertices[offs + 2];
				offs = i*2;
				nTextures[offs] = textureCoords[offs];
				nTextures[offs+1] = textureCoords[offs+1];
				offs = i * 4;
				nColors[offs] = colors[offs];
				nColors[offs + 1] = colors[offs + 1];
				nColors[offs + 2] = colors[offs + 2];
				nColors[offs + 3] = colors[offs + 3];
			}

		setArrays:
			vertices = nVertices;
			textureCoords = nTextures;
			colors = nColors;
		}

		public void AddPoint(float x, float y, float z, float u, float v, float r, float g, float b, float a)
		{
			if(count==(capacity-1))
			{
				capacity *= 2;
				RebuildArrays();
			}
			state = States.Triangle;
			vertices[count * 3] = x;
			vertices[count * 3 + 1] = y;
			vertices[count * 3 + 2] = z;

			colors[count * 4] = r;
			colors[count * 4 + 1] = g;
			colors[count * 4 + 2] = b;
			colors[count * 4 + 3] = a;

			textureCoords[count * 2] = u;
			textureCoords[count * 2 + 1] = v;

			count++;
			if (count % 3 == 0)
				state = States.None;
		}

		public void SetTexture(Texture id)
		{
            bool a = id == null;
            bool b = texture == null;
            if((a && !b) || (!a && b))
            {
                Flush();
                texture = id;
                return;
            }
            if (a & b)
                return;
            if (texture.TextureID == id.TextureID)
                return;
			Flush();
			texture = id;
		}

		public void SetProgram(ShadingProgram program)
		{
			Flush();
            arraysParser.UseShader(program);
			this.program = program;
		}

		public void Flush()
		{
			if(state != States.None)
				throw new InvalidOperationException("Tried to flush an unfinished primitive");
            DrawObject obj;
            obj.color = colors;
            obj.vertex = vertices;
            obj.texture = textureCoords;
            obj.textureObj = texture;
            obj.shader = program;
            obj.elemCount = count;
            obj.vbos = IntPtr.Zero;
            arraysParser.UseShader(obj.shader);
            arraysParser.Put(obj);
			/*Renderer.UseShader(program);
			Renderer.SetTexture(texture);
			Renderer.DrawArray(vertices, textureCoords, colors, count, DrawMethod.Triangles);*/

			count = 0;
		}
	}
}