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
        #region Trade client

        public SubscribeTradeOptions SubscribeTradeOptions { get; } = new SubscribeTradeOptions(_exchangeName, false)
        {
            SupportsMultipleSymbols = true,
            MaxSymbolCount = 20
        };
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(SubscribeTradeRequest request, Action<DataEvent<SharedTrade[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeTradeOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, symbolInfoResult.Error!);

            var symbols = request.Symbols?.Length > 0 ? request.Symbols.Select(x => x.GetSymbol(FormatSymbol)) : [request.Symbol!.GetSymbol(FormatSymbol)];
            var result = await _api.SubscribeToTradeUpdatesAsync(symbols, update =>
            {
                if (update.UpdateType == SocketUpdateType.Snapshot)
                    return;

                handler(update.ToType<SharedTrade[]>(update.Data.Select(x =>
                    new SharedTrade(
                        ExchangeSymbolCache.ParseSymbol(request.TradingMode == TradingMode.Spot ? _topicSpotId : _topicFuturesId, _api.EnvironmentName, null, x.Symbol),
                        x.Symbol,
                        new SharedOrderQuantity(x.HomeNotional, x.ForeignNotional, x.Quantity),
                        x.Price,
                        x.Timestamp)
                    {
                        Side = x.Side == OrderSide.Sell ? SharedOrderSide.Sell : SharedOrderSide.Buy,
                    }
                    ).ToArray()));
                }, ct).ConfigureAwait(false);

            return result;
        }

        #endregion
    }
}
