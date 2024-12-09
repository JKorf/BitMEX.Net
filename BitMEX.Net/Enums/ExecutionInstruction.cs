using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Execution instructions
    /// </summary>
    public enum ExecutionInstruction
    {
        /// <summary>
        /// Post only order
        /// </summary>
        [Map("ParticipateDoNotInitiate")]
        PostOnly,
        /// <summary>
        /// All or none
        /// </summary>
        [Map("AllOrNone")]
        AllOrNone,
        /// <summary>
        /// Mark price as trigger price type for Stop and If-Touched orders
        /// </summary>
        [Map("MarkPrice")]
        MarkPrice,
        /// <summary>
        /// Index price as trigger price type for Stop and If-Touched orders
        /// </summary>
        [Map("IndexPrice")]
        IndexPrice,
        /// <summary>
        /// Last price as trigger price type for Stop and If-Touched orders
        /// </summary>
        [Map("LastPrice")]
        LastPrice,
        /// <summary>
        /// Close implies ReduceOnly. A Close order will cancel other active limit orders with the same side and symbol if the open quantity exceeds the current position
        /// </summary>
        [Map("Close")]
        Close,
        /// <summary>
        /// Reduce only
        /// </summary>
        [Map("ReduceOnly")]
        ReduceOnly,
        /// <summary>
        /// Pegged orders must have an execInst of Fixed
        /// </summary>
        [Map("Fixed")]
        Fixed,
        /// <summary>
        /// Last within mark
        /// </summary>
        [Map("LastWithinMark")]
        LastWithinMark
    }
}
