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
        /// ["<c>rootSymbol</c>"] Root symbol
        /// </summary>
        [JsonPropertyName("rootSymbol")]
        public string RootSymbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>currency</c>"] Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string? Asset { get; set; }
        /// <summary>
        /// ["<c>volume24h</c>"] Volume 24 hours
        /// </summary>
        [JsonPropertyName("volume24h")]
        public long Volume24h { get; set; }
        /// <summary>
        /// ["<c>turnover24h</c>"] Turnover 24 hours
        /// </summary>
        [JsonPropertyName("turnover24h")]
        public long Turnover24h { get; set; }
        /// <summary>
        /// ["<c>openInterest</c>"] Open interest
        /// </summary>
        [JsonPropertyName("openInterest")]
        public long OpenInterest { get; set; }
        /// <summary>
        /// ["<c>openValue</c>"] Open value
        /// </summary>
        [JsonPropertyName("openValue")]
        public long OpenValue { get; set; }
    }


}
