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
        /// <param name="asset">The asset in which the quantity is</param>
        /// <returns></returns>
        public static decimal ToSharedAssetQuantity(this long value, string asset)
        {
            var scale = BitMEXUtils.GetAssetScale(asset);
            if (scale == 1)
                return value;

            return value / (decimal)Math.Pow(10, scale);
        }

        /// <summary>
        /// Convert a BitMEX quantity to a normalized quantity
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="symbol">The symbol for which the quantity is</param>
        /// <returns></returns>
        public static decimal ToSharedSymbolQuantity(this long value, string symbol)
        {
            var scale = BitMEXUtils.GetSymbolQuantityScale(symbol);
            if (scale == 1)
                return value;

            return value / (decimal)Math.Pow(10, scale);
        }

        /// <summary>
        /// Convert a BitMEX quantity to a normalized quantity
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="asset">The asset in which the quantity is</param>
        /// <returns></returns>
        public static decimal? ToSharedAssetQuantity(this long? value, string asset)
        {
            if (value == null)
                return null;

            var scale = BitMEXUtils.GetAssetScale(asset);
            if (scale == 1)
                return value;

            return value / (decimal)Math.Pow(10, scale);
        }

        /// <summary>
        /// Convert a BitMEX quantity to a normalized quantity
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="symbol">The symbol for which the quantity is</param>
        /// <returns></returns>
        public static decimal? ToSharedSymbolQuantity(this long? value, string symbol)
        {
            if (value == null)
                return null;

            var scale = BitMEXUtils.GetSymbolQuantityScale(symbol);
            if (scale == 1)
                return value;

            return value / (decimal)Math.Pow(10, scale);
        }

        /// <summary>
        /// Convert a normalized quantity to a BitMEX quantity
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="asset">The asset which the quantity is in</param>
        /// <returns></returns>
        public static long ToBitMEXAssetQuantity(this decimal value, string asset)
        {
            var scale = BitMEXUtils.GetAssetScale(asset);
            if (scale == 1)
                return (long)value;

            return (long)(value * (decimal)Math.Pow(10, scale));
        }

        /// <summary>
        /// Convert a normalized quantity to a BitMEX quantity
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="asset">The asset which the quantity is in</param>
        /// <returns></returns>
        public static long? ToBitMEXAssetQuantity(this decimal? value, string asset)
        {
            if (value == null)
                return null;

            var scale = BitMEXUtils.GetAssetScale(asset);
            if (scale == 1)
                return (long)value;

            return (long)(value * (decimal)Math.Pow(10, scale));
        }

        /// <summary>
        /// Convert a normalized quantity to a BitMEX quantity
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="symbol">The symbol for which the quantity is</param>
        /// <returns></returns>
        public static long ToBitMEXSymbolQuantity(this decimal value, string symbol)
        {
            var scale = BitMEXUtils.GetSymbolQuantityScale(symbol);
            if (scale == 1)
                return (long)value;

            return (long)(value * (decimal)Math.Pow(10, scale));
        }

        /// <summary>
        /// Convert a normalized quantity to a BitMEX quantity
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="symbol">The symbol for which the quantity is</param>
        /// <returns></returns>
        public static long? ToBitMEXSymbolQuantity(this decimal? value, string symbol)
        {
            if (value == null)
                return null;

            var scale = BitMEXUtils.GetSymbolQuantityScale(symbol);
            if (scale == 1)
                return (long)value;

            return (long)(value * (decimal)Math.Pow(10, scale));
        }
    }
}
