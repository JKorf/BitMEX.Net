using System;

namespace BitMEX.Net.ExtensionMethods
{
    /// <summary>
    /// Extension methods specific to using the BitMEX API
    /// </summary>
    public static class BitMEXExtensionMethods
    {
        /// <summary>
        /// Convert a BitMEX quantity to a normalized quantity
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="scale">Scale</param>
        /// <returns></returns>
        public static decimal ToSharedQuantity(this long value, int scale)
        {
            if (scale == 1)
                return value;

            return value / (decimal)Math.Pow(10, scale);
        }

        /// <summary>
        /// Convert a BitMEX quantity to a normalized quantity
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="scale">Scale</param>
        /// <returns></returns>
        public static decimal? ToSharedQuantity(this long? value, int scale)
        {
            if (value == null)
                return null;

            if (scale == 1)
                return value;

            return value / (decimal)Math.Pow(10, scale);
        }

        /// <summary>
        /// Convert a normalized quantity to a BitMEX quantity
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="scale">Scale</param>
        /// <returns></returns>
        public static long ToBitMEXQuantity(this decimal value, int scale)
        {
            if (scale == 1)
                return (long)value;

            return (long)(value * (decimal)Math.Pow(10, scale));
        }

        /// <summary>
        /// Convert a normalized quantity to a BitMEX quantity
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="scale">Scale</param>
        /// <returns></returns>
        public static long? ToBitMEXQuantity(this decimal? value, int scale)
        {
            if (value == null)
                return null;

            if (scale == 1)
                return (long)value;

            return (long)(value * (decimal)Math.Pow(10, scale));
        }
    }
}
