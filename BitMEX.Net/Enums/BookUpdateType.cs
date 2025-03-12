using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Book type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<BookUpdateType>))]
    public enum BookUpdateType
    {
        /// <summary>
        /// Snapshot
        /// </summary>
        [Map("partial")]
        Snapshot,
        /// <summary>
        /// Insert
        /// </summary>
        [Map("insert")]
        Insert,
        /// <summary>
        /// Update
        /// </summary>
        [Map("update")]
        Update,
        /// <summary>
        /// Delete
        /// </summary>
        [Map("delete")]
        Delete
    }
}
