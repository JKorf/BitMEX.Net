using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Internal
{
    internal class SocketUpdate<T>
    {
        [JsonPropertyName("table")]
        public string Table { get; set; } = string.Empty;
        [JsonPropertyName("action")]
        public string Action { get; set; } = string.Empty;
        [JsonPropertyName("data")]
        public T Data { get; set; } = default!;

        [JsonPropertyName("filter")]
        public Dictionary<string, string>? Filter { get; set; }
    }
}
