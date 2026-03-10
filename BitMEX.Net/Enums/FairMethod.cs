using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Fair method
    /// </summary>
    [JsonConverter(typeof(EnumConverter<FairMethod>))]
    public enum FairMethod
    {
        /// <summary>
        /// ["<c>FundingRate</c>"] Funding rate
        /// </summary>
        [Map("FundingRate")]
        FundingRate,
        /// <summary>
        /// ["<c>ImpactMidPrice</c>"] Impact mid price
        /// </summary>
        [Map("ImpactMidPrice")]
        ImpactMidPrice
    }
}
