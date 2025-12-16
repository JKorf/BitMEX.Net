using CryptoExchange.Net.Converters.SystemTextJson;
using BitMEX.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    [SerializationModel]
    internal record BitMEXUserEventWrapper
    {
        [JsonPropertyName("userEvents")]
        public BitMEXUserEvent[] UserEvents { get; set; } = [];
    }

    /// <summary>
    /// User event
    /// </summary>
    [SerializationModel]
    public record BitMEXUserEvent
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }
        /// <summary>
        /// Event type
        /// </summary>
        [JsonPropertyName("type")]
        public EventType EventType { get; set; }
        /// <summary>
        /// Event status
        /// </summary>
        [JsonPropertyName("status")]
        public EventStatus EventStatus { get; set; }
        /// <summary>
        /// User id
        /// </summary>
        [JsonPropertyName("userId")]
        public long UserId { get; set; }
        /// <summary>
        /// Created by id
        /// </summary>
        [JsonPropertyName("createdById")]
        public long CreatedById { get; set; }
        /// <summary>
        /// Ip
        /// </summary>
        [JsonPropertyName("ip")]
        public string Ip { get; set; } = string.Empty;
        /// <summary>
        /// Geo-ip country
        /// </summary>
        [JsonPropertyName("geoipCountry")]
        public string GeoipCountry { get; set; } = string.Empty;
        /// <summary>
        /// Geo-ip region
        /// </summary>
        [JsonPropertyName("geoipRegion")]
        public string GeoipRegion { get; set; } = string.Empty;
        /// <summary>
        /// Geo-ip sub region
        /// </summary>
        [JsonPropertyName("geoipSubRegion")]
        public string GeoipSubRegion { get; set; } = string.Empty;
        /// <summary>
        /// Event meta
        /// </summary>
        [JsonPropertyName("eventMeta")]
        public Dictionary<string, object>? EventMeta { get; set; }
        /// <summary>
        /// Created
        /// </summary>
        [JsonPropertyName("created")]
        public DateTime Created { get; set; }
    }
}
