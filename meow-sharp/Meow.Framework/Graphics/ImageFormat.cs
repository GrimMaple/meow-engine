namespace Meow.Framework.Graphics
{
    /// <summary>
    /// Image storage types
    /// </summary>
    public enum ImageFormat : uint
    {
        /// <summary>
        /// 32 bits, 0xRRGGBBAA
        /// </summary>
        RGBA = 4, 

        /// <summary>
        /// 24 bits, 0xRRGGBB
        /// </summary>
        RGB = 3,

        /// <summary>
        /// 8 bits, 0xRR
        /// </summary>
        R = 1
    }
}
