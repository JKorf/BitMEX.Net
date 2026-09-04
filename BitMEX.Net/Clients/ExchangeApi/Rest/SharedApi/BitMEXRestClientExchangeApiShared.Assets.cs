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
        #region Get All Assets

        Task<HttpResult<SharedAsset[]>> IAssetsRestClient.GetAssetsAsync(GetAssetsRequest request, CancellationToken ct)
            => GetAllAssetsAsync(request, ct);
        GetAllAssetsOptions IAssetsRestClient.GetAssetsOptions => GetAllAssetsOptions;

        public GetAllAssetsOptions GetAllAssetsOptions { get; } = new GetAllAssetsOptions(_exchangeName, true);

        async Task<ICallResult<SharedAsset[]>> IGetAllAssets.GetAllAssetsAsync(GetAssetsRequest request, CancellationToken ct)
            => await GetAllAssetsAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedAsset[]>> GetAllAssetsAsync(GetAssetsRequest request, CancellationToken ct)
        {
            var validationError = GetAllAssetsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedAsset[]>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedAsset[]>(Exchange, symbolInfoResult.Error!);

            var assets = await _api.ExchangeData.GetAssetsAsync(ct: ct).ConfigureAwait(false);
            if (!assets.Success)
                return HttpResult.Fail<SharedAsset[]>(assets);

            return HttpResult.Ok(assets, assets.Data.Select(x =>
            {
                return new SharedAsset(BitMEXExchange.AssetAliases.ExchangeToCommonName(x.Asset))
                {
                    FullName = x.Name,
                    Networks = x.Networks.Select(n => new SharedAssetNetwork(n.Asset)
                        {
                            DepositEnabled = n.DepositEnabled,
                            MinWithdrawQuantity = x.MinWithdrawalQuantity.ToSharedAssetQuantity(x.Currency),
                            MaxWithdrawQuantity = x.MaxWithdrawalQuantity.ToSharedAssetQuantity(x.Currency),
                            WithdrawEnabled = n.WithdrawalEnabled,
                            WithdrawFee = n.WithdrawalFee.ToSharedAssetQuantity(x.Currency) ?? n.MinFee.ToSharedAssetQuantity(x.Currency),
                            ContractAddress = n.TokenAddress
                        }
                    ).ToArray()
                };
            }).ToArray());
        }

        #endregion

        #region Get Asset

        public GetAssetOptions GetAssetOptions { get; } = new GetAssetOptions(_exchangeName, false);
        async Task<ICallResult<SharedAsset>> IGetAsset.GetAssetAsync(GetAssetRequest request, CancellationToken ct)
            => await GetAssetAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedAsset>> GetAssetAsync(GetAssetRequest request, CancellationToken ct)
        {
            var validationError = GetAssetOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedAsset>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedAsset>(Exchange, symbolInfoResult.Error!);

            var assets = await _api.ExchangeData.GetAssetsAsync(ct: ct).ConfigureAwait(false);
            if (!assets.Success)
                return HttpResult.Fail<SharedAsset>(assets);

            var asset = assets.Data.SingleOrDefault(x => x.Asset == request.Asset || BitMEXExchange.AssetAliases.CommonToExchangeName(x.Asset) == request.Asset);
            if (asset == null)
                return HttpResult.Fail<SharedAsset>(assets, new ServerError(new ErrorInfo(ErrorType.UnknownAsset, "Asset not found")));

            return HttpResult.Ok(assets, 
                new SharedAsset(BitMEXExchange.AssetAliases.ExchangeToCommonName(asset.Asset))
                {
                    FullName = asset.Name,
                    Networks = asset.Networks.Select(n => new SharedAssetNetwork(n.Asset)
                    {
                        DepositEnabled = n.DepositEnabled,
                        MinWithdrawQuantity = asset.MinWithdrawalQuantity.ToSharedAssetQuantity(asset.Currency),
                        MaxWithdrawQuantity = asset.MaxWithdrawalQuantity.ToSharedAssetQuantity(asset.Currency),
                        WithdrawEnabled = n.WithdrawalEnabled,
                        WithdrawFee = n.WithdrawalFee.ToSharedAssetQuantity(asset.Currency) ?? n.MinFee.ToSharedAssetQuantity(asset.Currency),
                        ContractAddress = n.TokenAddress
                    }
                    ).ToArray()
                }
            );
        }

        #endregion

    }
}
