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
        Task<WebCallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default);

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
        Task<WebCallResult<BitMEXSymbol[]>> GetActiveSymbolsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get all symbols matching the filters
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Instrument/Instrument_get" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/instrument
        /// </para>
        /// </summary>
        /// <param name="symbol">Filter by symbol. When sending an asset name (for example XBT) it will filter on nearest expiring contract by default, unless symbolFilter is specified</param>
        /// <param name="symbolFilter">Symbol additional filter when using an asset to filter</param>
        /// <param name="filter">Filter on fields</param>
        /// <param name="columns">Filter columns</param>
        /// <param name="reverse">Reverse direction</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="offset">Result offset</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXSymbol[]>> GetSymbolsAsync(
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
        Task<WebCallResult<BitMEXIntervals>> GetActiveIntervalsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get composite indexes
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Instrument/Instrument_getCompositeIndex" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/instrument/compositeIndex
        /// </para>
        /// </summary>
        /// <param name="symbol">Filter by symbol</param>
        /// <param name="filter">Filter on fields</param>
        /// <param name="columns">Filter columns</param>
        /// <param name="reverse">Reverse direction</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="offset">Result offset</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXCompositeIndex[]>> GetCompositeIndexesAsync(
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
        Task<WebCallResult<BitMEXIndex[]>> GetIndicesAsync(CancellationToken ct = default);

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
        Task<WebCallResult<BitMEXSymbolVolume[]>> GetSymbolVolumesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get public trades
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Trade/Trade_get" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/trade
        /// </para>
        /// </summary>
        /// <param name="symbol">Filter by symbol. When sending an asset name (for example XBT) it will filter on nearest expiring contract by default, unless symbolFilter is specified</param>
        /// <param name="symbolFilter">Symbol additional filter when using an asset to filter</param>
        /// <param name="filter">Filter on fields</param>
        /// <param name="columns">Filter columns</param>
        /// <param name="reverse">Reverse direction</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="offset">Result offset</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXTrade[]>> GetTradesAsync(
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
        /// <param name="symbol">Filter by symbol. When sending an asset name (for example XBT) it will filter on nearest expiring contract by default, unless symbolFilter is specified</param>
        /// <param name="symbolFilter">Symbol additional filter when using an asset to filter</param>
        /// <param name="filter">Filter on fields</param>
        /// <param name="period">Period to aggregate by</param>
        /// <param name="partial">Whether to include the current unfished period</param>
        /// <param name="columns">Filter columns</param>
        /// <param name="reverse">Reverse direction</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="offset">Result offset</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXAggTrade[]>> GetKlinesAsync(
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
        Task<WebCallResult<BitMEXExchangeStat[]>> GetExchangeStatsAsync(CancellationToken ct = default);

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
        Task<WebCallResult<BitMEXExchangeStatHistory[]>> GetExchangeStatHistoryAsync(CancellationToken ct = default);

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
        Task<WebCallResult<BitMEXExchangeStatHistoryUsd[]>> GetExchangeStatHistoryUSDAsync(CancellationToken ct = default);

        /// <summary>
        /// Get settlement history
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Settlement/Settlement_get" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/settlement
        /// </para>
        /// </summary>
        /// <param name="symbol">Filter by symbol. When sending an asset name (for example XBT) it will filter on nearest expiring contract by default, unless symbolFilter is specified</param>
        /// <param name="symbolFilter">Symbol additional filter when using an asset to filter</param>
        /// <param name="filter">Filter on fields</param>
        /// <param name="columns">Filter columns</param>
        /// <param name="reverse">Reverse direction</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="offset">Result offset</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXSettlementHistory[]>> GetSettlementHistoryAsync(
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
        /// <param name="symbol">Filter by symbol. When sending an asset name (for example XBT) it will filter on nearest expiring contract by default, unless symbolFilter is specified</param>
        /// <param name="symbolFilter">Symbol additional filter when using an asset to filter</param>
        /// <param name="filter">Filter on fields</param>
        /// <param name="columns">Filter columns</param>
        /// <param name="reverse">Reverse direction</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="offset">Result offset</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXBookTicker[]>> GetBookTickerHistoryAsync(
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
        /// <param name="symbol">Filter by symbol. When sending an asset name (for example XBT) it will filter on nearest expiring contract by default, unless symbolFilter is specified</param>
        /// <param name="symbolFilter">Symbol additional filter when using an asset to filter</param>
        /// <param name="filter">Filter on fields</param>
        /// <param name="period">Period to aggregate by</param>
        /// <param name="partial">Whether to include the current unfished period</param>
        /// <param name="columns">Filter columns</param>
        /// <param name="reverse">Reverse direction</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="offset">Result offset</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXBookTicker[]>> GetAggregatedBookTickerHistoryAsync(
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
        /// <param name="symbol">Symbol</param>
        /// <param name="limit">Number of rows</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXOrderBook>> GetOrderBookAsync(string symbol, int limit, CancellationToken ct = default);

        /// <summary>
        /// Get insurance history
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Insurance/Insurance_get" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/insurance
        /// </para>
        /// </summary>
        /// <param name="asset">Filter by asset. When sending an asset name (for example XBT) it will filter on nearest expiring contract by default, unless symbolFilter is specified</param>
        /// <param name="symbolFilter">Symbol additional filter when using an asset to filter</param>
        /// <param name="filter">Filter on fields</param>
        /// <param name="columns">Filter columns</param>
        /// <param name="reverse">Reverse direction</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="offset">Result offset</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXInsurance[]>> GetInsuranceAsync(
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
        /// <param name="symbol">Filter by symbol. When sending an asset name (for example XBT) it will filter on nearest expiring contract by default, unless symbolFilter is specified</param>
        /// <param name="symbolFilter">Symbol additional filter when using an asset to filter</param>
        /// <param name="filter">Filter on fields</param>
        /// <param name="columns">Filter columns</param>
        /// <param name="reverse">Reverse direction</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="offset">Result offset</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXFundingRate[]>> GetFundingHistoryAsync(
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
        /// <param name="columns"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXAnnouncement[]>> GetAnnouncementsAsync(
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
        /// <param name="columns"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXAnnouncement[]>> GetUrgentAnnouncementsAsync(
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
        Task<WebCallResult<BitMEXAsset[]>> GetAssetsAsync(CancellationToken ct = default);

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
        Task<WebCallResult<BitMEXNetwork[]>> GetAssetNetworksAsync(CancellationToken ct = default);

        /// <summary>
        /// Get liquidations
        /// </summary>
        /// <param name="symbol">Filter by symbol. When sending an asset name (for example XBT) it will filter on nearest expiring contract by default, unless symbolFilter is specified</param>
        /// <param name="symbolFilter">Symbol additional filter when using an asset to filter</param>
        /// <param name="filter">Filter on fields</param>
        /// <param name="columns">Filter columns</param>
        /// <param name="reverse">Reverse direction</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="offset">Result offset</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXLiquidation[]>> GetLiquidationsAsync(string? symbol = null,
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
