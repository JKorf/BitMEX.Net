using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Announcement
    /// </summary>
    public record BitMEXAnnouncement
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }
        /// <summary>
        /// Link
        /// </summary>
        [JsonPropertyName("link")]
        public string Link { get; set; } = string.Empty;
        /// <summary>
        /// Title
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// Content
        /// </summary>
        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime Timestamp { get; set; }
    }


}
