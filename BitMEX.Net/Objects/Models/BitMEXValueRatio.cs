using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Value ratio
    /// </summary>
    [SerializationModel]
    public record BitMEXValueRatio
    {
        /// <summary>
        /// ["<c>timestamp</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>account</c>"] Account id
        /// </summary>
        [JsonPropertyName("account")]
        public decimal AccountId { get; set; }
        /// <summary>
        /// ["<c>symbol</c>"] Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>quoteCount</c>"] Quote count
        /// </summary>
        [JsonPropertyName("quoteCount")]
        public int QuoteCount { get; set; }
        /// <summary>
        /// ["<c>volumeXBT</c>"] Volume XBT
        /// </summary>
        [JsonPropertyName("volumeXBT")]
        public long VolumeXBT { get; set; }
        /// <summary>
        /// ["<c>QVR</c>"] Quote value ratio
        /// </summary>
        [JsonPropertyName("QVR")]
        public decimal QuoteValueRatio { get; set; }
        /// <summary>
        /// ["<c>id</c>"] Id
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }
    }


}
