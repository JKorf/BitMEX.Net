using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Asset type
    /// </summary>
    public enum AssetType
    {
        /// <summary>
        /// Crypto asset
        /// </summary>
        [Map("Crypto")]
        Crypto,
        /// <summary>
        /// Synthetic asset
        /// </summary>
        [Map("Synthetic")]
        Synthetic
    }
}
