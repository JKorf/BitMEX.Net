using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Composite index info
    /// </summary>
    [SerializationModel]
    public record BitMEXCompositeIndex
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
        /// ["<c>indexSymbol</c>"] Index symbol
        /// </summary>
        [JsonPropertyName("indexSymbol")]
        public string IndexSymbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>indexMultiplier</c>"] Index multiplier
        /// </summary>
        [JsonPropertyName("indexMultiplier")]
        public decimal? IndexMultiplier { get; set; }
        /// <summary>
        /// ["<c>reference</c>"] Reference
        /// </summary>
        [JsonPropertyName("reference")]
        public string Reference { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>lastPrice</c>"] Last price
        /// </summary>
        [JsonPropertyName("lastPrice")]
        public decimal LastPrice { get; set; }
        /// <summary>
        /// ["<c>sourcePrice</c>"] Source price
        /// </summary>
        [JsonPropertyName("sourcePrice")]
        public decimal? SourcePrice { get; set; }
        /// <summary>
        /// ["<c>conversionIndex</c>"] Conversion index
        /// </summary>
        [JsonPropertyName("conversionIndex")]
        public string? ConversionIndex { get; set; }
        /// <summary>
        /// ["<c>conversionIndexPrice</c>"] Conversion index price
        /// </summary>
        [JsonPropertyName("conversionIndexPrice")]
        public decimal? ConversionIndexPrice { get; set; }
        /// <summary>
        /// ["<c>weight</c>"] Weight
        /// </summary>
        [JsonPropertyName("weight")]
        public decimal Weight { get; set; }
        /// <summary>
        /// ["<c>logged</c>"] Log time
        /// </summary>
        [JsonPropertyName("logged")]
        public DateTime LogTime { get; set; }
    }


}
