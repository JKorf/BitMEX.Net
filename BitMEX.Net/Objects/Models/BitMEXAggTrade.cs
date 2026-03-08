using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Aggregated trade info
    /// </summary>
    [SerializationModel]
    public record BitMEXAggTrade
    {
        /// <summary>
        /// ["<c>timestamp</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>symbol</c>"] Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>open</c>"] Open price
        /// </summary>
        [JsonPropertyName("open")]
        public decimal OpenPrice { get; set; }
        /// <summary>
        /// ["<c>high</c>"] High price
        /// </summary>
        [JsonPropertyName("high")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// ["<c>low</c>"] Low price
        /// </summary>
        [JsonPropertyName("low")]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// ["<c>close</c>"] Close price
        /// </summary>
        [JsonPropertyName("close")]
        public decimal ClosePrice { get; set; }
        /// <summary>
        /// ["<c>trades</c>"] Number of trades
        /// </summary>
        [JsonPropertyName("trades")]
        public decimal Trades { get; set; }
        /// <summary>
        /// ["<c>volume</c>"] Volume
        /// </summary>
        [JsonPropertyName("volume")]
        public long Volume { get; set; }
        /// <summary>
        /// ["<c>lastSize</c>"] Last quantity
        /// </summary>
        [JsonPropertyName("lastSize")]
        public long LastQuantity { get; set; }
        /// <summary>
        /// ["<c>vwap</c>"] Volume weighted average price
        /// </summary>
        [JsonPropertyName("vwap")]
        public decimal? VolumeWeightedAveragePrice { get; set; }
        /// <summary>
        /// ["<c>turnover</c>"] Turnover
        /// </summary>
        [JsonPropertyName("turnover")]
        public decimal Turnover { get; set; }
        /// <summary>
        /// ["<c>homeNotional</c>"] Home notional
        /// </summary>
        [JsonPropertyName("homeNotional")]
        public decimal HomeNotional { get; set; }
        /// <summary>
        /// ["<c>foreignNotional</c>"] Foreign notional
        /// </summary>
        [JsonPropertyName("foreignNotional")]
        public decimal ForeignNotional { get; set; }
    }


}
