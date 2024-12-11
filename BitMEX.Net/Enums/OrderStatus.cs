﻿using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Order status
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// New order
        /// </summary>
        [Map("new")]
        New,
        /// <summary>
        /// Filled
        /// </summary>
#warning check, is there also a partial fill?
        [Map("filled")]
        Filled,
        /// <summary>
        /// Canceled
        /// </summary>
        [Map("Canceled")]
        Canceled,
        /// <summary>
        /// Rejected
        /// </summary>
        [Map("Rejected")]
        Rejected
    }
}
