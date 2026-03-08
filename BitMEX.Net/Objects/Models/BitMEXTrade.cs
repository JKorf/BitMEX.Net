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
        /// ["<c>side</c>"] Order side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// ["<c>size</c>"] Trade quantity
        /// </summary>
        [JsonPropertyName("size")]
        public long Quantity { get; set; }
        /// <summary>
        /// ["<c>price</c>"] Trade price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// ["<c>tickDirection</c>"] Tick direction
        /// </summary>
        [JsonPropertyName("tickDirection")]
        public TickDirection TickDirection { get; set; }
        /// <summary>
        /// ["<c>trdMatchID</c>"] Trade match id
        /// </summary>
        [JsonPropertyName("trdMatchID")]
        public string? TradeMatchId { get; set; }
        /// <summary>
        /// ["<c>grossValue</c>"] Trade value
        /// </summary>
        [JsonPropertyName("grossValue")]
        public long? Value { get; set; }
        /// <summary>
        /// ["<c>homeNotional</c>"] Home notional
        /// </summary>
        [JsonPropertyName("homeNotional")]
        public decimal? HomeNotional { get; set; }
        /// <summary>
        /// ["<c>foreignNotional</c>"] Foreign notional
        /// </summary>
        [JsonPropertyName("foreignNotional")]
        public decimal? ForeignNotional { get; set; }
    }
}
