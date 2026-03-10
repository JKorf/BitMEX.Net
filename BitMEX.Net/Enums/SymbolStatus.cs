using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Symbol status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<SymbolStatus>))]
    public enum SymbolStatus
    {
        /// <summary>
        /// ["<c>Unlisted</c>"] Unlisted
        /// </summary>
        [Map("Unlisted")]
        Unlisted,
        /// <summary>
        /// ["<c>Open</c>"] Open
        /// </summary>
        [Map("Open")]
        Open,
        /// <summary>
        /// ["<c>Closed</c>"] Closed
        /// </summary>
        [Map("Closed")]
        Closed,
        /// <summary>
        /// ["<c>Settled</c>"] Settled
        /// </summary>
        [Map("Settled")]
        Settled
    }
}
