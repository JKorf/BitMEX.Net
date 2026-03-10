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
        /// ["<c>ZeroMinusTick</c>"] Minus zero tick
        /// </summary>
        [Map("ZeroMinusTick")]
        ZeroMinusTick,
        /// <summary>
        /// ["<c>MinusTick</c>"] Minus tick
        /// </summary>
        [Map("MinusTick")]
        MinusTick,
        /// <summary>
        /// ["<c>ZeroPlusTick</c>"] Up zero tick
        /// </summary>
        [Map("ZeroPlusTick")]
        ZeroPlusTick,
        /// <summary>
        /// ["<c>PlusTick</c>"] Up tick
        /// </summary>
        [Map("PlusTick")]
        PlusTick
    }
}
