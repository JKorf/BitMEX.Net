using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Symbol filter
    /// </summary>
    public enum SymbolFilter
    {
        /// <summary>
        /// Nearest expiring contract
        /// </summary>
        [Map("nearest")]
        Nearest,
        /// <summary>
        /// Daily
        /// </summary>
        [Map("daily")]
        Daily,
        /// <summary>
        /// Weekly
        /// </summary>
        [Map("weekly")]
        Weekly,
        /// <summary>
        /// Monthly
        /// </summary>
        [Map("monthly")]
        Monthly,
        /// <summary>
        /// Quarterly
        /// </summary>
        [Map("quarterly")]
        Quarterly,
        /// <summary>
        /// Bi-quarterly
        /// </summary>
        [Map("biquarterly")]
        BiQuarterly,
        /// <summary>
        /// Perpetual
        /// </summary>
        [Map("perpetual")]
        Perpetual
    }
}
