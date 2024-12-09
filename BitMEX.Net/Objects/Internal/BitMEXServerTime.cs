using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Internal
{
    internal class BitMEXServerTime
    {
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
