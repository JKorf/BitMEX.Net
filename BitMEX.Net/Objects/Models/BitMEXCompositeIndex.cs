using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Composite index info
    /// </summary>
    public record BitMEXCompositeIndex
    {
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Index symbol
        /// </summary>
        [JsonPropertyName("indexSymbol")]
        public string IndexSymbol { get; set; } = string.Empty;
        /// <summary>
        /// Index multiplier
        /// </summary>
        [JsonPropertyName("indexMultiplier")]
        public decimal? IndexMultiplier { get; set; }
        /// <summary>
        /// Reference
        /// </summary>
        [JsonPropertyName("reference")]
        public string Reference { get; set; } = string.Empty;
        /// <summary>
        /// Last price
        /// </summary>
        [JsonPropertyName("lastPrice")]
        public decimal LastPrice { get; set; }
        /// <summary>
        /// Source price
        /// </summary>
        [JsonPropertyName("sourcePrice")]
        public decimal? SourcePrice { get; set; }
        /// <summary>
        /// Conversion index
        /// </summary>
        [JsonPropertyName("conversionIndex")]
        public string? ConversionIndex { get; set; }
        /// <summary>
        /// Conversion index price
        /// </summary>
        [JsonPropertyName("conversionIndexPrice")]
        public decimal? ConversionIndexPrice { get; set; }
        /// <summary>
        /// Weight
        /// </summary>
        [JsonPropertyName("weight")]
        public decimal Weight { get; set; }
        /// <summary>
        /// Log time
        /// </summary>
        [JsonPropertyName("logged")]
        public DateTime LogTime { get; set; }
    }


}
