using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Peg price type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<PeggedPriceType>))]
    public enum PeggedPriceType
    {
        /// <summary>
        /// Market peg
        /// </summary>
        [Map("MarketPeg")]
        MarketPeg,
        /// <summary>
        /// Primary peg
        /// </summary>
        [Map("PrimaryPeg")]
        PrimaryPeg,
        /// <summary>
        /// Trailing stop peg
        /// </summary>
        [Map("TrailingStopPeg")]
        TrailingStopPeg
    }
}
