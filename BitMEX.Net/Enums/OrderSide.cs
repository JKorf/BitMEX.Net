using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Order side
    /// </summary>
    public enum OrderSide
    {
        /// <summary>
        /// Buy
        /// </summary>
        [Map("Buy")]
        Buy,
        /// <summary>
        /// Sell
        /// </summary>
        [Map("Sell")]
        Sell
    }
}
