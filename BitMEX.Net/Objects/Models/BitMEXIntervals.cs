using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Interval
    /// </summary>
    [SerializationModel]
    public record BitMEXIntervals
    {
        /// <summary>
        /// Intervals
        /// </summary>
        [JsonPropertyName("intervals")]
        public string[] Intervals { get; set; } = [];
        /// <summary>
        /// Symbols
        /// </summary>
        [JsonPropertyName("symbols")]
        public string[] Symbols { get; set; } = [];
    }
}
