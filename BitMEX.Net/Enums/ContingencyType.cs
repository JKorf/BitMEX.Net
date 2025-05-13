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
        /// One cancels other order linked with clientOrderLinkId
        /// </summary>
        [Map("OneCancelsTheOther")]
        OneCancelsTheOther,
        /// <summary>
        /// One triggers other order linked with clientOrderLinkId
        /// </summary>
        [Map("OneTriggersTheOther")]
        OneTriggersTheOther
    }
}
