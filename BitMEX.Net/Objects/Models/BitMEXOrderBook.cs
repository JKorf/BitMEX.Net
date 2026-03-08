using CryptoExchange.Net.Converters.SystemTextJson;
using BitMEX.Net.Enums;
using CryptoExchange.Net.Interfaces;
using System;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Order book
    /// </summary>
    [SerializationModel]
    public record BitMEXOrderBook
    {
        /// <summary>
        /// Asks
        /// </summary>
        public BitMEXOrderBookEntry[] Asks { get; set; } = [];
        /// <summary>
        /// Bids
        /// </summary>
        public BitMEXOrderBookEntry[] Bids { get; set; } = [];
    }

    /// <summary>
    /// Order book entry
    /// </summary>
    [SerializationModel]
    public record BitMEXOrderBookEntry : ISymbolOrderBookEntry
    {
        /// <summary>
        /// ["<c>symbol</c>"] Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>id</c>"] Id
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }
        /// <summary>
        /// ["<c>side</c>"] Side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// ["<c>size</c>"] Quantity
        /// </summary>
        [JsonPropertyName("size")]
        public decimal Quantity { get; set; }

        /// <summary>
        /// ["<c>price</c>"] Price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// ["<c>timestamp</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>transactTime</c>"] TransactTime
        /// </summary>
        [JsonPropertyName("transactTime")]
        public DateTime TransactTime { get; set; }
    }
}
