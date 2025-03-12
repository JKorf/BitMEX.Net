using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Symbol filter
    /// </summary>
    [JsonConverter(typeof(EnumConverter<SymbolFilter>))]
    public enum SymbolFilter
    {
        /// <summary>
        /// Nearest expiring contract
        /// </summary>
        [Map("nearest")]
        Nearest,
        /// <summary>
        /// Daily
        /// </summary>
        [Map("daily")]
        Daily,
        /// <summary>
        /// Weekly
        /// </summary>
        [Map("weekly")]
        Weekly,
        /// <summary>
        /// Monthly
        /// </summary>
        [Map("monthly")]
        Monthly,
        /// <summary>
        /// Quarterly
        /// </summary>
        [Map("quarterly")]
        Quarterly,
        /// <summary>
        /// Bi-quarterly
        /// </summary>
        [Map("biquarterly")]
        BiQuarterly,
        /// <summary>
        /// Perpetual
        /// </summary>
        [Map("perpetual")]
        Perpetual
    }
}
