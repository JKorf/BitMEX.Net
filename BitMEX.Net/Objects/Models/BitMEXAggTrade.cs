using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Aggregated trade info
    /// </summary>
    public record BitMEXAggTrade
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
        /// Open price
        /// </summary>
        [JsonPropertyName("open")]
        public decimal OpenPrice { get; set; }
        /// <summary>
        /// High price
        /// </summary>
        [JsonPropertyName("high")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// Low price
        /// </summary>
        [JsonPropertyName("low")]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// Close price
        /// </summary>
        [JsonPropertyName("close")]
        public decimal ClosePrice { get; set; }
        /// <summary>
        /// Number of trades
        /// </summary>
        [JsonPropertyName("trades")]
        public decimal Trades { get; set; }
        /// <summary>
        /// Volume
        /// </summary>
        [JsonPropertyName("volume")]
        public long Volume { get; set; }
        /// <summary>
        /// Last quantity
        /// </summary>
        [JsonPropertyName("lastSize")]
        public long LastQuantity { get; set; }
        /// <summary>
        /// Volume weighted average price
        /// </summary>
        [JsonPropertyName("vwap")]
        public decimal? VolumeWeightedAveragePrice { get; set; }
        /// <summary>
        /// Turnover
        /// </summary>
        [JsonPropertyName("turnover")]
        public decimal Turnover { get; set; }
        /// <summary>
        /// Home notional
        /// </summary>
        [JsonPropertyName("homeNotional")]
        public decimal HomeNotional { get; set; }
        /// <summary>
        /// Foreign notional
        /// </summary>
        [JsonPropertyName("foreignNotional")]
        public decimal ForeignNotional { get; set; }
    }


}
