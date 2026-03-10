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
        /// ["<c>Completed</c>"] Completed
        /// </summary>
        [Map("Completed")]
        Completed,
        /// <summary>
        /// ["<c>Canceled</c>"] Canceled
        /// </summary>
        [Map("Canceled")]
        Canceled
    }
}
