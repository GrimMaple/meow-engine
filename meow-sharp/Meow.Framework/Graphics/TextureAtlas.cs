using System;
using System.Collections.Generic;
using System.IO;
using Meow.Framework.Util;

namespace Meow.Framework.Graphics
{
    public struct TexInfo
    {
        public string name;
        public int xpos;
        public int ypos;
        public int width;
        public int height;
    }

    /// <summary>
    /// Represents a texture as a subset of smaller sprites
    /// </summary>
    public sealed class TextureAtlas : IDisposable
    {
        private const int KEY = 0x3341544D; // MTA3

        private byte flags = 0;
        private Dictionary<string, Sprite> textures;
        private Texture[] loadedTextures;
        private int width;
        private Image baseImage;
        private int x, y, maxy;
        private List<SpriteData> sprites;

        private struct SpriteData
        {
            public string name;
            public int x, y;
            public int w, h;
        }

        private int GetDimensions()
        {
            return 1024;// Renderer.MaxTextureSize;
        }

        /// <summary>
        /// Get named sprite
        /// </summary>
        /// <param name="texture">Sprite name</param>
        /// <returns>Sprite</returns>
        public Sprite this[string texture]
        {
            get
            {
                return textures[texture];
            }
        }

        public string[] SpriteNames
        {
            get
            {
                string[] res = new string[textures.Keys.Count];
                textures.Keys.CopyTo(res, 0);
                return res;
            }
        }

        private void Prepare(int side)
        {
            width = side;
            baseImage = new Image(side, side, (ImageFormat)((flags & 3) + 1));
            x = y = maxy = 0;
            sprites = new List<SpriteData>();
        }

        private void Build()
        {
            loadedTextures = new Texture[1];
            loadedTextures[0] = new Texture(baseImage);
            textures = new Dictionary<string, Sprite>();
            foreach(SpriteData dt in sprites)
            {
                textures.Add(dt.name, loadedTextures[0].ToSprite(new Point(dt.x, dt.y), dt.w, dt.h));
            }
            baseImage.Dispose();
            baseImage = null;
        }

        private void Put(Image t)
        {
            maxy = maxy > t.Height ? maxy : t.Height;
            if(x + t.Width > width)
            {
                y += maxy;
                maxy = 0;
                x = 0;
            }
            baseImage.SetSubimage(t, x, y);
            SpriteData dt;
            dt.h = t.Height;
            dt.w = t.Width;
            dt.name = t.Name;
            dt.x = x;
            dt.y = y;
            x += t.Width;


            sprites.Add(dt);
        }

        /// <summary>
        /// Load texture atlas from stream
        /// </summary>
        /// <param name="s">Stream</param>
        /// <returns>New TextureAtlas</returns>
        public static TextureAtlas Load(Stream s)
        {
            BinaryReader br = new BinaryReader(s);
            if (br.ReadInt32() != KEY)
                throw new IOException("Wrong file");

            TextureAtlas atlas = new TextureAtlas();
            atlas.flags = br.ReadByte();
            int requiredSide = br.ReadInt32();
            atlas.Prepare(requiredSide);

            while(br.BaseStream.Position < br.BaseStream.Length)
            {
                ImageType type = (ImageType)br.ReadByte();
                int length = br.ReadInt32();
                string name = br.ReadString();
                Image t;
                MemoryStream ms = new MemoryStream(br.ReadBytes(length));
                switch(type)
                {
                    case ImageType.MTX:
                        t = MTX.FromMemory(ms);
                        break;
                    default:
                        throw new Exception();
                }
                t.Rename(name);
                ms.Close();
                atlas.Put(t);
                t.Dispose();
            }
            atlas.Build();
            return atlas;
        }

        /// <summary>
        /// Load texture atlas from file
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>New TextureAtlas</returns>
        public static TextureAtlas FromFile(string path)
        {
            return Load(File.OpenRead(path));
        }

        /// <summary>
        /// Ensures that all unmanaged resources are freed properly
        /// </summary>
        public void Dispose()
        {
            foreach (Texture t in loadedTextures)
                t.Dispose();
        }

        private TextureAtlas()
        {

        }
    }
}
