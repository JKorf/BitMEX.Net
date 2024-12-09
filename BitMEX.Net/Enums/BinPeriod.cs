using CryptoExchange.Net.Attributes;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Bin period
    /// </summary>
    public enum BinPeriod
    {
        /// <summary>
        /// One minute
        /// </summary>
        [Map("1m")]
        OneMinute,
        /// <summary>
        /// Five minutes
        /// </summary>
        [Map("5m")]
        FiveMinutes,
        /// <summary>
        /// One hour
        /// </summary>
        [Map("1h")]
        OneHour,
        /// <summary>
        /// One day
        /// </summary>
        [Map("1d")]
        OneDay
    }
}
