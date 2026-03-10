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
        /// ["<c>nearest</c>"] Nearest expiring contract
        /// </summary>
        [Map("nearest")]
        Nearest,
        /// <summary>
        /// ["<c>daily</c>"] Daily
        /// </summary>
        [Map("daily")]
        Daily,
        /// <summary>
        /// ["<c>weekly</c>"] Weekly
        /// </summary>
        [Map("weekly")]
        Weekly,
        /// <summary>
        /// ["<c>monthly</c>"] Monthly
        /// </summary>
        [Map("monthly")]
        Monthly,
        /// <summary>
        /// ["<c>quarterly</c>"] Quarterly
        /// </summary>
        [Map("quarterly")]
        Quarterly,
        /// <summary>
        /// ["<c>biquarterly</c>"] Bi-quarterly
        /// </summary>
        [Map("biquarterly")]
        BiQuarterly,
        /// <summary>
        /// ["<c>perpetual</c>"] Perpetual
        /// </summary>
        [Map("perpetual")]
        Perpetual
    }
}
