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
        /// ["<c>partial</c>"] Snapshot
        /// </summary>
        [Map("partial")]
        Snapshot,
        /// <summary>
        /// ["<c>insert</c>"] Insert
        /// </summary>
        [Map("insert")]
        Insert,
        /// <summary>
        /// ["<c>update</c>"] Update
        /// </summary>
        [Map("update")]
        Update,
        /// <summary>
        /// ["<c>delete</c>"] Delete
        /// </summary>
        [Map("delete")]
        Delete
    }
}
