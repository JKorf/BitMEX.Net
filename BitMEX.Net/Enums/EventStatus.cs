using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Event status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<EventStatus>))]
    public enum EventStatus
    {
        /// <summary>
        /// ["<c>success</c>"] Success
        /// </summary>
        [Map("success")]
        Success,
        /// <summary>
        /// ["<c>failure</c>"] Failure
        /// </summary>
        [Map("failure")]
        Failure
    }
}
