using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Asset type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<AssetType>))]
    public enum AssetType
    {
        /// <summary>
        /// Crypto asset
        /// </summary>
        [Map("Crypto")]
        Crypto,
        /// <summary>
        /// Synthetic asset
        /// </summary>
        [Map("Synthetic")]
        Synthetic
    }
}
