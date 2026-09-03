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
        #region Balance client
        public SubscribeBalanceOptions SubscribeBalanceOptions { get; } = new SubscribeBalanceOptions(_exchangeName, false);
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(SubscribeBalancesRequest request, Action<DataEvent<SharedBalance[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeBalanceOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, symbolInfoResult.Error!);

            var result = await _api.SubscribeToBalanceUpdatesAsync(
                update => handler(update.ToType<SharedBalance[]>(update.Data.Select(x => 
                new SharedBalance(
                    SupportedTradingModes,
                    BitMEXExchange.AssetAliases.ExchangeToCommonName(BitMEXUtils.GetAssetFromCurrency(x.Currency) ?? x.Currency), 
                    x.Quantity.ToSharedAssetQuantity(x.Currency) ?? 0,
                    (x.Quantity + x.PendingCredit).ToSharedAssetQuantity(x.Currency) ?? 0)).ToArray())),
                ct: ct).ConfigureAwait(false);

            return result;
        }

        #endregion
    }
}
