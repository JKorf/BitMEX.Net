using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Event status
    /// </summary>
    public enum EventStatus
    {
        /// <summary>
        /// Success
        /// </summary>
        [Map("success")]
        Success,
        /// <summary>
        /// Failure
        /// </summary>
        [Map("failure")]
        Failure
    }
}
