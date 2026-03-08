using CryptoExchange.Net.Converters.SystemTextJson;
using BitMEX.Net.Enums;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Liquidation
    /// </summary>
    [SerializationModel]
    public record BitMEXLiquidation
    {
        /// <summary>
        /// ["<c>symbol</c>"] Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>orderID</c>"] Order id
        /// </summary>
        [JsonPropertyName("orderID")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>side</c>"] Order side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// ["<c>price</c>"] Price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// ["<c>leavesQty</c>"] Quantity
        /// </summary>
        [JsonPropertyName("leavesQty")]
        public long Quantity { get; set; }
    }
}
