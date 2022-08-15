namespace BetterTouchpadBehavior.Extensions
{
    public static class IntPtrExtensions
    {
        /// <summary>
        /// Gets high bits values of the pointer.
        /// </summary>
        public static int GetHiword(this IntPtr ptr)
        {
            var val = Environment.Is64BitProcess ? (int)ptr.ToInt64() : ptr.ToInt32();
            return ((val >> 16) & 0xFFFF);
        }

        /// <summary>
        /// Gets low bits values of the pointer.
        /// </summary>
        public static long GetLoword(this IntPtr ptr)
        {
            var val = Environment.Is64BitProcess ? (int)ptr.ToInt64() : ptr.ToInt32();

            return (val & 0xFFFF);
        }
    }
}
