using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Mark method
    /// </summary>
    public enum MarkMethod
    {
        /// <summary>
        /// Fair price
        /// </summary>
        [Map("FairPrice")]
        FairPrice,
        /// <summary>
        /// Last price
        /// </summary>
        [Map("LastPrice")]
        LastPrice,
        /// <summary>
        /// Last price protected
        /// </summary>
        [Map("LastPriceProtected")]
        LastPriceProtected,
        /// <summary>
        /// Indicative settle price
        /// </summary>
        [Map("IndicativeSettlePrice")]
        IndicativeSettlePrice
    }
}
