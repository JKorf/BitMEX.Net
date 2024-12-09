using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting.Filters;
using CryptoExchange.Net.RateLimiting.Guards;
using CryptoExchange.Net.RateLimiting.Interfaces;
using CryptoExchange.Net.RateLimiting;
using System;
using System.Collections.Generic;
using System.Text;
using CryptoExchange.Net.SharedApis;

namespace BitMEX.Net
{
    /// <summary>
    /// BitMEX exchange information and configuration
    /// </summary>
    public static class BitMEXExchange
    {
        /// <summary>
        /// Exchange name
        /// </summary>
        public static string ExchangeName => "BitMEX";

        /// <summary>
        /// Url to the main website
        /// </summary>
        public static string Url { get; } = "https://www.bitmex.com";

        /// <summary>
        /// Urls to the API documentation
        /// </summary>
        public static string[] ApiDocsUrl { get; } = new[] {
            "https://www.bitmex.com/api/explorer/#/"
            };

        /// <summary>
        /// Format a base and quote asset to an BitMEX recognized symbol 
        /// </summary>
        /// <param name="baseAsset">Base asset</param>
        /// <param name="quoteAsset">Quote asset</param>
        /// <param name="tradingMode">Trading mode</param>
        /// <param name="deliverTime">Delivery time for delivery futures</param>
        /// <returns></returns>
        public static string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Rate limiter configuration for the BitMEX API
        /// </summary>
        public static BitMEXRateLimiters RateLimiter { get; } = new BitMEXRateLimiters();
    }

    /// <summary>
    /// Rate limiter configuration for the BitMEX API
    /// </summary>
    public class BitMEXRateLimiters
    {
        /// <summary>
        /// Event for when a rate limit is triggered
        /// </summary>
        public event Action<RateLimitEvent> RateLimitTriggered;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        internal BitMEXRateLimiters()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Initialize();
        }

        private void Initialize()
        {
            BitMEX = new RateLimitGate("BitMEX")
                .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, [], 120, TimeSpan.FromSeconds(60), RateLimitWindowType.Sliding));
            BitMEX.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
        }


        internal IRateLimitGate BitMEX { get; private set; }

    }
}
