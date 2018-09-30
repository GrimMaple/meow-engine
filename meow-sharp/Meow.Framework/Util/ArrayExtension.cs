namespace Meow.Framework.Util
{
    /// <summary>
    /// Helper class for arrays
    /// </summary>
    public static class ArrayExtension
    {
        /// <summary>
        /// Creates a copy of array
        /// </summary>
        /// <typeparam name="T">Array type</typeparam>
        /// <param name="array">Source array</param>
        /// <param name="offset">Offset in the source array</param>
        /// <param name="count">Number of elements to copy</param>
        /// <returns>New subarray</returns>
        public static T[] Copy<T>(this T[] array, int offset, int count)
        {
            T[] ret = new T[count];
            for (int i = 0; i < count; i++)
                ret[i] = array[offset + i];

            return ret;
        }
    }
}
