using CryptoExchange.Net.Converters.SystemTextJson;
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
        /// ["<c>symbol</c>"] Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>currency</c>"] Base asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string BaseAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>turnover24h</c>"] Turnover 24 hours
        /// </summary>
        [JsonPropertyName("turnover24h")]
        public long Turnover24h { get; set; }
        /// <summary>
        /// ["<c>turnover7d</c>"] Turnover 7 days
        /// </summary>
        [JsonPropertyName("turnover7d")]
        public long Turnover7d { get; set; }
        /// <summary>
        /// ["<c>turnover30d</c>"] Turnover 30 days
        /// </summary>
        [JsonPropertyName("turnover30d")]
        public long Turnover30d { get; set; }
        /// <summary>
        /// ["<c>turnoverYTD</c>"] Turnover year to date
        /// </summary>
        [JsonPropertyName("turnoverYTD")]
        public long TurnoverYTD { get; set; }
        /// <summary>
        /// ["<c>turnover365d</c>"] Turnover 1 year
        /// </summary>
        [JsonPropertyName("turnover365d")]
        public long Turnover365d { get; set; }
        /// <summary>
        /// ["<c>turnover</c>"] Turnover
        /// </summary>
        [JsonPropertyName("turnover")]
        public long Turnover { get; set; }
        /// <summary>
        /// ["<c>price24h</c>"] Price 24 hours
        /// </summary>
        [JsonPropertyName("price24h")]
        public decimal Price24h { get; set; }
        /// <summary>
        /// ["<c>price7d</c>"] Price 7 days
        /// </summary>
        [JsonPropertyName("price7d")]
        public decimal Price7d { get; set; }
        /// <summary>
        /// ["<c>price30d</c>"] Price 30 days
        /// </summary>
        [JsonPropertyName("price30d")]
        public decimal Price30d { get; set; }
        /// <summary>
        /// ["<c>priceYTD</c>"] Price year to date
        /// </summary>
        [JsonPropertyName("priceYTD")]
        public decimal PriceYTD { get; set; }
        /// <summary>
        /// ["<c>price365d</c>"] Price 1 year
        /// </summary>
        [JsonPropertyName("price365d")]
        public decimal Price365d { get; set; }
        /// <summary>
        /// ["<c>lastPrice</c>"] Last price
        /// </summary>
        [JsonPropertyName("lastPrice")]
        public decimal LastPrice { get; set; }
    }


}
