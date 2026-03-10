using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Bin period
    /// </summary>
    [JsonConverter(typeof(EnumConverter<BinPeriod>))]
    public enum BinPeriod
    {
        /// <summary>
        /// ["<c>1m</c>"] One minute
        /// </summary>
        [Map("1m")]
        OneMinute = 60,
        /// <summary>
        /// ["<c>5m</c>"] Five minutes
        /// </summary>
        [Map("5m")]
        FiveMinutes = 60 * 5,
        /// <summary>
        /// ["<c>1h</c>"] One hour
        /// </summary>
        [Map("1h")]
        OneHour = 60 * 60,
        /// <summary>
        /// ["<c>1d</c>"] One day
        /// </summary>
        [Map("1d")]
        OneDay = 60 * 60 * 24
    }
}
