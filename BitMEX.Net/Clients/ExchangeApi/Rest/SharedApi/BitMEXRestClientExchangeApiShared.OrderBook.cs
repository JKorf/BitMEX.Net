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
        #region Order Book client
        public GetOrderBookOptions GetOrderBookOptions { get; } = new GetOrderBookOptions(_exchangeName, 1, 5000, false);
        public async Task<HttpResult<SharedOrderBook>> GetOrderBookAsync(GetOrderBookRequest request, CancellationToken ct)
        {
            var validationError = GetOrderBookOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedOrderBook>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedOrderBook>(Exchange, symbolInfoResult.Error!);

            var result = await _api.ExchangeData.GetOrderBookAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                limit: request.Limit ?? 20,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedOrderBook>(result);

            var spot = request.Symbol!.TradingMode == TradingMode.Spot;
            var book = new SharedOrderBook(spot ? SharedQuantityType.BaseAsset : SharedQuantityType.Contracts, null, result.Data.Asks, result.Data.Bids);
            if (spot)
            {
                foreach (var item in book.Asks)
                    item.Quantity = ((long)item.Quantity).ToSharedAssetQuantity(request.Symbol!.BaseAsset) ?? 0;
                foreach (var item in book.Bids)
                    item.Quantity = ((long)item.Quantity).ToSharedAssetQuantity(request.Symbol!.BaseAsset) ?? 0;
            }

            return HttpResult.Ok(result, book);
        }

        #endregion
    }
}
