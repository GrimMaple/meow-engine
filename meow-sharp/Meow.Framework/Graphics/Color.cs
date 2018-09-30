using System;

namespace Meow.Framework.Graphics
{
    /// <summary>
    /// Represents a single color
    /// </summary>
	public struct Color
	{
		#region Predefined Colors
		public static Color White = new Color(255, 255, 255);
		public static Color Black = new Color(0, 0, 0);
		public static Color Transparent = new Color(0, 0, 0, 0);
		public static Color Red255 = new Color(255, 0, 0);
		public static Color Green255 = new Color(0, 255, 0);
		public static Color Blue255 = new Color(0, 0, 255);
		public static Color Yellow255 = new Color(255, 255, 0);
		public static Color LightBlue255 = new Color(0, 255, 255);
		public static Color Purple255 = new Color(255, 0, 255);
		#endregion

		// 0xAABBGGRR
		private uint data;

        /// <summary>
        /// Red component as float
        /// </summary>
		public float R
		{
			get 
			{
				return (float)RByte/255;
			}
		}

        /// <summary>
        /// Green component as float
        /// </summary>
		public float G
		{
			get 
			{
				return (float)GByte/255;
			}
		}

        /// <summary>
        /// Blue component as float
        /// </summary>
		public float B
		{
			get 
			{
				return (float)BByte/255;
			}
		}

        /// <summary>
        /// Alpha component as float
        /// </summary>
		public float A
		{
			get 
			{
				return (float)AByte/255;
			}
		}

        /// <summary>
        /// Red component as byte
        /// </summary>
		public byte RByte
		{
			get 
			{
				return (byte)((data & 0x000000FF));
			}
		}

        /// <summary>
        /// Green component as byte
        /// </summary>
		public byte GByte
		{
			get
			{
				return (byte)((data & 0x0000FF00) >> 8);
			}
		}

        /// <summary>
        /// Blue component as byte
        /// </summary>
		public byte BByte
		{
			get
			{
				return (byte)((data & 0x00FF0000) >> 16);
			}
		}

        /// <summary>
        /// Alpha component as byte
        /// </summary>
		public byte AByte
		{
			get
			{
				return (byte)((data & 0xFF000000)>>24);	
			}
		} 

        /// <summary>
        /// Hex representation of color as 0xAABBGGRR
        /// </summary>
		public uint Hexcode
		{
			get
			{
				return data;
			}
		}

		/// <summary>
		/// Creates new color from float values
		/// </summary>
		/// <param name="r">Red component</param>
		/// <param name="g">Green component</param>
		/// <param name="b">Blue component</param>
		/// <param name="a">Alpha component</param>
		public Color(float r, float g, float b, float a = 1.0f) 
			 : this((int)(r*255), (int)(g*255), (int)(b*255), (int)(a*255))
		{
		}

		/// <summary>
		/// Creates new color from byte values
		/// </summary>
		/// <param name="r">Red component</param>
		/// <param name="g">Green component</param>
		/// <param name="b">Blue component</param>
		/// <param name="a">Alpha component</param>
		public Color(int r, int g, int b, int a = 255) : this((a<<24) | (b << 16) | (g << 8) | r)
		{
			
		}

        /// <summary>
        /// Create a new color from hexcode
        /// </summary>
        /// <param name="hexcode">Hex value as 0xAABBGGRR</param>
		public Color(uint hexcode)
		{
			data = hexcode;
		}

        private Color(int hexcode) : this(BitConverter.ToUInt32(BitConverter.GetBytes(hexcode), 0))
        {

        }

		/// <summary>
		/// Check if two colors are per-component equal
		/// </summary>
		/// <param name="a">Obvious</param>
		/// <param name="b">Obvious</param>
		/// <returns>True if every component of both colors are same</returns>
		public static bool operator ==(Color a, Color b)
		{
			return a.data == b.data;
		}

		/// <summary>
		/// Check if two colors are per-component equal
		/// </summary>
		/// <param name="a">Obvious</param>
		/// <param name="b">Obvious</param>
		/// <returns>True if at least one component of both colors is not same</returns>
		public static bool operator !=(Color a, Color b)
		{
			return !(a == b);
		}

		/// <summary>
		/// Checks for equality of two objects
		/// </summary>
		/// <param name="obj">Object to compare to</param>
		/// <returns>True if given object is a color and all components are same</returns>
		public override bool Equals(object obj)
		{
			if (obj is Color)
				return this == (Color)obj;
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>
		/// Converts color representation to a string
		/// </summary>
		/// <returns>A line with 4 components in [0...255] range</returns>
		public override string ToString()
		{
			return string.Format("{0} {1} {2} {3}", RByte, GByte, BByte, AByte);
		}
	}
}
