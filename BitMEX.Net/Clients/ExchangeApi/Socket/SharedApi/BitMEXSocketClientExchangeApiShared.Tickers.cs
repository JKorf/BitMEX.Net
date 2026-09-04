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
        #region Subscribe To Ticker Updates

        async Task<WebSocketResult<UpdateSubscription>> ISubscribeTickerSocket.SubscribeToTickerUpdatesAsync(SubscribeTickerRequest request, Action<DataEvent<SharedTicker>> handler, CancellationToken ct)
            => await SubscribeToTickerUpdatesAsync(request, x => handler(x.ToType<SharedTicker>(x.Data)), ct).ConfigureAwait(false);

        public SubscribeTickerOptions SubscribeTickerOptions { get; } = new SubscribeTickerOptions(_exchangeName);
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(SubscribeTickerRequest request, Action<DataEvent<SharedSpotTicker>> handler, CancellationToken ct)
        {
            var validationError = SubscribeTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, symbolInfoResult.Error!);

            SharedSpotTicker? ticker = null;
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await _api.SubscribeToSymbolUpdatesAsync(symbol, update =>
            {
                if (ticker == null)
                {
                    ticker = new SharedSpotTicker(
                        ExchangeSymbolCache.ParseSymbol(request.Symbol.TradingMode == TradingMode.Spot ? _topicSpotId : _topicFuturesId, _api.EnvironmentName, null, update.Data.Symbol),
                        update.Data.Symbol,
                        update.Data.LastPrice,
                        update.Data.HighPrice,
                        update.Data.LowPrice,
                        new SharedOrderQuantity(
                            update.Data.HomeNotional24h,
                            update.Data.ForeignNotional24h, 
                            update.Data.Volume24h),
                        update.Data.LastChangePcnt * 100
                        )
                    {
                    };
                }
                else
                {
                    ticker.LastPrice = update.Data.LastPrice ?? ticker.LastPrice;
                    ticker.HighPrice = update.Data.HighPrice ?? ticker.HighPrice;
                    ticker.LowPrice = update.Data.LowPrice ?? ticker.LowPrice;
                    ticker.Volumes = new SharedOrderQuantity(
                        (update.Data.HomeNotional24h ?? ticker.Volumes.QuantityInBaseAsset),
                        update.Data.ForeignNotional24h ?? ticker.Volumes.QuantityInQuoteAsset,
                        update.Data.Volume24h ?? ticker.Volumes.QuantityInContracts);
                    ticker.ChangePercentage = update.Data.LastChangePcnt == null ? ticker.ChangePercentage : update.Data.LastChangePcnt * 100;
                }

                handler(update.ToType(ticker));
            }, ct).ConfigureAwait(false);

            return result;
        }

        #endregion
    }
}
