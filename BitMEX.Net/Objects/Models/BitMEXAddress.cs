using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Saved address
    /// </summary>
    [SerializationModel]
    public record BitMEXAddress
    {
        /// <summary>
        /// ["<c>id</c>"] Id
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }
        /// <summary>
        /// ["<c>currency</c>"] Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>created</c>"] Created time
        /// </summary>
        [JsonPropertyName("created")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// ["<c>userId</c>"] User id
        /// </summary>
        [JsonPropertyName("userId")]
        public long UserId { get; set; }
        /// <summary>
        /// ["<c>address</c>"] Address
        /// </summary>
        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>name</c>"] Name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>note</c>"] Note
        /// </summary>
        [JsonPropertyName("note")]
        public string Note { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>skipConfirm</c>"] Skip confirm
        /// </summary>
        [JsonPropertyName("skipConfirm")]
        public bool SkipConfirm { get; set; }
        /// <summary>
        /// ["<c>skipConfirmVerified</c>"] Skip confirm verified
        /// </summary>
        [JsonPropertyName("skipConfirmVerified")]
        public bool SkipConfirmVerified { get; set; }
        /// <summary>
        /// ["<c>skip2FA</c>"] Skip 2FA
        /// </summary>
        [JsonPropertyName("skip2FA")]
        public bool Skip2FA { get; set; }
        /// <summary>
        /// ["<c>skip2FAVerified</c>"] Skip 2FA verified
        /// </summary>
        [JsonPropertyName("skip2FAVerified")]
        public bool Skip2FAVerified { get; set; }
        /// <summary>
        /// ["<c>network</c>"] Network
        /// </summary>
        [JsonPropertyName("network")]
        public string Network { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>memo</c>"] Memo
        /// </summary>
        [JsonPropertyName("memo")]
        public string? Memo { get; set; }
        /// <summary>
        /// ["<c>cooldownExpires</c>"] Cooldown expires
        /// </summary>
        [JsonPropertyName("cooldownExpires")]
        public DateTime? CooldownExpires { get; set; }
        /// <summary>
        /// ["<c>verified</c>"] Verified
        /// </summary>
        [JsonPropertyName("verified")]
        public bool Verified { get; set; }
    }


}
