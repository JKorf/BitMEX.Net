using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Order side
    /// </summary>
    [JsonConverter(typeof(EnumConverter<OrderSide>))]
    public enum OrderSide
    {
        /// <summary>
        /// ["<c>Buy</c>"] Buy
        /// </summary>
        [Map("Buy")]
        Buy,
        /// <summary>
        /// ["<c>Sell</c>"] Sell
        /// </summary>
        [Map("Sell")]
        Sell
    }
}
