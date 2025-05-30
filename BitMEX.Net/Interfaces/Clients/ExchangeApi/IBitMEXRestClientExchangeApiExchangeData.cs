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
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Instrument/Instrument_getActive" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXSymbol[]>> GetActiveSymbolsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get all symbols matching the filters
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Instrument/Instrument_get" /></para>
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
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Instrument/Instrument_getActiveIntervals" /></para>
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXIntervals>> GetActiveIntervalsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get composite indexes
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Instrument/Instrument_getCompositeIndex" /></para>
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
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Instrument/Instrument_getIndices" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXIndex[]>> GetIndicesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get symbol USD volumes
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Instrument/Instrument_getUsdVolume" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXSymbolVolume[]>> GetSymbolVolumesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get public trades
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Trade/Trade_get" /></para>
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
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Trade/Trade_getBucketed" /></para>
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
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Stats/Stats_get" /></para>
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXExchangeStat[]>> GetExchangeStatsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get exchange stat history
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Stats/Stats_history" /></para>
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXExchangeStatHistory[]>> GetExchangeStatHistoryAsync(CancellationToken ct = default);

        /// <summary>
        /// Get exchange USD stats
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Stats/Stats_historyUSD" /></para>
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXExchangeStatHistoryUsd[]>> GetExchangeStatHistoryUSDAsync(CancellationToken ct = default);

        /// <summary>
        /// Get settlement history
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Settlement/Settlement_get" /></para>
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
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Quote/Quote_get" /></para>
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
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Quote/Quote_getBucketed" /></para>
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
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/OrderBook/OrderBook_getL2" /></para>
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="limit">Number of rows</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXOrderBook>> GetOrderBookAsync(string symbol, int limit, CancellationToken ct = default);

        /// <summary>
        /// Get insurance history
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Insurance/Insurance_get" /></para>
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
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Funding/Funding_get" /></para>
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
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Announcement/Announcement_get" /></para>
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXAnnouncement[]>> GetAnnouncementsAsync(
            IEnumerable<string>? columns = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get urgent announcements
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Announcement/Announcement_getUrgent" /></para>
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXAnnouncement[]>> GetUrgentAnnouncementsAsync(
            IEnumerable<string>? columns = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get assets
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Wallet/Wallet_getAssetsConfig" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXAsset[]>> GetAssetsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get asset networks
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Wallet/Wallet_getNetworksConfig" /></para>
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
