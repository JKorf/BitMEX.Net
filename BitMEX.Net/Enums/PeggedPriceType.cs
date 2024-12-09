using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Peg price type
    /// </summary>
    public enum PeggedPriceType
    {
        /// <summary>
        /// Market peg
        /// </summary>
        [Map("MarketPeg")]
        MarketPeg,
        /// <summary>
        /// Primary peg
        /// </summary>
        [Map("PrimaryPeg")]
        PrimaryPeg,
        /// <summary>
        /// Trailing stop peg
        /// </summary>
        [Map("TrailingStopPeg")]
        TrailingStopPeg
    }
}
