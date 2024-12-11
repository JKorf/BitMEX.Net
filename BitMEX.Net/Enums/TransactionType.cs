using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Transaction type
    /// </summary>
    public enum TransactionType
    {
        /// <summary>
        /// Deposit
        /// </summary>
        [Map("Deposit")]
        Deposit,
        /// <summary>
        /// Withdrawal
        /// </summary>
        [Map("Withdrawal")]
        Withdrawal
    }
}
