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
        /// ["<c>id</c>"] Id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>name</c>"] Name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>nonce</c>"] Nonce
        /// </summary>
        [JsonPropertyName("nonce")]
        public long Nonce { get; set; }
        /// <summary>
        /// ["<c>cidr</c>"] Cidr
        /// </summary>
        [JsonPropertyName("cidr")]
        public string Cidr { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>cidrs</c>"] Cidr
        /// </summary>
        [JsonPropertyName("cidrs")]
        public string[] Cidrs { get; set; } = [];
        /// <summary>
        /// ["<c>permissions</c>"] Permissions
        /// </summary>
        [JsonPropertyName("permissions")]
        public string[] Permissions { get; set; } = [];
        /// <summary>
        /// ["<c>targetAccountId</c>"] Target account id
        /// </summary>
        [JsonPropertyName("targetAccountId")]
        public long? TargetAccountId { get; set; }
        /// <summary>
        /// ["<c>enabled</c>"] Enabled
        /// </summary>
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }
        /// <summary>
        /// ["<c>userId</c>"] User id
        /// </summary>
        [JsonPropertyName("userId")]
        public long UserId { get; set; }
        /// <summary>
        /// ["<c>created</c>"] Create time
        /// </summary>
        [JsonPropertyName("created")]
        public DateTime CreateTime { get; set; }
    }


}
