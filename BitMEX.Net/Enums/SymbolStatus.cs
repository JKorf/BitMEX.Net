using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Symbol status
    /// </summary>
    public enum SymbolStatus
    {
        /// <summary>
        /// Unlisted
        /// </summary>
        [Map("Unlisted")]
        Unlisted,
        /// <summary>
        /// Open
        /// </summary>
        [Map("Open")]
        Open,
        /// <summary>
        /// Closed
        /// </summary>
        [Map("Closed")]
        Closed,
        /// <summary>
        /// Settled
        /// </summary>
        [Map("Settled")]
        Settled
    }
}
