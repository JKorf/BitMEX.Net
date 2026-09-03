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
        #region Futures Ticker client

        public GetFuturesTickerOptions GetFuturesTickerOptions { get; } = new GetFuturesTickerOptions(_exchangeName);
        public async Task<HttpResult<SharedFuturesTicker>> GetFuturesTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var validationError = GetFuturesTickerOptions.ValidateRequest(request, this);
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

        Task<HttpResult<SharedFuturesTicker[]>> IFuturesTickerRestClient.GetFuturesTickersAsync(GetTickersRequest request, CancellationToken ct)
            => GetAllFuturesTickersAsync(request, ct);
        GetAllFuturesTickersOptions IFuturesTickerRestClient.GetFuturesTickersOptions => GetAllFuturesTickersOptions;

        public GetAllFuturesTickersOptions GetAllFuturesTickersOptions { get; } = new GetAllFuturesTickersOptions(_exchangeName);
        public async Task<HttpResult<SharedFuturesTicker[]>> GetAllFuturesTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            var validationError = GetAllFuturesTickersOptions.ValidateRequest(request, this);
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
