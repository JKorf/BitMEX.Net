using CryptoExchange.Net;
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
using CryptoExchange.Net.Objects.Errors;

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
        public async Task<HttpResult<BitMEXOrder>> PlaceOrderAsync(
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
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BitMEXExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("side", orderSide);
            parameters.Add("ordType", orderType);

            parameters.Add("orderQty", quantity);
            parameters.Add("price", price);
            parameters.Add("displayQty", displayQuantity);
            parameters.Add("stopPx", stopPrice);
            parameters.Add("clOrdID", clientOrderId);
            parameters.Add("clOrdLinkID", clientOrderLinkId);
            parameters.Add("pegOffsetValue", pegOffsetValue);
            parameters.Add("pegPriceType", pegPriceType);
            parameters.Add("timeInForce", timeInForce);
            parameters.Add("execInst", executionInstruction);
            parameters.Add("contingencyType", contingencyType);

            parameters.Add("text", LibraryHelpers.GetClientReference(() => _baseClient.ClientOptions.BrokerId, _baseClient.Exchange));
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "api/v1/order", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXOrder>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Edit Order

        /// <inheritdoc />
        public async Task<HttpResult<BitMEXOrder>> EditOrderAsync(
            string? orderId = null,
            string? origClientOrderId = null,
            string? newClientOrderId = null,
            long? quantity = null,
            long? quantityRemaining = null,
            decimal? price = null,
            decimal? stopPrice = null,
            decimal? pegOffsetValue = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BitMEXExchange._parameterSerializationSettings);
            parameters.Add("orderID", orderId);
            parameters.Add("origClOrdID", origClientOrderId);
            parameters.Add("clOrdID", newClientOrderId);
            parameters.Add("orderQty", quantity);
            parameters.Add("leavesQty", quantityRemaining);
            parameters.Add("price", price);
            parameters.Add("stopPx", stopPrice);
            parameters.Add("pegOffsetValue", pegOffsetValue);
            parameters.Add("text", LibraryHelpers.GetClientReference(() => _baseClient.ClientOptions.BrokerId, _baseClient.Exchange)); // Client reference
            var request = _definitions.GetOrCreate(HttpMethod.Put, _baseClient.BaseAddress, "api/v1/order", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXOrder>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Orders

        /// <inheritdoc />
        public async Task<HttpResult<BitMEXOrder[]>> GetOrdersAsync(
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
            parameters.Add("start", offset);
            parameters.Add("count", limit);
            parameters.Add("reverse", reverse ?? true);
            parameters.Add("startTime", startTime?.ToRfc3339String());
            parameters.Add("endTime", endTime?.ToRfc3339String());
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "api/v1/order", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXOrder[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Order History By Day

        /// <inheritdoc />
        public async Task<HttpResult<BitMEXExecution[]>> GetExecutionHistoryByDayAsync(string symbol, DateTime day, CancellationToken ct = default)
        {
            var parameters = new Parameters(BitMEXExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol ?? "all");
            parameters.Add("timestamp", day.ToString("yyyy-MM-ddT00:00:00.000Z"));
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "api/v1/user/executionHistory", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXExecution[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Cancel Order

        /// <inheritdoc />
        public async Task<HttpResult<BitMEXOrder>> CancelOrderAsync(
            string? orderId = null,
            string? clientOrderId = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BitMEXExchange._parameterSerializationSettings);
            parameters.Add("orderID", orderId);
            parameters.Add("clOrdID", clientOrderId);
            parameters.Add("text", LibraryHelpers.GetClientReference(() => _baseClient.ClientOptions.BrokerId, _baseClient.Exchange)); // Client reference
            var request = _definitions.GetOrCreate(HttpMethod.Delete, _baseClient.BaseAddress, "api/v1/order", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            var result = await _baseClient.SendAsync<BitMEXOrder[]>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BitMEXOrder>(result);

            var order = result.Data.Single();
            if (!string.IsNullOrEmpty(order.Error))
                return HttpResult.Fail<BitMEXOrder>(result, new ServerError(new ErrorInfo(ErrorType.UnknownOrder, order.Error!)));

            return HttpResult.Ok(result, order);
        }

        #endregion

        #region Cancel Orders

        /// <inheritdoc />
        public async Task<HttpResult<BitMEXOrder[]>> CancelOrdersAsync(
            IEnumerable<string>? orderIds = null,
            IEnumerable<string>? clientOrderIds = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BitMEXExchange._parameterSerializationSettings);
            parameters.Add("orderID", orderIds == null ? null : string.Join(",", orderIds));
            parameters.Add("clOrdID", clientOrderIds == null ? null : string.Join(",", clientOrderIds));
            parameters.Add("text", LibraryHelpers.GetClientReference(() => _baseClient.ClientOptions.BrokerId, _baseClient.Exchange)); // Client reference
            var request = _definitions.GetOrCreate(HttpMethod.Delete, _baseClient.BaseAddress, "api/v1/order", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXOrder[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Cancel All Orders

        /// <inheritdoc />
        public async Task<HttpResult<BitMEXOrder[]>> CancelAllOrdersAsync(
            IEnumerable<long>? targetAccountIds = null,
            string? symbol = null,
            Dictionary<string, object>? filter = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BitMEXExchange._parameterSerializationSettings);
            parameters.Add("targetAccountIds", targetAccountIds == null ? "*" : string.Join(",", targetAccountIds));
            parameters.Add("symbol", symbol);
            parameters.AddRaw("filter", filter);
            parameters.Add("text", LibraryHelpers.GetClientReference(() => _baseClient.ClientOptions.BrokerId, _baseClient.Exchange)); // Client reference
            var request = _definitions.GetOrCreate(HttpMethod.Delete, _baseClient.BaseAddress, "api/v1/order/all", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXOrder[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Cancel All Orders After

        /// <inheritdoc />
        public async Task<HttpResult> CancelAllAfterAsync(TimeSpan timeout, CancellationToken ct = default)
        {
            var parameters = new Parameters(BitMEXExchange._parameterSerializationSettings);
            parameters.Add("timeout", (int)timeout.TotalMilliseconds);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "api/v1/order/cancelAllAfter", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get User Executions

        /// <inheritdoc />
        public async Task<HttpResult<BitMEXExecution[]>> GetUserExecutionsAsync(
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
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "api/v1/execution", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXExecution[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get User Trades

        /// <inheritdoc />
        public async Task<HttpResult<BitMEXExecution[]>> GetUserTradesAsync(
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
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BitMEXExchange._parameterSerializationSettings);
            parameters.Add("targetAccountIds", targetAccountIds?.Any() != true ? "*": string.Join(",", targetAccountIds));
            if (symbol != null)
                parameters.Add("symbol", symbol + (symbolFilter == null ? "" : ":" + EnumConverter.GetString(symbolFilter)));
            parameters.AddRaw("filter", filter);
            parameters.AddArray("columns", columns);
            parameters.Add("start", offset);
            parameters.Add("count", limit);
            parameters.Add("reverse", reverse);
            parameters.Add("startTime", startTime?.ToRfc3339String());
            parameters.Add("endTime", endTime?.ToRfc3339String());
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "api/v1/execution/tradeHistory", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXExecution[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Positions

        /// <inheritdoc />
        public async Task<HttpResult<BitMEXPosition[]>> GetPositionsAsync(
            Dictionary<string, object>? filter = null,
            IEnumerable<string>? columns = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BitMEXExchange._parameterSerializationSettings);
            parameters.AddRaw("filter", filter);
            parameters.AddArray("columns", columns);
            parameters.Add("count", limit);            
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "api/v1/position", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXPosition[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Set Cross Margin leverage

        /// <inheritdoc />
        public async Task<HttpResult<BitMEXPosition>> SetCrossMarginLeverageAsync(
            string symbol,
            decimal leverage,
            long? targetAccountId = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BitMEXExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("leverage", leverage);
            parameters.Add("targetAccountId", targetAccountId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "api/v1/position/crossLeverage", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXPosition>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Set Isolated Margin leverage

        /// <inheritdoc />
        public async Task<HttpResult<BitMEXPosition>> SetIsolatedMarginLeverageAsync(
            string symbol,
            decimal leverage,
            long? targetAccountId = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BitMEXExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("leverage", leverage);
            parameters.Add("targetAccountId", targetAccountId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "api/v1/position/leverage", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXPosition>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion
    }
}
