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
        /// ["<c>Day</c>"] Current day
        /// </summary>
        [Map("Day")]
        Day,
        /// <summary>
        /// ["<c>GoodTillCancel</c>"] Good until canceled
        /// </summary>
        [Map("GoodTillCancel")]
        GoodTillCancel,
        /// <summary>
        /// ["<c>ImmediateOrCancel</c>"] Immediate or cancel
        /// </summary>
        [Map("ImmediateOrCancel")]
        ImmediateOrCancel,
        /// <summary>
        /// ["<c>FillOrKill</c>"] Fill or kill
        /// </summary>
        [Map("FillOrKill")]
        FillOrKill,
        /// <summary>
        /// ["<c>AtTheClose</c>"] At the close
        /// </summary>
        [Map("AtTheClose")]
        AtTheClose
    }
}
