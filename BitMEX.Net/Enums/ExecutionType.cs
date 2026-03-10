using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Execution type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<ExecutionType>))]
    public enum ExecutionType
    {
        /// <summary>
        /// ["<c>New</c>"] New
        /// </summary>
        [Map("New")]
        New,
        /// <summary>
        /// ["<c>Trade</c>"] Trade
        /// </summary>
        [Map("Trade")]
        Trade,
        /// <summary>
        /// ["<c>Canceled</c>"] Canceled
        /// </summary>
        [Map("Canceled")]
        Canceled,
        /// <summary>
        /// ["<c>Funding</c>"] Funding
        /// </summary>
        [Map("Funding")]
        Funding,
        /// <summary>
        /// ["<c>Rejected</c>"] Rejected
        /// </summary>
        [Map("Rejected")]
        Rejected,
        /// <summary>
        /// ["<c>CancelReject</c>"] Cancellation rejected
        /// </summary>
        [Map("CancelReject")]
        CancelRejected
    }
}
