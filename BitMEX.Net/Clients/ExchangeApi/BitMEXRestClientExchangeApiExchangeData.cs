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
        public async Task<HttpResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "api/v1", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            var result = await _baseClient.SendAsync<BitMEXServerTime>(request, null, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<DateTime>(result);

            return HttpResult.Ok(result, result.Data.Timestamp);
        }

        #endregion

        #region Get Active Symbols

        /// <inheritdoc />
        public async Task<HttpResult<BitMEXSymbol[]>> GetActiveSymbolsAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "api/v1/instrument/active", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<BitMEXSymbol[]>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get All Symbols

        /// <inheritdoc />
        public async Task<HttpResult<BitMEXSymbol[]>> GetSymbolsAsync(
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
            var parameters = new Parameters(BitMEXExchange._parameterSerializationSettings);
            if (symbol != null)
                parameters.Add("symbol", symbol + (symbolFilter == null ? "" : ":" + EnumConverter.GetString(symbolFilter)));
            parameters.AddRaw("filter", filter);
            parameters.AddArray("columns", columns);
            parameters.Add("count", limit);
            parameters.Add("start", offset);
            parameters.Add("reverse", reverse);
            parameters.Add("startTime", startTime?.ToRfc3339String());
            parameters.Add("endTime", endTime?.ToRfc3339String());
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "api/v1/instrument", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<BitMEXSymbol[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Active Intervals

        /// <inheritdoc />
        public async Task<HttpResult<BitMEXIntervals>> GetActiveIntervalsAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "api/v1/instrument/activeIntervals", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<BitMEXIntervals>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Composite Indexes

        /// <inheritdoc />
        public async Task<HttpResult<BitMEXCompositeIndex[]>> GetCompositeIndexesAsync(
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
            var parameters = new Parameters(BitMEXExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.AddRaw("filter", filter);
            parameters.AddArray("columns", columns);
            parameters.Add("count", limit);
            parameters.Add("start", offset);
            parameters.Add("reverse", reverse);
            parameters.Add("startTime", startTime?.ToRfc3339String());
            parameters.Add("endTime", endTime?.ToRfc3339String());

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "api/v1/instrument/compositeIndex", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<BitMEXCompositeIndex[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Indices

        /// <inheritdoc />
        public async Task<HttpResult<BitMEXIndex[]>> GetIndicesAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "api/v1/instrument/indices", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<BitMEXIndex[]>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Symbol Volumes

        /// <inheritdoc />
        public async Task<HttpResult<BitMEXSymbolVolume[]>> GetSymbolVolumesAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "api/v1/instrument/usdVolume", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<BitMEXSymbolVolume[]>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Trades

        /// <inheritdoc />
        public async Task<HttpResult<BitMEXTrade[]>> GetTradesAsync(
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
            var parameters = new Parameters(BitMEXExchange._parameterSerializationSettings);
            if (symbol != null)
                parameters.Add("symbol", symbol + (symbolFilter == null ? "" : ":" + EnumConverter.GetString(symbolFilter)));
            parameters.AddRaw("filter", filter);
            parameters.AddArray("columns", columns);
            parameters.Add("count", limit);
            parameters.Add("reverse", reverse ?? true);
            parameters.Add("startTime", startTime?.ToRfc3339String());
            parameters.Add("endTime", endTime?.ToRfc3339String());
            parameters.Add("start", offset);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "api/v1/trade", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<BitMEXTrade[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Klines

        /// <inheritdoc />
        public async Task<HttpResult<BitMEXAggTrade[]>> GetKlinesAsync(
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
            var parameters = new Parameters(BitMEXExchange._parameterSerializationSettings);
            if (symbol != null)
                parameters.Add("symbol", symbol + (symbolFilter == null ? "" : ":" + EnumConverter.GetString(symbolFilter)));
            parameters.Add("binSize", period);
            parameters.Add("partial", partial);
            parameters.AddRaw("filter", filter);
            parameters.AddArray("columns", columns);
            parameters.Add("count", limit);
            parameters.Add("start", offset);
            parameters.Add("reverse", reverse ?? true);
            parameters.Add("startTime", startTime?.ToRfc3339String());
            parameters.Add("endTime", endTime?.ToRfc3339String());

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "api/v1/trade/bucketed", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<BitMEXAggTrade[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Exchange Stats

        /// <inheritdoc />
        public async Task<HttpResult<BitMEXExchangeStat[]>> GetExchangeStatsAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(BitMEXExchange._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "api/v1/stats", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<BitMEXExchangeStat[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Exchange Stat History

        /// <inheritdoc />
        public async Task<HttpResult<BitMEXExchangeStatHistory[]>> GetExchangeStatHistoryAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(BitMEXExchange._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "api/v1/stats/history", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<BitMEXExchangeStatHistory[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Exchange Stat History USD

        /// <inheritdoc />
        public async Task<HttpResult<BitMEXExchangeStatHistoryUsd[]>> GetExchangeStatHistoryUSDAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(BitMEXExchange._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "api/v1/stats/historyUSD", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<BitMEXExchangeStatHistoryUsd[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Settlement History

        /// <inheritdoc />
        public async Task<HttpResult<BitMEXSettlementHistory[]>> GetSettlementHistoryAsync(
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
            var parameters = new Parameters(BitMEXExchange._parameterSerializationSettings);
            if (symbol != null)
                parameters.Add("symbol", symbol + (symbolFilter == null ? "" : ":" + EnumConverter.GetString(symbolFilter)));
            parameters.AddRaw("filter", filter);
            parameters.AddArray("columns", columns);
            parameters.Add("count", limit);
            parameters.Add("start", offset);
            parameters.Add("reverse", reverse ?? true);
            parameters.Add("startTime", startTime?.ToRfc3339String());
            parameters.Add("endTime", endTime?.ToRfc3339String());
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "api/v1/settlement", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<BitMEXSettlementHistory[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Book Ticker History

        /// <inheritdoc />
        public async Task<HttpResult<BitMEXBookTicker[]>> GetBookTickerHistoryAsync(
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
            var parameters = new Parameters(BitMEXExchange._parameterSerializationSettings);
            if (symbol != null)
                parameters.Add("symbol", symbol + (symbolFilter == null ? "" : ":" + EnumConverter.GetString(symbolFilter)));
            parameters.AddRaw("filter", filter);
            parameters.AddArray("columns", columns);
            parameters.Add("count", limit);
            parameters.Add("start", offset);
            parameters.Add("reverse", reverse ?? true);
            parameters.Add("startTime", startTime?.ToRfc3339String());
            parameters.Add("endTime", endTime?.ToRfc3339String());
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "api/v1/quote", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<BitMEXBookTicker[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Aggregated Book Ticker

        /// <inheritdoc />
        public async Task<HttpResult<BitMEXBookTicker[]>> GetAggregatedBookTickerHistoryAsync(
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
            var parameters = new Parameters(BitMEXExchange._parameterSerializationSettings);
            if (symbol != null)
                parameters.Add("symbol", symbol + (symbolFilter == null ? "" : ":" + EnumConverter.GetString(symbolFilter)));
            parameters.Add("binSize", period);
            parameters.Add("partial", partial);
            parameters.AddRaw("filter", filter);
            parameters.AddArray("columns", columns);
            parameters.Add("count", limit);
            parameters.Add("start", offset);
            parameters.Add("reverse", reverse ?? true);
            parameters.Add("startTime", startTime?.ToRfc3339String());
            parameters.Add("endTime", endTime?.ToRfc3339String());
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "api/v1/quote/bucketed", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<BitMEXBookTicker[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Order Book

        /// <inheritdoc />
        public async Task<HttpResult<BitMEXOrderBook>> GetOrderBookAsync(string symbol, int limit, CancellationToken ct = default)
        {
            var parameters = new Parameters(BitMEXExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("depth", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "api/v1/orderBook/L2", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            var result = await _baseClient.SendAsync<BitMEXOrderBookEntry[]>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BitMEXOrderBook>(result);

            var data = new BitMEXOrderBook
            {
                Asks = result.Data.Where(x => x.Side == OrderSide.Sell).OrderBy(x => x.Price).ToArray(),
                Bids = result.Data.Where(x => x.Side == OrderSide.Buy).OrderByDescending(x => x.Price).ToArray()
            };

            return HttpResult.Ok(result, data);
        }

        #endregion

        #region Get Insurance

        /// <inheritdoc />
        public async Task<HttpResult<BitMEXInsurance[]>> GetInsuranceAsync(
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
            var parameters = new Parameters(BitMEXExchange._parameterSerializationSettings);
            if (asset != null)
                parameters.Add("currency", asset + (symbolFilter == null ? "" : ":" + EnumConverter.GetString(symbolFilter)));
            parameters.AddRaw("filter", filter);
            parameters.AddArray("columns", columns);
            parameters.Add("count", limit);
            parameters.Add("start", offset);
            parameters.Add("reverse", reverse ?? true);
            parameters.Add("startTime", startTime?.ToRfc3339String());
            parameters.Add("endTime", endTime?.ToRfc3339String());
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "api/v1/insurance", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<BitMEXInsurance[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Funding History

        /// <inheritdoc />
        public async Task<HttpResult<BitMEXFundingRate[]>> GetFundingHistoryAsync(
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
            var parameters = new Parameters(BitMEXExchange._parameterSerializationSettings);
            if (symbol != null)
                parameters.Add("symbol", symbol + (symbolFilter == null ? "" : ":" + EnumConverter.GetString(symbolFilter)));
            parameters.AddRaw("filter", filter);
            parameters.AddArray("columns", columns);
            parameters.Add("count", limit);
            parameters.Add("start", offset);
            parameters.Add("reverse", reverse ?? true);
            parameters.Add("startTime", startTime?.ToRfc3339String());
            parameters.Add("endTime", endTime?.ToRfc3339String());
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "api/v1/funding", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<BitMEXFundingRate[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Announcements

        /// <inheritdoc />
        public async Task<HttpResult<BitMEXAnnouncement[]>> GetAnnouncementsAsync(
            IEnumerable<string>? columns = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BitMEXExchange._parameterSerializationSettings);
            parameters.AddArray("columns", columns);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "api/v1/announcement", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<BitMEXAnnouncement[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Urgent Announcements

        /// <inheritdoc />
        public async Task<HttpResult<BitMEXAnnouncement[]>> GetUrgentAnnouncementsAsync(
            IEnumerable<string>? columns = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BitMEXExchange._parameterSerializationSettings);
            parameters.AddArray("columns", columns);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "api/v1/announcement/urgent", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<BitMEXAnnouncement[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Assets

        /// <inheritdoc />
        public async Task<HttpResult<BitMEXAsset[]>> GetAssetsAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "api/v1/wallet/assets", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<BitMEXAsset[]>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Asset Networks

        /// <inheritdoc />
        public async Task<HttpResult<BitMEXNetwork[]>> GetAssetNetworksAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "api/v1/wallet/networks", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<BitMEXNetwork[]>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Liquidations

        /// <inheritdoc />
        public async Task<HttpResult<BitMEXLiquidation[]>> GetLiquidationsAsync(string? symbol = null,
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
            var parameters = new Parameters(BitMEXExchange._parameterSerializationSettings);
            if (symbol != null)
                parameters.Add("symbol", symbol + (symbolFilter == null ? "" : ":" + EnumConverter.GetString(symbolFilter)));
            parameters.AddRaw("filter", filter);
            parameters.AddArray("columns", columns);
            parameters.Add("count", limit);
            parameters.Add("start", offset);
            parameters.Add("reverse", reverse ?? true);
            parameters.Add("startTime", startTime?.ToRfc3339String());
            parameters.Add("endTime", endTime?.ToRfc3339String());
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "api/v1/liquidation", BitMEXExchange.RateLimiter.BitMEX, 1, false);
            return await _baseClient.SendAsync<BitMEXLiquidation[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

    }
}
