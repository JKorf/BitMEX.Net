using CryptoExchange.Net.Converters.SystemTextJson;
using BitMEX.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Trade info
    /// </summary>
    [SerializationModel]
    public record BitMEXTrade
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
        /// Order side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Trade quantity
        /// </summary>
        [JsonPropertyName("size")]
        public long Quantity { get; set; }
        /// <summary>
        /// Trade price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// Tick direction
        /// </summary>
        [JsonPropertyName("tickDirection")]
        public TickDirection TickDirection { get; set; }
        /// <summary>
        /// Trade match id
        /// </summary>
        [JsonPropertyName("trdMatchID")]
        public string? TradeMatchId { get; set; }
        /// <summary>
        /// Trade value
        /// </summary>
        [JsonPropertyName("grossValue")]
        public long? Value { get; set; }
        /// <summary>
        /// Home notional
        /// </summary>
        [JsonPropertyName("homeNotional")]
        public decimal? HomeNotional { get; set; }
        /// <summary>
        /// Foreign notional
        /// </summary>
        [JsonPropertyName("foreignNotional")]
        public decimal? ForeignNotional { get; set; }
    }
}
