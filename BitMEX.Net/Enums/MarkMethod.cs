using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Mark method
    /// </summary>
    [JsonConverter(typeof(EnumConverter<MarkMethod>))]
    public enum MarkMethod
    {
        /// <summary>
        /// ["<c>FairPrice</c>"] Fair price
        /// </summary>
        [Map("FairPrice")]
        FairPrice,
        /// <summary>
        /// ["<c>LastPrice</c>"] Last price
        /// </summary>
        [Map("LastPrice")]
        LastPrice,
        /// <summary>
        /// ["<c>LastPriceProtected</c>"] Last price protected
        /// </summary>
        [Map("LastPriceProtected")]
        LastPriceProtected,
        /// <summary>
        /// ["<c>IndicativeSettlePrice</c>"] Indicative settle price
        /// </summary>
        [Map("IndicativeSettlePrice")]
        IndicativeSettlePrice,
        /// <summary>
        /// ["<c>FairPriceStox</c>"] Fair price stocks
        /// </summary>
        [Map("FairPriceStox")]
        FairPriceStox
    }
}
