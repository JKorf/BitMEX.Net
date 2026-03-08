using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Book ticker
    /// </summary>
    [SerializationModel]
    public record BitMEXBookTicker
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
        /// ["<c>bidSize</c>"] Best bid quantity
        /// </summary>
        [JsonPropertyName("bidSize")]
        public long BestBidQuantity { get; set; }
        /// <summary>
        /// ["<c>bidPrice</c>"] Best bid price
        /// </summary>
        [JsonPropertyName("bidPrice")]
        public decimal BestBidPrice { get; set; }
        /// <summary>
        /// ["<c>askPrice</c>"] Best ask price
        /// </summary>
        [JsonPropertyName("askPrice")]
        public decimal BestAskPrice { get; set; }
        /// <summary>
        /// ["<c>askSize</c>"] Best ask quantity
        /// </summary>
        [JsonPropertyName("askSize")]
        public long BestAskQuantity { get; set; }
    }


}
