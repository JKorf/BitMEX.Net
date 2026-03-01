using CryptoExchange.Net.Objects;
using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects.Sockets;
using BitMEX.Net.Objects.Models;
using System.Collections.Generic;
using BitMEX.Net.Enums;
using CryptoExchange.Net.Interfaces.Clients;

namespace BitMEX.Net.Interfaces.Clients.ExchangeApi
{
    /// <summary>
    /// BitMEX Exchange streams
    /// </summary>
    public interface IBitMEXSocketClientExchangeApi : ISocketApiClient, IDisposable
    {
        /// <summary>
        /// Subscribe to trade updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/app/wsAPI" /><br />
        /// Endpoint:<br />
        /// WS /realtime: trade
        /// </para>
        /// </summary>
        /// <param name="symbol">Symbol name</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<BitMEXTradeUpdate[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to trade updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/app/wsAPI" /><br />
        /// Endpoint:<br />
        /// WS /realtime: trade
        /// </para>
        /// </summary>
        /// <param name="symbols">Symbols to subscribe</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BitMEXTradeUpdate[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to kline updates. Note that this subscription only pushes an update when the period is finished. Updates during a kline will not be pushed.
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/app/wsAPI" /><br />
        /// Endpoint:<br />
        /// WS /realtime: tradeBin{period}
        /// </para>
        /// </summary>
        /// <param name="symbol">Symbol to subscribe</param>
        /// <param name="period">Period</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, BinPeriod period, Action<DataEvent<BitMEXAggTrade[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to kline updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/app/wsAPI" /><br />
        /// Endpoint:<br />
        /// WS /realtime: tradeBin{period}
        /// </para>
        /// </summary>
        /// <param name="symbols">Symbols to subscribe</param>
        /// <param name="period">Period</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, BinPeriod period, Action<DataEvent<BitMEXAggTrade[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to book ticker updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/app/wsAPI" /><br />
        /// Endpoint:<br />
        /// WS /realtime: quote
        /// </para>
        /// </summary>
        /// <param name="symbol">Symbol to subscribe</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToBookTickerUpdatesAsync(string symbol, Action<DataEvent<BitMEXBookTicker>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to book ticker updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/app/wsAPI" /><br />
        /// Endpoint:<br />
        /// WS /realtime: quote
        /// </para>
        /// </summary>
        /// <param name="symbols">Symbols to subscribe</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToBookTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BitMEXBookTicker>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to aggregated book ticker updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/app/wsAPI" /><br />
        /// Endpoint:<br />
        /// WS /realtime: quoteBin
        /// </para>
        /// </summary>
        /// <param name="symbol">Symbol to subscribe</param>
        /// <param name="period">Period</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToAggregatedBookTickerUpdatesAsync(string symbol, BinPeriod period, Action<DataEvent<BitMEXBookTicker>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to aggregated book ticker updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/app/wsAPI" /><br />
        /// Endpoint:<br />
        /// WS /realtime: quoteBin
        /// </para>
        /// </summary>
        /// <param name="symbols">Symbols to subscribe</param>
        /// <param name="period">Period</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToAggregatedBookTickerUpdatesAsync(IEnumerable<string> symbols, BinPeriod period, Action<DataEvent<BitMEXBookTicker>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to settlement updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/app/wsAPI" /><br />
        /// Endpoint:<br />
        /// WS /realtime: settlement
        /// </para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSettlementUpdatesAsync(Action<DataEvent<BitMEXSettlementHistory[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to order book updated for the first 10 levels on each update
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/app/wsAPI" /><br />
        /// Endpoint:<br />
        /// WS /realtime: orderBook10
        /// </para>
        /// </summary>
        /// <param name="symbol">Symbol to subscribe</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, Action<DataEvent<BitMEXOrderBookUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to order book updated for the first 10 levels on each update
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/app/wsAPI" /><br />
        /// Endpoint:<br />
        /// WS /realtime: orderBook10
        /// </para>
        /// </summary>
        /// <param name="symbols">Symbols to subscribe</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BitMEXOrderBookUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to incremental order book updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/app/wsAPI" /><br />
        /// Endpoint:<br />
        /// WS /realtime: orderBookL2{_25}
        /// </para>
        /// </summary>
        /// <param name="symbol">Symbol to subscribe</param>
        /// <param name="limit">Limit</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToIncrementalOrderBookUpdatesAsync(string symbol, IncrementalBookLimit limit, Action<DataEvent<BitMEXOrderBookIncrementalUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to incremental order book updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/app/wsAPI" /><br />
        /// Endpoint:<br />
        /// WS /realtime: orderBookL2{_25}
        /// </para>
        /// </summary>
        /// <param name="symbols">Symbols to subscribe</param>
        /// <param name="limit">Limit</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToIncrementalOrderBookUpdatesAsync(IEnumerable<string> symbols, IncrementalBookLimit limit, Action<DataEvent<BitMEXOrderBookIncrementalUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to liquidation updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/app/wsAPI" /><br />
        /// Endpoint:<br />
        /// WS /realtime: liquidation
        /// </para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToLiquidationUpdatesAsync(Action<DataEvent<BitMEXLiquidation[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to insurance updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/app/wsAPI" /><br />
        /// Endpoint:<br />
        /// WS /realtime: insurance
        /// </para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToInsuranceUpdatesAsync(Action<DataEvent<BitMEXInsurance[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to symbol updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/app/wsAPI" /><br />
        /// Endpoint:<br />
        /// WS /realtime: instrument
        /// </para>
        /// </summary>
        /// <param name="category">Optional category</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSymbolUpdatesAsync(SymbolCategory? category, Action<DataEvent<BitMEXSymbolUpdate[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to symbol updates. Note that only changed properties will be filled; unchanged properties will be null.
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/app/wsAPI" /><br />
        /// Endpoint:<br />
        /// WS /realtime: instrument
        /// </para>
        /// </summary>
        /// <param name="symbol">Symbol to subscribe</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSymbolUpdatesAsync(string symbol, Action<DataEvent<BitMEXSymbolUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to symbol updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/app/wsAPI" /><br />
        /// Endpoint:<br />
        /// WS /realtime: instrument
        /// </para>
        /// </summary>
        /// <param name="symbols">Symbols to subscribe</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSymbolUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BitMEXSymbolUpdate[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to funding rate updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/app/wsAPI" /><br />
        /// Endpoint:<br />
        /// WS /realtime: funding
        /// </para>
        /// </summary>
        /// <param name="symbol">Symbol to subscribe</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToFundingUpdatesAsync(string symbol, Action<DataEvent<BitMEXFundingRate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to funding rate updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/app/wsAPI" /><br />
        /// Endpoint:<br />
        /// WS /realtime: funding
        /// </para>
        /// </summary>
        /// <param name="symbols">Symbols to subscribe</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToFundingUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BitMEXFundingRate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to platform announcement updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/app/wsAPI" /><br />
        /// Endpoint:<br />
        /// WS /realtimePlatform: announcement
        /// </para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToAnnouncementUpdatesAsync(Action<DataEvent<BitMEXAnnouncement[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to public short lived notifications
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/app/wsAPI" /><br />
        /// Endpoint:<br />
        /// WS /realtime: publicNotifications
        /// </para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToNotificationUpdatesAsync(Action<DataEvent<BitMEXNotification[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to user balance updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/app/wsAPI" /><br />
        /// Endpoint:<br />
        /// WS /realtime: wallet
        /// </para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(Action<DataEvent<BitMEXBalance[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to user transaction (deposit/withdrawal) updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/app/wsAPI" /><br />
        /// Endpoint:<br />
        /// WS /realtime: transact
        /// </para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTransactionUpdatesAsync(Action<DataEvent<BitMEXTransaction[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to user position updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/app/wsAPI" /><br />
        /// Endpoint:<br />
        /// WS /realtime: position
        /// </para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToPositionUpdatesAsync(Action<DataEvent<BitMEXPosition[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to user margin updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/app/wsAPI" /><br />
        /// Endpoint:<br />
        /// WS /realtime: margin
        /// </para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToMarginUpdatesAsync(Action<DataEvent<BitMEXMarginStatus>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to user order updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/app/wsAPI" /><br />
        /// Endpoint:<br />
        /// WS /realtime: order
        /// </para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(Action<DataEvent<BitMEXOrder[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to user trade updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/app/wsAPI" /><br />
        /// Endpoint:<br />
        /// WS /realtime: execution
        /// </para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(Action<DataEvent<BitMEXExecution[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Get the shared socket requests client. This interface is shared with other exhanges to allow for a common implementation for different exchanges.
        /// </summary>
        public IBitMEXSocketClientExchangeApiShared SharedClient { get; }
    }
}
