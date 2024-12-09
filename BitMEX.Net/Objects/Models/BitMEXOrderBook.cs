using BitMEX.Net.Enums;
using CryptoExchange.Net.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Order book
    /// </summary>
    public record BitMEXOrderBook
    {
        /// <summary>
        /// Asks
        /// </summary>
        public IEnumerable<BitMEXOrderBookEntry> Asks { get; set; } = [];
        /// <summary>
        /// Bids
        /// </summary>
        public IEnumerable<BitMEXOrderBookEntry> Bids { get; set; } = [];
    }

    /// <summary>
    /// Order book entry
    /// </summary>
    public record BitMEXOrderBookEntry : ISymbolOrderBookEntry
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }
        /// <summary>
        /// Side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("size")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// TransactTime
        /// </summary>
        [JsonPropertyName("transactTime")]
        public DateTime TransactTime { get; set; }
    }
}
