using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Value ratio
    /// </summary>
    public record BitMEXValueRatio
    {
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Account id
        /// </summary>
        [JsonPropertyName("account")]
        public decimal AccountId { get; set; }
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Quote count
        /// </summary>
        [JsonPropertyName("quoteCount")]
        public int QuoteCount { get; set; }
        /// <summary>
        /// Volume XBT
        /// </summary>
        [JsonPropertyName("volumeXBT")]
        public long VolumeXBT { get; set; }
        /// <summary>
        /// Quote value ratio
        /// </summary>
        [JsonPropertyName("QVR")]
        public decimal QuoteValueRatio { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }
    }


}
