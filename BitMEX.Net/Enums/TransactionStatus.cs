using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Transaction status
    /// </summary>
    public enum TransactionStatus
    {
        /// <summary>
        /// Completed
        /// </summary>
        [Map("Completed")]
        Completed,
        /// <summary>
        /// Canceled
        /// </summary>
        [Map("Canceled")]
        Canceled
    }
}
