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
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public long? Id { get; set; }
        /// <summary>
        /// Whitelist
        /// </summary>
        [JsonPropertyName("whitelist")]
        public bool Whitelist { get; set; }
        /// <summary>
        /// Created time
        /// </summary>
        [JsonPropertyName("created")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// Disabled time
        /// </summary>
        [JsonPropertyName("disabled")]
        public DateTime? DisableTime { get; set; }
        /// <summary>
        /// User id
        /// </summary>
        [JsonPropertyName("userId")]
        public long UserId { get; set; }
        /// <summary>
        /// Default cooldown
        /// </summary>
        [JsonPropertyName("defaultCooldown")]
        public int DefaultCooldown { get; set; }
        /// <summary>
        /// Frozen
        /// </summary>
        [JsonPropertyName("frozen")]
        public bool Frozen { get; set; }
    }


}
