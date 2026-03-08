using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Announcement
    /// </summary>
    [SerializationModel]
    public record BitMEXAnnouncement
    {
        /// <summary>
        /// ["<c>id</c>"] Id
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }
        /// <summary>
        /// ["<c>link</c>"] Link
        /// </summary>
        [JsonPropertyName("link")]
        public string Link { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>title</c>"] Title
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>content</c>"] Content
        /// </summary>
        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>date</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime Timestamp { get; set; }
    }


}
