using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Exchange stat history
    /// </summary>
    [SerializationModel]
    public record BitMEXExchangeStatHistory
    {
        /// <summary>
        /// ["<c>date</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>rootSymbol</c>"] Root symbol
        /// </summary>
        [JsonPropertyName("rootSymbol")]
        public string RootSymbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>currency</c>"] Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string? Asset { get; set; }
        /// <summary>
        /// ["<c>volume</c>"] Volume
        /// </summary>
        [JsonPropertyName("volume")]
        public long Volume { get; set; }
        /// <summary>
        /// ["<c>turnover</c>"] Turnover
        /// </summary>
        [JsonPropertyName("turnover")]
        public long Turnover { get; set; }
    }


}
