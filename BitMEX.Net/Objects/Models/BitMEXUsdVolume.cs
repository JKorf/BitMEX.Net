using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Average trading volume 30 days
    /// </summary>
    public record BitMEXUsdVolume
    {
        /// <summary>
        /// Average trading volume USD
        /// </summary>
        [JsonPropertyName("advUsd")]
        public long AverageVolumeUsd { get; set; }
        /// <summary>
        /// Average trading volume USD spot
        /// </summary>
        [JsonPropertyName("advUsdSpot")]
        public long AverageVolumeUsdSpot { get; set; }
        /// <summary>
        /// Average trading volume USD contract
        /// </summary>
        [JsonPropertyName("advUsdContract")]
        public long AverageVolumeUsdContract { get; set; }
    }


}
