using System;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Internal
{
    internal class InfoUpdate
    {
        [JsonPropertyName("info")]
        public string Info { get; set; } = string.Empty;
        [JsonPropertyName("version")]
        public string Version { get; set; } = string.Empty;
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
