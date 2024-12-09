using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Exchange stat history USD
    /// </summary>
    public record BitMEXExchangeStatHistoryUsd
    {
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
        /// Turnover 24 hours
        /// </summary>
        [JsonPropertyName("turnover24h")]
        public decimal Turnover24h { get; set; }
        /// <summary>
        /// Turnover 30 days
        /// </summary>
        [JsonPropertyName("turnover30d")]
        public decimal Turnover30d { get; set; }
        /// <summary>
        /// Turnover 1 year
        /// </summary>
        [JsonPropertyName("turnover365d")]
        public decimal Turnover1y { get; set; }
        /// <summary>
        /// Turnover
        /// </summary>
        [JsonPropertyName("turnover")]
        public decimal Turnover { get; set; }
    }


}
