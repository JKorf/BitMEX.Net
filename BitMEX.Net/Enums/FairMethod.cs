using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Fair method
    /// </summary>
    public enum FairMethod
    {
        /// <summary>
        /// Funding rate
        /// </summary>
        [Map("FundingRate")]
        FundingRate,
        /// <summary>
        /// Impact mid price
        /// </summary>
        [Map("ImpactMidPrice")]
        ImpactMidPrice
    }
}
