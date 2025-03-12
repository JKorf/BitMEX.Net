using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using BitMEX.Net.Interfaces.Clients.ExchangeApi;
using BitMEX.Net.Objects.Models;
using BitMEX.Net.Enums;
using System.Linq;
using BitMEX.Net.Objects.Internal;
using CryptoExchange.Net;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace BitMEX.Net.Clients.ExchangeApi
{
    /// <inheritdoc />
    internal class BitMEXRestClientExchangeApiExchangeData : IBitMEXRestClientExchangeApiExchangeData
    {
        private readonly BitMEXRestClientExchangeApi _baseClient;
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        internal BitMEXRestClientExchangeApiExchangeData(ILogger logger, BitMEXRestClientExchangeApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Server Time

        /// <inheritdoc />
        public async Task<WebCallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            var result = await _baseClient.SendAsync<BitMEXServerTime>(request, null, ct).ConfigureAwait(false);
            return result.As(result.Data?.Timestamp ?? default);
        }

        #endregion

        #region Get Active Symbols

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXSymbol>>> GetActiveSymbolsAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/instrument/active", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<IEnumerable<BitMEXSymbol>>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get All Symbols

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXSymbol>>> GetSymbolsAsync(
            string? symbol = null,
            SymbolFilter? symbolFilter = null,
            Dictionary<string, object>? filter = null,
            IEnumerable<string>? columns = null,
            bool? reverse = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? offset = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            if (symbol != null)
                parameters.AddOptional("symbol", symbol + (symbolFilter == null ? "" : ":" + EnumConverter.GetString(symbolFilter)));
            parameters.AddOptional("filter", filter);
            parameters.AddOptional("columns", columns);
            parameters.AddOptional("count", limit);
            parameters.AddOptional("start", offset);
            parameters.AddOptional("reverse", reverse);
            parameters.AddOptional("startTime", startTime?.ToRfc3339String());
            parameters.AddOptional("endTime", endTime?.ToRfc3339String());
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/instrument", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<IEnumerable<BitMEXSymbol>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Active Intervals

        /// <inheritdoc />
        public async Task<WebCallResult<BitMEXIntervals>> GetActiveIntervalsAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/instrument/activeIntervals", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<BitMEXIntervals>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Composite Indexes

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXCompositeIndex>>> GetCompositeIndexesAsync(
            string symbol,
            Dictionary<string, object>? filter = null,
            IEnumerable<string>? columns = null,
            bool? reverse = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? offset = null,
            int? limit = null, 
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("symbol", symbol);
            parameters.AddOptional("filter", filter);
            parameters.AddOptional("columns", columns);
            parameters.AddOptional("count", limit);
            parameters.AddOptional("start", offset);
            parameters.AddOptional("reverse", reverse);
            parameters.AddOptional("startTime", startTime?.ToRfc3339String());
            parameters.AddOptional("endTime", endTime?.ToRfc3339String());

            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/instrument/compositeIndex", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<IEnumerable<BitMEXCompositeIndex>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Indices

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXIndex>>> GetIndicesAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/instrument/indices", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<IEnumerable<BitMEXIndex>>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Symbol Volumes

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXSymbolVolume>>> GetSymbolVolumesAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/instrument/usdVolume", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<IEnumerable<BitMEXSymbolVolume>>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Trades

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXTrade>>> GetTradesAsync(
            string symbol,
            SymbolFilter? symbolFilter = null,
            Dictionary<string, object>? filter = null,
            IEnumerable<string>? columns = null,
            bool? reverse = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? offset = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            if (symbol != null)
                parameters.AddOptional("symbol", symbol + (symbolFilter == null ? "" : ":" + EnumConverter.GetString(symbolFilter)));
            parameters.AddOptional("filter", filter);
            parameters.AddOptional("columns", columns);
            parameters.AddOptional("count", limit);
            parameters.Add("reverse", reverse ?? true);
            parameters.AddOptional("startTime", startTime?.ToRfc3339String());
            parameters.AddOptional("endTime", endTime?.ToRfc3339String());
            parameters.AddOptional("start", offset);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/trade", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<IEnumerable<BitMEXTrade>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Klines

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXAggTrade>>> GetKlinesAsync(
            string symbol,
            BinPeriod period,
            SymbolFilter? symbolFilter = null,
            bool? partial = null,
            Dictionary<string, object>? filter = null,
            IEnumerable<string>? columns = null,
            bool? reverse = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? offset = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            if (symbol != null)
                parameters.AddOptional("symbol", symbol + (symbolFilter == null ? "" : ":" + EnumConverter.GetString(symbolFilter)));
            parameters.AddEnum("binSize", period);
            parameters.AddOptional("partial", partial);
            parameters.AddOptional("filter", filter);
            parameters.AddOptional("columns", columns);
            parameters.AddOptional("count", limit);
            parameters.AddOptional("start", offset);
            parameters.Add("reverse", reverse ?? true);
            parameters.AddOptional("startTime", startTime?.ToRfc3339String());
            parameters.AddOptional("endTime", endTime?.ToRfc3339String());

            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/trade/bucketed", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<IEnumerable<BitMEXAggTrade>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Exchange Stats

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXExchangeStat>>> GetExchangeStatsAsync(CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/stats", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<IEnumerable<BitMEXExchangeStat>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Exchange Stat History

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXExchangeStatHistory>>> GetExchangeStatHistoryAsync(CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/stats/history", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<IEnumerable<BitMEXExchangeStatHistory>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Exchange Stat History USD

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXExchangeStatHistoryUsd>>> GetExchangeStatHistoryUSDAsync(CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/stats/historyUSD", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<IEnumerable<BitMEXExchangeStatHistoryUsd>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Settlement History

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXSettlementHistory>>> GetSettlementHistoryAsync(
            string symbol,
            SymbolFilter? symbolFilter = null,
            Dictionary<string, object>? filter = null,
            IEnumerable<string>? columns = null,
            bool? reverse = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? offset = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            if (symbol != null)
                parameters.AddOptional("symbol", symbol + (symbolFilter == null ? "" : ":" + EnumConverter.GetString(symbolFilter)));
            parameters.AddOptional("filter", filter);
            parameters.AddOptional("columns", columns);
            parameters.AddOptional("count", limit);
            parameters.AddOptional("start", offset);
            parameters.Add("reverse", reverse ?? true);
            parameters.AddOptional("startTime", startTime?.ToRfc3339String());
            parameters.AddOptional("endTime", endTime?.ToRfc3339String());
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/settlement", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<IEnumerable<BitMEXSettlementHistory>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Book Ticker History

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXBookTicker>>> GetBookTickerHistoryAsync(
            string symbol,
            SymbolFilter? symbolFilter = null,
            Dictionary<string, object>? filter = null,
            IEnumerable<string>? columns = null,
            bool? reverse = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? offset = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            if (symbol != null)
                parameters.AddOptional("symbol", symbol + (symbolFilter == null ? "" : ":" + EnumConverter.GetString(symbolFilter)));
            parameters.AddOptional("filter", filter);
            parameters.AddOptional("columns", columns);
            parameters.AddOptional("count", limit);
            parameters.AddOptional("start", offset);
            parameters.Add("reverse", reverse ?? true);
            parameters.AddOptional("startTime", startTime?.ToRfc3339String());
            parameters.AddOptional("endTime", endTime?.ToRfc3339String());
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/quote", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<IEnumerable<BitMEXBookTicker>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Aggregated Book Ticker

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXBookTicker>>> GetAggregatedBookTickerHistoryAsync(
            string symbol,
            BinPeriod period,
            SymbolFilter? symbolFilter = null,
            bool? partial = null,
            Dictionary<string, object>? filter = null,
            IEnumerable<string>? columns = null,
            bool? reverse = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? offset = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            if (symbol != null)
                parameters.AddOptional("symbol", symbol + (symbolFilter == null ? "" : ":" + EnumConverter.GetString(symbolFilter)));
            parameters.AddEnum("binSize", period);
            parameters.AddOptional("partial", partial);
            parameters.AddOptional("filter", filter);
            parameters.AddOptional("columns", columns);
            parameters.AddOptional("count", limit);
            parameters.AddOptional("start", offset);
            parameters.Add("reverse", reverse ?? true);
            parameters.AddOptional("startTime", startTime?.ToRfc3339String());
            parameters.AddOptional("endTime", endTime?.ToRfc3339String());
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/quote/bucketed", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<IEnumerable<BitMEXBookTicker>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Order Book

        /// <inheritdoc />
        public async Task<WebCallResult<BitMEXOrderBook>> GetOrderBookAsync(string symbol, int limit, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("symbol", symbol);
            parameters.Add("depth", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/orderBook/L2", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            var result = await _baseClient.SendAsync<IEnumerable<BitMEXOrderBookEntry>>(request, parameters, ct).ConfigureAwait(false);
            if (!result)
                return result.As<BitMEXOrderBook>(default);

            var data = new BitMEXOrderBook
            {
                Asks = result.Data.Where(x => x.Side == OrderSide.Sell).OrderBy(x => x.Price).ToArray(),
                Bids = result.Data.Where(x => x.Side == OrderSide.Buy).OrderByDescending(x => x.Price).ToArray()
            };

            return result.As(data);
        }

        #endregion

        #region Get Insurance

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXInsurance>>> GetInsuranceAsync(
            string? asset = null,
            SymbolFilter? symbolFilter = null,
            Dictionary<string, object>? filter = null,
            IEnumerable<string>? columns = null,
            bool? reverse = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            int? offset = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            if (asset != null)
                parameters.AddOptional("currency", asset + (symbolFilter == null ? "" : ":" + EnumConverter.GetString(symbolFilter)));
            parameters.AddOptional("filter", filter);
            parameters.AddOptional("columns", columns);
            parameters.AddOptional("count", limit);
            parameters.AddOptional("start", offset);
            parameters.Add("reverse", reverse ?? true);
            parameters.AddOptional("startTime", startTime?.ToRfc3339String());
            parameters.AddOptional("endTime", endTime?.ToRfc3339String());
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/insurance", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<IEnumerable<BitMEXInsurance>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Funding History

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXFundingRate>>> GetFundingHistoryAsync(
            string? symbol = null,
            SymbolFilter? symbolFilter = null,
            Dictionary<string, object>? filter = null,
            IEnumerable<string>? columns = null,
            bool? reverse = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? offset = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            if (symbol != null)
                parameters.AddOptional("symbol", symbol + (symbolFilter == null ? "" : ":" + EnumConverter.GetString(symbolFilter)));
            parameters.AddOptional("filter", filter);
            parameters.AddOptional("columns", columns);
            parameters.AddOptional("count", limit);
            parameters.AddOptional("start", offset);
            parameters.Add("reverse", reverse ?? true);
            parameters.AddOptional("startTime", startTime?.ToRfc3339String());
            parameters.AddOptional("endTime", endTime?.ToRfc3339String());
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/funding", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<IEnumerable<BitMEXFundingRate>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Announcements

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXAnnouncement>>> GetAnnouncementsAsync(
            IEnumerable<string>? columns = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();            
            parameters.AddOptional("columns", columns);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/announcement", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<IEnumerable<BitMEXAnnouncement>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion


        #region Get Urgent Announcements

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXAnnouncement>>> GetUrgentAnnouncementsAsync(
            IEnumerable<string>? columns = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("columns", columns);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/announcement/urgent", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<IEnumerable<BitMEXAnnouncement>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Assets

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXAsset>>> GetAssetsAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/wallet/assets", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<IEnumerable<BitMEXAsset>>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Asset Networks

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXNetwork>>> GetAssetNetworksAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/wallet/networks", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<IEnumerable<BitMEXNetwork>>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Liquidations

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXLiquidation>>> GetLiquidationsAsync(string? symbol = null,
            SymbolFilter? symbolFilter = null,
            Dictionary<string, object>? filter = null,
            IEnumerable<string>? columns = null,
            bool? reverse = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? offset = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            if (symbol != null)
                parameters.AddOptional("symbol", symbol + (symbolFilter == null ? "" : ":" + EnumConverter.GetString(symbolFilter)));
            parameters.AddOptional("filter", filter);
            parameters.AddOptional("columns", columns);
            parameters.AddOptional("count", limit);
            parameters.AddOptional("start", offset);
            parameters.Add("reverse", reverse ?? true);
            parameters.AddOptional("startTime", startTime?.ToRfc3339String());
            parameters.AddOptional("endTime", endTime?.ToRfc3339String());
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/liquidation", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<IEnumerable<BitMEXLiquidation>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

    }
}
