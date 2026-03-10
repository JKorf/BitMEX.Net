using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Execution instructions
    /// </summary>
    [JsonConverter(typeof(EnumConverter<ExecutionInstruction>))]
    public enum ExecutionInstruction
    {
        /// <summary>
        /// ["<c>ParticipateDoNotInitiate</c>"] Post only order
        /// </summary>
        [Map("ParticipateDoNotInitiate")]
        PostOnly,
        /// <summary>
        /// ["<c>AllOrNone</c>"] All or none
        /// </summary>
        [Map("AllOrNone")]
        AllOrNone,
        /// <summary>
        /// ["<c>MarkPrice</c>"] Mark price as trigger price type for Stop and If-Touched orders
        /// </summary>
        [Map("MarkPrice")]
        MarkPrice,
        /// <summary>
        /// ["<c>IndexPrice</c>"] Index price as trigger price type for Stop and If-Touched orders
        /// </summary>
        [Map("IndexPrice")]
        IndexPrice,
        /// <summary>
        /// ["<c>LastPrice</c>"] Last price as trigger price type for Stop and If-Touched orders
        /// </summary>
        [Map("LastPrice")]
        LastPrice,
        /// <summary>
        /// ["<c>Close</c>"] Close implies ReduceOnly. A Close order will cancel other active limit orders with the same side and symbol if the open quantity exceeds the current position
        /// </summary>
        [Map("Close")]
        Close,
        /// <summary>
        /// ["<c>ReduceOnly</c>"] Reduce only
        /// </summary>
        [Map("ReduceOnly")]
        ReduceOnly,
        /// <summary>
        /// ["<c>Fixed</c>"] Pegged orders must have an execInst of Fixed
        /// </summary>
        [Map("Fixed")]
        Fixed,
        /// <summary>
        /// ["<c>LastWithinMark</c>"] Last within mark
        /// </summary>
        [Map("LastWithinMark")]
        LastWithinMark
    }
}
