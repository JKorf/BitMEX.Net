using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Transaction status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TransactionStatus>))]
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
