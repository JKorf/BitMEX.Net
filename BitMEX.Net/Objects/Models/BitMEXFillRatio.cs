using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Fill ratio
    /// </summary>
    public record BitMEXFillRatio
    {
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Account id
        /// </summary>
        [JsonPropertyName("account")]
        public decimal AccountId { get; set; }
        /// <summary>
        /// Quote count
        /// </summary>
        [JsonPropertyName("quoteCount")]
        public decimal QuoteCount { get; set; }
        /// <summary>
        /// Dealt count
        /// </summary>
        [JsonPropertyName("dealtCount")]
        public decimal DealtCount { get; set; }
        /// <summary>
        /// Quotes moving average 7 days
        /// </summary>
        [JsonPropertyName("quotesMavg7")]
        public decimal QuotesMavg7d { get; set; }
        /// <summary>
        /// Dealt moving average 7 days
        /// </summary>
        [JsonPropertyName("dealtMavg7")]
        public decimal DealtMavg7d { get; set; }
        /// <summary>
        /// Quote fill ratio moving average 7 days 
        /// </summary>
        [JsonPropertyName("quoteFillRatioMavg7")]
        public decimal QuoteFillRatioMavg7d { get; set; }
    }


}
