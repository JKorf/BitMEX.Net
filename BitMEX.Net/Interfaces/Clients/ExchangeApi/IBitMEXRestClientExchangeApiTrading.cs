using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using BitMEX.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using BitMEX.Net.Enums;
using BitMEX.Net.ExtensionMethods;

namespace BitMEX.Net.Interfaces.Clients.ExchangeApi
{
    /// <summary>
    /// BitMEX Exchange trading endpoints, placing and managing orders.
    /// </summary>
    public interface IBitMEXRestClientExchangeApiTrading
    {
        /// <summary>
        /// Get user trade history by day
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/User/User_getExecutionHistory" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/user/executionHistory
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Symbol name</param>
        /// <param name="day">["<c>timestamp</c>"] Day</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXExecution[]>> GetExecutionHistoryByDayAsync(string symbol, DateTime day, CancellationToken ct = default);

        /// <summary>
        /// Place a new order
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Order/Order_new" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/order
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Symbol, for example `XBTUSD` for perps or `XBT_USDT` for spot</param>
        /// <param name="orderSide">["<c>side</c>"] Order side</param>
        /// <param name="orderType">["<c>ordType</c>"] Order type</param>
        /// <param name="quantity">["<c>orderQty</c>"] Quantity. Note that this is in base units. <br /> 
        /// For example use 1000(gwei) for trading 0.000001(eth).<br />
        /// Conversion is available using <see cref="BitMEXExtensionMethods.ToBitMEXAssetQuantity(decimal, string)"/>: 0.000001m.ToBitMEXAssetQuantity("ETH"). This requires <see cref="BitMEXUtils.UpdateSymbolInfoAsync" /> to be called beforehand.</param>
        /// <param name="price">["<c>price</c>"] Limit price</param>
        /// <param name="timeInForce">["<c>timeInForce</c>"] Time in force</param>
        /// <param name="executionInstruction">["<c>execInst</c>"] Execution instructions</param>
        /// <param name="contingencyType">["<c>contingencyType</c>"] Contingency type</param>
        /// <param name="displayQuantity">["<c>displayQty</c>"] Display quantity</param>
        /// <param name="stopPrice">["<c>stopPx</c>"] Stop price</param>
        /// <param name="pegOffsetValue">["<c>pegOffsetValue</c>"] Peg offset value</param>
        /// <param name="pegPriceType">["<c>pegPriceType</c>"] Peg price type</param>
        /// <param name="clientOrderLinkId">["<c>clOrdLinkID</c>"] Client order link id</param>
        /// <param name="clientOrderId">["<c>clOrdID</c>"] Client order id</param>
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
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Order/Order_getOrders" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/order
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
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Order/Order_amend" /><br />
        /// Endpoint:<br />
        /// PUT /api/v1/order
        /// </para>
        /// </summary>
        /// <param name="orderId">["<c>orderID</c>"] Order id. Either this or clientOrderId should be provided</param>
        /// <param name="origClientOrderId">["<c>origClOrdID</c>"] Client order id. Either this or orderId should be provided</param>
        /// <param name="newClientOrderId">["<c>clOrdID</c>"] New client order id</param>
        /// <param name="quantity">["<c>orderQty</c>"] New total order quantity</param>
        /// <param name="quantityRemaining">["<c>leavesQty</c>"] New order quantity remaining to execute</param>
        /// <param name="price">["<c>price</c>"] New price</param>
        /// <param name="stopPrice">["<c>stopPx</c>"] New stop price</param>
        /// <param name="pegOffsetValue">["<c>pegOffsetValue</c>"] New peg offset value</param>
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
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Order/Order_cancel" /><br />
        /// Endpoint:<br />
        /// DELETE /api/v1/order
        /// </para>
        /// </summary>
        /// <param name="orderId">["<c>orderID</c>"] Order id. Either this or clientOrderId should be provided</param>
        /// <param name="clientOrderId">["<c>clOrdID</c>"] Client order id. Either this or orderId should be provided</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXOrder>> CancelOrderAsync(
            string? orderId = null,
            string? clientOrderId = null,
            CancellationToken ct = default);

        /// <summary>
        /// Cancel orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Order/Order_cancel" /><br />
        /// Endpoint:<br />
        /// DELETE /api/v1/order
        /// </para>
        /// </summary>
        /// <param name="orderIds">["<c>orderID</c>"] Order ids. Either this or clientOrderIds should be provided</param>
        /// <param name="clientOrderIds">["<c>clOrdID</c>"] Client order ids. Either this or orderIds should be provided</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXOrder[]>> CancelOrdersAsync(
           IEnumerable<string>? orderIds = null,
           IEnumerable<string>? clientOrderIds = null,
           CancellationToken ct = default);

        /// <summary>
        /// Cancel all orders matching the parameters
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Order/Order_cancelAll" /><br />
        /// Endpoint:<br />
        /// DELETE /api/v1/order/all
        /// </para>
        /// </summary>
        /// <param name="targetAccountIds">["<c>targetAccountIds</c>"] Account ids</param>
        /// <param name="symbol">["<c>symbol</c>"] Filter by symbol</param>
        /// <param name="filter">["<c>filter</c>"] Filter orders to cancel</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXOrder[]>> CancelAllOrdersAsync(
            IEnumerable<long>? targetAccountIds = null,
            string? symbol = null,
            Dictionary<string, object>? filter = null,
            CancellationToken ct = default);

        /// <summary>
        /// Cancel all orders after the timeout passes. Acts as a dead-man switch. Use TimeSpan.Zero to cancel timeout
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Order/Order_cancelAllAfter" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/order/cancelAllAfter
        /// </para>
        /// </summary>
        /// <param name="timeout">["<c>timeout</c>"] Timeout after which to cancel all orders. Use TimeSpan.Zero to cancel timeout</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult> CancelAllAfterAsync(TimeSpan timeout, CancellationToken ct = default);

        /// <summary>
        /// Get raw user execution history
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Execution/Execution_get" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/execution
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
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Execution/Execution_getTradeHistory" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/execution/tradeHistory
        /// </para>
        /// </summary>
        /// <param name="targetAccountIds">["<c>targetAccountIds</c>"] Target account ids</param>
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
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Position/Position_get" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/position
        /// </para>
        /// </summary>
        /// <param name="filter">["<c>filter</c>"] Filter on fields</param>
        /// <param name="columns">["<c>columns</c>"] Filter columns</param>
        /// <param name="limit">["<c>count</c>"] Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXPosition[]>> GetPositionsAsync(
            Dictionary<string, object>? filter = null,
            IEnumerable<string>? columns = null,
            int? limit = null,
            CancellationToken ct = default);

        /// <summary>
        /// Set cross margin leverage. Automatically enables cross margin.
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Position/Position_updateCrossLeverage" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/position/crossLeverage
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Symbol</param>
        /// <param name="leverage">["<c>leverage</c>"] New leverage</param>
        /// <param name="targetAccountId">["<c>targetAccountId</c>"] Target account id</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXPosition>> SetCrossMarginLeverageAsync(
            string symbol,
            decimal leverage,
            long? targetAccountId = null,
            CancellationToken ct = default);

        /// <summary>
        /// Set isolated margin leverage. Automatically enables isolated margin
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Position/Position_updateLeverage" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/position/leverage
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Symbol</param>
        /// <param name="leverage">["<c>leverage</c>"] New leverage</param>
        /// <param name="targetAccountId">["<c>targetAccountId</c>"] Target account id</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXPosition>> SetIsolatedMarginLeverageAsync(
            string symbol,
            decimal leverage,
            long? targetAccountId = null,
            CancellationToken ct = default);
    }
}
