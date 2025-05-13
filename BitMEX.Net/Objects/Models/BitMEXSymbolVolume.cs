using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Volume info
    /// </summary>
    [SerializationModel]
    public record BitMEXSymbolVolume
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Base asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string BaseAsset { get; set; } = string.Empty;
        /// <summary>
        /// Turnover 24 hours
        /// </summary>
        [JsonPropertyName("turnover24h")]
        public long Turnover24h { get; set; }
        /// <summary>
        /// Turnover 7 days
        /// </summary>
        [JsonPropertyName("turnover7d")]
        public long Turnover7d { get; set; }
        /// <summary>
        /// Turnover 30 days
        /// </summary>
        [JsonPropertyName("turnover30d")]
        public long Turnover30d { get; set; }
        /// <summary>
        /// Turnover year to date
        /// </summary>
        [JsonPropertyName("turnoverYTD")]
        public long TurnoverYTD { get; set; }
        /// <summary>
        /// Turnover 1 year
        /// </summary>
        [JsonPropertyName("turnover365d")]
        public long Turnover365d { get; set; }
        /// <summary>
        /// Turnover
        /// </summary>
        [JsonPropertyName("turnover")]
        public long Turnover { get; set; }
        /// <summary>
        /// Price 24 hours
        /// </summary>
        [JsonPropertyName("price24h")]
        public decimal Price24h { get; set; }
        /// <summary>
        /// Price 7 days
        /// </summary>
        [JsonPropertyName("price7d")]
        public decimal Price7d { get; set; }
        /// <summary>
        /// Price 30 days
        /// </summary>
        [JsonPropertyName("price30d")]
        public decimal Price30d { get; set; }
        /// <summary>
        /// Price year to date
        /// </summary>
        [JsonPropertyName("priceYTD")]
        public decimal PriceYTD { get; set; }
        /// <summary>
        /// Price 1 year
        /// </summary>
        [JsonPropertyName("price365d")]
        public decimal Price365d { get; set; }
        /// <summary>
        /// Last price
        /// </summary>
        [JsonPropertyName("lastPrice")]
        public decimal LastPrice { get; set; }
    }


}
