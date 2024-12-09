using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Tick direction 
    /// </summary>
    public enum TickDirection
    {
        /// <summary>
        /// Minus zero tick
        /// </summary>
        [Map("ZeroMinusTick")]
        ZeroMinusTick,
        /// <summary>
        /// Minus tick
        /// </summary>
        [Map("MinusTick")]
        MinusTick,
        /// <summary>
        /// Up zero tick
        /// </summary>
        [Map("ZeroPlusTick")]
        ZeroPlusTick,
        /// <summary>
        /// Up tick
        /// </summary>
        [Map("PlusTick")]
        PlusTick
    }
}
