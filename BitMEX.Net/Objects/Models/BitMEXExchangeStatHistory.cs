using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Exchange stat history
    /// </summary>
    public record BitMEXExchangeStatHistory
    {
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Root symbol
        /// </summary>
        [JsonPropertyName("rootSymbol")]
        public string RootSymbol { get; set; } = string.Empty;
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string? Asset { get; set; }
        /// <summary>
        /// Volume
        /// </summary>
        [JsonPropertyName("volume")]
        public decimal Volume { get; set; }
        /// <summary>
        /// Turnover
        /// </summary>
        [JsonPropertyName("turnover")]
        public decimal Turnover { get; set; }
    }


}
