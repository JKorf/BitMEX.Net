using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using System;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Order book update
    /// </summary>
    [SerializationModel]
    public record BitMEXOrderBookUpdate
    {
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// Bids
        /// </summary>
        [JsonPropertyName("bids")]
        public BitMEXOrderBookUpdateEntry[] Bids { get; set; } = [];
        /// <summary>
        /// Asks
        /// </summary>
        [JsonPropertyName("asks")]
        public BitMEXOrderBookUpdateEntry[] Asks { get; set; } = [];

        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// Order book entry
    /// </summary>
    [JsonConverter(typeof(ArrayConverter<BitMEXOrderBookUpdateEntry>))]
    public record BitMEXOrderBookUpdateEntry : ISymbolOrderBookEntry
    {
        /// <summary>
        /// Price
        /// </summary>
        [ArrayProperty(0)]
        public decimal Price { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [ArrayProperty(1)]
        public long Quantity { get; set; }
        decimal ISymbolOrderBookEntry.Quantity
        {
            get => Quantity;
            set => Quantity = (long)value;
        }
    }
}
