using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BitMEX.Net.Enums;
using BitMEX.Net.Objects.Models;
using CryptoExchange.Net.Objects;

namespace BitMEX.Net.Interfaces.Clients.ExchangeApi
{
    /// <summary>
    /// BitMEX Exchange exchange data endpoints. Exchange data includes market data (tickers, order books, etc) and system status.
    /// </summary>
    public interface IBitMEXRestClientExchangeApiExchangeData
    {
        /// <summary>
        /// Get server time
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<HttpResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default);

        /// <summary>
        /// Get active symbols
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Instrument/Instrument_getActive" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/instrument/active
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<BitMEXSymbol[]>> GetActiveSymbolsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get all symbols matching the filters
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Instrument/Instrument_get" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/instrument
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Filter by symbol. When sending an asset name (for example XBT) it will filter on nearest expiring contract by default, unless symbolFilter is specified</param>
        /// <param name="symbolFilter">Symbol additional filter when using an asset to filter</param>
        /// <param name="filter">["<c>filter</c>"] Filter on fields</param>
        /// <param name="columns">["<c>columns</c>"] Filter columns</param>
        /// <param name="reverse">["<c>reverse</c>"] Reverse direction</param>
        /// <param name="startTime">["<c>startTime</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>endTime</c>"] Filter by end time</param>
        /// <param name="offset">["<c>start</c>"] Result offset</param>
        /// <param name="limit">["<c>count</c>"] Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<BitMEXSymbol[]>> GetSymbolsAsync(
            string? symbol = null,
            SymbolFilter? symbolFilter = null,
            Dictionary<string, object>? filter = null,
            IEnumerable<string>? columns = null,
            bool? reverse = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? offset = null,
            int? limit = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get active intervals
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Instrument/Instrument_getActiveIntervals" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/instrument/activeIntervals
        /// </para>
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<HttpResult<BitMEXIntervals>> GetActiveIntervalsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get composite indexes
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Instrument/Instrument_getCompositeIndex" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/instrument/compositeIndex
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Filter by symbol</param>
        /// <param name="filter">["<c>filter</c>"] Filter on fields</param>
        /// <param name="columns">["<c>columns</c>"] Filter columns</param>
        /// <param name="reverse">["<c>reverse</c>"] Reverse direction</param>
        /// <param name="startTime">["<c>startTime</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>endTime</c>"] Filter by end time</param>
        /// <param name="offset">["<c>start</c>"] Result offset</param>
        /// <param name="limit">["<c>count</c>"] Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<BitMEXCompositeIndex[]>> GetCompositeIndexesAsync(
            string symbol,
            Dictionary<string, object>? filter = null,
            IEnumerable<string>? columns = null,
            bool? reverse = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? offset = null,
            int? limit = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get indices
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Instrument/Instrument_getIndices" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/instrument/indices
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<BitMEXIndex[]>> GetIndicesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get symbol USD volumes
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Instrument/Instrument_getUsdVolume" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/instrument/usdVolume
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<BitMEXSymbolVolume[]>> GetSymbolVolumesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get public trades
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Trade/Trade_get" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/trade
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Filter by symbol. When sending an asset name (for example XBT) it will filter on nearest expiring contract by default, unless symbolFilter is specified</param>
        /// <param name="symbolFilter">Symbol additional filter when using an asset to filter</param>
        /// <param name="filter">["<c>filter</c>"] Filter on fields</param>
        /// <param name="columns">["<c>columns</c>"] Filter columns</param>
        /// <param name="reverse">["<c>reverse</c>"] Reverse direction</param>
        /// <param name="startTime">["<c>startTime</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>endTime</c>"] Filter by end time</param>
        /// <param name="offset">["<c>start</c>"] Result offset</param>
        /// <param name="limit">["<c>count</c>"] Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<BitMEXTrade[]>> GetTradesAsync(
            string symbol,
            SymbolFilter? symbolFilter = null,
            Dictionary<string, object>? filter = null,
            IEnumerable<string>? columns = null,
            bool? reverse = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? offset = null,
            int? limit = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get klines
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Trade/Trade_getBucketed" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/trade/bucketed
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Filter by symbol. When sending an asset name (for example XBT) it will filter on nearest expiring contract by default, unless symbolFilter is specified</param>
        /// <param name="symbolFilter">Symbol additional filter when using an asset to filter</param>
        /// <param name="filter">["<c>filter</c>"] Filter on fields</param>
        /// <param name="period">["<c>binSize</c>"] Period to aggregate by</param>
        /// <param name="partial">["<c>partial</c>"] Whether to include the current unfished period</param>
        /// <param name="columns">["<c>columns</c>"] Filter columns</param>
        /// <param name="reverse">["<c>reverse</c>"] Reverse direction</param>
        /// <param name="startTime">["<c>startTime</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>endTime</c>"] Filter by end time</param>
        /// <param name="offset">["<c>start</c>"] Result offset</param>
        /// <param name="limit">["<c>count</c>"] Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<BitMEXAggTrade[]>> GetKlinesAsync(
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
            CancellationToken ct = default);

        /// <summary>
        /// Get exchange stats
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Stats/Stats_get" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/stats
        /// </para>
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<HttpResult<BitMEXExchangeStat[]>> GetExchangeStatsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get exchange stat history
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Stats/Stats_history" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/stats/history
        /// </para>
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<HttpResult<BitMEXExchangeStatHistory[]>> GetExchangeStatHistoryAsync(CancellationToken ct = default);

        /// <summary>
        /// Get exchange USD stats
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Stats/Stats_historyUSD" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/stats/historyUSD
        /// </para>
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<HttpResult<BitMEXExchangeStatHistoryUsd[]>> GetExchangeStatHistoryUSDAsync(CancellationToken ct = default);

        /// <summary>
        /// Get settlement history
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Settlement/Settlement_get" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/settlement
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Filter by symbol. When sending an asset name (for example XBT) it will filter on nearest expiring contract by default, unless symbolFilter is specified</param>
        /// <param name="symbolFilter">Symbol additional filter when using an asset to filter</param>
        /// <param name="filter">["<c>filter</c>"] Filter on fields</param>
        /// <param name="columns">["<c>columns</c>"] Filter columns</param>
        /// <param name="reverse">["<c>reverse</c>"] Reverse direction</param>
        /// <param name="startTime">["<c>startTime</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>endTime</c>"] Filter by end time</param>
        /// <param name="offset">["<c>start</c>"] Result offset</param>
        /// <param name="limit">["<c>count</c>"] Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<BitMEXSettlementHistory[]>> GetSettlementHistoryAsync(
            string symbol,
            SymbolFilter? symbolFilter = null,
            Dictionary<string, object>? filter = null,
            IEnumerable<string>? columns = null,
            bool? reverse = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? offset = null,
            int? limit = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get book ticker history
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Quote/Quote_get" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/quote
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Filter by symbol. When sending an asset name (for example XBT) it will filter on nearest expiring contract by default, unless symbolFilter is specified</param>
        /// <param name="symbolFilter">Symbol additional filter when using an asset to filter</param>
        /// <param name="filter">["<c>filter</c>"] Filter on fields</param>
        /// <param name="columns">["<c>columns</c>"] Filter columns</param>
        /// <param name="reverse">["<c>reverse</c>"] Reverse direction</param>
        /// <param name="startTime">["<c>startTime</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>endTime</c>"] Filter by end time</param>
        /// <param name="offset">["<c>start</c>"] Result offset</param>
        /// <param name="limit">["<c>count</c>"] Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<BitMEXBookTicker[]>> GetBookTickerHistoryAsync(
            string symbol,
            SymbolFilter? symbolFilter = null,
            Dictionary<string, object>? filter = null,
            IEnumerable<string>? columns = null,
            bool? reverse = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? offset = null,
            int? limit = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get aggregated book ticker history
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Quote/Quote_getBucketed" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/quote/bucketed
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Filter by symbol. When sending an asset name (for example XBT) it will filter on nearest expiring contract by default, unless symbolFilter is specified</param>
        /// <param name="symbolFilter">Symbol additional filter when using an asset to filter</param>
        /// <param name="filter">["<c>filter</c>"] Filter on fields</param>
        /// <param name="period">["<c>binSize</c>"] Period to aggregate by</param>
        /// <param name="partial">["<c>partial</c>"] Whether to include the current unfished period</param>
        /// <param name="columns">["<c>columns</c>"] Filter columns</param>
        /// <param name="reverse">["<c>reverse</c>"] Reverse direction</param>
        /// <param name="startTime">["<c>startTime</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>endTime</c>"] Filter by end time</param>
        /// <param name="offset">["<c>start</c>"] Result offset</param>
        /// <param name="limit">["<c>count</c>"] Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<BitMEXBookTicker[]>> GetAggregatedBookTickerHistoryAsync(
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
            CancellationToken ct = default);

        /// <summary>
        /// Get order book
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/OrderBook/OrderBook_getL2" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/orderBook/L2
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Symbol</param>
        /// <param name="limit">["<c>depth</c>"] Number of rows</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<BitMEXOrderBook>> GetOrderBookAsync(string symbol, int limit, CancellationToken ct = default);

        /// <summary>
        /// Get insurance history
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Insurance/Insurance_get" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/insurance
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>currency</c>"] Filter by asset. When sending an asset name (for example XBT) it will filter on nearest expiring contract by default, unless symbolFilter is specified</param>
        /// <param name="symbolFilter">Symbol additional filter when using an asset to filter</param>
        /// <param name="filter">["<c>filter</c>"] Filter on fields</param>
        /// <param name="columns">["<c>columns</c>"] Filter columns</param>
        /// <param name="reverse">["<c>reverse</c>"] Reverse direction</param>
        /// <param name="startTime">["<c>startTime</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>endTime</c>"] Filter by end time</param>
        /// <param name="offset">["<c>start</c>"] Result offset</param>
        /// <param name="limit">["<c>count</c>"] Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<BitMEXInsurance[]>> GetInsuranceAsync(
            string asset,
            SymbolFilter? symbolFilter = null,
            Dictionary<string, object>? filter = null,
            IEnumerable<string>? columns = null,
            bool? reverse = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? offset = null,
            int? limit = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get funding rate history
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Funding/Funding_get" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/funding
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Filter by symbol. When sending an asset name (for example XBT) it will filter on nearest expiring contract by default, unless symbolFilter is specified</param>
        /// <param name="symbolFilter">Symbol additional filter when using an asset to filter</param>
        /// <param name="filter">["<c>filter</c>"] Filter on fields</param>
        /// <param name="columns">["<c>columns</c>"] Filter columns</param>
        /// <param name="reverse">["<c>reverse</c>"] Reverse direction</param>
        /// <param name="startTime">["<c>startTime</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>endTime</c>"] Filter by end time</param>
        /// <param name="offset">["<c>start</c>"] Result offset</param>
        /// <param name="limit">["<c>count</c>"] Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<BitMEXFundingRate[]>> GetFundingHistoryAsync(
            string? symbol = null,
            SymbolFilter? symbolFilter = null,
            Dictionary<string, object>? filter = null,
            IEnumerable<string>? columns = null,
            bool? reverse = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? offset = null,
            int? limit = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get public announcements
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Announcement/Announcement_get" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/announcement
        /// </para>
        /// </summary>
        /// <param name="columns">["<c>columns</c>"]</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<HttpResult<BitMEXAnnouncement[]>> GetAnnouncementsAsync(
            IEnumerable<string>? columns = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get urgent announcements
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Announcement/Announcement_getUrgent" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/announcement/urgent
        /// </para>
        /// </summary>
        /// <param name="columns">["<c>columns</c>"]</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<HttpResult<BitMEXAnnouncement[]>> GetUrgentAnnouncementsAsync(
            IEnumerable<string>? columns = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get assets
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Wallet/Wallet_getAssetsConfig" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/wallet/assets
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<BitMEXAsset[]>> GetAssetsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get asset networks
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Wallet/Wallet_getNetworksConfig" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/wallet/networks
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<BitMEXNetwork[]>> GetAssetNetworksAsync(CancellationToken ct = default);

        /// <summary>
        /// Get liquidations
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Filter by symbol. When sending an asset name (for example XBT) it will filter on nearest expiring contract by default, unless symbolFilter is specified</param>
        /// <param name="symbolFilter">Symbol additional filter when using an asset to filter</param>
        /// <param name="filter">["<c>filter</c>"] Filter on fields</param>
        /// <param name="columns">["<c>columns</c>"] Filter columns</param>
        /// <param name="reverse">["<c>reverse</c>"] Reverse direction</param>
        /// <param name="startTime">["<c>startTime</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>endTime</c>"] Filter by end time</param>
        /// <param name="offset">["<c>start</c>"] Result offset</param>
        /// <param name="limit">["<c>count</c>"] Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<BitMEXLiquidation[]>> GetLiquidationsAsync(string? symbol = null,
            SymbolFilter? symbolFilter = null,
            Dictionary<string, object>? filter = null,
            IEnumerable<string>? columns = null,
            bool? reverse = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? offset = null,
            int? limit = null,
            CancellationToken ct = default);
    }
}
