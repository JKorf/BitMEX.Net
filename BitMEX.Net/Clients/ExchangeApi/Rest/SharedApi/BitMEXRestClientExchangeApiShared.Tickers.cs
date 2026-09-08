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
        #region Get Ticker

        async Task<ICallResult<SharedTicker>> IGetTicker.GetTickerAsync(GetTickerRequest request, CancellationToken ct)
            => await ((IGetTickerRest)this).GetTickerAsync(request, ct).ConfigureAwait(false);

        async Task<HttpResult<SharedTicker>> IGetTickerRest.GetTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            if (request.Symbol!.TradingMode == TradingMode.Spot)
            {
                var result = await GetSpotTickerAsync(request, ct).ConfigureAwait(false);
                if (!result.Success)
                    return HttpResult.Fail<SharedTicker>(result);

                return HttpResult.Ok<SharedTicker>(result, result.Data);
            }
            else
            {
                var result = await GetFuturesTickerAsync(request, ct).ConfigureAwait(false);
                if (!result.Success)
                    return HttpResult.Fail<SharedTicker>(result);

                return HttpResult.Ok<SharedTicker>(result, result.Data);
            }
        }

        GetTickerOptions ISpotTickerRestClient.GetSpotTickerOptions => GetTickerOptions;
        GetTickerOptions IFuturesTickerRestClient.GetFuturesTickerOptions => GetTickerOptions;

        public GetTickerOptions GetTickerOptions { get; } = new GetTickerOptions(_exchangeName);

        public async Task<HttpResult<SharedSpotTicker>> GetSpotTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var validationError = GetTickerOptions.ValidateRequest(request, this);
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

        public async Task<HttpResult<SharedFuturesTicker>> GetFuturesTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var validationError = GetTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesTicker>(Exchange, validationError);

            var resultTicker = await _api.ExchangeData.GetSymbolsAsync(request.Symbol!.GetSymbol(FormatSymbol), ct: ct).ConfigureAwait(false);
            if (!resultTicker.Success)
                return HttpResult.Fail<SharedFuturesTicker>(resultTicker);

            var symbol = resultTicker.Data.SingleOrDefault();
            if (symbol == null)
                return HttpResult.Fail<SharedFuturesTicker>(resultTicker, new ServerError(new ErrorInfo(ErrorType.UnknownSymbol, "Symbol not found")));

            return HttpResult.Ok(resultTicker,
                new SharedFuturesTicker(
                    ExchangeSymbolCache.ParseSymbol(_topicFuturesId, _api.EnvironmentName, null, symbol.Symbol),
                    symbol.Symbol,
                    symbol.LastPrice,
                    symbol.HighPrice,
                    symbol.LowPrice,
                    new SharedOrderQuantity(symbol.HomeNotional24h, symbol.ForeignNotional24h, symbol.Volume24h),
                    symbol.LastChangePercentage)
            {
                MarkPrice = symbol.MarkPrice,
                FundingRate = symbol.FundingRate,
                NextFundingTime = symbol.FundingTimestamp
            });
        }

        #endregion

        #region Get All Tickers

        async Task<ICallResult<SharedTicker[]>> IGetAllTickers.GetAllTickersAsync(GetTickersRequest request, CancellationToken ct)
            => await ((IGetAllTickersRest)this).GetAllTickersAsync(request, ct).ConfigureAwait(false);

        async Task<HttpResult<SharedTicker[]>> IGetAllTickersRest.GetAllTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            var validationError = GetAllTickersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedTicker[]>(Exchange, validationError);

            if (request.TradingMode == null || request.TradingMode == TradingMode.Spot)
            {
                var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
                if (!symbolInfoResult.Success)
                    return HttpResult.Fail<SharedTicker[]>(Exchange, symbolInfoResult.Error!);
            }

            var result = await _api.ExchangeData.GetActiveSymbolsAsync(ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedTicker[]>(result);

            IEnumerable<BitMEXSymbol> data = result.Data;
            if (request.TradingMode == TradingMode.Spot)
            {
                data = data.Where(x => x.SymbolType == SymbolType.Spot);
            }
            else if (request.TradingMode != null)
            {
                data = data.Where(x => x.SymbolType != SymbolType.Spot);
                data = data.Where(x => request.TradingMode.Value.IsPerpetual() ? x.SymbolType == SymbolType.PerpetualContract : x.SymbolType == SymbolType.Futures);
                data = data.Where(x => request.TradingMode.Value.IsInverse() ? x.IsInverse : !x.IsInverse);
            }

            return HttpResult.Ok<SharedTicker[]>(result, data.Select(x =>
            {
                if (x.SymbolType == SymbolType.Spot)
                {
                    return new SharedSpotTicker(
                        ExchangeSymbolCache.ParseSymbol(_topicSpotId, _api.EnvironmentName, null, x.Symbol),
                        x.Symbol,
                        x.LastPrice,
                        x.HighPrice,
                        x.LowPrice,
                        new SharedOrderQuantity(x.HomeNotional24h, x.ForeignNotional24h, x.Volume24h),
                        Math.Round(x.PrevPrice24h == 0 ? 0 : x.LastPrice / x.PrevPrice24h * 100 - 100, 3));
                }

                return (SharedTicker)new SharedFuturesTicker(
                    ExchangeSymbolCache.ParseSymbol(_topicFuturesId, _api.EnvironmentName, null, x.Symbol),
                    x.Symbol,
                    x.LastPrice,
                    x.HighPrice,
                    x.LowPrice,
                    new SharedOrderQuantity(null, x.Turnover24h, x.Volume24h),
                    x.LastChangePercentage)
                {
                    MarkPrice = x.MarkPrice,
                    FundingRate = x.FundingRate,
                    NextFundingTime = x.FundingTimestamp
                };
            }).ToArray());
        }

        Task<HttpResult<SharedSpotTicker[]>> ISpotTickerRestClient.GetSpotTickersAsync(GetTickersRequest request, CancellationToken ct)
            => GetAllSpotTickersAsync(request, ct);
        GetAllTickersOptions ISpotTickerRestClient.GetSpotTickersOptions => GetAllTickersOptions;

        Task<HttpResult<SharedFuturesTicker[]>> IFuturesTickerRestClient.GetFuturesTickersAsync(GetTickersRequest request, CancellationToken ct)
            => GetAllFuturesTickersAsync(request, ct);
        GetAllTickersOptions IFuturesTickerRestClient.GetFuturesTickersOptions => GetAllTickersOptions;

        public GetAllTickersOptions GetAllTickersOptions { get; } = new GetAllTickersOptions(_exchangeName);

        public async Task<HttpResult<SharedSpotTicker[]>> GetAllSpotTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            var validationError = GetAllTickersOptions.ValidateRequest(request, this);
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

        public async Task<HttpResult<SharedFuturesTicker[]>> GetAllFuturesTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            var validationError = GetAllTickersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesTicker[]>(Exchange, validationError);

            var resultTickers = await _api.ExchangeData.GetActiveSymbolsAsync(ct: ct).ConfigureAwait(false);
            if (!resultTickers.Success)
                return HttpResult.Fail<SharedFuturesTicker[]>(resultTickers);

            var data = resultTickers.Data.Where(x => x.SymbolType != SymbolType.Spot);
            if (request.TradingMode != null)
            {
                data = data.Where(x => request.TradingMode.Value.IsPerpetual() ? x.SymbolType == SymbolType.PerpetualContract : x.SymbolType == SymbolType.Futures);
                data = data.Where(x => request.TradingMode.Value.IsInverse() ? x.IsInverse : !x.IsInverse);
            }

            return HttpResult.Ok(resultTickers, data.Select(x =>
            {
                var markPrice = resultTickers.Data.Single(p => p.Symbol == x.Symbol);
                return new SharedFuturesTicker(
                    ExchangeSymbolCache.ParseSymbol(_topicFuturesId, _api.EnvironmentName, null, x.Symbol),
                    x.Symbol,
                    x.LastPrice,
                    x.HighPrice,
                    x.LowPrice,
                    new SharedOrderQuantity(null, x.Turnover24h, x.Volume24h),
                    x.LastChangePercentage)
                {
                    MarkPrice = markPrice.MarkPrice,
                    FundingRate = markPrice.FundingRate,
                    NextFundingTime = markPrice.FundingTimestamp
                };
            }).ToArray());
        }

        #endregion

    }
}
