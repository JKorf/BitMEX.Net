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
    public record BitMEXIntervals
    {
        /// <summary>
        /// Intervals
        /// </summary>
        [JsonPropertyName("intervals")]
        public IEnumerable<string> Intervals { get; set; } = [];
        /// <summary>
        /// Symbols
        /// </summary>
        [JsonPropertyName("symbols")]
        public IEnumerable<string> Symbols { get; set; } = [];
    }
}
