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
        /// ["<c>CONTRACTS</c>"] Contracts
        /// </summary>
        [Map("CONTRACTS")]
        Contracts,
        /// <summary>
        /// ["<c>INDICES</c>"] Indices
        /// </summary>
        [Map("INDICES")]
        Indices,
        /// <summary>
        /// ["<c>DERIVATIVES</c>"] Derivatives
        /// </summary>
        [Map("DERIVATIVES")]
        Derivatives,
        /// <summary>
        /// ["<c>SPOT</c>"] Spot
        /// </summary>
        [Map("SPOT")]
        Spot
    }
}
