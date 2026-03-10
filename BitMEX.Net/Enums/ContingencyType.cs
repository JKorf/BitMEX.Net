using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Contingency type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<ContingencyType>))]
    public enum ContingencyType
    {
        /// <summary>
        /// ["<c>OneCancelsTheOther</c>"] One cancels other order linked with clientOrderLinkId
        /// </summary>
        [Map("OneCancelsTheOther")]
        OneCancelsTheOther,
        /// <summary>
        /// ["<c>OneTriggersTheOther</c>"] One triggers other order linked with clientOrderLinkId
        /// </summary>
        [Map("OneTriggersTheOther")]
        OneTriggersTheOther
    }
}
