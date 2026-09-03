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
        #region Withdrawal client
        Task<HttpResult<SharedWithdrawal[]>> IWithdrawalRestClient.GetWithdrawalsAsync(GetWithdrawalsRequest request, PageRequest? nextPageToken, CancellationToken ct)
            => GetWithdrawalHistoryAsync(request, nextPageToken, ct);
        GetWithdrawalHistoryOptions IWithdrawalRestClient.GetWithdrawalsOptions => GetWithdrawalHistoryOptions;

        public GetWithdrawalHistoryOptions GetWithdrawalHistoryOptions { get; } = new GetWithdrawalHistoryOptions(_exchangeName, true, true, true, 1000)
        {
            RequestNotes = "Due to the API not offering a filter on withdrawal type less results may be returned per page"
        };
        public async Task<HttpResult<SharedWithdrawal[]>> GetWithdrawalHistoryAsync(GetWithdrawalsRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = GetWithdrawalHistoryOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedWithdrawal[]>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedWithdrawal[]>(Exchange, symbolInfoResult.Error!);

            var direction = request.Direction ?? DataDirection.Descending;
            var limit = request.Limit ?? 100;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest, true);
            bool includeNow = request.EndTime == null && pageParams.Offset == 0;

            // Get data
            var result = await _api.Account.GetBalanceHistoryAsync(
                request.Asset != null ? BitMEXExchange.AssetAliases.CommonToExchangeName(request.Asset) : null,
                limit: pageParams.Limit,
                offset: pageParams.Offset,
                reverse: direction == DataDirection.Descending,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedWithdrawal[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                () => Pagination.NextPageFromOffset(pageParams, result.Data.Length),
                result.Data.Length,
                result.Data.Select(x => x.Timestamp),
                request.StartTime,
                request.EndTime ?? DateTime.UtcNow,
                pageParams);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data, x => x.Timestamp, request.StartTime, request.EndTime, direction)
                       .Where(x => x.TransactionType == TransactionType.Withdrawal)
                       .Select(x => 
                            new SharedWithdrawal(
                                BitMEXExchange.AssetAliases.ExchangeToCommonName(BitMEXUtils.GetAssetFromCurrency(x.Currency) ?? x.Currency), 
                                x.Address,
                                x.Quantity.ToSharedAssetQuantity(x.Currency) ?? 0,
                                x.TransactionStatus == TransactionStatus.Completed,
                                x.Timestamp,
                                GetWithdrawalStatus(x))
                            {
                                Network = x.Network,
                                Tag = x.Memo,
                                TransactionId = x.Transaction,
                                Fee = x.Fee.ToSharedAssetQuantity(x.Currency),
                                Id = x.TransactionId
                            })
                       .ToArray(), nextPageRequest);
        }

        private SharedTransferStatus GetWithdrawalStatus(BitMEXTransaction x)
        {
            if (x.TransactionStatus == TransactionStatus.Canceled)
                return SharedTransferStatus.Failed;

            if (x.TransactionStatus == TransactionStatus.Completed)
                return SharedTransferStatus.Completed;

            return SharedTransferStatus.Unknown;
        }

        #endregion

        #region Withdraw client

        public WithdrawOptions WithdrawOptions { get; } = new WithdrawOptions(_exchangeName)
        {
            RequiredRequestParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(WithdrawRequest.Network), typeof(string), "Network to use", "btc")
            }
        };
        public async Task<HttpResult<SharedId>> WithdrawAsync(WithdrawRequest request, CancellationToken ct)
        {
            var validationError = WithdrawOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedId>(Exchange, symbolInfoResult.Error!);

            // Get data
            var withdrawal = await _api.Account.WithdrawAsync(
                BitMEXExchange.AssetAliases.ExchangeToCommonName(request.Asset),
                network: request.Network!,
                request.Quantity.ToBitMEXAssetQuantity(request.Asset) ?? 0,
                request.Address,
                memo: request.AddressTag,
                ct: ct).ConfigureAwait(false);
            if (!withdrawal.Success)
                return HttpResult.Fail<SharedId>(withdrawal);

            return HttpResult.Ok(withdrawal, new SharedId(withdrawal.Data.TransactionId));
        }

        #endregion
    }
}
