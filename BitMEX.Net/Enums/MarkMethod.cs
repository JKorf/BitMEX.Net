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
        /// Fair price
        /// </summary>
        [Map("FairPrice")]
        FairPrice,
        /// <summary>
        /// Last price
        /// </summary>
        [Map("LastPrice")]
        LastPrice,
        /// <summary>
        /// Last price protected
        /// </summary>
        [Map("LastPriceProtected")]
        LastPriceProtected,
        /// <summary>
        /// Indicative settle price
        /// </summary>
        [Map("IndicativeSettlePrice")]
        IndicativeSettlePrice,
        /// <summary>
        /// Fair price stocks
        /// </summary>
        [Map("FairPriceStox")]
        FairPriceStox
    }
}
