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
        #region Get Deposit Addresses

        public GetDepositAddressesOptions GetDepositAddressesOptions { get; } = new GetDepositAddressesOptions(_exchangeName, true)
        {
            ParameterRuleOverrides = [
                RequestParameterRuleOverride<GetDepositAddressesRequest>.Required(x => x.Network)
            ]
        };
        async Task<ICallResult<SharedDepositAddress[]>> IGetDepositAddresses.GetDepositAddressesAsync(GetDepositAddressesRequest request, CancellationToken ct)
            => await GetDepositAddressesAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedDepositAddress[]>> GetDepositAddressesAsync(GetDepositAddressesRequest request, CancellationToken ct)
        {
            var validationError = GetDepositAddressesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedDepositAddress[]>(Exchange, validationError);

            var depositAddresses = await _api.Account.GetDepositAddressAsync(BitMEXExchange.AssetAliases.CommonToExchangeName(request.Asset), request.Network!, ct: ct).ConfigureAwait(false);
            if (!depositAddresses.Success)
                return HttpResult.Fail<SharedDepositAddress[]>(depositAddresses);

            return HttpResult.Ok(depositAddresses, new[] { new SharedDepositAddress(request.Asset, depositAddresses.Data) });
        }

        #endregion

        #region Get Deposit History

        Task<HttpResult<SharedDeposit[]>> IDepositRestClient.GetDepositsAsync(GetDepositsRequest request, PageRequest? nextPageToken, CancellationToken ct)
            => GetDepositHistoryAsync(request, nextPageToken, ct);
        GetDepositHistoryOptions IDepositRestClient.GetDepositsOptions => GetDepositHistoryOptions;

        public GetDepositHistoryOptions GetDepositHistoryOptions { get; } = new GetDepositHistoryOptions(_exchangeName, true, true, false, 10000)
        {
            RequestNotes = "Due to the API not offering a filter on deposit type less results may be returned per page",
            ParameterRuleOverrides = [
                RequestParameterRuleOverride<GetDepositsRequest>.NotSupported(x => x.StartTime),
                RequestParameterRuleOverride<GetDepositsRequest>.NotSupported(x => x.EndTime)
                ]
        };
        async Task<ICallResult<SharedDeposit[]>> IGetDepositHistory.GetDepositHistoryAsync(GetDepositsRequest request, PageRequest? pageRequest, CancellationToken ct)
            => await GetDepositHistoryAsync(request, pageRequest, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedDeposit[]>> GetDepositHistoryAsync(GetDepositsRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = GetDepositHistoryOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedDeposit[]>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedDeposit[]>(Exchange, symbolInfoResult.Error!);

            var direction = request.Direction ?? DataDirection.Descending;
            var limit = request.Limit ?? 100;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest, true);

            // Get data
            var result = await _api.Account.GetBalanceHistoryAsync(
                request.Asset != null ? BitMEXExchange.AssetAliases.CommonToExchangeName(request.Asset) : null,
                limit: pageParams.Limit,
                offset: pageParams.Offset,
                reverse: direction == DataDirection.Descending,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedDeposit[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                () => Pagination.NextPageFromOffset(pageParams, result.Data.Length),
                result.Data.Length,
                result.Data.Select(x => x.Timestamp),
                request.StartTime,
                request.EndTime ?? DateTime.UtcNow,
                pageParams);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data, x => x.Timestamp, request.StartTime, request.EndTime, direction)
                       .Where(x => x.TransactionType == TransactionType.Deposit)
                       .Select(x => 
                            new SharedDeposit(
                                BitMEXExchange.AssetAliases.ExchangeToCommonName(BitMEXUtils.GetAssetFromCurrency(x.Currency) ?? x.Currency),
                                x.Quantity.ToSharedAssetQuantity(x.Currency) ?? 0,
                                x.TransactionStatus == TransactionStatus.Completed,
                                x.TransactionTime,
                                ParseTransferStatus(x.TransactionStatus))
                            {
                                Network = x.Network,
                                TransactionId = x.Transaction,
                                Tag = x.Memo,
                                Id = x.TransactionId
                            })
                       .ToArray(), nextPageRequest);
        }

        #endregion

        private SharedTransferStatus ParseTransferStatus(TransactionStatus status)
        {
            if (status == TransactionStatus.Completed)
                return SharedTransferStatus.Completed;

            if (status == TransactionStatus.Canceled)
                return SharedTransferStatus.Failed;

            return SharedTransferStatus.Unknown;
        }

    }
}
