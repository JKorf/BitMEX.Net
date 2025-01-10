using BitMEX.Net.Converter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Funding rate
    /// </summary>
    public record BitMEXFundingRate
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
        /// Funding interval
        /// </summary>
        [JsonPropertyName("fundingInterval")]
        [JsonConverter(typeof(IntervalConverter))]
        public TimeSpan FundingInterval { get; set; }

        /// <summary>
        /// Funding rate
        /// </summary>
        [JsonPropertyName("fundingRate")]
        public decimal FundingRate { get; set; }

        /// <summary>
        /// Funding rate daily
        /// </summary>
        [JsonPropertyName("fundingRateDaily")]
        public decimal FundingRateDaily { get; set; }
    }
}
