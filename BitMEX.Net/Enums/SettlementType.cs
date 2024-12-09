using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Settlement type
    /// </summary>
    public enum SettlementType
    {
        /// <summary>
        /// Settlement
        /// </summary>
        [Map("settlement")]
        Settlement
    }
}
