using CryptoExchange.Net.Converters.SystemTextJson;
using BitMEX.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Settlement info
    /// </summary>
    [SerializationModel]
    public record BitMEXSettlementHistory
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
        /// ["<c>settlementType</c>"] Settlement type
        /// </summary>
        [JsonPropertyName("settlementType")]
        public SettlementType SettlementType { get; set; }
        /// <summary>
        /// ["<c>settledPrice</c>"] Settled price
        /// </summary>
        [JsonPropertyName("settledPrice")]
        public decimal SettledPrice { get; set; }
    }


}
