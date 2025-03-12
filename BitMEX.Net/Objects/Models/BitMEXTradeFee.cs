using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Trading fee
    /// </summary>
    [SerializationModel]
    public record BitMEXTradeFee
    {
        /// <summary>
        /// Max fee
        /// </summary>
        [JsonPropertyName("maxFee")]
        public decimal MaxFee { get; set; }
        /// <summary>
        /// Maker fee
        /// </summary>
        [JsonPropertyName("makerFee")]
        public decimal MakerFee { get; set; }
        /// <summary>
        /// Taker fee
        /// </summary>
        [JsonPropertyName("takerFee")]
        public decimal TakerFee { get; set; }
        /// <summary>
        /// Settlement fee
        /// </summary>
        [JsonPropertyName("settlementFee")]
        public decimal SettlementFee { get; set; }
    }


}
