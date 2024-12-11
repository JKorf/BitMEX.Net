using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Internal
{
    internal class SocketResponse
    {
        [JsonPropertyName("subscribe")]
        public string Subscription { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }
    }
}
