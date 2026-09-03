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
        #region Spot Ticker client

        public GetSpotTickerOptions GetSpotTickerOptions { get; } = new GetSpotTickerOptions(_exchangeName);
        public async Task<HttpResult<SharedSpotTicker>> GetSpotTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var validationError = GetSpotTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotTicker>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedSpotTicker>(Exchange, symbolInfoResult.Error!);

            var result = await _api.ExchangeData.GetActiveSymbolsAsync(ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedSpotTicker>(result);

            var symbol = result.Data.SingleOrDefault(x => x.Symbol == request.Symbol!.GetSymbol(FormatSymbol));
            if (symbol == null)
                return HttpResult.Fail<SharedSpotTicker>(result, new ServerError(new ErrorInfo(ErrorType.UnknownSymbol, "Symbol not found")));

            return HttpResult.Ok(result, new SharedSpotTicker(
                ExchangeSymbolCache.ParseSymbol(_topicSpotId, _api.EnvironmentName, null, symbol.Symbol),
                symbol.Symbol,
                symbol.LastPrice,
                symbol.HighPrice,
                symbol.LowPrice,
                new SharedOrderQuantity(symbol.Volume24h.ToSharedSymbolQuantity(symbol.Symbol), symbol.Turnover24h.ToSharedAssetQuantity(symbol.Symbol!.Split('_').Last()), symbol.Volume24h),
                Math.Round(symbol.PrevPrice24h == 0 ? 0 : symbol.LastPrice / symbol.PrevPrice24h * 100 - 100, 3)
                )
            {
            });
        }

        Task<HttpResult<SharedSpotTicker[]>> ISpotTickerRestClient.GetSpotTickersAsync(GetTickersRequest request, CancellationToken ct)
            => GetAllSpotTickersAsync(request, ct);
        GetAllSpotTickersOptions ISpotTickerRestClient.GetSpotTickersOptions => GetAllSpotTickersOptions;

        public GetAllSpotTickersOptions GetAllSpotTickersOptions { get; } = new GetAllSpotTickersOptions(_exchangeName);
        public async Task<HttpResult<SharedSpotTicker[]>> GetAllSpotTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            var validationError = GetAllSpotTickersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotTicker[]>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedSpotTicker[]>(Exchange, symbolInfoResult.Error!);

            var result = await _api.ExchangeData.GetActiveSymbolsAsync(ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedSpotTicker[]>(result);

            return HttpResult.Ok(result, result.Data.Where(x => x.SymbolType == SymbolType.Spot).Select(x =>
            new SharedSpotTicker(
                ExchangeSymbolCache.ParseSymbol(_topicSpotId, _api.EnvironmentName, null, x.Symbol),
                x.Symbol,
                x.LastPrice,
                x.HighPrice,
                x.LowPrice,
                new SharedOrderQuantity(x.HomeNotional24h, x.ForeignNotional24h, x.Volume24h),
                Math.Round(x.PrevPrice24h == 0 ? 0 : x.LastPrice / x.PrevPrice24h * 100 - 100, 3))
            {
            }).ToArray());
        }

        #endregion
    }
}
