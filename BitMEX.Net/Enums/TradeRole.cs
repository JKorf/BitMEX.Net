using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Trade role
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TradeRole>))]
    public enum TradeRole
    {
        /// <summary>
        /// Maker
        /// </summary>
        [Map("maker", "Maker")]
        Maker,
        /// <summary>
        /// Taker
        /// </summary>
        [Map("taker", "Taker")]
        Taker
    }
}
