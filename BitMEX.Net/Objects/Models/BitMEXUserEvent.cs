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
        /// ["<c>id</c>"] Id
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }
        /// <summary>
        /// ["<c>type</c>"] Event type
        /// </summary>
        [JsonPropertyName("type")]
        public EventType EventType { get; set; }
        /// <summary>
        /// ["<c>status</c>"] Event status
        /// </summary>
        [JsonPropertyName("status")]
        public EventStatus EventStatus { get; set; }
        /// <summary>
        /// ["<c>userId</c>"] User id
        /// </summary>
        [JsonPropertyName("userId")]
        public long UserId { get; set; }
        /// <summary>
        /// ["<c>createdById</c>"] Created by id
        /// </summary>
        [JsonPropertyName("createdById")]
        public long CreatedById { get; set; }
        /// <summary>
        /// ["<c>ip</c>"] Ip
        /// </summary>
        [JsonPropertyName("ip")]
        public string Ip { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>geoipCountry</c>"] Geo-ip country
        /// </summary>
        [JsonPropertyName("geoipCountry")]
        public string GeoipCountry { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>geoipRegion</c>"] Geo-ip region
        /// </summary>
        [JsonPropertyName("geoipRegion")]
        public string GeoipRegion { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>geoipSubRegion</c>"] Geo-ip sub region
        /// </summary>
        [JsonPropertyName("geoipSubRegion")]
        public string GeoipSubRegion { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>eventMeta</c>"] Event meta
        /// </summary>
        [JsonPropertyName("eventMeta")]
        public Dictionary<string, object>? EventMeta { get; set; }
        /// <summary>
        /// ["<c>created</c>"] Created
        /// </summary>
        [JsonPropertyName("created")]
        public DateTime Created { get; set; }
    }
}
