using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Notification
    /// </summary>
    [SerializationModel]
    public record BitMEXNotification
    {
        /// <summary>
        /// ["<c>title</c>"] Title
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>body</c>"] Body
        /// </summary>
        [JsonPropertyName("body")]
        public string Body { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>ttl</c>"] Time to live
        /// </summary>
        [JsonPropertyName("ttl")]
        public int? TimeToLive { get; set; }
        /// <summary>
        /// ["<c>closable</c>"] Closable
        /// </summary>
        [JsonPropertyName("closable")]
        public bool Closable { get; set; }
        /// <summary>
        /// ["<c>persist</c>"] Persist
        /// </summary>
        [JsonPropertyName("persist")]
        public bool Persist { get; set; }
        /// <summary>
        /// ["<c>sound</c>"] Sound
        /// </summary>
        [JsonPropertyName("sound")]
        public string Sound { get; set; } = string.Empty;
    }
}
