using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// API key info
    /// </summary>
    [SerializationModel]
    public record BitMEXApiKey
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// Name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Nonce
        /// </summary>
        [JsonPropertyName("nonce")]
        public long Nonce { get; set; }
        /// <summary>
        /// Cidr
        /// </summary>
        [JsonPropertyName("cidr")]
        public string Cidr { get; set; } = string.Empty;
        /// <summary>
        /// Cidr
        /// </summary>
        [JsonPropertyName("cidrs")]
        public string[] Cidrs { get; set; } = [];
        /// <summary>
        /// Permissions
        /// </summary>
        [JsonPropertyName("permissions")]
        public string[] Permissions { get; set; } = [];
        /// <summary>
        /// Target account id
        /// </summary>
        [JsonPropertyName("targetAccountId")]
        public long? TargetAccountId { get; set; }
        /// <summary>
        /// Enabled
        /// </summary>
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }
        /// <summary>
        /// User id
        /// </summary>
        [JsonPropertyName("userId")]
        public long UserId { get; set; }
        /// <summary>
        /// Create time
        /// </summary>
        [JsonPropertyName("created")]
        public DateTime CreateTime { get; set; }
    }


}
