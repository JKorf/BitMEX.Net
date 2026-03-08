using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Address book config
    /// </summary>
    [SerializationModel]
    public record BitMEXAddressBookConfig
    {
        /// <summary>
        /// ["<c>id</c>"] Id
        /// </summary>
        [JsonPropertyName("id")]
        public long? Id { get; set; }
        /// <summary>
        /// ["<c>whitelist</c>"] Whitelist
        /// </summary>
        [JsonPropertyName("whitelist")]
        public bool Whitelist { get; set; }
        /// <summary>
        /// ["<c>created</c>"] Created time
        /// </summary>
        [JsonPropertyName("created")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// ["<c>disabled</c>"] Disabled time
        /// </summary>
        [JsonPropertyName("disabled")]
        public DateTime? DisableTime { get; set; }
        /// <summary>
        /// ["<c>userId</c>"] User id
        /// </summary>
        [JsonPropertyName("userId")]
        public long UserId { get; set; }
        /// <summary>
        /// ["<c>defaultCooldown</c>"] Default cooldown
        /// </summary>
        [JsonPropertyName("defaultCooldown")]
        public int DefaultCooldown { get; set; }
        /// <summary>
        /// ["<c>frozen</c>"] Frozen
        /// </summary>
        [JsonPropertyName("frozen")]
        public bool Frozen { get; set; }
    }


}
