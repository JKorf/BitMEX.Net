using CryptoExchange.Net.Converters.SystemTextJson;
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
        /// ["<c>maxFee</c>"] Max fee
        /// </summary>
        [JsonPropertyName("maxFee")]
        public decimal MaxFee { get; set; }
        /// <summary>
        /// ["<c>makerFee</c>"] Maker fee
        /// </summary>
        [JsonPropertyName("makerFee")]
        public decimal MakerFee { get; set; }
        /// <summary>
        /// ["<c>takerFee</c>"] Taker fee
        /// </summary>
        [JsonPropertyName("takerFee")]
        public decimal TakerFee { get; set; }
        /// <summary>
        /// ["<c>settlementFee</c>"] Settlement fee
        /// </summary>
        [JsonPropertyName("settlementFee")]
        public decimal SettlementFee { get; set; }
    }


}
