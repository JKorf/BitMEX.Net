using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Tick direction 
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TickDirection>))]
    public enum TickDirection
    {
        /// <summary>
        /// Minus zero tick
        /// </summary>
        [Map("ZeroMinusTick")]
        ZeroMinusTick,
        /// <summary>
        /// Minus tick
        /// </summary>
        [Map("MinusTick")]
        MinusTick,
        /// <summary>
        /// Up zero tick
        /// </summary>
        [Map("ZeroPlusTick")]
        ZeroPlusTick,
        /// <summary>
        /// Up tick
        /// </summary>
        [Map("PlusTick")]
        PlusTick
    }
}
