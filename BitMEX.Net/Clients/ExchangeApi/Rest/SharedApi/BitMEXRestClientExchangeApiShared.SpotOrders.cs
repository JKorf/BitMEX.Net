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
        #region Spot Order Client
        public SharedFeeDeductionType SpotFeeDeductionType => SharedFeeDeductionType.AddToCost;
        public SharedFeeAssetType SpotFeeAssetType => SharedFeeAssetType.QuoteAsset;
        public SharedOrderType[] SpotSupportedOrderTypes { get; } = new[] { SharedOrderType.Limit, SharedOrderType.Market, SharedOrderType.LimitMaker };
        public SharedTimeInForce[] SpotSupportedTimeInForce { get; } = new[] { SharedTimeInForce.GoodTillCanceled, SharedTimeInForce.ImmediateOrCancel, SharedTimeInForce.FillOrKill };
        public SharedQuantitySupport SpotSupportedOrderQuantity { get; } = new SharedQuantitySupport(
                SharedQuantityType.BaseAsset,
                SharedQuantityType.BaseAsset,
                SharedQuantityType.BaseAsset,
                SharedQuantityType.BaseAsset);

        public string GenerateClientOrderId() => ExchangeHelpers.RandomString(32);

        async Task<ICallResult<SharedId>> IPlaceSpotOrder.PlaceSpotOrderAsync(PlaceSpotOrderRequest request, CancellationToken ct)
            => await PlaceSpotOrderAsync(request, ct).ConfigureAwait(false);

        public PlaceSpotOrderOptions PlaceSpotOrderOptions { get; } = new PlaceSpotOrderOptions(_exchangeName);
        public async Task<HttpResult<SharedId>> PlaceSpotOrderAsync(PlaceSpotOrderRequest request, CancellationToken ct)
        {
            var validationError = PlaceSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedId>(Exchange, symbolInfoResult.Error!);

            var result = await _api.Trading.PlaceOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                request.Side == SharedOrderSide.Buy ? Enums.OrderSide.Buy : Enums.OrderSide.Sell,
                request.OrderType == SharedOrderType.Limit ? Enums.OrderType.Limit : Enums.OrderType.Market,
                quantity: request.Quantity?.QuantityInBaseAsset.ToBitMEXAssetQuantity(request.Symbol!.BaseAsset),
                price: request.Price,
                timeInForce: GetTimeInForce(request.TimeInForce),
                executionInstruction: request.OrderType == SharedOrderType.LimitMaker ? ExecutionInstruction.PostOnly : null,
                clientOrderId: request.ClientOrderId,
                ct: ct).ConfigureAwait(false);

            if (!result.Success)
                return HttpResult.Fail<SharedId>(result);

            return HttpResult.Ok(result, new SharedId(result.Data.OrderId));
        }

        public GetSpotOrderOptions GetSpotOrderOptions { get; } = new GetSpotOrderOptions(_exchangeName, true);
        public async Task<HttpResult<SharedSpotOrder>> GetSpotOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = GetSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotOrder>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedSpotOrder>(Exchange, symbolInfoResult.Error!);

            var orders = await _api.Trading.GetOrdersAsync(request.Symbol!.GetSymbol(FormatSymbol), 
                filter: new Dictionary<string, object>
                {
                    { "orderID", request.OrderId }
                },
                ct: ct).ConfigureAwait(false);
            if (!orders.Success)
                return HttpResult.Fail<SharedSpotOrder>(orders);

            var order = orders.Data.SingleOrDefault();
            if (order == null)
                return HttpResult.Fail<SharedSpotOrder>(orders, new ServerError(new ErrorInfo(ErrorType.UnknownOrder, "Order not found")));

            return HttpResult.Ok(orders, new SharedSpotOrder(
                ExchangeSymbolCache.ParseSymbol(_topicSpotId, _api.EnvironmentName, null, order.Symbol),
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
                OrderQuantity = new SharedOrderQuantity(order.Quantity.ToSharedSymbolQuantity(order.Symbol), null, null),
                QuantityFilled = new SharedOrderQuantity(order.QuantityFilled.ToSharedSymbolQuantity(order.Symbol), null, null),
                TimeInForce = ParseTimeInForce(order.TimeInForce),
                UpdateTime = order.TransactTime,
                TriggerPrice = order.StopPrice,
                IsTriggerOrder = order.StopPrice != null
            });
        }

        public GetOpenSpotOrdersOptions GetOpenSpotOrdersOptions { get; } = new GetOpenSpotOrdersOptions(_exchangeName, true);
        public async Task<HttpResult<SharedSpotOrder[]>> GetOpenSpotOrdersAsync(GetOpenOrdersRequest request, CancellationToken ct)
        {
            var validationError = GetOpenSpotOrdersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotOrder[]>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedSpotOrder[]>(Exchange, symbolInfoResult.Error!);

            var symbol = request.Symbol?.GetSymbol(FormatSymbol);
            var orders = await _api.Trading.GetOrdersAsync(symbol,
                filter: new Dictionary<string, object>
                {
                    { "open", true }
                },
                ct: ct).ConfigureAwait(false);
            if (!orders.Success)
                return HttpResult.Fail<SharedSpotOrder[]>(orders);

            var data = orders.Data.Where(x => BitMEXUtils.GetSymbolType(x.Symbol) == SymbolType.Spot);
            return HttpResult.Ok(orders, data.Select(x => new SharedSpotOrder(
                ExchangeSymbolCache.ParseSymbol(_topicSpotId, _api.EnvironmentName, null, x.Symbol),
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
                OrderQuantity = new SharedOrderQuantity(x.Quantity.ToSharedSymbolQuantity(x.Symbol)),
                QuantityFilled = new SharedOrderQuantity(x.QuantityFilled.ToSharedSymbolQuantity(x.Symbol)),
                TimeInForce = ParseTimeInForce(x.TimeInForce),
                UpdateTime = x.TransactTime,
                TriggerPrice = x.StopPrice,
                IsTriggerOrder = x.StopPrice != null
            }).ToArray());
        }

        public GetSpotClosedOrdersOptions GetClosedSpotOrdersOptions { get; } = new GetSpotClosedOrdersOptions(_exchangeName, true, true, true, 500);
        public async Task<HttpResult<SharedSpotOrder[]>> GetClosedSpotOrdersAsync(GetClosedOrdersRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = GetClosedSpotOrdersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotOrder[]>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedSpotOrder[]>(Exchange, symbolInfoResult.Error!);

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
                return HttpResult.Fail<SharedSpotOrder[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                () => Pagination.NextPageFromOffset(pageParams, result.Data.Length),
                result.Data.Length,
                result.Data.Select(x => x.Timestamp),
                request.StartTime,
                request.EndTime ?? DateTime.UtcNow,
                pageParams);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data, x => x.Timestamp, request.StartTime, request.EndTime, direction)
                       .Select(x => 
                           new SharedSpotOrder(
                                ExchangeSymbolCache.ParseSymbol(_topicSpotId, _api.EnvironmentName, null, x.Symbol), 
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
                                OrderQuantity = new SharedOrderQuantity(x.Quantity.ToSharedSymbolQuantity(x.Symbol)),
                                QuantityFilled = new SharedOrderQuantity(x.QuantityFilled.ToSharedSymbolQuantity(x.Symbol)),
                                TimeInForce = ParseTimeInForce(x.TimeInForce),
                                UpdateTime = x.TransactTime,
                                TriggerPrice = x.StopPrice,
                                IsTriggerOrder = x.StopPrice != null
                            })
                       .ToArray(), nextPageRequest);
        }

        public GetSpotOrderTradesOptions GetSpotOrderTradesOptions { get; } = new GetSpotOrderTradesOptions(_exchangeName, true);
        public async Task<HttpResult<SharedUserTrade[]>> GetSpotOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct)
        {
            var validationError = GetSpotOrderTradesOptions.ValidateRequest(request, this);
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
                ExchangeSymbolCache.ParseSymbol(_topicSpotId, _api.EnvironmentName, null, x.Symbol), 
                x.Symbol,
                x.OrderId,
                x.TradeId,
                x.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                new SharedOrderQuantity(x.Quantity?.ToSharedSymbolQuantity(x.Symbol) ?? 0),
                x.LastTradePrice!.Value,
                x.Timestamp)
            {
                ClientOrderId = x.ClientOrderId,
                Fee = x.Fee.ToSharedAssetQuantity(x.Currency),
                Role = x.Role == TradeRole.Maker ? SharedRole.Maker : SharedRole.Taker,
                FeeAsset = x.FeeAsset
            }).ToArray());
        }

        Task<HttpResult<SharedUserTrade[]>> ISpotOrderRestClient.GetSpotUserTradesAsync(GetUserTradesRequest request, PageRequest? nextPageToken, CancellationToken ct)
            => GetSpotUserTradeHistoryAsync(request, nextPageToken, ct);
        GetSpotUserTradeHistoryOptions ISpotOrderRestClient.GetSpotUserTradesOptions => GetSpotUserTradeHistoryOptions;

        public GetSpotUserTradeHistoryOptions GetSpotUserTradeHistoryOptions { get; } = new GetSpotUserTradeHistoryOptions(_exchangeName, true, true, true, 1000);
        public async Task<HttpResult<SharedUserTrade[]>> GetSpotUserTradeHistoryAsync(GetUserTradesRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = GetSpotUserTradeHistoryOptions.ValidateRequest(request, this);
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
                                ExchangeSymbolCache.ParseSymbol(_topicSpotId, _api.EnvironmentName, null, x.Symbol), 
                                x.Symbol,
                                x.OrderId,
                                x.TradeId,
                                x.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                                new SharedOrderQuantity(x.Quantity?.ToSharedSymbolQuantity(x.Symbol) ?? 0),
                                x.LastTradePrice!.Value,
                                x.Timestamp)
                            {
                                ClientOrderId = x.ClientOrderId,
                                Fee = x.Fee.ToSharedAssetQuantity(x.Currency),
                                Role = x.Role == TradeRole.Maker ? SharedRole.Maker : SharedRole.Taker,
                                FeeAsset = x.FeeAsset
                           })
                       .ToArray(), nextPageRequest);
        }

        public CancelSpotOrderOptions CancelSpotOrderOptions { get; } = new CancelSpotOrderOptions(_exchangeName, true);
        public async Task<HttpResult<SharedId>> CancelSpotOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = CancelSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var order = await _api.Trading.CancelOrderAsync(request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedId>(order);

            return HttpResult.Ok(order, new SharedId(order.Data.OrderId));
        }

        private Enums.TimeInForce? GetTimeInForce(SharedTimeInForce? tif)
        {
            if (tif == SharedTimeInForce.FillOrKill) return TimeInForce.FillOrKill;
            if (tif == SharedTimeInForce.ImmediateOrCancel) return TimeInForce.ImmediateOrCancel;
            if (tif == SharedTimeInForce.GoodTillCanceled) return TimeInForce.GoodTillCancel;

            return null;
        }

        private SharedOrderStatus ParseOrderStatus(OrderStatus status)
        {
            if (status == OrderStatus.New || status == OrderStatus.PartiallyFilled) return SharedOrderStatus.Open;
            if (status == OrderStatus.Rejected || status == OrderStatus.Canceled) return SharedOrderStatus.Canceled;
            if (status == OrderStatus.Filled) return SharedOrderStatus.Filled;

            return SharedOrderStatus.Unknown;
        }

        private SharedOrderType ParseOrderType(OrderType type, ExecutionInstruction[]? executionInstruction)
        {
            if (type == OrderType.Market) return SharedOrderType.Market;
            if (type == OrderType.Limit && executionInstruction?.Contains(ExecutionInstruction.PostOnly) == true) return SharedOrderType.LimitMaker;
            if (type == OrderType.Limit) return SharedOrderType.Limit;

            return SharedOrderType.Other;
        }

        private SharedTimeInForce? ParseTimeInForce(TimeInForce tif)
        {
            if (tif == TimeInForce.GoodTillCancel) return SharedTimeInForce.GoodTillCanceled;
            if (tif == TimeInForce.ImmediateOrCancel) return SharedTimeInForce.ImmediateOrCancel;
            if (tif == TimeInForce.FillOrKill) return SharedTimeInForce.FillOrKill;

            return null;
        }

        #endregion

        #region Spot Client Id Order Client

        public GetSpotOrderByClientOrderIdOptions GetSpotOrderByClientOrderIdOptions { get; } = new GetSpotOrderByClientOrderIdOptions(_exchangeName, true);
        public async Task<HttpResult<SharedSpotOrder>> GetSpotOrderByClientOrderIdAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = GetSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotOrder>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedSpotOrder>(Exchange, symbolInfoResult.Error!);

            var orders = await _api.Trading.GetOrdersAsync(request.Symbol!.GetSymbol(FormatSymbol),
                filter: new Dictionary<string, object>
                {
                    { "clOrdID", request.OrderId }
                },
                ct: ct).ConfigureAwait(false);
            if (!orders.Success)
                return HttpResult.Fail<SharedSpotOrder>(orders);

            var order = orders.Data.SingleOrDefault();
            if (order == null)
                return HttpResult.Fail<SharedSpotOrder>(orders, new ServerError(new ErrorInfo(ErrorType.UnknownOrder, "Order not found")));

            return HttpResult.Ok(orders, new SharedSpotOrder(
                ExchangeSymbolCache.ParseSymbol(_topicSpotId, _api.EnvironmentName, null, order.Symbol),
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
                OrderQuantity = new SharedOrderQuantity(order.Quantity.ToSharedSymbolQuantity(order.Symbol)),
                QuantityFilled = new SharedOrderQuantity(order.QuantityFilled.ToSharedSymbolQuantity(order.Symbol)),
                TimeInForce = ParseTimeInForce(order.TimeInForce),
                UpdateTime = order.TransactTime,
                TriggerPrice = order.StopPrice,
                IsTriggerOrder = order.StopPrice != null
            });
        }

        public CancelSpotOrderByClientOrderIdOptions CancelSpotOrderByClientOrderIdOptions { get; } = new CancelSpotOrderByClientOrderIdOptions(_exchangeName, true);
        public async Task<HttpResult<SharedId>> CancelSpotOrderByClientOrderIdAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = CancelSpotOrderByClientOrderIdOptions.ValidateRequest(request, this);
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
