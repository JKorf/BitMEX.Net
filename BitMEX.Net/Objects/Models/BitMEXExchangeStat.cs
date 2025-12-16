using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Exchange stats
    /// </summary>
    [SerializationModel]
    public record BitMEXExchangeStat
    {
        /// <summary>
        /// Root symbol
        /// </summary>
        [JsonPropertyName("rootSymbol")]
        public string RootSymbol { get; set; } = string.Empty;
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string? Asset { get; set; }
        /// <summary>
        /// Volume 24 hours
        /// </summary>
        [JsonPropertyName("volume24h")]
        public long Volume24h { get; set; }
        /// <summary>
        /// Turnover 24 hours
        /// </summary>
        [JsonPropertyName("turnover24h")]
        public long Turnover24h { get; set; }
        /// <summary>
        /// Open interest
        /// </summary>
        [JsonPropertyName("openInterest")]
        public long OpenInterest { get; set; }
        /// <summary>
        /// Open value
        /// </summary>
        [JsonPropertyName("openValue")]
        public long OpenValue { get; set; }
    }


}
