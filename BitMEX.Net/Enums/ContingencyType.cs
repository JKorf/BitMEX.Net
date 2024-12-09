using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Contingency type
    /// </summary>
    public enum ContingencyType
    {
        /// <summary>
        /// One cancels other order linked with clientOrderLinkId
        /// </summary>
        [Map("OneCancelsTheOther")]
        OneCancelsTheOther,
        /// <summary>
        /// One triggers other order linked with clientOrderLinkId
        /// </summary>
        [Map("OneTriggersTheOther")]
        OneTriggersTheOther
    }
}
