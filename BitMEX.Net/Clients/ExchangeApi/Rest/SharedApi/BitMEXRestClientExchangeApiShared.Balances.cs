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
        #region Balance Client
        public GetBalancesOptions GetBalancesOptions { get; } = new GetBalancesOptions(_exchangeName, AccountTypeFilter.Spot, AccountTypeFilter.Funding, AccountTypeFilter.Futures);

        public async Task<HttpResult<SharedBalance[]>> GetBalancesAsync(GetBalancesRequest request, CancellationToken ct)
        {
            var validationError = GetBalancesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedBalance[]>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedBalance[]>(Exchange, symbolInfoResult.Error!);

            var result = await _api.Account.GetBalancesAsync(ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedBalance[]>(result);

            return HttpResult.Ok(result, result.Data.Select(x => 
                new SharedBalance(
                    SupportedTradingModes,
                    BitMEXExchange.AssetAliases.ExchangeToCommonName(BitMEXUtils.GetAssetFromCurrency(x.Currency) ?? x.Currency),
                    x.Quantity.ToSharedAssetQuantity(x.Currency) ?? 0,
                    (x.Quantity + x.PendingCredit).ToSharedAssetQuantity(x.Currency) ?? 0)
                ).ToArray());
        }

        #endregion
    }
}
