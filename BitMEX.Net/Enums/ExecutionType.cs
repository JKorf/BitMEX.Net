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
        Canceled,
        /// <summary>
        /// Funding
        /// </summary>
        [Map("Funding")]
        Funding,
        /// <summary>
        /// Rejected
        /// </summary>
        [Map("Rejected")]
        Rejected,
        /// <summary>
        /// Cancellation rejected
        /// </summary>
        [Map("CancelReject")]
        CancelRejected
    }
}
