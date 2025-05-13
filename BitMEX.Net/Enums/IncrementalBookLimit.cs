using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Book limit
    /// </summary>
    [JsonConverter(typeof(EnumConverter<IncrementalBookLimit>))]
    public enum IncrementalBookLimit
    {
        /// <summary>
        /// Top 25 order book
        /// </summary>
        Top25,
        /// <summary>
        ///  Full order book
        /// </summary>
        Full
    }
}
