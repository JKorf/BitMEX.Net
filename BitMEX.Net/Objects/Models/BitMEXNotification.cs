using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Notification
    /// </summary>
    public record BitMEXNotification
    {
        /// <summary>
        /// Title
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// Body
        /// </summary>
        [JsonPropertyName("body")]
        public string Body { get; set; } = string.Empty;
        /// <summary>
        /// Time to live
        /// </summary>
        [JsonPropertyName("ttl")]
        public int? TimeToLive { get; set; }
        /// <summary>
        /// Closable
        /// </summary>
        [JsonPropertyName("closable")]
        public bool Closable { get; set; }
        /// <summary>
        /// Persist
        /// </summary>
        [JsonPropertyName("persist")]
        public bool Persist { get; set; }
        /// <summary>
        /// Sound
        /// </summary>
        [JsonPropertyName("sound")]
        public string Sound { get; set; } = string.Empty;
    }
}
