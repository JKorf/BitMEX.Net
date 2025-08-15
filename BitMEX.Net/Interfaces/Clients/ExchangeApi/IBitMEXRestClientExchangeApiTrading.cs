using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using BitMEX.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using BitMEX.Net.Enums;

namespace BitMEX.Net.Interfaces.Clients.ExchangeApi
{
    /// <summary>
    /// BitMEX Exchange trading endpoints, placing and managing orders.
    /// </summary>
    public interface IBitMEXRestClientExchangeApiTrading
    {
        /// <summary>
        /// Get user trade history by day
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/User/User_getExecutionHistory" /></para>
        /// </summary>
        /// <param name="symbol">Symbol name</param>
        /// <param name="day">Day</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXExecution[]>> GetExecutionHistoryByDayAsync(string symbol, DateTime day, CancellationToken ct = default);

        /// <summary>
        /// Place a new order
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Order/Order_new" /></para>
        /// </summary>
        /// <param name="symbol">Symbol, for example `XBTUSD`</param>
        /// <param name="orderSide">Order side</param>
        /// <param name="orderType">Order type</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="price">Limit price</param>
        /// <param name="timeInForce">Time in force</param>
        /// <param name="executionInstruction">Execution instructions</param>
        /// <param name="contingencyType">Contingency type</param>
        /// <param name="displayQuantity">Display quantity</param>
        /// <param name="stopPrice">Stop price</param>
        /// <param name="pegOffsetValue">Peg offset value</param>
        /// <param name="pegPriceType">Peg price type</param>
        /// <param name="clientOrderLinkId">Client order link id</param>
        /// <param name="clientOrderId">Client order id</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXOrder>> PlaceOrderAsync(
            string symbol,
            OrderSide orderSide,
            OrderType orderType,
            long? quantity = null,
            decimal? price = null,
            TimeInForce? timeInForce = null,
            ExecutionInstruction? executionInstruction = null,
            ContingencyType? contingencyType = null,
            long? displayQuantity = null,
            decimal? stopPrice = null,
            decimal? pegOffsetValue = null,
            PeggedPriceType? pegPriceType = null,
            string? clientOrderLinkId = null,
            string? clientOrderId = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get user orders
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Order/Order_getOrders" /></para>
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
        Task<WebCallResult<BitMEXOrder[]>> GetOrdersAsync(
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
        /// Edit an active order
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Order/Order_amend" /></para>
        /// </summary>
        /// <param name="orderId">Order id. Either this or clientOrderId should be provided</param>
        /// <param name="origClientOrderId">Client order id. Either this or orderId should be provided</param>
        /// <param name="newClientOrderId">New client order id</param>
        /// <param name="quantity">New total order quantity</param>
        /// <param name="quantityRemaining">New order quantity remaining to execute</param>
        /// <param name="price">New price</param>
        /// <param name="stopPrice">New stop price</param>
        /// <param name="pegOffsetValue">New peg offset value</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXOrder>> EditOrderAsync(
            string? orderId = null,
            string? origClientOrderId = null,
            string? newClientOrderId = null,
            long? quantity = null,
            long? quantityRemaining = null,
            decimal? price = null,
            decimal? stopPrice = null,
            decimal? pegOffsetValue = null,
            CancellationToken ct = default);

        /// <summary>
        /// Cancel an order
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Order/Order_cancel" /></para>
        /// </summary>
        /// <param name="orderId">Order id. Either this or clientOrderId should be provided</param>
        /// <param name="clientOrderId">Client order id. Either this or orderId should be provided</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXOrder>> CancelOrderAsync(
            string? orderId = null,
            string? clientOrderId = null,
            CancellationToken ct = default);

        /// <summary>
        /// Cancel orders
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Order/Order_cancel" /></para>
        /// </summary>
        /// <param name="orderIds">Order ids. Either this or clientOrderIds should be provided</param>
        /// <param name="clientOrderIds">Client order ids. Either this or orderIds should be provided</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXOrder[]>> CancelOrdersAsync(
           IEnumerable<string>? orderIds = null,
           IEnumerable<string>? clientOrderIds = null,
           CancellationToken ct = default);

        /// <summary>
        /// Cancel all orders matching the parameters
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Order/Order_cancelAll" /></para>
        /// </summary>
        /// <param name="targetAccountIds">Account ids</param>
        /// <param name="symbol">Filter by symbol</param>
        /// <param name="filter">Filter orders to cancel</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXOrder[]>> CancelAllOrdersAsync(
            IEnumerable<long>? targetAccountIds = null,
            string? symbol = null,
            Dictionary<string, object>? filter = null,
            CancellationToken ct = default);

        /// <summary>
        /// Cancel all orders after the timeout passes. Acts as a dead-man switch. Use TimeSpan.Zero to cancel timeout
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Order/Order_cancelAllAfter" /></para>
        /// </summary>
        /// <param name="timeout">Timeout after which to cancel all orders. Use TimeSpan.Zero to cancel timeout</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult> CancelAllAfterAsync(TimeSpan timeout, CancellationToken ct = default);

        /// <summary>
        /// Get raw user execution history
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Execution/Execution_get" /></para>
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
        Task<WebCallResult<BitMEXExecution[]>> GetUserExecutionsAsync(
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
        /// Get user trade history
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Execution/Execution_getTradeHistory" /></para>
        /// </summary>
        /// <param name="targetAccountIds">Target account ids</param>
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
        Task<WebCallResult<BitMEXExecution[]>> GetUserTradesAsync(
            IEnumerable<long>? targetAccountIds = null,
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
        /// Get positions
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Position/Position_get" /></para>
        /// </summary>
        /// <param name="filter">Filter on fields</param>
        /// <param name="columns">Filter columns</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXPosition[]>> GetPositionsAsync(
            Dictionary<string, object>? filter = null,
            IEnumerable<string>? columns = null,
            int? limit = null,
            CancellationToken ct = default);

        /// <summary>
        /// Set cross margin leverage. Automatically enables cross margin.
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Position/Position_updateCrossLeverage" /></para>
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="leverage">New leverage</param>
        /// <param name="targetAccountId">Target account id</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXPosition>> SetCrossMarginLeverageAsync(
            string symbol,
            decimal leverage,
            long? targetAccountId = null,
            CancellationToken ct = default);

        /// <summary>
        /// Set isolated margin leverage. Automatically enables isolated margin
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Position/Position_updateLeverage" /></para>
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="leverage">New leverage</param>
        /// <param name="targetAccountId">Target account id</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXPosition>> SetIsolatedMarginLeverageAsync(
            string symbol,
            decimal leverage,
            long? targetAccountId = null,
            CancellationToken ct = default);
    }
}
