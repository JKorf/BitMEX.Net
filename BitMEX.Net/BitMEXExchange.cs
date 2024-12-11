using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting.Filters;
using CryptoExchange.Net.RateLimiting.Guards;
using CryptoExchange.Net.RateLimiting.Interfaces;
using CryptoExchange.Net.RateLimiting;
using System;
using System.Collections.Generic;
using System.Text;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading;
using BitMEX.Net.Clients;
using BitMEX.Net.Enums;
using System.Linq;

namespace BitMEX.Net
{
    /// <summary>
    /// BitMEX exchange information and configuration
    /// </summary>
    public static class BitMEXExchange
    {
        private static DateTime _lastUpdateTime;
#warning if we change balance from gwei to eth then we also not to update from currency to asset (in balances request/updates)
        private static ConcurrentDictionary<string, int> _scalesByCurrency = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        private static ConcurrentDictionary<string, int> _scalesByAsset = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        private static ConcurrentDictionary<string, int> _scalesBySymbol = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        private static ConcurrentDictionary<string, SymbolType> _symbolTypes = new ConcurrentDictionary<string, SymbolType>(StringComparer.OrdinalIgnoreCase);

        internal static async Task UpdateScalesAsync(CancellationToken ct)
        {
            if (DateTime.UtcNow - _lastUpdateTime < TimeSpan.FromDays(1))
                return;

            var assets = await new BitMEXRestClient().ExchangeApi.ExchangeData.GetAssetsAsync(ct: ct).ConfigureAwait(false);
            var symbols = await new BitMEXRestClient().ExchangeApi.ExchangeData.GetActiveSymbolsAsync(ct: ct).ConfigureAwait(false);
            foreach (var asset in assets.Data)
            {
                _scalesByCurrency.TryAdd(asset.Currency, asset.Scale);
                _scalesByAsset.TryAdd(asset.Asset, asset.Scale);
            }

            foreach(var symbol in symbols.Data)
            {
                _scalesBySymbol.TryAdd(symbol.Symbol, symbol.SymbolType == SymbolType.Spot ? _scalesByAsset[symbol.BaseAsset] : 1);
                _symbolTypes.TryAdd(symbol.Symbol, symbol.SymbolType);
            }

            _lastUpdateTime = DateTime.UtcNow;
        }

        internal static int GetAssetScale(string asset) => _scalesByAsset[asset];
        internal static int GetCurrencyScale(string currency) => _scalesByCurrency[currency];
        internal static int GetSymbolQuantityScale(string symbol) => _scalesBySymbol[symbol];
        internal static SymbolType GetSymbolType(string symbol) => _symbolTypes[symbol];

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
            if (tradingMode == TradingMode.Spot)
                return $"{baseAsset}_{quoteAsset}";

            if (deliverTime == null)
                return $"{baseAsset}{quoteAsset}";

            if (tradingMode.IsLinear())
                return $"{baseAsset}{quoteAsset}{ExchangeHelpers.GetDeliveryMonthSymbol(deliverTime.Value)}{deliverTime.Value.ToString("yy")}";

            return $"{baseAsset}{ExchangeHelpers.GetDeliveryMonthSymbol(deliverTime.Value)}{deliverTime.Value.ToString("yy")}";
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
