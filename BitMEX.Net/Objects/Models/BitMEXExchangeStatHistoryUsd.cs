using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Exchange stat history USD
    /// </summary>
    [SerializationModel]
    public record BitMEXExchangeStatHistoryUsd
    {
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
        /// ["<c>turnover24h</c>"] Turnover 24 hours
        /// </summary>
        [JsonPropertyName("turnover24h")]
        public long Turnover24h { get; set; }
        /// <summary>
        /// ["<c>turnover30d</c>"] Turnover 30 days
        /// </summary>
        [JsonPropertyName("turnover30d")]
        public long Turnover30d { get; set; }
        /// <summary>
        /// ["<c>turnover365d</c>"] Turnover 1 year
        /// </summary>
        [JsonPropertyName("turnover365d")]
        public long Turnover1y { get; set; }
        /// <summary>
        /// ["<c>turnover</c>"] Turnover
        /// </summary>
        [JsonPropertyName("turnover")]
        public long Turnover { get; set; }
    }


}
