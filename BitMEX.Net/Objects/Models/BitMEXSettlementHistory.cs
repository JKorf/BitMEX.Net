using BitMEX.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Settlement info
    /// </summary>
    public record BitMEXSettlementHistory
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
        /// Settlement type
        /// </summary>
        [JsonPropertyName("settlementType")]
        public SettlementType SettlementType { get; set; }
        /// <summary>
        /// Settled price
        /// </summary>
        [JsonPropertyName("settledPrice")]
        public decimal SettledPrice { get; set; }
    }


}
