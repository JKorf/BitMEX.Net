using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Category
    /// </summary>
    [JsonConverter(typeof(EnumConverter<SymbolCategory>))]
    public enum SymbolCategory
    {
        /// <summary>
        /// Contracts
        /// </summary>
        [Map("CONTRACTS")]
        Contracts,
        /// <summary>
        /// Indices
        /// </summary>
        [Map("INDICES")]
        Indices,
        /// <summary>
        /// Derivatives
        /// </summary>
        [Map("DERIVATIVES")]
        Derivatives,
        /// <summary>
        /// Spot
        /// </summary>
        [Map("SPOT")]
        Spot
    }
}
