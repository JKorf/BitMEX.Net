using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Fill ratio
    /// </summary>
    [SerializationModel]
    public record BitMEXFillRatio
    {
        /// <summary>
        /// ["<c>date</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>account</c>"] Account id
        /// </summary>
        [JsonPropertyName("account")]
        public long AccountId { get; set; }
        /// <summary>
        /// ["<c>quoteCount</c>"] Quote count
        /// </summary>
        [JsonPropertyName("quoteCount")]
        public long QuoteCount { get; set; }
        /// <summary>
        /// ["<c>dealtCount</c>"] Dealt count
        /// </summary>
        [JsonPropertyName("dealtCount")]
        public long DealtCount { get; set; }
        /// <summary>
        /// ["<c>quotesMavg7</c>"] Quotes moving average 7 days
        /// </summary>
        [JsonPropertyName("quotesMavg7")]
        public decimal QuotesMavg7d { get; set; }
        /// <summary>
        /// ["<c>dealtMavg7</c>"] Dealt moving average 7 days
        /// </summary>
        [JsonPropertyName("dealtMavg7")]
        public decimal DealtMavg7d { get; set; }
        /// <summary>
        /// ["<c>quoteFillRatioMavg7</c>"] Quote fill ratio moving average 7 days 
        /// </summary>
        [JsonPropertyName("quoteFillRatioMavg7")]
        public decimal QuoteFillRatioMavg7d { get; set; }
    }


}
