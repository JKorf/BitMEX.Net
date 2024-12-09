using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Time in force
    /// </summary>
    public enum TimeInForce
    {
        /// <summary>
        /// Current day
        /// </summary>
        [Map("Day")]
        Day,
        /// <summary>
        /// Good until canceled
        /// </summary>
        [Map("GoodTillCancel")]
        GoodTillCancel,
        /// <summary>
        /// Immediate or cancel
        /// </summary>
        [Map("ImmediateOrCancel")]
        ImmediateOrCancel,
        /// <summary>
        /// Fill or kill
        /// </summary>
        [Map("FillOrKill")]
        FillOrKill
    }
}
