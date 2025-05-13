using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text;
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
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Created time
        /// </summary>
        [JsonPropertyName("created")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// User id
        /// </summary>
        [JsonPropertyName("userId")]
        public long UserId { get; set; }
        /// <summary>
        /// Address
        /// </summary>
        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;
        /// <summary>
        /// Name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Note
        /// </summary>
        [JsonPropertyName("note")]
        public string Note { get; set; } = string.Empty;
        /// <summary>
        /// Skip confirm
        /// </summary>
        [JsonPropertyName("skipConfirm")]
        public bool SkipConfirm { get; set; }
        /// <summary>
        /// Skip confirm verified
        /// </summary>
        [JsonPropertyName("skipConfirmVerified")]
        public bool SkipConfirmVerified { get; set; }
        /// <summary>
        /// Skip 2FA
        /// </summary>
        [JsonPropertyName("skip2FA")]
        public bool Skip2FA { get; set; }
        /// <summary>
        /// Skip 2FA verified
        /// </summary>
        [JsonPropertyName("skip2FAVerified")]
        public bool Skip2FAVerified { get; set; }
        /// <summary>
        /// Network
        /// </summary>
        [JsonPropertyName("network")]
        public string Network { get; set; } = string.Empty;
        /// <summary>
        /// Memo
        /// </summary>
        [JsonPropertyName("memo")]
        public string? Memo { get; set; }
        /// <summary>
        /// Cooldown expires
        /// </summary>
        [JsonPropertyName("cooldownExpires")]
        public DateTime? CooldownExpires { get; set; }
        /// <summary>
        /// Verified
        /// </summary>
        [JsonPropertyName("verified")]
        public bool Verified { get; set; }
    }


}
