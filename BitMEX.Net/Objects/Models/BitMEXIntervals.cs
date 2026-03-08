using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Interval
    /// </summary>
    [SerializationModel]
    public record BitMEXIntervals
    {
        /// <summary>
        /// ["<c>intervals</c>"] Intervals
        /// </summary>
        [JsonPropertyName("intervals")]
        public string[] Intervals { get; set; } = [];
        /// <summary>
        /// ["<c>symbols</c>"] Symbols
        /// </summary>
        [JsonPropertyName("symbols")]
        public string[] Symbols { get; set; } = [];
    }
}
