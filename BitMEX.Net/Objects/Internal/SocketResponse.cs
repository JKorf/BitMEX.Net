using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Internal
{
    internal class SocketResponse
    {
        [JsonPropertyName("subscribe")]
        public string Subscription { get; set; } = string.Empty;

        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("error")]
        public string? Error { get; set; }

        [JsonPropertyName("status")]
        public int? Status { get; set; }
    }
}
