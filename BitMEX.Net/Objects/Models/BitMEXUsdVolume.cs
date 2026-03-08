using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Average trading volume 30 days
    /// </summary>
    [SerializationModel]
    public record BitMEXUsdVolume
    {
        /// <summary>
        /// ["<c>advUsd</c>"] Average trading volume USD
        /// </summary>
        [JsonPropertyName("advUsd")]
        public decimal AverageVolumeUsd { get; set; }
        /// <summary>
        /// ["<c>advUsdSpot</c>"] Average trading volume USD spot
        /// </summary>
        [JsonPropertyName("advUsdSpot")]
        public decimal AverageVolumeUsdSpot { get; set; }
        /// <summary>
        /// ["<c>advUsdContract</c>"] Average trading volume USD contract
        /// </summary>
        [JsonPropertyName("advUsdContract")]
        public decimal AverageVolumeUsdContract { get; set; }
    }


}
