using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Internal
{
    internal class SocketCommand
    {
        [JsonPropertyName("op")]
        public string Operation { get; set; } = string.Empty;

        [JsonPropertyName("args")]
        public string[] Parameters { get; set; } = [];
    }
}
