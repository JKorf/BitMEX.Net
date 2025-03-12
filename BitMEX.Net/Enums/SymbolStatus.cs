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
        /// Unlisted
        /// </summary>
        [Map("Unlisted")]
        Unlisted,
        /// <summary>
        /// Open
        /// </summary>
        [Map("Open")]
        Open,
        /// <summary>
        /// Closed
        /// </summary>
        [Map("Closed")]
        Closed,
        /// <summary>
        /// Settled
        /// </summary>
        [Map("Settled")]
        Settled
    }
}
