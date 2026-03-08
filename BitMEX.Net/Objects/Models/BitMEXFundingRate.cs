using CryptoExchange.Net.Converters.SystemTextJson;
using BitMEX.Net.Converters;
using System;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Funding rate
    /// </summary>
    [SerializationModel]
    public record BitMEXFundingRate
    {
        /// <summary>
        /// ["<c>timestamp</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// ["<c>symbol</c>"] Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>fundingInterval</c>"] Funding interval
        /// </summary>
        [JsonPropertyName("fundingInterval")]
        [JsonConverter(typeof(IntervalConverter))]
        public TimeSpan FundingInterval { get; set; }

        /// <summary>
        /// ["<c>fundingRate</c>"] Funding rate
        /// </summary>
        [JsonPropertyName("fundingRate")]
        public decimal FundingRate { get; set; }

        /// <summary>
        /// ["<c>fundingRateDaily</c>"] Funding rate daily
        /// </summary>
        [JsonPropertyName("fundingRateDaily")]
        public decimal FundingRateDaily { get; set; }
    }
}
