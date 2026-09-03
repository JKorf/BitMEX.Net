using CryptoExchange.Net.SharedApis;
using System;
using BitMEX.Net.Interfaces.Clients.ExchangeApi;
using System.Threading.Tasks;
using System.Threading;
using CryptoExchange.Net.Objects.Sockets;
using System.Linq;
using BitMEX.Net.Enums;
using BitMEX.Net.ExtensionMethods;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net;

namespace BitMEX.Net.Clients.ExchangeApi
{
    internal partial class BitMEXSocketClientExchangeSharedApi
    {
        #region Futures Order client
        async Task<WebSocketResult<UpdateSubscription>> IFuturesOrderSocketClient.SubscribeToFuturesOrderUpdatesAsync(SubscribeFuturesOrderRequest request, Action<DataEvent<SharedFuturesOrder[]>> handler, CancellationToken ct)
            => await SubscribeToFuturesOrderUpdatesAsync(request, x => handler(x.ToType<SharedFuturesOrder[]>(x.Data)), ct).ConfigureAwait(false);

        public SubscribeFuturesOrderOptions SubscribeFuturesOrderOptions { get; } = new SubscribeFuturesOrderOptions(_exchangeName, false);
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToFuturesOrderUpdatesAsync(SubscribeFuturesOrderRequest request, Action<DataEvent<SharedFuturesOrderUpdate[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, symbolInfoResult.Error!);

            var result = await _api.SubscribeToOrderUpdatesAsync(
                update => {
                    if (update.UpdateType == SocketUpdateType.Snapshot)
                        return;

                    var data = update.Data.Where(x => BitMEXUtils.GetSymbolType(x.Symbol) != SymbolType.Spot).ToList();
                    if (!data.Any())
                        return;

                    handler(update.ToType<SharedFuturesOrderUpdate[]>(data.Select(x =>
                        new SharedFuturesOrderUpdate(
                            ExchangeSymbolCache.ParseSymbol(_topicFuturesId, _api.EnvironmentName, null, x.Symbol),
                            x.Symbol,
                            x.OrderId,
                            ParseOrderType(x.OrderType, x.ExecutionInstruction),
                            x.OrderSide == Enums.OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                            ParseOrderStatus(x.Status),
                            x.Timestamp)
                        {
                            ClientOrderId = x.ClientOrderId,
                            OrderPrice = x.Price,
                            OrderQuantity = new SharedOrderQuantity(contractQuantity: x.Quantity),
                            QuantityFilled = new SharedOrderQuantity(contractQuantity: x.QuantityFilled),
                            UpdateTime = x.TransactTime,
                            TimeInForce = ParseTimeInForce(x.TimeInForce),
                            AveragePrice = x.AveragePrice,
                            ReduceOnly = x.ExecutionInstruction?.Contains(ExecutionInstruction.ReduceOnly) == true,
                            TriggerPrice = x.StopPrice,
                            IsTriggerOrder = x.StopPrice > 0,
                            IsCloseOrder = x.ExecutionInstruction?.Contains(ExecutionInstruction.Close) == true
                        }
                    ).ToArray()));
                },
                ct: ct).ConfigureAwait(false);

            return result;
        }
        #endregion
    }
}
