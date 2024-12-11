using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Execution type
    /// </summary>
    public enum ExecutionType
    {
        /// <summary>
        /// New
        /// </summary>
        [Map("New")]
        New,
        /// <summary>
        /// Trade
        /// </summary>
        [Map("Trade")]
        Trade,
        /// <summary>
        /// Canceled
        /// </summary>
        [Map("Canceled")]
        Canceled
    }
}
