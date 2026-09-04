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
        #region Place Futures Trigger Order

        public PlaceFuturesTriggerOrderOptions PlaceFuturesTriggerOrderOptions { get; } = new PlaceFuturesTriggerOrderOptions(_exchangeName, false)
        {
        };

        async Task<ICallResult<SharedId>> IPlaceFuturesTriggerOrder.PlaceFuturesTriggerOrderAsync(PlaceFuturesTriggerOrderRequest request, CancellationToken ct)
            => await PlaceFuturesTriggerOrderAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedId>> PlaceFuturesTriggerOrderAsync(PlaceFuturesTriggerOrderRequest request, CancellationToken ct)
        {
            var side = GetTriggerOrderSide(request);
            var validationError = PlaceFuturesTriggerOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var result = await _api.Trading.PlaceOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                side,
                GetTriggerOrderType(request),
                quantity: (int)(request.Quantity?.QuantityInContracts ?? 0),
                price: request.OrderPrice,
                stopPrice: request.TriggerPrice,
                executionInstruction: GetExecutionInstruction(request),
                timeInForce: GetTimeInForce(request.TimeInForce),
                clientOrderId: request.ClientOrderId,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedId>(result);

            // Return
            return HttpResult.Ok(result, new SharedId(result.Data.OrderId.ToString()));
        }

        #endregion

        #region Get Futures Trigger Order

        public GetFuturesTriggerOrderOptions GetFuturesTriggerOrderOptions { get; } = new GetFuturesTriggerOrderOptions(_exchangeName, true)
        {
            RequestNotes = "Only pending trigger orders can be requested, executed trigger orders are not available in the API"
        };
        async Task<ICallResult<SharedFuturesTriggerOrder>> IGetFuturesTriggerOrder.GetFuturesTriggerOrderAsync(GetOrderRequest request, CancellationToken ct)
            => await GetFuturesTriggerOrderAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedFuturesTriggerOrder>> GetFuturesTriggerOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = GetFuturesTriggerOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesTriggerOrder>(Exchange, validationError);

            var orders = await _api.Trading.GetOrdersAsync(request.Symbol!.GetSymbol(FormatSymbol),
                filter: new Dictionary<string, object>
                {
                    { "orderID", request.OrderId }
                },
                ct: ct).ConfigureAwait(false);
            if (!orders.Success)
                return HttpResult.Fail<SharedFuturesTriggerOrder>(orders);

            var order = orders.Data.SingleOrDefault();
            if (order == null)
                return HttpResult.Fail<SharedFuturesTriggerOrder>(orders, new ServerError(new ErrorInfo(ErrorType.UnknownOrder, "Order not found")));

            return HttpResult.Ok(orders, new SharedFuturesTriggerOrder(
                ExchangeSymbolCache.ParseSymbol(_topicFuturesId, _api.EnvironmentName, null, order.Symbol),
                order.Symbol,
                order.OrderId,
                order.OrderType == OrderType.StopLimit ? SharedOrderType.Limit : SharedOrderType.Market,
                null,
                ParseTriggerOrderStatus(order.Status),
                order.StopPrice ?? 0,
                null,
                order.Timestamp)
            {
                AveragePrice = order.AveragePrice,
                OrderPrice = order.Price,
                OrderQuantity = new SharedOrderQuantity(contractQuantity: order.Quantity),
                QuantityFilled = new SharedOrderQuantity(contractQuantity: order.QuantityFilled),
                TimeInForce = ParseTimeInForce(order.TimeInForce),
                UpdateTime = order.TransactTime,
                ClientOrderId = order.ClientOrderId
            });
        }

        #endregion

        #region Cancel Futures Trigger Order

        public CancelFuturesTriggerOrderOptions CancelFuturesTriggerOrderOptions { get; } = new CancelFuturesTriggerOrderOptions(_exchangeName, true);
        async Task<ICallResult<SharedId>> ICancelFuturesTriggerOrder.CancelFuturesTriggerOrderAsync(CancelOrderRequest request, CancellationToken ct)
            => await CancelFuturesTriggerOrderAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedId>> CancelFuturesTriggerOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = CancelFuturesTriggerOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var order = await _api.Trading.CancelOrderAsync(
                request.OrderId,
                ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedId>(order);

            return HttpResult.Ok(order, new SharedId(request.OrderId));
        }

        #endregion

        private OrderType GetTriggerOrderType(PlaceSpotTriggerOrderRequest request)
        {
            if (request.OrderSide == SharedOrderSide.Buy)
            {
                if (request.PriceDirection == SharedTriggerPriceDirection.PriceAbove)
                    return request.OrderPrice == null ? OrderType.StopMarket : OrderType.StopLimit;

                return request.OrderPrice == null ? OrderType.MarketIfTouched : OrderType.LimitIfTouched;
            }

            if (request.PriceDirection == SharedTriggerPriceDirection.PriceAbove)
                return request.OrderPrice == null ? OrderType.MarketIfTouched : OrderType.LimitIfTouched;

            return request.OrderPrice == null ? OrderType.StopMarket : OrderType.StopLimit;
        }

        private OrderType GetTriggerOrderType(PlaceFuturesTriggerOrderRequest request)
        {
            if (request.PositionSide == SharedPositionSide.Long)
            {
                if (request.OrderDirection == SharedTriggerOrderDirection.Enter)
                {
                    // Long enter
                    if (request.PriceDirection == SharedTriggerPriceDirection.PriceAbove)
                        return request.OrderPrice == null ? OrderType.StopMarket : OrderType.StopLimit;

                    return request.OrderPrice == null ? OrderType.MarketIfTouched : OrderType.LimitIfTouched;
                }

                // Long exit
                if (request.PriceDirection == SharedTriggerPriceDirection.PriceAbove)
                    return request.OrderPrice == null ? OrderType.MarketIfTouched : OrderType.LimitIfTouched;

                return request.OrderPrice == null ? OrderType.StopMarket : OrderType.StopLimit;
            }
            else
            {
                if (request.OrderDirection == SharedTriggerOrderDirection.Enter)
                {
                    // Short enter
                    if (request.PriceDirection == SharedTriggerPriceDirection.PriceAbove)
                        return request.OrderPrice == null ? OrderType.MarketIfTouched : OrderType.LimitIfTouched;

                    return request.OrderPrice == null ? OrderType.StopMarket : OrderType.StopLimit;
                }

                // Short exit
                if (request.PriceDirection == SharedTriggerPriceDirection.PriceAbove)
                    return request.OrderPrice == null ? OrderType.StopMarket : OrderType.StopLimit;

                return request.OrderPrice == null ? OrderType.MarketIfTouched : OrderType.LimitIfTouched;
            }
        }

        private ExecutionInstruction? GetExecutionInstruction(PlaceFuturesTriggerOrderRequest request)
        {
            return request.TriggerPriceType switch
            {
                null => null,
                SharedTriggerPriceType.LastPrice => ExecutionInstruction.LastPrice,
                SharedTriggerPriceType.IndexPrice => ExecutionInstruction.IndexPrice,
                _ => ExecutionInstruction.MarkPrice
            };
        }

        private OrderSide GetTriggerOrderSide(PlaceFuturesTriggerOrderRequest request)
        {
            if (request.PositionSide == SharedPositionSide.Long)
                return request.OrderDirection == SharedTriggerOrderDirection.Enter ? OrderSide.Buy : OrderSide.Sell;

            return request.OrderDirection == SharedTriggerOrderDirection.Enter ? OrderSide.Sell : OrderSide.Buy;
        }
    }
}
