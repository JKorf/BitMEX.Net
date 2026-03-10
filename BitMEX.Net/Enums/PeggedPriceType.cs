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
        /// ["<c>MarketPeg</c>"] Market peg
        /// </summary>
        [Map("MarketPeg")]
        MarketPeg,
        /// <summary>
        /// ["<c>PrimaryPeg</c>"] Primary peg
        /// </summary>
        [Map("PrimaryPeg")]
        PrimaryPeg,
        /// <summary>
        /// ["<c>TrailingStopPeg</c>"] Trailing stop peg
        /// </summary>
        [Map("TrailingStopPeg")]
        TrailingStopPeg
    }
}
