using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Order type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<OrderType>))]
    public enum OrderType
    {
        /// <summary>
        /// ["<c>Limit</c>"] Limit order
        /// </summary>
        [Map("Limit")]
        Limit,
        /// <summary>
        /// ["<c>Market</c>"] Market order
        /// </summary>
        [Map("Market")]
        Market,
        /// <summary>
        /// ["<c>Stop</c>"] Market stop order
        /// </summary>
        [Map("Stop")]
        StopMarket,
        /// <summary>
        /// ["<c>StopLimit</c>"] Limit stop order
        /// </summary>
        [Map("StopLimit")]
        StopLimit,
        /// <summary>
        /// ["<c>MarketIfTouched</c>"] Similar to a Stop, but triggers are done in the opposite direction. Useful for Take Profit Market orders.
        /// </summary>
        [Map("MarketIfTouched")]
        MarketIfTouched,
        /// <summary>
        /// ["<c>LimitIfTouched</c>"] Similar to a Stop, but triggers are done in the opposite direction. Useful for Take Profit Limit orders.
        /// </summary>
        [Map("LimitIfTouched")]
        LimitIfTouched,
        /// <summary>
        /// ["<c>Pegged</c>"] Pegged orders allow users to submit a limit price relative to the current market price
        /// </summary>
        [Map("Pegged")]
        Pegged
    }
}
