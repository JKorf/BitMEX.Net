﻿using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Order type
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// Limit order
        /// </summary>
        [Map("Limit")]
        Limit,
        /// <summary>
        /// Market order
        /// </summary>
        [Map("Market")]
        Market,
        /// <summary>
        /// Market stop order
        /// </summary>
        [Map("Stop")]
        StopMarket,
        /// <summary>
        /// Limit stop order
        /// </summary>
        [Map("StopLimit")]
        StopLimit,
        /// <summary>
        /// Similar to a Stop, but triggers are done in the opposite direction. Useful for Take Profit Market orders.
        /// </summary>
        [Map("MarketIfTouched")]
        MarketIfTouched,
        /// <summary>
        /// Similar to a Stop, but triggers are done in the opposite direction. Useful for Take Profit Limit orders.
        /// </summary>
        [Map("LimitIfTouched")]
        LimitIfTouched,
        /// <summary>
        /// Pegged orders allow users to submit a limit price relative to the current market price
        /// </summary>
        [Map("Pegged")]
        Pegged
    }
}
