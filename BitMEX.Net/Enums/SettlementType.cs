using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Settlement type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<SettlementType>))]
    public enum SettlementType
    {
        /// <summary>
        /// Settlement
        /// </summary>
        [Map("settlement")]
        Settlement
    }
}
