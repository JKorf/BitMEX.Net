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
        #region Futures Symbol client
        public SharedSymbolCatalog? FuturesSymbolCatalog => ExchangeSymbolCache.GetSymbolCatalog(_exchangeName, _topicFuturesId, _api.EnvironmentName, null);

        public GetFuturesSymbolsOptions GetFuturesSymbolsOptions { get; } = new GetFuturesSymbolsOptions(_exchangeName, false);
        public async Task<HttpResult<SharedFuturesSymbol[]>> GetFuturesSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
        {
            var validationError = GetFuturesSymbolsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesSymbol[]>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedFuturesSymbol[]>(Exchange, symbolInfoResult.Error!);

            var result = await _api.ExchangeData.GetActiveSymbolsAsync(ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedFuturesSymbol[]>(result);

            // Quanto not supported as it doesn't currently fit in the response models
            var data = result.Data.Where(x => x.SymbolType != SymbolType.Spot && !x.IsQuanto);
            var symbols = data
                .Select(x => ParseFuturesSymbol(x))
                .ToArray();

            ExchangeSymbolCache.UpdateSymbolInfo(_topicFuturesId, _api.EnvironmentName, null, symbols);
            return HttpResult.Ok(result, SharedUtils.ApplySymbolFilter(symbols, request));
        }

        private SharedFuturesSymbol ParseFuturesSymbol(BitMEXSymbol s)
        {
            var result = new SharedFuturesSymbol(
                    s.SymbolType == SymbolType.PerpetualContract ?
                            (s.IsInverse ? TradingMode.PerpetualInverse : TradingMode.PerpetualLinear) :
                            (s.IsInverse ? TradingMode.DeliveryInverse : TradingMode.DeliveryLinear),
                    BitMEXExchange.AssetAliases.ExchangeToCommonName(s.BaseAsset),
                    BitMEXExchange.AssetAliases.ExchangeToCommonName(s.QuoteAsset),
                    s.Symbol,
                    s.Status == SymbolStatus.Open)
            {
                MinTradeQuantity = s.LotSize,
                MaxTradeQuantity = s.MaxOrderQuantity,
                QuantityStep = s.LotSize,
                PriceStep = s.PriceStep,
                ContractSize = s.IsInverse ? (s.Multiplier / s.UnderlyingToSettleMultiplier) : (1 / s.UnderlyingToPositionMultiplier),
                DeliveryTime = s.SettleTime,
                QuoteAssetType = SharedAssetType.Crypto,
                QuoteAssetSubType = SharedAssetSubType.StableCoin,
                DisplayName = s.Symbol
            };
            
            if (s.Tags.Contains("cm"))
            {
                result.BaseAssetType = SharedAssetType.TradFi;
                result.BaseAssetSubType = SharedAssetSubType.Commodity;
            }
            else if (s.Tags.Contains("fx"))
            {
                result.BaseAssetType = SharedAssetType.Fiat;
            }
            else if (s.Tags.Contains("eq"))
            {
                result.BaseAssetType = SharedAssetType.TradFi;
                result.BaseAssetSubType = SharedAssetSubType.Equity;
            }
            else
            {
                result.BaseAssetType = SharedAssetType.Crypto;
            }

            return result;
        }

        public async Task<ExchangeCallResult<SharedSymbol[]>> GetFuturesSymbolsForBaseAssetAsync(string baseAsset)
        {
            if (!ExchangeSymbolCache.HasCached(_topicFuturesId, _api.EnvironmentName, null))
            {
                var symbols = await GetFuturesSymbolsAsync(new GetSymbolsRequest(), default).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<SharedSymbol[]>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<SharedSymbol[]>.Ok(Exchange, ExchangeSymbolCache.GetSymbolsForBaseAsset(_topicFuturesId, _api.EnvironmentName, null, baseAsset));
        }

        public async Task<ExchangeCallResult<bool>> SupportsFuturesSymbolAsync(SharedSymbol symbol)
        {
            if (symbol.TradingMode == TradingMode.Spot)
                throw new ArgumentException(nameof(symbol), "Spot symbols not allowed");

            if (!ExchangeSymbolCache.HasCached(_topicFuturesId, _api.EnvironmentName, null))
            {
                var symbols = await GetFuturesSymbolsAsync(new GetSymbolsRequest(), default).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicFuturesId, _api.EnvironmentName, null, symbol));
        }

        public async Task<ExchangeCallResult<bool>> SupportsFuturesSymbolAsync(string symbolName)
        {
            if (!ExchangeSymbolCache.HasCached(_topicFuturesId, _api.EnvironmentName, null))
            {
                var symbols = await GetFuturesSymbolsAsync(new GetSymbolsRequest(), default).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicFuturesId, _api.EnvironmentName, null, symbolName));
        }
        #endregion
    }
}
