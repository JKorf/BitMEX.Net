using BitMEX.Net.Enums;
using BitMEX.Net.ExtensionMethods;
using BitMEX.Net.Interfaces.Clients.ExchangeApi;
using BitMEX.Net.Objects.Models;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BitMEX.Net.Clients.ExchangeApi
{
    internal partial class BitMEXRestClientExchangeSharedApi
    {
        #region Place Futures Order

        public SharedFeeDeductionType FuturesFeeDeductionType => SharedFeeDeductionType.AddToCost;
        public SharedFeeAssetType FuturesFeeAssetType => SharedFeeAssetType.QuoteAsset;

        public SharedOrderType[] FuturesSupportedOrderTypes { get; } = new[] { SharedOrderType.Limit, SharedOrderType.Market };
        public SharedTimeInForce[] FuturesSupportedTimeInForce { get; } = new[] { SharedTimeInForce.GoodTillCanceled, SharedTimeInForce.ImmediateOrCancel, SharedTimeInForce.FillOrKill };
        public SharedQuantitySupport FuturesSupportedOrderQuantity { get; } = new SharedQuantitySupport(
                SharedQuantityType.Contracts,
                SharedQuantityType.Contracts,
                SharedQuantityType.Contracts,
                SharedQuantityType.Contracts);

        public PlaceFuturesOrderOptions PlaceFuturesOrderOptions { get; } = new PlaceFuturesOrderOptions(_exchangeName, false)
        {
            ParameterRuleOverrides = [
                RequestParameterRuleOverride<PlaceFuturesOrderRequest>.NotSupported(x => x.Leverage),
                RequestParameterRuleOverride<PlaceFuturesOrderRequest>.NotSupported(x => x.StopLossPrice),
                RequestParameterRuleOverride<PlaceFuturesOrderRequest>.NotSupported(x => x.TakeProfitPrice),
                RequestParameterRuleOverride<PlaceFuturesOrderRequest>.NotSupported(x => x.MarginMode),
                RequestParameterRuleOverride<PlaceFuturesOrderRequest>.NotSupported(x => x.PositionSide),
                ]
        };
        async Task<ICallResult<SharedId>> IPlaceFuturesOrder.PlaceFuturesOrderAsync(PlaceFuturesOrderRequest request, CancellationToken ct)
            => await PlaceFuturesOrderAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedId>> PlaceFuturesOrderAsync(PlaceFuturesOrderRequest request, CancellationToken ct)
        {
            var validationError = PlaceFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var result = await _api.Trading.PlaceOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                request.Side == SharedOrderSide.Buy ? Enums.OrderSide.Buy : Enums.OrderSide.Sell,
                request.OrderType == SharedOrderType.Limit ? Enums.OrderType.Limit : Enums.OrderType.Market,
                quantity: (long?)request.Quantity?.QuantityInContracts,
                price: request.Price,
                timeInForce: GetTimeInForce(request.TimeInForce),
                executionInstruction: request.OrderType == SharedOrderType.LimitMaker ? ExecutionInstruction.PostOnly : request.ReduceOnly == true ? ExecutionInstruction.ReduceOnly: null,
                clientOrderId: request.ClientOrderId,
                ct: ct).ConfigureAwait(false);

            if (!result.Success)
                return HttpResult.Fail<SharedId>(result);

            return HttpResult.Ok(result, new SharedId(result.Data.OrderId));
        }

        #endregion

        #region Get Futures Order

        public GetFuturesOrderOptions GetFuturesOrderOptions { get; } = new GetFuturesOrderOptions(_exchangeName, true);
        async Task<ICallResult<SharedFuturesOrder>> IGetFuturesOrder.GetFuturesOrderAsync(GetOrderRequest request, CancellationToken ct)
            => await GetFuturesOrderAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedFuturesOrder>> GetFuturesOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = GetFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesOrder>(Exchange, validationError);

            var orders = await _api.Trading.GetOrdersAsync(request.Symbol!.GetSymbol(FormatSymbol),
                filter: new Dictionary<string, object>
                {
                    { "orderID", request.OrderId }
                },
                ct: ct).ConfigureAwait(false);
            if (!orders.Success)
                return HttpResult.Fail<SharedFuturesOrder>(orders);

            var order = orders.Data.SingleOrDefault();
            if (order == null)
                return HttpResult.Fail<SharedFuturesOrder>(orders, new ServerError(new ErrorInfo(ErrorType.UnknownOrder, "Order not found")));

            return HttpResult.Ok(orders, new SharedFuturesOrder(
                ExchangeSymbolCache.ParseSymbol(_topicFuturesId, _api.EnvironmentName, null, order.Symbol),
                order.Symbol,
                order.OrderId,
                ParseOrderType(order.OrderType, order.ExecutionInstruction),
                order.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(order.Status),
                order.Timestamp)
            {
                ClientOrderId = order.ClientOrderId,
                AveragePrice = order.AveragePrice,
                OrderPrice = order.Price,
                OrderQuantity = new SharedOrderQuantity(contractQuantity: order.Quantity),
                QuantityFilled = new SharedOrderQuantity(contractQuantity: order.QuantityFilled),
                TimeInForce = ParseTimeInForce(order.TimeInForce),
                UpdateTime = order.TransactTime,
                ReduceOnly = order.ExecutionInstruction?.Contains(ExecutionInstruction.ReduceOnly) == true,
                TriggerPrice = order.StopPrice,
                IsTriggerOrder = order.StopPrice > 0,
                IsCloseOrder = order.ExecutionInstruction?.Contains(ExecutionInstruction.Close) == true
            });
        }

        #endregion

        #region Get Open Futures Orders

        public GetOpenFuturesOrdersOptions GetOpenFuturesOrdersOptions { get; } = new GetOpenFuturesOrdersOptions(_exchangeName, true);
        async Task<ICallResult<SharedFuturesOrder[]>> IGetOpenFuturesOrders.GetOpenFuturesOrdersAsync(GetOpenOrdersRequest request, CancellationToken ct)
            => await GetOpenFuturesOrdersAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedFuturesOrder[]>> GetOpenFuturesOrdersAsync(GetOpenOrdersRequest request, CancellationToken ct)
        {
            var validationError = GetOpenFuturesOrdersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesOrder[]>(Exchange, validationError);

            var symbol = request.Symbol?.GetSymbol(FormatSymbol);
            var orders = await _api.Trading.GetOrdersAsync(symbol,
                filter: new Dictionary<string, object>
                {
                    { "ordStatus", new [] { "New", "PartiallyFilled" } }
                },
                ct: ct).ConfigureAwait(false);
            if (!orders.Success)
                return HttpResult.Fail<SharedFuturesOrder[]>(orders);

            var data = orders.Data.Where(x => BitMEXUtils.GetSymbolType(x.Symbol) != SymbolType.Spot);
            return HttpResult.Ok(orders, data.Select(x => new SharedFuturesOrder(
                ExchangeSymbolCache.ParseSymbol(_topicFuturesId, _api.EnvironmentName, null, x.Symbol),
                x.Symbol,
                x.OrderId,
                ParseOrderType(x.OrderType, x.ExecutionInstruction),
                x.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(x.Status),
                x.Timestamp)
            {
                ClientOrderId = x.ClientOrderId,
                AveragePrice = x.AveragePrice,
                OrderPrice = x.Price,
                OrderQuantity = new SharedOrderQuantity(contractQuantity: x.Quantity),
                QuantityFilled = new SharedOrderQuantity(contractQuantity: x.QuantityFilled),
                TimeInForce = ParseTimeInForce(x.TimeInForce),
                UpdateTime = x.TransactTime,
                ReduceOnly = x.ExecutionInstruction?.Contains(ExecutionInstruction.ReduceOnly) == true,
                TriggerPrice = x.StopPrice,
                IsTriggerOrder = x.StopPrice > 0,
                IsCloseOrder = x.ExecutionInstruction?.Contains(ExecutionInstruction.Close) == true
            }).ToArray());
        }

        #endregion

        #region Get Closed Futures Orders

        public GetFuturesClosedOrdersOptions GetClosedFuturesOrdersOptions { get; } = new GetFuturesClosedOrdersOptions(_exchangeName, true, true, true, 1000);
        async Task<ICallResult<SharedFuturesOrder[]>> IGetClosedFuturesOrders.GetClosedFuturesOrdersAsync(GetClosedOrdersRequest request, PageRequest? pageRequest, CancellationToken ct)
            => await GetClosedFuturesOrdersAsync(request, pageRequest, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedFuturesOrder[]>> GetClosedFuturesOrdersAsync(GetClosedOrdersRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = GetClosedFuturesOrdersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesOrder[]>(Exchange, validationError);

            var direction = request.Direction ?? DataDirection.Descending;
            var limit = request.Limit ?? 500;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            // Get data
            var result = await _api.Trading.GetOrdersAsync(request.Symbol!.GetSymbol(FormatSymbol),
                filter: new Dictionary<string, object>
                {
                    { "ordStatus", new string[] {"Canceled", "Filled" } }
                },
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                limit: pageParams.Limit,
                offset: pageParams.Offset,
                reverse: direction == DataDirection.Descending,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedFuturesOrder[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                () => Pagination.NextPageFromOffset(pageParams, result.Data.Length),
                result.Data.Length,
                result.Data.Select(x => x.Timestamp),
                request.StartTime,
                request.EndTime ?? DateTime.UtcNow,
                pageParams);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data, x => x.Timestamp, request.StartTime, request.EndTime, direction)
                       .Select(x => 
                           new SharedFuturesOrder(
                                ExchangeSymbolCache.ParseSymbol(_topicFuturesId, _api.EnvironmentName, null, x.Symbol), 
                                x.Symbol,
                                x.OrderId,
                                ParseOrderType(x.OrderType, x.ExecutionInstruction),
                                x.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                                ParseOrderStatus(x.Status),
                                x.Timestamp)
                            {
                                ClientOrderId = x.ClientOrderId,
                                AveragePrice = x.AveragePrice,
                                OrderPrice = x.Price,
                                OrderQuantity = new SharedOrderQuantity(contractQuantity: x.Quantity),
                                QuantityFilled = new SharedOrderQuantity(contractQuantity: x.QuantityFilled),
                                TimeInForce = ParseTimeInForce(x.TimeInForce),
                                UpdateTime = x.TransactTime,
                                ReduceOnly = x.ExecutionInstruction?.Contains(ExecutionInstruction.ReduceOnly) == true,
                                TriggerPrice = x.StopPrice,
                                IsTriggerOrder = x.StopPrice > 0,
                                IsCloseOrder = x.ExecutionInstruction?.Contains(ExecutionInstruction.Close) == true
                            })
                       .ToArray(), nextPageRequest);
        }

        #endregion

        #region Get Futures Order Trades

        public GetFuturesOrderTradesOptions GetFuturesOrderTradesOptions { get; } = new GetFuturesOrderTradesOptions(_exchangeName, true);
        async Task<ICallResult<SharedUserTrade[]>> IGetFuturesOrderTrades.GetFuturesOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct)
            => await GetFuturesOrderTradesAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedUserTrade[]>> GetFuturesOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct)
        {
            var validationError = GetFuturesOrderTradesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, symbolInfoResult.Error!);

            var trades = await _api.Trading.GetUserTradesAsync(symbol: request.Symbol!.GetSymbol(FormatSymbol),
                filter: new Dictionary<string, object>
                {
                    { "orderID", request.OrderId }
                },
                ct: ct).ConfigureAwait(false);
            if (!trades.Success)
                return HttpResult.Fail<SharedUserTrade[]>(trades);

            return HttpResult.Ok(trades, trades.Data.Select(x => new SharedUserTrade(
                ExchangeSymbolCache.ParseSymbol(_topicFuturesId, _api.EnvironmentName, null, x.Symbol), 
                x.Symbol,
                x.OrderId,
                x.TradeId,
                x.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                new SharedOrderQuantity(x.Quantity ?? 0),
                x.LastTradePrice!.Value,
                x.Timestamp)
            {
                ClientOrderId = x.ClientOrderId,
                Fee = x.Fee.ToSharedAssetQuantity(x.SettlementCurrency ?? x.Currency),
                Role = x.Role == TradeRole.Maker ? SharedRole.Maker : SharedRole.Taker
            }).ToArray());
        }

        #endregion

        #region Get Futures User Trade History

        Task<HttpResult<SharedUserTrade[]>> IFuturesOrderRestClient.GetFuturesUserTradesAsync(GetUserTradesRequest request, PageRequest? nextPageToken, CancellationToken ct)
            => GetFuturesUserTradeHistoryAsync(request, nextPageToken, ct);
        GetFuturesUserTradeHistoryOptions IFuturesOrderRestClient.GetFuturesUserTradesOptions => GetFuturesUserTradeHistoryOptions;

        public GetFuturesUserTradeHistoryOptions GetFuturesUserTradeHistoryOptions { get; } = new GetFuturesUserTradeHistoryOptions(_exchangeName, true, true, true, 1000);
        async Task<ICallResult<SharedUserTrade[]>> IGetFuturesUserTradeHistory.GetFuturesUserTradeHistoryAsync(GetUserTradesRequest request, PageRequest? pageRequest, CancellationToken ct)
            => await GetFuturesUserTradeHistoryAsync(request, pageRequest, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedUserTrade[]>> GetFuturesUserTradeHistoryAsync(GetUserTradesRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = GetFuturesUserTradeHistoryOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, symbolInfoResult.Error!);

            var direction = request.Direction ?? DataDirection.Descending;
            var limit = request.Limit ?? 500;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            // Get data
            var result = await _api.Trading.GetUserTradesAsync(symbol: request.Symbol!.GetSymbol(FormatSymbol),
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                limit: pageParams.Limit,
                offset: pageParams.Offset,
                reverse: direction == DataDirection.Descending,
                ct: ct
                ).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedUserTrade[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                () => Pagination.NextPageFromOffset(pageParams, result.Data.Length),
                result.Data.Length,
                result.Data.Select(x => x.Timestamp),
                request.StartTime,
                request.EndTime ?? DateTime.UtcNow,
                pageParams);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data, x => x.Timestamp, request.StartTime, request.EndTime, direction)
                       .Select(x => 
                            new SharedUserTrade(
                                ExchangeSymbolCache.ParseSymbol(_topicFuturesId, _api.EnvironmentName, null, x.Symbol), 
                                x.Symbol,
                                x.OrderId,
                                x.TradeId,
                                x.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                                new SharedOrderQuantity(x.Quantity ?? 0),
                                x.LastTradePrice!.Value,
                                x.Timestamp)
                            {
                                ClientOrderId = x.ClientOrderId,
                                Fee = x.Fee.ToSharedAssetQuantity(x.SettlementCurrency ?? x.Currency),
                                Role = x.Role == TradeRole.Maker ? SharedRole.Maker : SharedRole.Taker
                            })
                       .ToArray(), nextPageRequest);
        }

        #endregion

        #region Cancel Futures Order

        public CancelFuturesOrderOptions CancelFuturesOrderOptions { get; } = new CancelFuturesOrderOptions(_exchangeName, true);
        async Task<ICallResult<SharedId>> ICancelFuturesOrder.CancelFuturesOrderAsync(CancelOrderRequest request, CancellationToken ct)
            => await CancelFuturesOrderAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedId>> CancelFuturesOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = CancelFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var order = await _api.Trading.CancelOrderAsync(request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedId>(order);

            return HttpResult.Ok(order, new SharedId(request.OrderId));
        }

        #endregion

        #region Get Positions

        public GetPositionsOptions GetPositionsOptions { get; } = new GetPositionsOptions(_exchangeName, true);
        async Task<ICallResult<SharedPosition[]>> IGetPositions.GetPositionsAsync(GetPositionsRequest request, CancellationToken ct)
            => await GetPositionsAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedPosition[]>> GetPositionsAsync(GetPositionsRequest request, CancellationToken ct)
        {
            var validationError = GetPositionsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedPosition[]>(Exchange, validationError);

            var result = await _api.Trading.GetPositionsAsync(filter: request.Symbol == null ? null : new Dictionary<string, object>{
                { "symbol", request.Symbol!.GetSymbol(FormatSymbol) }
            }, ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedPosition[]>(result);

            IEnumerable<BitMEXPosition> data = result.Data;
            if (request.TradingMode != null)
                data = data.Where(x => request.TradingMode.Value.IsPerpetual() ? BitMEXUtils.GetSymbolType(x.Symbol) == SymbolType.PerpetualContract : BitMEXUtils.GetSymbolType(x.Symbol) == SymbolType.Futures);
            
            var resultTypes = request.Symbol == null && request.TradingMode == null ? SupportedTradingModes : request.Symbol != null ? new[] { request.Symbol!.TradingMode } : new[] { request.TradingMode!.Value };
            return HttpResult.Ok(result, data.Where(x => x.Currency != null).Select(x =>
                new SharedPosition(
                    ExchangeSymbolCache.ParseSymbol(_topicFuturesId, _api.EnvironmentName, null, x.Symbol),
                    x.Symbol,
                    new SharedOrderQuantity(Math.Abs(x.CurrentQuantity ?? 0)),
                    x.Timestamp)
                {
                    UnrealizedPnl = x.UnrealizedPnl.ToSharedAssetQuantity(x.Currency!),
                    LiquidationPrice = x.LiquidationPrice == 0 ? null : x.LiquidationPrice,
                    Leverage = x.Leverage,
                    AverageOpenPrice = x.AverageEntryPrice,
                    PositionMode = SharedPositionMode.OneWay,
                    PositionSide = x.CurrentQuantity < 0 ? SharedPositionSide.Short : SharedPositionSide.Long
                }).ToArray());
        }

        #endregion

        #region Close Position

        public ClosePositionOptions ClosePositionOptions { get; } = new ClosePositionOptions(_exchangeName, true)
        {
            ParameterRuleOverrides = [
                RequestParameterRuleOverride<ClosePositionRequest>.Required(x => x.PositionSide)
            ]
        };

        public async Task<HttpResult<SharedId>> ClosePositionAsync(ClosePositionRequest request, CancellationToken ct)
        {
            var validationError = ClosePositionOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol); 
            var result = await _api.Trading.PlaceOrderAsync(
                symbol,
                request.PositionSide == SharedPositionSide.Long ? OrderSide.Sell : OrderSide.Buy,
                OrderType.Market,
                executionInstruction: ExecutionInstruction.Close,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedId>(result);

            return HttpResult.Ok(result, new SharedId(result.Data.OrderId));
        }

        #endregion

        #region Get Futures Order By Client Order Id

        public GetFuturesOrderByClientOrderIdOptions GetFuturesOrderByClientOrderIdOptions { get; } = new GetFuturesOrderByClientOrderIdOptions(_exchangeName, true);
        async Task<ICallResult<SharedFuturesOrder>> IGetFuturesOrderByClientOrderId.GetFuturesOrderByClientOrderIdAsync(GetOrderRequest request, CancellationToken ct)
            => await GetFuturesOrderByClientOrderIdAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedFuturesOrder>> GetFuturesOrderByClientOrderIdAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = GetFuturesOrderByClientOrderIdOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesOrder>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedFuturesOrder>(Exchange, symbolInfoResult.Error!);

            var orders = await _api.Trading.GetOrdersAsync(request.Symbol!.GetSymbol(FormatSymbol),
                filter: new Dictionary<string, object>
                {
                    { "clOrdID", request.OrderId }
                },
                ct: ct).ConfigureAwait(false);
            if (!orders.Success)
                return HttpResult.Fail<SharedFuturesOrder>(orders);

            var order = orders.Data.SingleOrDefault();
            if (order == null)
                return HttpResult.Fail<SharedFuturesOrder>(orders, new ServerError(new ErrorInfo(ErrorType.UnknownOrder, "Order not found")));

            return HttpResult.Ok(orders, new SharedFuturesOrder(
                ExchangeSymbolCache.ParseSymbol(_topicFuturesId, _api.EnvironmentName, null, order.Symbol),
                order.Symbol,
                order.OrderId,
                ParseOrderType(order.OrderType, order.ExecutionInstruction),
                order.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(order.Status),
                order.Timestamp)
            {
                ClientOrderId = order.ClientOrderId,
                AveragePrice = order.AveragePrice,
                OrderPrice = order.Price,
                OrderQuantity = new SharedOrderQuantity(contractQuantity: order.Quantity),
                QuantityFilled = new SharedOrderQuantity(contractQuantity: order.QuantityFilled),
                TimeInForce = ParseTimeInForce(order.TimeInForce),
                UpdateTime = order.TransactTime,
                ReduceOnly = order.ExecutionInstruction?.Contains(ExecutionInstruction.ReduceOnly) == true,
                TriggerPrice = order.StopPrice,
                IsTriggerOrder = order.StopPrice > 0,
                IsCloseOrder = order.ExecutionInstruction?.Contains(ExecutionInstruction.Close) == true
            });
        }

        #endregion

        #region Cancel Futures Order By Client Order Id

        public CancelFuturesOrderByClientOrderIdOptions CancelFuturesOrderByClientOrderIdOptions { get; } = new CancelFuturesOrderByClientOrderIdOptions(_exchangeName, true);
        async Task<ICallResult<SharedId>> ICancelFuturesOrderByClientOrderId.CancelFuturesOrderByClientOrderIdAsync(CancelOrderRequest request, CancellationToken ct)
            => await CancelFuturesOrderByClientOrderIdAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedId>> CancelFuturesOrderByClientOrderIdAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = CancelFuturesOrderByClientOrderIdOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var order = await _api.Trading.CancelOrderAsync(clientOrderId: request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedId>(order);

            return HttpResult.Ok(order, new SharedId(order.Data.OrderId));
        }

        #endregion
    }
}
