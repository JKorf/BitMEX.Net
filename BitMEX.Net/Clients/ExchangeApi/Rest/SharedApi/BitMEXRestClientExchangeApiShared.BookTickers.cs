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
        #region Get Book Ticker

        public GetBookTickerOptions GetBookTickerOptions { get; } = new GetBookTickerOptions(_exchangeName, false);
        async Task<ICallResult<SharedBookTicker>> IGetBookTicker.GetBookTickerAsync(GetBookTickerRequest request, CancellationToken ct)
            => await GetBookTickerAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedBookTicker>> GetBookTickerAsync(GetBookTickerRequest request, CancellationToken ct)
        {
            var validationError = GetBookTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedBookTicker>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedBookTicker>(Exchange, symbolInfoResult.Error!);

            var resultTicker = await _api.ExchangeData.GetBookTickerHistoryAsync(request.Symbol!.GetSymbol(FormatSymbol), reverse: true, limit: 1, ct: ct).ConfigureAwait(false);
            if (!resultTicker.Success)
                return HttpResult.Fail<SharedBookTicker>(resultTicker);

            if (!resultTicker.Data.Any())
                return HttpResult.Fail<SharedBookTicker>(resultTicker, new ServerError(new ErrorInfo(ErrorType.UnknownSymbol, "Symbol not found")));

            var spot = request.Symbol!.TradingMode == TradingMode.Spot;
            return HttpResult.Ok(resultTicker, new SharedBookTicker(
                ExchangeSymbolCache.ParseSymbol(spot ? _topicSpotId : _topicFuturesId, _api.EnvironmentName, null, resultTicker.Data[0].Symbol),
                resultTicker.Data[0].Symbol,
                resultTicker.Data[0].BestAskPrice,
                new SharedOrderQuantity(spot ? resultTicker.Data[0].BestAskQuantity.ToSharedSymbolQuantity(resultTicker.Data[0].Symbol) : null, 
                                        contractQuantity: spot ? null : resultTicker.Data[0].BestAskQuantity.ToSharedSymbolQuantity(resultTicker.Data[0].Symbol)),
                resultTicker.Data[0].BestBidPrice,
                new SharedOrderQuantity(spot ? resultTicker.Data[0].BestBidQuantity.ToSharedSymbolQuantity(resultTicker.Data[0].Symbol) : null, 
                                        contractQuantity: spot ? null : resultTicker.Data[0].BestBidQuantity.ToSharedSymbolQuantity(resultTicker.Data[0].Symbol))));
        }

        #endregion

    }
}
