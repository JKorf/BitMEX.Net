using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Time in force
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TimeInForce>))]
    public enum TimeInForce
    {
        /// <summary>
        /// Current day
        /// </summary>
        [Map("Day")]
        Day,
        /// <summary>
        /// Good until canceled
        /// </summary>
        [Map("GoodTillCancel")]
        GoodTillCancel,
        /// <summary>
        /// Immediate or cancel
        /// </summary>
        [Map("ImmediateOrCancel")]
        ImmediateOrCancel,
        /// <summary>
        /// Fill or kill
        /// </summary>
        [Map("FillOrKill")]
        FillOrKill,
        /// <summary>
        /// At the close
        /// </summary>
        [Map("AtTheClose")]
        AtTheClose
    }
}
