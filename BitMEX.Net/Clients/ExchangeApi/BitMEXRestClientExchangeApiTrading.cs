using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using BitMEX.Net.Interfaces.Clients.ExchangeApi;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System;
using BitMEX.Net.Objects.Models;
using BitMEX.Net.Enums;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Linq;

namespace BitMEX.Net.Clients.ExchangeApi
{
    /// <inheritdoc />
    internal class BitMEXRestClientExchangeApiTrading : IBitMEXRestClientExchangeApiTrading
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly BitMEXRestClientExchangeApi _baseClient;
        private readonly ILogger _logger;

        internal BitMEXRestClientExchangeApiTrading(ILogger logger, BitMEXRestClientExchangeApi baseClient)
        {
            _baseClient = baseClient;
            _logger = logger;
        }

        #region Place Order

        /// <inheritdoc />
        public async Task<WebCallResult<BitMEXOrder>> PlaceOrderAsync(
            string symbol, 
            OrderSide orderSide,
            OrderType orderType,
            decimal? quantity = null,
            decimal? price = null,
            TimeInForce? timeInForce = null,
            ExecutionInstruction? executionInstruction = null,
            ContingencyType? contingencyType = null,
            decimal? displayQuantity = null,
            decimal? stopPrice = null,
            decimal? pegOffsetValue = null,
            PeggedPriceType? pegPriceType = null,
            string? clientOrderLinkId = null,
            string? clientOrderId = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("symbol", symbol);
            parameters.AddEnum("side", orderSide);
            parameters.AddEnum("ordType", orderType);

            parameters.AddOptional("orderQty", quantity);
            parameters.AddOptional("price", price);
            parameters.AddOptional("displayQty", displayQuantity);
            parameters.AddOptional("stopPx", stopPrice);
            parameters.AddOptional("clOrdID", clientOrderId);
            parameters.AddOptional("clOrdLinkID", clientOrderLinkId);
            parameters.AddOptional("pegOffsetValue", pegOffsetValue);
            parameters.AddOptionalEnum("pegPriceType", pegPriceType);
            parameters.AddOptionalEnum("timeInForce", timeInForce);
            parameters.AddOptionalEnum("execInst", executionInstruction);
            parameters.AddOptionalEnum("contingencyType", contingencyType);

            parameters.Add("text", "Sent from Copytrader.pw"); // Client reference
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/order", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXOrder>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Edit Order

        /// <inheritdoc />
        public async Task<WebCallResult<BitMEXOrder>> EditOrderAsync(
            string? orderId = null,
            string? origClientOrderId = null,
            string? newClientOrderId = null,
            decimal? quantity = null,
            decimal? quantityRemaining = null,
            decimal? price = null,
            decimal? stopPrice = null,
            decimal? pegOffsetValue = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("orderID", orderId);
            parameters.AddOptional("origClOrdID", origClientOrderId);
            parameters.AddOptional("clOrdID", newClientOrderId);
            parameters.AddOptionalString("orderQty", quantity);
            parameters.AddOptionalString("leavesQty", quantityRemaining);
            parameters.AddOptionalString("price", price);
            parameters.AddOptionalString("stopPx", stopPrice);
            parameters.AddOptionalString("pegOffsetValue", pegOffsetValue);
            parameters.Add("text", "Sent from Copytrader.pw"); // Client reference
            var request = _definitions.GetOrCreate(HttpMethod.Put, "/order", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXOrder>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Orders

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXOrder>>> GetOrdersAsync(
            string? symbol = null,
            SymbolFilter? symbolFilter = null,
            Dictionary<string, object>? filter = null,
            IEnumerable<string>? columns = null,
            bool? reverse = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            if (symbol != null)
                parameters.AddOptional("symbol", symbol + (symbolFilter == null ? "" : ":" + EnumConverter.GetString(symbolFilter)));
            parameters.AddOptional("filter", filter);
            parameters.AddOptional("columns", columns);
            parameters.AddOptional("count", limit);
            parameters.AddOptional("reverse", reverse);
            parameters.AddOptionalMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/order", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<IEnumerable<BitMEXOrder>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Order History By Day

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXExecutionHistory>>> GetExecutionHistoryByDayAsync(string symbol, DateTime day, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("symbol", symbol ?? "all");
            parameters.Add("timestamp", day.ToString("yyyy-MM-dd"));
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/user/executionHistory", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<IEnumerable<BitMEXExecutionHistory>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Cancel Order

        /// <inheritdoc />
        public async Task<WebCallResult<BitMEXOrder>> CancelOrderAsync(
            string? orderId = null,
            string? clientOrderId = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("orderID", orderId);
            parameters.AddOptional("clOrdID", clientOrderId);
            parameters.Add("text", "Sent from Copytrader.pw"); // Client reference
            var request = _definitions.GetOrCreate(HttpMethod.Delete, "/order", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            var result = await _baseClient.SendAsync<IEnumerable<BitMEXOrder>>(request, parameters, ct).ConfigureAwait(false);
            return result.As<BitMEXOrder>(result.Data?.Single());
        }

        #endregion

        #region Cancel Orders

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXOrder>>> CancelOrdersAsync(
            IEnumerable<string>? orderIds = null,
            IEnumerable<string>? clientOrderIds = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("orderID", orderIds == null ? null : string.Join(",", orderIds));
            parameters.AddOptional("clOrdID", clientOrderIds == null ? null : string.Join(",", clientOrderIds));
            parameters.Add("text", "Sent from Copytrader.pw"); // Client reference
            var request = _definitions.GetOrCreate(HttpMethod.Delete, "/order", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<IEnumerable<BitMEXOrder>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Cancel All Orders

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXOrder>>> CancelAllOrdersAsync(
            IEnumerable<long>? targetAccountIds = null,
            string? symbol = null,
            Dictionary<string, object>? filter = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("targetAccountIds", targetAccountIds == null ? "*" : string.Join(",", targetAccountIds));
            parameters.AddOptional("symbol", symbol);
            parameters.AddOptional("filter", filter);
            parameters.Add("text", "Sent from Copytrader.pw"); // Client reference
            var request = _definitions.GetOrCreate(HttpMethod.Delete, "/order/all", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<IEnumerable<BitMEXOrder>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Cancel All Orders After

        /// <inheritdoc />
        public async Task<WebCallResult> CancelAllAfterAsync(TimeSpan timeout, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("timeout", (int)timeout.TotalMilliseconds);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/order/cancelAllAfter", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Close Position

        /// <inheritdoc />
        public async Task<WebCallResult<BitMEXOrder>> ClosePositionAsync(string symbol, decimal? price = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("symbol", symbol);
            parameters.AddOptional("price", price);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/order/closePosition", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXOrder>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get User Executions

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXExecutionHistory>>> GetUserExecutionsAsync(
            string? symbol = null,
            SymbolFilter? symbolFilter = null,
            Dictionary<string, object>? filter = null,
            IEnumerable<string>? columns = null,
            bool? reverse = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            if (symbol != null)
                parameters.AddOptional("symbol", symbol + (symbolFilter == null ? "" : ":" + EnumConverter.GetString(symbolFilter)));
            parameters.AddOptional("filter", filter);
            parameters.AddOptional("columns", columns);
            parameters.AddOptional("count", limit);
            parameters.AddOptional("reverse", reverse);
            parameters.AddOptionalMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/execution", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<IEnumerable<BitMEXExecutionHistory>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get User Trades

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXExecutionHistory>>> GetUserTradesAsync(
            IEnumerable<long>? targetAccountIds = null,
            string? symbol = null,
            SymbolFilter? symbolFilter = null,
            Dictionary<string, object>? filter = null,
            IEnumerable<string>? columns = null,
            bool? reverse = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("targetAccountIds", targetAccountIds?.Any() != true ? "*": string.Join(",", targetAccountIds));
            if (symbol != null)
                parameters.AddOptional("symbol", symbol + (symbolFilter == null ? "" : ":" + EnumConverter.GetString(symbolFilter)));
            parameters.AddOptional("filter", filter);
            parameters.AddOptional("columns", columns);
            parameters.AddOptional("count", limit);
            parameters.AddOptional("reverse", reverse);
            parameters.AddOptionalMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/execution/tradeHistory", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<IEnumerable<BitMEXExecutionHistory>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Positions

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXPosition>>> GetPositionsAsync(
            Dictionary<string, object>? filter = null,
            IEnumerable<string>? columns = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("filter", filter);
            parameters.AddOptional("columns", columns);
            parameters.AddOptional("count", limit);            
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/position", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<IEnumerable<BitMEXPosition>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Set Cross Margin leverage

        /// <inheritdoc />
        public async Task<WebCallResult<BitMEXPosition>> SetCrossMarginLeverageAsync(
            string symbol,
            decimal leverage,
            long? targetAccountId = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("symbol", symbol);
            parameters.AddString("leverage", leverage);
            parameters.AddOptional("targetAccountId", targetAccountId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/position/crossLeverage", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXPosition>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Set Isolated Margin leverage

        /// <inheritdoc />
        public async Task<WebCallResult<BitMEXPosition>> SetIsolatedMarginLeverageAsync(
            string symbol,
            decimal leverage,
            long? targetAccountId = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("symbol", symbol);
            parameters.AddString("leverage", leverage);
            parameters.AddOptional("targetAccountId", targetAccountId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/position/leverage", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXPosition>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion
    }
}
