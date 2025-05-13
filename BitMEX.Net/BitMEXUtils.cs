using BitMEX.Net.Clients;
using BitMEX.Net.Enums;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BitMEX.Net
{
    /// <summary>
    /// Utility methods for BitMEX
    /// </summary>
    public static class BitMEXUtils
    {
        private static DateTime _lastUpdateTime;
        private static SemaphoreSlim _sem = new SemaphoreSlim(1, 1);
        private static ConcurrentDictionary<string, int> _scalesByCurrency = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        private static ConcurrentDictionary<string, int> _scalesByAsset = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        private static ConcurrentDictionary<string, int> _scalesBySymbol = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        private static ConcurrentDictionary<string, SymbolType> _symbolTypes = new ConcurrentDictionary<string, SymbolType>(StringComparer.OrdinalIgnoreCase);
        private static ConcurrentDictionary<string, string> _currencyToAsset = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Update the cached symbol and asset info. This should be called before using other methods in this class.
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public static async Task<CallResult> UpdateSymbolInfoAsync(CancellationToken ct = default)
        {
            await _sem.WaitAsync(ct).ConfigureAwait(false);
            try
            {
                if (DateTime.UtcNow - _lastUpdateTime < TimeSpan.FromDays(1))
                    return CallResult.SuccessResult;

                var assets = await new BitMEXRestClient().ExchangeApi.ExchangeData.GetAssetsAsync(ct: ct).ConfigureAwait(false);
                var symbols = await new BitMEXRestClient().ExchangeApi.ExchangeData.GetActiveSymbolsAsync(ct: ct).ConfigureAwait(false);
                if (!assets || !symbols)
                    return new CallResult(assets.Error ?? symbols.Error);

                foreach (var asset in assets.Data)
                {
                    _scalesByCurrency.TryAdd(asset.Currency, asset.Scale);
                    _scalesByAsset.TryAdd(asset.Asset, asset.Scale);
                    _currencyToAsset.TryAdd(asset.Currency, asset.Asset);
                }

                foreach (var symbol in symbols.Data)
                {
                    _scalesBySymbol.TryAdd(symbol.Symbol, symbol.SymbolType == SymbolType.Spot ? _scalesByAsset[symbol.BaseAsset] : 1);
                    _symbolTypes.TryAdd(symbol.Symbol, symbol.SymbolType);
                }

                _lastUpdateTime = DateTime.UtcNow;
                return CallResult.SuccessResult;
            }
            finally
            {
                _sem.Release();
            }
        }

        /// <summary>
        /// Get the scale of an asset.
        /// </summary>
        /// <param name="asset">Asset name</param>
        /// <returns></returns>
        public static int GetAssetScale(string asset)
        {
            if (char.IsUpper(asset.Last()))
                return _scalesByAsset[asset];

            return _scalesByCurrency[asset];
        }

        /// <summary>
        /// Get the scale of quantity for a symbol
        /// </summary>
        /// <param name="symbol">Symbol name</param>
        /// <returns></returns>
        public static int GetSymbolQuantityScale(string symbol) => _scalesBySymbol[symbol];
        
        /// <summary>
        /// Get the type for a symbol
        /// </summary>
        /// <param name="symbol">Symbol name</param>
        /// <returns></returns>
        public static SymbolType GetSymbolType(string symbol) => _symbolTypes[symbol];

        /// <summary>
        /// Get asset name for a currency
        /// </summary>
        /// <param name="currency">Currency name</param>
        /// <returns></returns>
        public static string GetAssetFromCurrency(string currency) => _currencyToAsset[currency];
    }
}
