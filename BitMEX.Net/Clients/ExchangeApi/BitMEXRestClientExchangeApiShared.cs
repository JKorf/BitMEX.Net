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
    internal partial class BitMEXRestClientExchangeApi : IBitMEXRestClientExchangeApiShared
    {
        private const string _topicSpotId = "BitMEXSpot";
        private const string _topicFuturesId = "BitMEXFutures";
        private const string _exchangeName = "BitMEX";

        public TradingMode[] SupportedTradingModes => new[] { TradingMode.Spot, TradingMode.PerpetualLinear, TradingMode.DeliveryLinear, TradingMode.PerpetualInverse, TradingMode.DeliveryInverse };

        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();
        public SharedClientInfo Discover() => SharedUtils.GetClientInfo(BitMEXExchange.Metadata, this);

        #region Asset client
        GetAssetsOptions IAssetsRestClient.GetAssetsOptions { get; } = new GetAssetsOptions(_exchangeName, true);

        async Task<HttpResult<SharedAsset[]>> IAssetsRestClient.GetAssetsAsync(GetAssetsRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetAssetsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedAsset[]>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedAsset[]>(Exchange, symbolInfoResult.Error!);

            var assets = await ExchangeData.GetAssetsAsync(ct: ct).ConfigureAwait(false);
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

        GetAssetOptions IAssetsRestClient.GetAssetOptions { get; } = new GetAssetOptions(_exchangeName, false);
        async Task<HttpResult<SharedAsset>> IAssetsRestClient.GetAssetAsync(GetAssetRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetAssetOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedAsset>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedAsset>(Exchange, symbolInfoResult.Error!);

            var assets = await ExchangeData.GetAssetsAsync(ct: ct).ConfigureAwait(false);
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

        #region Balance Client
        GetBalancesOptions IBalanceRestClient.GetBalancesOptions { get; } = new GetBalancesOptions(_exchangeName, AccountTypeFilter.Spot, AccountTypeFilter.Funding, AccountTypeFilter.Futures);

        async Task<HttpResult<SharedBalance[]>> IBalanceRestClient.GetBalancesAsync(GetBalancesRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetBalancesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedBalance[]>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedBalance[]>(Exchange, symbolInfoResult.Error!);

            var result = await Account.GetBalancesAsync(ct: ct).ConfigureAwait(false);
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

        #region Deposit client

        GetDepositAddressesOptions IDepositRestClient.GetDepositAddressesOptions { get; } = new GetDepositAddressesOptions(_exchangeName, true)
        {
            RequiredOptionalParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(GetDepositAddressesRequest.Network), typeof(string), "Network to use", "btc")
            }
        };
        async Task<HttpResult<SharedDepositAddress[]>> IDepositRestClient.GetDepositAddressesAsync(GetDepositAddressesRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetDepositAddressesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedDepositAddress[]>(Exchange, validationError);

            var depositAddresses = await Account.GetDepositAddressAsync(BitMEXExchange.AssetAliases.CommonToExchangeName(request.Asset), request.Network!, ct: ct).ConfigureAwait(false);
            if (!depositAddresses.Success)
                return HttpResult.Fail<SharedDepositAddress[]>(depositAddresses);

            return HttpResult.Ok(depositAddresses, new[] { new SharedDepositAddress(request.Asset, depositAddresses.Data) });
        }

        GetDepositsOptions IDepositRestClient.GetDepositsOptions { get; } = new GetDepositsOptions(_exchangeName, true, true, false, 10000)
        {
            RequestNotes = "Due to the API not offering a filter on deposit type less results may be returned per page"
        };
        async Task<HttpResult<SharedDeposit[]>> IDepositRestClient.GetDepositsAsync(GetDepositsRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = SharedClient.GetDepositsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedDeposit[]>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedDeposit[]>(Exchange, symbolInfoResult.Error!);

            var direction = request.Direction ?? DataDirection.Descending;
            var limit = request.Limit ?? 100;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest, true);

            // Get data
            var result = await Account.GetBalanceHistoryAsync(
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

        private SharedTransferStatus ParseTransferStatus(TransactionStatus status)
        {
            if (status == TransactionStatus.Completed)
                return SharedTransferStatus.Completed;

            if (status == TransactionStatus.Canceled)
                return SharedTransferStatus.Failed;

            return SharedTransferStatus.Unknown;
        }

        #endregion

        #region Fee Client
        GetFeeOptions IFeeRestClient.GetFeeOptions { get; } = new GetFeeOptions(_exchangeName, true);

        async Task<HttpResult<SharedFee>> IFeeRestClient.GetFeesAsync(GetFeeRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetFeeOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFee>(Exchange, validationError);

            // Get data
            var result = await Account.GetFeesAsync(ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedFee>(result);

            if(!result.Data.TryGetValue(request.Symbol!.GetSymbol(FormatSymbol), out var fees))
                return HttpResult.Fail<SharedFee>(result, new ServerError(new ErrorInfo(ErrorType.Unknown, "Not found")));

            // Return
            return HttpResult.Ok(result, new SharedFee(fees.MakerFee * 100, fees.TakerFee * 100));
        }
        #endregion

        #region Klines Client

        GetKlinesOptions IKlineRestClient.GetKlinesOptions { get; } = new GetKlinesOptions(_exchangeName, false, true, true, 1000, false,
            SharedKlineInterval.OneMinute,
            SharedKlineInterval.FiveMinutes,
            SharedKlineInterval.OneHour,
            SharedKlineInterval.OneDay);

        async Task<HttpResult<SharedKline[]>> IKlineRestClient.GetKlinesAsync(GetKlinesRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var interval = (Enums.BinPeriod)request.Interval;
            if (!Enum.IsDefined(typeof(Enums.BinPeriod), interval))
                return HttpResult.Fail<SharedKline[]>(Exchange, ArgumentError.Invalid(nameof(GetKlinesRequest.Interval), "Interval not supported"));

            var validationError = SharedClient.GetKlinesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedKline[]>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedKline[]>(Exchange, symbolInfoResult.Error!);

            var direction = DataDirection.Descending;
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var limit = request.Limit ?? 100;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest, true);
            bool includeNow = (pageParams.EndTime == null || (DateTime.UtcNow - pageParams.EndTime < TimeSpan.FromSeconds(5))) && (pageParams.Offset == null || pageParams.Offset == 0);

            // Get data
            var result = await ExchangeData.GetKlinesAsync(
                symbol,
                period: interval,
                partial: includeNow == true ? true : null,
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                offset: pageParams.Offset,
                limit: limit,
                reverse: direction == DataDirection.Descending,
                ct: ct
                ).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedKline[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                    () => Pagination.NextPageFromOffset(pageParams, result.Data.Length),
                    result.Data.Length,
                    result.Data.Select(x => x.Timestamp),
                    request.StartTime,
                    request.EndTime ?? DateTime.UtcNow,
                    pageParams);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data, x => x.Timestamp, request.StartTime, request.EndTime, direction)
                   .Select(x =>
                        new SharedKline(
                            request.Symbol, 
                            symbol, 
                            x.Timestamp.AddSeconds(-(int)interval),
                            x.ClosePrice,
                            x.HighPrice,
                            x.LowPrice,
                            x.OpenPrice,
                            x.Volume.ToSharedSymbolQuantity(symbol) ?? 0))
                   .ToArray(), nextPageRequest);
        }

        #endregion

        #region Order Book client
        GetOrderBookOptions IOrderBookRestClient.GetOrderBookOptions { get; } = new GetOrderBookOptions(_exchangeName, 1, 5000, false);
        async Task<HttpResult<SharedOrderBook>> IOrderBookRestClient.GetOrderBookAsync(GetOrderBookRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetOrderBookOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedOrderBook>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedOrderBook>(Exchange, symbolInfoResult.Error!);

            var result = await ExchangeData.GetOrderBookAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                limit: request.Limit ?? 20,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedOrderBook>(result);

            var book = new SharedOrderBook(result.Data.Asks, result.Data.Bids);
            if (request.Symbol!.TradingMode == TradingMode.Spot)
            {
                foreach (var item in book.Asks)
                    item.Quantity = ((long)item.Quantity).ToSharedAssetQuantity(request.Symbol!.BaseAsset) ?? 0;
                foreach (var item in book.Bids)
                    item.Quantity = ((long)item.Quantity).ToSharedAssetQuantity(request.Symbol!.BaseAsset) ?? 0;
            }

            return HttpResult.Ok(result, book);
        }

        #endregion

        #region Recent Trades client
        GetRecentTradesOptions IRecentTradeRestClient.GetRecentTradesOptions { get; } = new GetRecentTradesOptions(_exchangeName, 1000, false);

        async Task<HttpResult<SharedTrade[]>> IRecentTradeRestClient.GetRecentTradesAsync(GetRecentTradesRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetRecentTradesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedTrade[]>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedTrade[]>(Exchange, symbolInfoResult.Error!);

            // Get data
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await ExchangeData.GetTradesAsync(
                symbol,
                limit: request.Limit,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedTrade[]>(result);

            // Return
            return HttpResult.Ok(result, result.Data.Select(x => 
            new SharedTrade(request.Symbol, symbol, x.Quantity.ToSharedSymbolQuantity(symbol) ?? 0, x.Price, x.Timestamp)
            {
                Side = x.Side == OrderSide.Sell ? SharedOrderSide.Sell : SharedOrderSide.Buy,
            }).ToArray());
        }
        #endregion

        #region Trade History client
        GetTradeHistoryOptions ITradeHistoryRestClient.GetTradeHistoryOptions { get; } = new GetTradeHistoryOptions(_exchangeName, true, true, true, 1000, false);

        async Task<HttpResult<SharedTrade[]>> ITradeHistoryRestClient.GetTradeHistoryAsync(GetTradeHistoryRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = SharedClient.GetTradeHistoryOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedTrade[]>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedTrade[]>(Exchange, symbolInfoResult.Error!);

            var direction = request.Direction ?? DataDirection.Descending;
            var limit = request.Limit ?? 1000;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest, false);

            // Get data
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await ExchangeData.GetTradesAsync(
                symbol,
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                limit: pageParams.Limit,
                offset: pageParams.Offset,
                reverse: direction == DataDirection.Descending,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedTrade[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                () => Pagination.NextPageFromOffset(pageParams, result.Data.Length),
                result.Data.Length,
                result.Data.Select(x => x.Timestamp),
                request.StartTime,
                request.EndTime ?? DateTime.UtcNow,
                pageParams);

            // Return
            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data, x => x.Timestamp, request.StartTime, request.EndTime, direction)
                       .Select(x => 
                            new SharedTrade(request.Symbol, symbol, x.Quantity.ToSharedSymbolQuantity(symbol) ?? 0, x.Price, x.Timestamp)
                            {
                                Side = x.Side == OrderSide.Sell ? SharedOrderSide.Sell : SharedOrderSide.Buy,
                            })
                       .ToArray(), nextPageRequest);
        }
        #endregion

        #region Book Ticker client

        GetBookTickerOptions IBookTickerRestClient.GetBookTickerOptions { get; } = new GetBookTickerOptions(_exchangeName, false);
        async Task<HttpResult<SharedBookTicker>> IBookTickerRestClient.GetBookTickerAsync(GetBookTickerRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetBookTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedBookTicker>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedBookTicker>(Exchange, symbolInfoResult.Error!);

            var resultTicker = await ExchangeData.GetBookTickerHistoryAsync(request.Symbol!.GetSymbol(FormatSymbol), reverse: true, limit: 1, ct: ct).ConfigureAwait(false);
            if (!resultTicker.Success)
                return HttpResult.Fail<SharedBookTicker>(resultTicker);

            if (!resultTicker.Data.Any())
                return HttpResult.Fail<SharedBookTicker>(resultTicker, new ServerError(new ErrorInfo(ErrorType.UnknownSymbol, "Symbol not found")));

            return HttpResult.Ok(resultTicker, new SharedBookTicker(
                ExchangeSymbolCache.ParseSymbol(request.Symbol!.TradingMode == TradingMode.Spot ? _topicSpotId : _topicFuturesId, EnvironmentName, null, resultTicker.Data[0].Symbol),
                resultTicker.Data[0].Symbol,
                resultTicker.Data[0].BestAskPrice,
                resultTicker.Data[0].BestAskQuantity.ToSharedSymbolQuantity(resultTicker.Data[0].Symbol) ?? 0,
                resultTicker.Data[0].BestBidPrice,
                resultTicker.Data[0].BestBidQuantity.ToSharedSymbolQuantity(resultTicker.Data[0].Symbol) ?? 0));
        }

        #endregion

        #region Withdrawal client

        GetWithdrawalsOptions IWithdrawalRestClient.GetWithdrawalsOptions { get; } = new GetWithdrawalsOptions(_exchangeName, true, true, true, 1000)
        {
            RequestNotes = "Due to the API not offering a filter on withdrawal type less results may be returned per page"
        };
        async Task<HttpResult<SharedWithdrawal[]>> IWithdrawalRestClient.GetWithdrawalsAsync(GetWithdrawalsRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = SharedClient.GetWithdrawalsOptions.ValidateRequest(request, this);
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
            var result = await Account.GetBalanceHistoryAsync(
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

        WithdrawOptions IWithdrawRestClient.WithdrawOptions { get; } = new WithdrawOptions(_exchangeName)
        {
            RequiredOptionalParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(WithdrawRequest.Network), typeof(string), "Network to use", "btc")
            }
        };
        async Task<HttpResult<SharedId>> IWithdrawRestClient.WithdrawAsync(WithdrawRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.WithdrawOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedId>(Exchange, symbolInfoResult.Error!);

            // Get data
            var withdrawal = await Account.WithdrawAsync(
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

        #region Spot Symbol client
        SharedSymbolCatalog? ISpotSymbolRestClient.SpotSymbolCatalog => ExchangeSymbolCache.GetSymbolCatalog(_topicSpotId, EnvironmentName, null);

        GetSpotSymbolsOptions ISpotSymbolRestClient.GetSpotSymbolsOptions { get; } = new GetSpotSymbolsOptions(_exchangeName, false);

        async Task<HttpResult<SharedSpotSymbol[]>> ISpotSymbolRestClient.GetSpotSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetSpotSymbolsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotSymbol[]>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedSpotSymbol[]>(Exchange, symbolInfoResult.Error!);

            var result = await ExchangeData.GetActiveSymbolsAsync(ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedSpotSymbol[]>(result);

            var data = result.Data
                .Where(x => x.SymbolType == SymbolType.Spot)
                .Select(x => ParseSpotSymbol(x)!)
                .Where(x => x != null)
                .ToArray();

            var response = HttpResult.Ok(result, result.Data.Where(x => x.SymbolType == SymbolType.Spot).Select(s => 
                ParseSpotSymbol(s)).ToArray());

            ExchangeSymbolCache.UpdateSymbolInfo(_topicSpotId, EnvironmentName, null, data);
            return HttpResult.Ok(result, SharedUtils.ApplySymbolFilter(data, request));
        }

        private SharedSpotSymbol ParseSpotSymbol(BitMEXSymbol s)
        {
            var result = new SharedSpotSymbol(BitMEXExchange.AssetAliases.ExchangeToCommonName(s.BaseAsset), BitMEXExchange.AssetAliases.ExchangeToCommonName(s.QuoteAsset), s.Symbol, s.Status == SymbolStatus.Open)
            {
                MinTradeQuantity = s.LotSize.ToSharedSymbolQuantity(s.Symbol),
                MaxTradeQuantity = s.MaxOrderQuantity.ToSharedSymbolQuantity(s.Symbol),
                QuantityStep = s.LotSize.ToSharedSymbolQuantity(s.Symbol),
                PriceStep = s.PriceStep,
                QuoteAssetType = SharedAssetType.Crypto,
                QuoteAssetSubType = SharedAssetSubType.StableCoin
            };

            if (LibraryHelpers.IsCommodity(s.BaseAsset))
            {
                result.BaseAssetType = SharedAssetType.TradFi;
                result.BaseAssetSubType = SharedAssetSubType.Commodity;
            }
            else
            {
                result.BaseAssetType = SharedAssetType.Crypto;
            }

            return result;
        }

        async Task<ExchangeCallResult<SharedSymbol[]>> ISpotSymbolRestClient.GetSpotSymbolsForBaseAssetAsync(string baseAsset)
        {
            if (!ExchangeSymbolCache.HasCached(_topicSpotId, EnvironmentName, null))
            {
                var symbols = await ((ISpotSymbolRestClient)this).GetSpotSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<SharedSymbol[]>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<SharedSymbol[]>.Ok(Exchange, ExchangeSymbolCache.GetSymbolsForBaseAsset(_topicSpotId, EnvironmentName, null, baseAsset));
        }

        async Task<ExchangeCallResult<bool>> ISpotSymbolRestClient.SupportsSpotSymbolAsync(SharedSymbol symbol)
        {
            if (symbol.TradingMode != TradingMode.Spot)
                throw new ArgumentException(nameof(symbol), "Only Spot symbols allowed");

            if (!ExchangeSymbolCache.HasCached(_topicSpotId, EnvironmentName, null))
            {
                var symbols = await ((ISpotSymbolRestClient)this).GetSpotSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicSpotId, EnvironmentName, null, symbol));
        }

        async Task<ExchangeCallResult<bool>> ISpotSymbolRestClient.SupportsSpotSymbolAsync(string symbolName)
        {
            if (!ExchangeSymbolCache.HasCached(_topicSpotId, EnvironmentName, null))
            {
                var symbols = await ((ISpotSymbolRestClient)this).GetSpotSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicSpotId, EnvironmentName, null, symbolName));
        }
        #endregion

        #region Spot Ticker client

        GetSpotTickerOptions ISpotTickerRestClient.GetSpotTickerOptions { get; } = new GetSpotTickerOptions(_exchangeName);
        async Task<HttpResult<SharedSpotTicker>> ISpotTickerRestClient.GetSpotTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetSpotTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotTicker>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedSpotTicker>(Exchange, symbolInfoResult.Error!);

            var result = await ExchangeData.GetActiveSymbolsAsync(ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedSpotTicker>(result);

            var symbol = result.Data.SingleOrDefault(x => x.Symbol == request.Symbol!.GetSymbol(FormatSymbol));
            if (symbol == null)
                return HttpResult.Fail<SharedSpotTicker>(result, new ServerError(new ErrorInfo(ErrorType.UnknownSymbol, "Symbol not found")));

            return HttpResult.Ok(result, new SharedSpotTicker(
                ExchangeSymbolCache.ParseSymbol(_topicSpotId, EnvironmentName, null, symbol.Symbol),
                symbol.Symbol,
                symbol.LastPrice,
                symbol.HighPrice,
                symbol.LowPrice,
                symbol.Volume24h.ToSharedSymbolQuantity(symbol.Symbol) ?? 0,
                Math.Round(symbol.PrevPrice24h == 0 ? 0 : symbol.LastPrice / symbol.PrevPrice24h * 100 - 100, 3)
                )
            {
                QuoteVolume = symbol.Turnover24h.ToSharedAssetQuantity(symbol.Symbol!.Split('_').Last())
            });
        }

        GetSpotTickersOptions ISpotTickerRestClient.GetSpotTickersOptions { get; } = new GetSpotTickersOptions(_exchangeName);
        async Task<HttpResult<SharedSpotTicker[]>> ISpotTickerRestClient.GetSpotTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetSpotTickersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotTicker[]>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedSpotTicker[]>(Exchange, symbolInfoResult.Error!);

            var result = await ExchangeData.GetActiveSymbolsAsync(ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedSpotTicker[]>(result);

            return HttpResult.Ok(result, result.Data.Where(x => x.SymbolType == SymbolType.Spot).Select(x =>
            new SharedSpotTicker(
                ExchangeSymbolCache.ParseSymbol(_topicSpotId, EnvironmentName, null, x.Symbol),
                x.Symbol,
                x.LastPrice,
                x.HighPrice,
                x.LowPrice,
                x.Volume24h.ToSharedSymbolQuantity(x.Symbol) ?? 0,
                Math.Round(x.PrevPrice24h == 0 ? 0 : x.LastPrice / x.PrevPrice24h * 100 - 100, 3))
            {
                QuoteVolume = x.Turnover24h.ToSharedAssetQuantity(x.Symbol!.Split('_').Last())
            }).ToArray());
        }

        #endregion

        #region Spot Order Client
        SharedFeeDeductionType ISpotOrderRestClient.SpotFeeDeductionType => SharedFeeDeductionType.AddToCost;
        SharedFeeAssetType ISpotOrderRestClient.SpotFeeAssetType => SharedFeeAssetType.QuoteAsset;
        SharedOrderType[] ISpotOrderRestClient.SpotSupportedOrderTypes { get; } = new[] { SharedOrderType.Limit, SharedOrderType.Market, SharedOrderType.LimitMaker };
        SharedTimeInForce[] ISpotOrderRestClient.SpotSupportedTimeInForce { get; } = new[] { SharedTimeInForce.GoodTillCanceled, SharedTimeInForce.ImmediateOrCancel, SharedTimeInForce.FillOrKill };
        SharedQuantitySupport ISpotOrderRestClient.SpotSupportedOrderQuantity { get; } = new SharedQuantitySupport(
                SharedQuantityType.BaseAsset,
                SharedQuantityType.BaseAsset,
                SharedQuantityType.BaseAsset,
                SharedQuantityType.BaseAsset);

        string ISpotOrderRestClient.GenerateClientOrderId() => ExchangeHelpers.RandomString(32);

        PlaceSpotOrderOptions ISpotOrderRestClient.PlaceSpotOrderOptions { get; } = new PlaceSpotOrderOptions(_exchangeName);
        async Task<HttpResult<SharedId>> ISpotOrderRestClient.PlaceSpotOrderAsync(PlaceSpotOrderRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.PlaceSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedId>(Exchange, symbolInfoResult.Error!);

            var result = await Trading.PlaceOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                request.Side == SharedOrderSide.Buy ? Enums.OrderSide.Buy : Enums.OrderSide.Sell,
                request.OrderType == SharedOrderType.Limit ? Enums.OrderType.Limit : Enums.OrderType.Market,
                quantity: request.Quantity?.QuantityInBaseAsset.ToBitMEXAssetQuantity(request.Symbol!.BaseAsset),
                price: request.Price,
                timeInForce: GetTimeInForce(request.TimeInForce),
                executionInstruction: request.OrderType == SharedOrderType.LimitMaker ? ExecutionInstruction.PostOnly : null,
                clientOrderId: request.ClientOrderId,
                ct: ct).ConfigureAwait(false);

            if (!result.Success)
                return HttpResult.Fail<SharedId>(result);

            return HttpResult.Ok(result, new SharedId(result.Data.OrderId));
        }

        GetSpotOrderOptions ISpotOrderRestClient.GetSpotOrderOptions { get; } = new GetSpotOrderOptions(_exchangeName, true);
        async Task<HttpResult<SharedSpotOrder>> ISpotOrderRestClient.GetSpotOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotOrder>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedSpotOrder>(Exchange, symbolInfoResult.Error!);

            var orders = await Trading.GetOrdersAsync(request.Symbol!.GetSymbol(FormatSymbol), 
                filter: new Dictionary<string, object>
                {
                    { "orderID", request.OrderId }
                },
                ct: ct).ConfigureAwait(false);
            if (!orders.Success)
                return HttpResult.Fail<SharedSpotOrder>(orders);

            var order = orders.Data.SingleOrDefault();
            if (order == null)
                return HttpResult.Fail<SharedSpotOrder>(orders, new ServerError(new ErrorInfo(ErrorType.UnknownOrder, "Order not found")));

            return HttpResult.Ok(orders, new SharedSpotOrder(
                ExchangeSymbolCache.ParseSymbol(_topicSpotId, EnvironmentName, null, order.Symbol),
                order.Symbol,
                order.OrderId,
                ParseOrderType(order.OrderType, order.ExecutionInstruction),
                order.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(order.Status),
                order.Timestamp)
            {
                ClientOrderId = order.ClientOrderId,
                AveragePrice = order.AveragePrice,
                OrderPrice = order.Price,
                OrderQuantity = new SharedOrderQuantity(order.Quantity.ToSharedSymbolQuantity(order.Symbol), null, null),
                QuantityFilled = new SharedOrderQuantity(order.QuantityFilled.ToSharedSymbolQuantity(order.Symbol), null, null),
                TimeInForce = ParseTimeInForce(order.TimeInForce),
                UpdateTime = order.TransactTime,
                TriggerPrice = order.StopPrice,
                IsTriggerOrder = order.StopPrice != null
            });
        }

        GetOpenSpotOrdersOptions ISpotOrderRestClient.GetOpenSpotOrdersOptions { get; } = new GetOpenSpotOrdersOptions(_exchangeName, true);
        async Task<HttpResult<SharedSpotOrder[]>> ISpotOrderRestClient.GetOpenSpotOrdersAsync(GetOpenOrdersRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetOpenSpotOrdersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotOrder[]>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedSpotOrder[]>(Exchange, symbolInfoResult.Error!);

            var symbol = request.Symbol?.GetSymbol(FormatSymbol);
            var orders = await Trading.GetOrdersAsync(symbol,
                filter: new Dictionary<string, object>
                {
                    { "open", true }
                },
                ct: ct).ConfigureAwait(false);
            if (!orders.Success)
                return HttpResult.Fail<SharedSpotOrder[]>(orders);

            var data = orders.Data.Where(x => BitMEXUtils.GetSymbolType(x.Symbol) == SymbolType.Spot);
            return HttpResult.Ok(orders, data.Select(x => new SharedSpotOrder(
                ExchangeSymbolCache.ParseSymbol(_topicSpotId, EnvironmentName, null, x.Symbol),
                x.Symbol,
                x.OrderId,
                ParseOrderType(x.OrderType, x.ExecutionInstruction),
                x.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(x.Status),
                x.Timestamp)
            {
                ClientOrderId = x.ClientOrderId,
                AveragePrice = x.AveragePrice,
                OrderPrice = x.Price,
                OrderQuantity = new SharedOrderQuantity(x.Quantity.ToSharedSymbolQuantity(x.Symbol)),
                QuantityFilled = new SharedOrderQuantity(x.QuantityFilled.ToSharedSymbolQuantity(x.Symbol)),
                TimeInForce = ParseTimeInForce(x.TimeInForce),
                UpdateTime = x.TransactTime,
                TriggerPrice = x.StopPrice,
                IsTriggerOrder = x.StopPrice != null
            }).ToArray());
        }

        GetSpotClosedOrdersOptions ISpotOrderRestClient.GetClosedSpotOrdersOptions { get; } = new GetSpotClosedOrdersOptions(_exchangeName, true, true, true, 500);
        async Task<HttpResult<SharedSpotOrder[]>> ISpotOrderRestClient.GetClosedSpotOrdersAsync(GetClosedOrdersRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = SharedClient.GetClosedSpotOrdersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotOrder[]>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedSpotOrder[]>(Exchange, symbolInfoResult.Error!);

            var direction = request.Direction ?? DataDirection.Descending;
            var limit = request.Limit ?? 500;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            // Get data
            var result = await Trading.GetOrdersAsync(request.Symbol!.GetSymbol(FormatSymbol),
                filter: new Dictionary<string, object>
                {
                    { "ordStatus", new string[] {"Canceled", "Filled" } }
                },
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                limit: pageParams.Limit,
                offset: pageParams.Offset,
                reverse: direction == DataDirection.Descending,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedSpotOrder[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                () => Pagination.NextPageFromOffset(pageParams, result.Data.Length),
                result.Data.Length,
                result.Data.Select(x => x.Timestamp),
                request.StartTime,
                request.EndTime ?? DateTime.UtcNow,
                pageParams);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data, x => x.Timestamp, request.StartTime, request.EndTime, direction)
                       .Select(x => 
                           new SharedSpotOrder(
                                ExchangeSymbolCache.ParseSymbol(_topicSpotId, EnvironmentName, null, x.Symbol), 
                                x.Symbol,
                                x.OrderId,
                                ParseOrderType(x.OrderType, x.ExecutionInstruction),
                                x.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                                ParseOrderStatus(x.Status),
                                x.Timestamp)
                            {
                                ClientOrderId = x.ClientOrderId,
                                AveragePrice = x.AveragePrice,
                                OrderPrice = x.Price,
                                OrderQuantity = new SharedOrderQuantity(x.Quantity.ToSharedSymbolQuantity(x.Symbol)),
                                QuantityFilled = new SharedOrderQuantity(x.QuantityFilled.ToSharedSymbolQuantity(x.Symbol)),
                                TimeInForce = ParseTimeInForce(x.TimeInForce),
                                UpdateTime = x.TransactTime,
                                TriggerPrice = x.StopPrice,
                                IsTriggerOrder = x.StopPrice != null
                            })
                       .ToArray(), nextPageRequest);
        }

        GetSpotOrderTradesOptions ISpotOrderRestClient.GetSpotOrderTradesOptions { get; } = new GetSpotOrderTradesOptions(_exchangeName, true);
        async Task<HttpResult<SharedUserTrade[]>> ISpotOrderRestClient.GetSpotOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetSpotOrderTradesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, symbolInfoResult.Error!);

            var trades = await Trading.GetUserTradesAsync(symbol: request.Symbol!.GetSymbol(FormatSymbol),
                filter: new Dictionary<string, object>
                {
                    { "orderID", request.OrderId }
                },
                ct: ct).ConfigureAwait(false);
            if (!trades.Success)
                return HttpResult.Fail<SharedUserTrade[]>(trades);

            return HttpResult.Ok(trades, trades.Data.Select(x => new SharedUserTrade(
                ExchangeSymbolCache.ParseSymbol(_topicSpotId, EnvironmentName, null, x.Symbol), 
                x.Symbol,
                x.OrderId,
                x.TradeId,
                x.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                x.Quantity?.ToSharedSymbolQuantity(x.Symbol) ?? 0,
                x.LastTradePrice!.Value,
                x.Timestamp)
            {
                ClientOrderId = x.ClientOrderId,
                Fee = x.Fee.ToSharedAssetQuantity(x.Currency),
                Role = x.Role == TradeRole.Maker ? SharedRole.Maker : SharedRole.Taker,
                FeeAsset = x.FeeAsset
            }).ToArray());
        }

        GetSpotUserTradesOptions ISpotOrderRestClient.GetSpotUserTradesOptions { get; } = new GetSpotUserTradesOptions(_exchangeName, true, true, true, 1000);
        async Task<HttpResult<SharedUserTrade[]>> ISpotOrderRestClient.GetSpotUserTradesAsync(GetUserTradesRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = SharedClient.GetSpotUserTradesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, symbolInfoResult.Error!);

            var direction = request.Direction ?? DataDirection.Descending;
            var limit = request.Limit ?? 500;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            // Get data
            var result = await Trading.GetUserTradesAsync(symbol: request.Symbol!.GetSymbol(FormatSymbol),
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                limit: pageParams.Limit,
                offset: pageParams.Offset,
                reverse: direction == DataDirection.Descending,
                ct: ct
                ).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedUserTrade[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                () => Pagination.NextPageFromOffset(pageParams, result.Data.Length),
                 result.Data.Length,
                 result.Data.Select(x => x.Timestamp),
                 request.StartTime,
                 request.EndTime ?? DateTime.UtcNow,
                 pageParams);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data, x => x.Timestamp, request.StartTime, request.EndTime, direction)
                       .Select(x => 
                           new SharedUserTrade(
                                ExchangeSymbolCache.ParseSymbol(_topicSpotId, EnvironmentName, null, x.Symbol), 
                                x.Symbol,
                                x.OrderId,
                                x.TradeId,
                                x.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                                x.Quantity?.ToSharedSymbolQuantity(x.Symbol) ?? 0,
                                x.LastTradePrice!.Value,
                                x.Timestamp)
                            {
                                ClientOrderId = x.ClientOrderId,
                                Fee = x.Fee.ToSharedAssetQuantity(x.Currency),
                                Role = x.Role == TradeRole.Maker ? SharedRole.Maker : SharedRole.Taker,
                                FeeAsset = x.FeeAsset
                           })
                       .ToArray(), nextPageRequest);
        }

        CancelSpotOrderOptions ISpotOrderRestClient.CancelSpotOrderOptions { get; } = new CancelSpotOrderOptions(_exchangeName, true);
        async Task<HttpResult<SharedId>> ISpotOrderRestClient.CancelSpotOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.CancelSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var order = await Trading.CancelOrderAsync(request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedId>(order);

            return HttpResult.Ok(order, new SharedId(order.Data.OrderId));
        }

        private Enums.TimeInForce? GetTimeInForce(SharedTimeInForce? tif)
        {
            if (tif == SharedTimeInForce.FillOrKill) return TimeInForce.FillOrKill;
            if (tif == SharedTimeInForce.ImmediateOrCancel) return TimeInForce.ImmediateOrCancel;
            if (tif == SharedTimeInForce.GoodTillCanceled) return TimeInForce.GoodTillCancel;

            return null;
        }

        private SharedOrderStatus ParseOrderStatus(OrderStatus status)
        {
            if (status == OrderStatus.New || status == OrderStatus.PartiallyFilled) return SharedOrderStatus.Open;
            if (status == OrderStatus.Rejected || status == OrderStatus.Canceled) return SharedOrderStatus.Canceled;
            if (status == OrderStatus.Filled) return SharedOrderStatus.Filled;

            return SharedOrderStatus.Unknown;
        }

        private SharedOrderType ParseOrderType(OrderType type, ExecutionInstruction[]? executionInstruction)
        {
            if (type == OrderType.Market) return SharedOrderType.Market;
            if (type == OrderType.Limit && executionInstruction?.Contains(ExecutionInstruction.PostOnly) == true) return SharedOrderType.LimitMaker;
            if (type == OrderType.Limit) return SharedOrderType.Limit;

            return SharedOrderType.Other;
        }

        private SharedTimeInForce? ParseTimeInForce(TimeInForce tif)
        {
            if (tif == TimeInForce.GoodTillCancel) return SharedTimeInForce.GoodTillCanceled;
            if (tif == TimeInForce.ImmediateOrCancel) return SharedTimeInForce.ImmediateOrCancel;
            if (tif == TimeInForce.FillOrKill) return SharedTimeInForce.FillOrKill;

            return null;
        }

        #endregion

        #region Spot Client Id Order Client

        GetSpotOrderByClientOrderIdOptions ISpotOrderClientIdRestClient.GetSpotOrderByClientOrderIdOptions { get; } = new GetSpotOrderByClientOrderIdOptions(_exchangeName, true);
        async Task<HttpResult<SharedSpotOrder>> ISpotOrderClientIdRestClient.GetSpotOrderByClientOrderIdAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotOrder>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedSpotOrder>(Exchange, symbolInfoResult.Error!);

            var orders = await Trading.GetOrdersAsync(request.Symbol!.GetSymbol(FormatSymbol),
                filter: new Dictionary<string, object>
                {
                    { "clOrdID", request.OrderId }
                },
                ct: ct).ConfigureAwait(false);
            if (!orders.Success)
                return HttpResult.Fail<SharedSpotOrder>(orders);

            var order = orders.Data.SingleOrDefault();
            if (order == null)
                return HttpResult.Fail<SharedSpotOrder>(orders, new ServerError(new ErrorInfo(ErrorType.UnknownOrder, "Order not found")));

            return HttpResult.Ok(orders, new SharedSpotOrder(
                ExchangeSymbolCache.ParseSymbol(_topicSpotId, EnvironmentName, null, order.Symbol),
                order.Symbol,
                order.OrderId,
                ParseOrderType(order.OrderType, order.ExecutionInstruction),
                order.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(order.Status),
                order.Timestamp)
            {
                ClientOrderId = order.ClientOrderId,
                AveragePrice = order.AveragePrice,
                OrderPrice = order.Price,
                OrderQuantity = new SharedOrderQuantity(order.Quantity.ToSharedSymbolQuantity(order.Symbol)),
                QuantityFilled = new SharedOrderQuantity(order.QuantityFilled.ToSharedSymbolQuantity(order.Symbol)),
                TimeInForce = ParseTimeInForce(order.TimeInForce),
                UpdateTime = order.TransactTime,
                TriggerPrice = order.StopPrice,
                IsTriggerOrder = order.StopPrice != null
            });
        }

        CancelSpotOrderByClientOrderIdOptions ISpotOrderClientIdRestClient.CancelSpotOrderByClientOrderIdOptions { get; } = new CancelSpotOrderByClientOrderIdOptions(_exchangeName, true);
        async Task<HttpResult<SharedId>> ISpotOrderClientIdRestClient.CancelSpotOrderByClientOrderIdAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.CancelSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var order = await Trading.CancelOrderAsync(clientOrderId: request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedId>(order);

            return HttpResult.Ok(order, new SharedId(order.Data.OrderId));
        }
        #endregion

        #region Funding Rate client
        GetFundingRateHistoryOptions IFundingRateRestClient.GetFundingRateHistoryOptions { get; } = new GetFundingRateHistoryOptions(_exchangeName, true, true, true, 500, false);

        async Task<HttpResult<SharedFundingRate[]>> IFundingRateRestClient.GetFundingRateHistoryAsync(GetFundingRateHistoryRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = SharedClient.GetFundingRateHistoryOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFundingRate[]>(Exchange, validationError);

            var direction = request.Direction ?? DataDirection.Descending;
            var limit = request.Limit ?? 500;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            // Get data
            var result = await ExchangeData.GetFundingHistoryAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                limit: pageParams.Limit,
                offset: pageParams.Offset,
                reverse: direction == DataDirection.Descending,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedFundingRate[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                () => Pagination.NextPageFromOffset(pageParams, result.Data.Length),
                 result.Data.Length,
                 result.Data.Select(x => x.Timestamp),
                 request.StartTime,
                 request.EndTime ?? DateTime.UtcNow,
                 pageParams);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data, x => x.Timestamp, request.StartTime, request.EndTime, direction)
                       .Select(x => 
                            new SharedFundingRate(x.FundingRate, x.Timestamp))
                       .ToArray(), nextPageRequest);
        }
        #endregion

        #region Futures Symbol client
        SharedSymbolCatalog? IFuturesSymbolRestClient.FuturesSymbolCatalog => ExchangeSymbolCache.GetSymbolCatalog(_topicFuturesId, EnvironmentName, null);

        GetFuturesSymbolsOptions IFuturesSymbolRestClient.GetFuturesSymbolsOptions { get; } = new GetFuturesSymbolsOptions(_exchangeName, false);
        async Task<HttpResult<SharedFuturesSymbol[]>> IFuturesSymbolRestClient.GetFuturesSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetFuturesSymbolsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesSymbol[]>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedFuturesSymbol[]>(Exchange, symbolInfoResult.Error!);

            var result = await ExchangeData.GetActiveSymbolsAsync(ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedFuturesSymbol[]>(result);

            // Quanto not supported as it doesn't currently fit in the response models
            var data = result.Data.Where(x => x.SymbolType != SymbolType.Spot && !x.IsQuanto);
            var symbols = data
                .Select(x => ParseFuturesSymbol(x))
                .ToArray();

            ExchangeSymbolCache.UpdateSymbolInfo(_topicFuturesId, EnvironmentName, null, symbols);
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

        async Task<ExchangeCallResult<SharedSymbol[]>> IFuturesSymbolRestClient.GetFuturesSymbolsForBaseAssetAsync(string baseAsset)
        {
            if (!ExchangeSymbolCache.HasCached(_topicFuturesId, EnvironmentName, null))
            {
                var symbols = await ((IFuturesSymbolRestClient)this).GetFuturesSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<SharedSymbol[]>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<SharedSymbol[]>.Ok(Exchange, ExchangeSymbolCache.GetSymbolsForBaseAsset(_topicFuturesId, EnvironmentName, null, baseAsset));
        }

        async Task<ExchangeCallResult<bool>> IFuturesSymbolRestClient.SupportsFuturesSymbolAsync(SharedSymbol symbol)
        {
            if (symbol.TradingMode == TradingMode.Spot)
                throw new ArgumentException(nameof(symbol), "Spot symbols not allowed");

            if (!ExchangeSymbolCache.HasCached(_topicFuturesId, EnvironmentName, null))
            {
                var symbols = await ((IFuturesSymbolRestClient)this).GetFuturesSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicFuturesId, EnvironmentName, null, symbol));
        }

        async Task<ExchangeCallResult<bool>> IFuturesSymbolRestClient.SupportsFuturesSymbolAsync(string symbolName)
        {
            if (!ExchangeSymbolCache.HasCached(_topicFuturesId, EnvironmentName, null))
            {
                var symbols = await ((IFuturesSymbolRestClient)this).GetFuturesSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicFuturesId, EnvironmentName, null, symbolName));
        }
        #endregion

        #region Futures Ticker client

        GetFuturesTickerOptions IFuturesTickerRestClient.GetFuturesTickerOptions { get; } = new GetFuturesTickerOptions(_exchangeName);
        async Task<HttpResult<SharedFuturesTicker>> IFuturesTickerRestClient.GetFuturesTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetFuturesTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesTicker>(Exchange, validationError);

            var resultTicker = await ExchangeData.GetSymbolsAsync(request.Symbol!.GetSymbol(FormatSymbol), ct: ct).ConfigureAwait(false);
            if (!resultTicker.Success)
                return HttpResult.Fail<SharedFuturesTicker>(resultTicker);

            var symbol = resultTicker.Data.SingleOrDefault();
            if (symbol == null)
                return HttpResult.Fail<SharedFuturesTicker>(resultTicker, new ServerError(new ErrorInfo(ErrorType.UnknownSymbol, "Symbol not found")));

            return HttpResult.Ok(resultTicker, 
                new SharedFuturesTicker(ExchangeSymbolCache.ParseSymbol(_topicFuturesId, EnvironmentName, null, symbol.Symbol), symbol.Symbol, symbol.LastPrice, symbol.HighPrice, symbol.LowPrice, symbol.Volume, symbol.LastChangePercentage)
            {
                MarkPrice = symbol.MarkPrice,
                FundingRate = symbol.FundingRate,
                NextFundingTime = symbol.FundingTimestamp
            });
        }

        GetFuturesTickersOptions IFuturesTickerRestClient.GetFuturesTickersOptions { get; } = new GetFuturesTickersOptions(_exchangeName);
        async Task<HttpResult<SharedFuturesTicker[]>> IFuturesTickerRestClient.GetFuturesTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetFuturesTickersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesTicker[]>(Exchange, validationError);

            var resultTickers = await ExchangeData.GetActiveSymbolsAsync(ct: ct).ConfigureAwait(false);
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
                return new SharedFuturesTicker(ExchangeSymbolCache.ParseSymbol(_topicFuturesId, EnvironmentName, null, x.Symbol), x.Symbol, x.LastPrice, x.HighPrice, x.LowPrice, x.Volume, x.LastChangePercentage)
                {
                    MarkPrice = markPrice.MarkPrice,
                    FundingRate = markPrice.FundingRate,
                    NextFundingTime = markPrice.FundingTimestamp
                };
            }).ToArray());
        }

        #endregion

        #region Leverage client
        SharedLeverageSettingMode ILeverageRestClient.LeverageSettingType => SharedLeverageSettingMode.PerSymbol;

        GetLeverageOptions ILeverageRestClient.GetLeverageOptions { get; } = new GetLeverageOptions(_exchangeName, true);
        async Task<HttpResult<SharedLeverage>> ILeverageRestClient.GetLeverageAsync(GetLeverageRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetLeverageOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedLeverage>(Exchange, validationError);

            var result = await Trading.GetPositionsAsync(filter: new Dictionary<string, object>{
                { "symbol", request.Symbol!.GetSymbol(FormatSymbol) }
            },
            columns: new[] { "leverage" },
            ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedLeverage>(result);

            if (!result.Data.Any())
                return HttpResult.Fail<SharedLeverage>(result, new ServerError(new ErrorInfo(ErrorType.NoPosition, "Position not found")));

            var position = result.Data.First();
            return HttpResult.Ok(result, new SharedLeverage(position.Leverage ?? 0)
            {
                MarginMode = position.CrossMargin ? SharedMarginMode.Cross : SharedMarginMode.Isolated
            });
        }

        SetLeverageOptions ILeverageRestClient.SetLeverageOptions { get; } = new SetLeverageOptions(_exchangeName)
        {
            RequiredOptionalParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(SetLeverageRequest.MarginMode), typeof(SharedMarginMode), "Margin mode to change leverage for", SharedMarginMode.Isolated)
            }
        };
        async Task<HttpResult<SharedLeverage>> ILeverageRestClient.SetLeverageAsync(SetLeverageRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.SetLeverageOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedLeverage>(Exchange, validationError);

            if (request.MarginMode == SharedMarginMode.Isolated)
            {
                var result = await Trading.SetIsolatedMarginLeverageAsync(symbol: request.Symbol!.GetSymbol(FormatSymbol), request.Leverage, ct: ct).ConfigureAwait(false);
                if (!result.Success)
                    return HttpResult.Fail<SharedLeverage>(result);

                return HttpResult.Ok(result, new SharedLeverage(result.Data.Leverage ?? 0) { MarginMode = request.MarginMode });
            }
            else
            {
                var result = await Trading.SetCrossMarginLeverageAsync(symbol: request.Symbol!.GetSymbol(FormatSymbol), request.Leverage, ct: ct).ConfigureAwait(false);
                if (!result.Success)
                    return HttpResult.Fail<SharedLeverage>(result);

                return HttpResult.Ok(result, new SharedLeverage(result.Data.Leverage ?? 0) { MarginMode = request.MarginMode });
            }

        }
        #endregion

        #region Open Interest client

        GetOpenInterestOptions IOpenInterestRestClient.GetOpenInterestOptions { get; } = new GetOpenInterestOptions(_exchangeName, true);
        async Task<HttpResult<SharedOpenInterest>> IOpenInterestRestClient.GetOpenInterestAsync(GetOpenInterestRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetOpenInterestOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedOpenInterest>(Exchange, validationError);

            var result = await ExchangeData.GetSymbolsAsync(filter: new Dictionary<string, object>{
                { "symbol", request.Symbol!.GetSymbol(FormatSymbol) }
            }, columns: ["openInterest"], ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedOpenInterest>(result);

            var symbol = result.Data.SingleOrDefault();
            if (symbol == null)
                return HttpResult.Fail<SharedOpenInterest>(result, new ServerError(new ErrorInfo(ErrorType.UnknownSymbol, "Symbol not found")));

            return HttpResult.Ok(result, new SharedOpenInterest(symbol.OpenInterest ?? 0));
        }

        #endregion

        #region Futures Order Client

        SharedFeeDeductionType IFuturesOrderRestClient.FuturesFeeDeductionType => SharedFeeDeductionType.AddToCost;
        SharedFeeAssetType IFuturesOrderRestClient.FuturesFeeAssetType => SharedFeeAssetType.QuoteAsset;

        SharedOrderType[] IFuturesOrderRestClient.FuturesSupportedOrderTypes { get; } = new[] { SharedOrderType.Limit, SharedOrderType.Market };
        SharedTimeInForce[] IFuturesOrderRestClient.FuturesSupportedTimeInForce { get; } = new[] { SharedTimeInForce.GoodTillCanceled, SharedTimeInForce.ImmediateOrCancel, SharedTimeInForce.FillOrKill };
        SharedQuantitySupport IFuturesOrderRestClient.FuturesSupportedOrderQuantity { get; } = new SharedQuantitySupport(
                SharedQuantityType.Contracts,
                SharedQuantityType.Contracts,
                SharedQuantityType.Contracts,
                SharedQuantityType.Contracts);

        string IFuturesOrderRestClient.GenerateClientOrderId() => ExchangeHelpers.RandomString(32);

        PlaceFuturesOrderOptions IFuturesOrderRestClient.PlaceFuturesOrderOptions { get; } = new PlaceFuturesOrderOptions(_exchangeName, false);
        async Task<HttpResult<SharedId>> IFuturesOrderRestClient.PlaceFuturesOrderAsync(PlaceFuturesOrderRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.PlaceFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var result = await Trading.PlaceOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                request.Side == SharedOrderSide.Buy ? Enums.OrderSide.Buy : Enums.OrderSide.Sell,
                request.OrderType == SharedOrderType.Limit ? Enums.OrderType.Limit : Enums.OrderType.Market,
                quantity: (long?)request.Quantity?.QuantityInContracts,
                price: request.Price,
                timeInForce: GetTimeInForce(request.TimeInForce),
                executionInstruction: request.OrderType == SharedOrderType.LimitMaker ? ExecutionInstruction.PostOnly : request.ReduceOnly == true ? ExecutionInstruction.ReduceOnly: null,
                clientOrderId: request.ClientOrderId,
                ct: ct).ConfigureAwait(false);

            if (!result.Success)
                return HttpResult.Fail<SharedId>(result);

            return HttpResult.Ok(result, new SharedId(result.Data.OrderId));
        }

        GetFuturesOrderOptions IFuturesOrderRestClient.GetFuturesOrderOptions { get; } = new GetFuturesOrderOptions(_exchangeName, true);
        async Task<HttpResult<SharedFuturesOrder>> IFuturesOrderRestClient.GetFuturesOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesOrder>(Exchange, validationError);

            var orders = await Trading.GetOrdersAsync(request.Symbol!.GetSymbol(FormatSymbol),
                filter: new Dictionary<string, object>
                {
                    { "orderID", request.OrderId }
                },
                ct: ct).ConfigureAwait(false);
            if (!orders.Success)
                return HttpResult.Fail<SharedFuturesOrder>(orders);

            var order = orders.Data.SingleOrDefault();
            if (order == null)
                return HttpResult.Fail<SharedFuturesOrder>(orders, new ServerError(new ErrorInfo(ErrorType.UnknownOrder, "Order not found")));

            return HttpResult.Ok(orders, new SharedFuturesOrder(
                ExchangeSymbolCache.ParseSymbol(_topicFuturesId, EnvironmentName, null, order.Symbol),
                order.Symbol,
                order.OrderId,
                ParseOrderType(order.OrderType, order.ExecutionInstruction),
                order.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(order.Status),
                order.Timestamp)
            {
                ClientOrderId = order.ClientOrderId,
                AveragePrice = order.AveragePrice,
                OrderPrice = order.Price,
                OrderQuantity = new SharedOrderQuantity(contractQuantity: order.Quantity),
                QuantityFilled = new SharedOrderQuantity(contractQuantity: order.QuantityFilled),
                TimeInForce = ParseTimeInForce(order.TimeInForce),
                UpdateTime = order.TransactTime,
                ReduceOnly = order.ExecutionInstruction?.Contains(ExecutionInstruction.ReduceOnly) == true,
                TriggerPrice = order.StopPrice,
                IsTriggerOrder = order.StopPrice > 0,
                IsCloseOrder = order.ExecutionInstruction?.Contains(ExecutionInstruction.Close) == true
            });
        }

        GetOpenFuturesOrdersOptions IFuturesOrderRestClient.GetOpenFuturesOrdersOptions { get; } = new GetOpenFuturesOrdersOptions(_exchangeName, true);
        async Task<HttpResult<SharedFuturesOrder[]>> IFuturesOrderRestClient.GetOpenFuturesOrdersAsync(GetOpenOrdersRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetOpenFuturesOrdersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesOrder[]>(Exchange, validationError);

            var symbol = request.Symbol?.GetSymbol(FormatSymbol);
            var orders = await Trading.GetOrdersAsync(symbol,
                filter: new Dictionary<string, object>
                {
                    { "ordStatus", new [] { "New", "PartiallyFilled" } }
                },
                ct: ct).ConfigureAwait(false);
            if (!orders.Success)
                return HttpResult.Fail<SharedFuturesOrder[]>(orders);

            var data = orders.Data.Where(x => BitMEXUtils.GetSymbolType(x.Symbol) != SymbolType.Spot);
            return HttpResult.Ok(orders, data.Select(x => new SharedFuturesOrder(
                ExchangeSymbolCache.ParseSymbol(_topicFuturesId, EnvironmentName, null, x.Symbol),
                x.Symbol,
                x.OrderId,
                ParseOrderType(x.OrderType, x.ExecutionInstruction),
                x.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(x.Status),
                x.Timestamp)
            {
                ClientOrderId = x.ClientOrderId,
                AveragePrice = x.AveragePrice,
                OrderPrice = x.Price,
                OrderQuantity = new SharedOrderQuantity(contractQuantity: x.Quantity),
                QuantityFilled = new SharedOrderQuantity(contractQuantity: x.QuantityFilled),
                TimeInForce = ParseTimeInForce(x.TimeInForce),
                UpdateTime = x.TransactTime,
                ReduceOnly = x.ExecutionInstruction?.Contains(ExecutionInstruction.ReduceOnly) == true,
                TriggerPrice = x.StopPrice,
                IsTriggerOrder = x.StopPrice > 0,
                IsCloseOrder = x.ExecutionInstruction?.Contains(ExecutionInstruction.Close) == true
            }).ToArray());
        }

        GetFuturesClosedOrdersOptions IFuturesOrderRestClient.GetClosedFuturesOrdersOptions { get; } = new GetFuturesClosedOrdersOptions(_exchangeName, true, true, true, 1000);
        async Task<HttpResult<SharedFuturesOrder[]>> IFuturesOrderRestClient.GetClosedFuturesOrdersAsync(GetClosedOrdersRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = SharedClient.GetClosedFuturesOrdersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesOrder[]>(Exchange, validationError);

            var direction = request.Direction ?? DataDirection.Descending;
            var limit = request.Limit ?? 500;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            // Get data
            var result = await Trading.GetOrdersAsync(request.Symbol!.GetSymbol(FormatSymbol),
                filter: new Dictionary<string, object>
                {
                    { "ordStatus", new string[] {"Canceled", "Filled" } }
                },
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                limit: pageParams.Limit,
                offset: pageParams.Offset,
                reverse: direction == DataDirection.Descending,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedFuturesOrder[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                () => Pagination.NextPageFromOffset(pageParams, result.Data.Length),
                result.Data.Length,
                result.Data.Select(x => x.Timestamp),
                request.StartTime,
                request.EndTime ?? DateTime.UtcNow,
                pageParams);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data, x => x.Timestamp, request.StartTime, request.EndTime, direction)
                       .Select(x => 
                           new SharedFuturesOrder(
                                ExchangeSymbolCache.ParseSymbol(_topicFuturesId, EnvironmentName, null, x.Symbol), 
                                x.Symbol,
                                x.OrderId,
                                ParseOrderType(x.OrderType, x.ExecutionInstruction),
                                x.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                                ParseOrderStatus(x.Status),
                                x.Timestamp)
                            {
                                ClientOrderId = x.ClientOrderId,
                                AveragePrice = x.AveragePrice,
                                OrderPrice = x.Price,
                                OrderQuantity = new SharedOrderQuantity(contractQuantity: x.Quantity),
                                QuantityFilled = new SharedOrderQuantity(contractQuantity: x.QuantityFilled),
                                TimeInForce = ParseTimeInForce(x.TimeInForce),
                                UpdateTime = x.TransactTime,
                                ReduceOnly = x.ExecutionInstruction?.Contains(ExecutionInstruction.ReduceOnly) == true,
                                TriggerPrice = x.StopPrice,
                                IsTriggerOrder = x.StopPrice > 0,
                                IsCloseOrder = x.ExecutionInstruction?.Contains(ExecutionInstruction.Close) == true
                            })
                       .ToArray(), nextPageRequest);
        }

        GetFuturesOrderTradesOptions IFuturesOrderRestClient.GetFuturesOrderTradesOptions { get; } = new GetFuturesOrderTradesOptions(_exchangeName, true);
        async Task<HttpResult<SharedUserTrade[]>> IFuturesOrderRestClient.GetFuturesOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetFuturesOrderTradesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, symbolInfoResult.Error!);

            var trades = await Trading.GetUserTradesAsync(symbol: request.Symbol!.GetSymbol(FormatSymbol),
                filter: new Dictionary<string, object>
                {
                    { "orderID", request.OrderId }
                },
                ct: ct).ConfigureAwait(false);
            if (!trades.Success)
                return HttpResult.Fail<SharedUserTrade[]>(trades);

            return HttpResult.Ok(trades, trades.Data.Select(x => new SharedUserTrade(
                ExchangeSymbolCache.ParseSymbol(_topicFuturesId, EnvironmentName, null, x.Symbol), 
                x.Symbol,
                x.OrderId,
                x.TradeId,
                x.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                x.Quantity ?? 0,
                x.LastTradePrice!.Value,
                x.Timestamp)
            {
                ClientOrderId = x.ClientOrderId,
                Fee = x.Fee.ToSharedAssetQuantity(x.SettlementCurrency ?? x.Currency),
                Role = x.Role == TradeRole.Maker ? SharedRole.Maker : SharedRole.Taker
            }).ToArray());
        }

        GetFuturesUserTradesOptions IFuturesOrderRestClient.GetFuturesUserTradesOptions { get; } = new GetFuturesUserTradesOptions(_exchangeName, true, true, true, 1000);
        async Task<HttpResult<SharedUserTrade[]>> IFuturesOrderRestClient.GetFuturesUserTradesAsync(GetUserTradesRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = SharedClient.GetFuturesUserTradesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, symbolInfoResult.Error!);

            var direction = request.Direction ?? DataDirection.Descending;
            var limit = request.Limit ?? 500;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            // Get data
            var result = await Trading.GetUserTradesAsync(symbol: request.Symbol!.GetSymbol(FormatSymbol),
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                limit: pageParams.Limit,
                offset: pageParams.Offset,
                reverse: direction == DataDirection.Descending,
                ct: ct
                ).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedUserTrade[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                () => Pagination.NextPageFromOffset(pageParams, result.Data.Length),
                result.Data.Length,
                result.Data.Select(x => x.Timestamp),
                request.StartTime,
                request.EndTime ?? DateTime.UtcNow,
                pageParams);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data, x => x.Timestamp, request.StartTime, request.EndTime, direction)
                       .Select(x => 
                            new SharedUserTrade(
                                ExchangeSymbolCache.ParseSymbol(_topicFuturesId, EnvironmentName, null, x.Symbol), 
                                x.Symbol,
                                x.OrderId,
                                x.TradeId,
                                x.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                                x.Quantity ?? 0,
                                x.LastTradePrice!.Value,
                                x.Timestamp)
                            {
                                ClientOrderId = x.ClientOrderId,
                                Fee = x.Fee.ToSharedAssetQuantity(x.SettlementCurrency ?? x.Currency),
                                Role = x.Role == TradeRole.Maker ? SharedRole.Maker : SharedRole.Taker
                            })
                       .ToArray(), nextPageRequest);
        }

        CancelFuturesOrderOptions IFuturesOrderRestClient.CancelFuturesOrderOptions { get; } = new CancelFuturesOrderOptions(_exchangeName, true);
        async Task<HttpResult<SharedId>> IFuturesOrderRestClient.CancelFuturesOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.CancelFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var order = await Trading.CancelOrderAsync(request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedId>(order);

            return HttpResult.Ok(order, new SharedId(request.OrderId));
        }

        GetPositionsOptions IFuturesOrderRestClient.GetPositionsOptions { get; } = new GetPositionsOptions(_exchangeName, true);
        async Task<HttpResult<SharedPosition[]>> IFuturesOrderRestClient.GetPositionsAsync(GetPositionsRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetPositionsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedPosition[]>(Exchange, validationError);

            var result = await Trading.GetPositionsAsync(filter: request.Symbol == null ? null : new Dictionary<string, object>{
                { "symbol", request.Symbol!.GetSymbol(FormatSymbol) }
            }, ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedPosition[]>(result);

            IEnumerable<BitMEXPosition> data = result.Data;
            if (request.TradingMode != null)
                data = data.Where(x => request.TradingMode.Value.IsPerpetual() ? BitMEXUtils.GetSymbolType(x.Symbol) == SymbolType.PerpetualContract : BitMEXUtils.GetSymbolType(x.Symbol) == SymbolType.Futures);
            
            var resultTypes = request.Symbol == null && request.TradingMode == null ? SupportedTradingModes : request.Symbol != null ? new[] { request.Symbol!.TradingMode } : new[] { request.TradingMode!.Value };
            return HttpResult.Ok(result, data.Where(x => x.Currency != null).Select(x => new SharedPosition(ExchangeSymbolCache.ParseSymbol(_topicFuturesId, EnvironmentName, null, x.Symbol), x.Symbol, Math.Abs(x.CurrentQuantity ?? 0), x.Timestamp)
            {
                UnrealizedPnl = x.UnrealizedPnl.ToSharedAssetQuantity(x.Currency!),
                LiquidationPrice = x.LiquidationPrice == 0 ? null : x.LiquidationPrice,
                Leverage = x.Leverage,
                AverageOpenPrice = x.AverageEntryPrice,
                PositionMode = SharedPositionMode.OneWay,
                PositionSide = x.CurrentQuantity < 0 ? SharedPositionSide.Short : SharedPositionSide.Long
            }).ToArray());
        }

        ClosePositionOptions IFuturesOrderRestClient.ClosePositionOptions { get; } = new ClosePositionOptions(_exchangeName, true)
        {
            RequiredOptionalParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(ClosePositionRequest.PositionSide), typeof(SharedPositionSide), "The position side to close", SharedPositionSide.Long),
            }
        };
        async Task<HttpResult<SharedId>> IFuturesOrderRestClient.ClosePositionAsync(ClosePositionRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.ClosePositionOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol); 
            var result = await Trading.PlaceOrderAsync(
                symbol,
                request.PositionSide == SharedPositionSide.Long ? OrderSide.Sell : OrderSide.Buy,
                OrderType.Market,
                executionInstruction: ExecutionInstruction.Close,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedId>(result);

            return HttpResult.Ok(result, new SharedId(result.Data.OrderId));
        }

        #endregion

        #region Futures Client Id Order Client

        GetFuturesOrderByClientOrderIdOptions IFuturesOrderClientIdRestClient.GetFuturesOrderByClientOrderIdOptions { get; } = new GetFuturesOrderByClientOrderIdOptions(_exchangeName, true);
        async Task<HttpResult<SharedFuturesOrder>> IFuturesOrderClientIdRestClient.GetFuturesOrderByClientOrderIdAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesOrder>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult.Success)
                return HttpResult.Fail<SharedFuturesOrder>(Exchange, symbolInfoResult.Error!);

            var orders = await Trading.GetOrdersAsync(request.Symbol!.GetSymbol(FormatSymbol),
                filter: new Dictionary<string, object>
                {
                    { "clOrdID", request.OrderId }
                },
                ct: ct).ConfigureAwait(false);
            if (!orders.Success)
                return HttpResult.Fail<SharedFuturesOrder>(orders);

            var order = orders.Data.SingleOrDefault();
            if (order == null)
                return HttpResult.Fail<SharedFuturesOrder>(orders, new ServerError(new ErrorInfo(ErrorType.UnknownOrder, "Order not found")));

            return HttpResult.Ok(orders, new SharedFuturesOrder(
                ExchangeSymbolCache.ParseSymbol(_topicFuturesId, EnvironmentName, null, order.Symbol),
                order.Symbol,
                order.OrderId,
                ParseOrderType(order.OrderType, order.ExecutionInstruction),
                order.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(order.Status),
                order.Timestamp)
            {
                ClientOrderId = order.ClientOrderId,
                AveragePrice = order.AveragePrice,
                OrderPrice = order.Price,
                OrderQuantity = new SharedOrderQuantity(contractQuantity: order.Quantity),
                QuantityFilled = new SharedOrderQuantity(contractQuantity: order.QuantityFilled),
                TimeInForce = ParseTimeInForce(order.TimeInForce),
                UpdateTime = order.TransactTime,
                ReduceOnly = order.ExecutionInstruction?.Contains(ExecutionInstruction.ReduceOnly) == true,
                TriggerPrice = order.StopPrice,
                IsTriggerOrder = order.StopPrice > 0,
                IsCloseOrder = order.ExecutionInstruction?.Contains(ExecutionInstruction.Close) == true
            });
        }

        CancelFuturesOrderByClientOrderIdOptions IFuturesOrderClientIdRestClient.CancelFuturesOrderByClientOrderIdOptions { get; } = new CancelFuturesOrderByClientOrderIdOptions(_exchangeName, true);
        async Task<HttpResult<SharedId>> IFuturesOrderClientIdRestClient.CancelFuturesOrderByClientOrderIdAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.CancelFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var order = await Trading.CancelOrderAsync(clientOrderId: request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedId>(order);

            return HttpResult.Ok(order, new SharedId(order.Data.OrderId));
        }
        #endregion

        #region Futures Trigger Order Client
        PlaceFuturesTriggerOrderOptions IFuturesTriggerOrderRestClient.PlaceFuturesTriggerOrderOptions { get; } = new PlaceFuturesTriggerOrderOptions(_exchangeName, false)
        {
        };

        async Task<HttpResult<SharedId>> IFuturesTriggerOrderRestClient.PlaceFuturesTriggerOrderAsync(PlaceFuturesTriggerOrderRequest request, CancellationToken ct)
        {
            var side = GetTriggerOrderSide(request);
            var validationError = SharedClient.PlaceFuturesTriggerOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var result = await Trading.PlaceOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                side,
                GetTriggerOrderType(request),
                quantity: (int)(request.Quantity?.QuantityInContracts ?? 0),
                price: request.OrderPrice,
                stopPrice: request.TriggerPrice,
                executionInstruction: GetExecutionInstruction(request),
                timeInForce: GetTimeInForce(request.TimeInForce),
                clientOrderId: request.ClientOrderId,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedId>(result);

            // Return
            return HttpResult.Ok(result, new SharedId(result.Data.OrderId.ToString()));
        }

        GetFuturesTriggerOrderOptions IFuturesTriggerOrderRestClient.GetFuturesTriggerOrderOptions { get; } = new GetFuturesTriggerOrderOptions(_exchangeName, true)
        {
            RequestNotes = "Only pending trigger orders can be requested, executed trigger orders are not available in the API"
        };
        async Task<HttpResult<SharedFuturesTriggerOrder>> IFuturesTriggerOrderRestClient.GetFuturesTriggerOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetFuturesTriggerOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesTriggerOrder>(Exchange, validationError);

            var orders = await Trading.GetOrdersAsync(request.Symbol!.GetSymbol(FormatSymbol),
                filter: new Dictionary<string, object>
                {
                    { "orderID", request.OrderId }
                },
                ct: ct).ConfigureAwait(false);
            if (!orders.Success)
                return HttpResult.Fail<SharedFuturesTriggerOrder>(orders);

            var order = orders.Data.SingleOrDefault();
            if (order == null)
                return HttpResult.Fail<SharedFuturesTriggerOrder>(orders, new ServerError(new ErrorInfo(ErrorType.UnknownOrder, "Order not found")));

            return HttpResult.Ok(orders, new SharedFuturesTriggerOrder(
                ExchangeSymbolCache.ParseSymbol(_topicFuturesId, EnvironmentName, null, order.Symbol),
                order.Symbol,
                order.OrderId,
                order.OrderType == OrderType.StopLimit ? SharedOrderType.Limit : SharedOrderType.Market,
                null,
                ParseTriggerOrderStatus(order.Status),
                order.StopPrice ?? 0,
                null,
                order.Timestamp)
            {
                AveragePrice = order.AveragePrice,
                OrderPrice = order.Price,
                OrderQuantity = new SharedOrderQuantity(contractQuantity: order.Quantity),
                QuantityFilled = new SharedOrderQuantity(contractQuantity: order.QuantityFilled),
                TimeInForce = ParseTimeInForce(order.TimeInForce),
                UpdateTime = order.TransactTime,
                ClientOrderId = order.ClientOrderId
            });
        }

        CancelFuturesTriggerOrderOptions IFuturesTriggerOrderRestClient.CancelFuturesTriggerOrderOptions { get; } = new CancelFuturesTriggerOrderOptions(_exchangeName, true);
        async Task<HttpResult<SharedId>> IFuturesTriggerOrderRestClient.CancelFuturesTriggerOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.CancelFuturesTriggerOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var order = await Trading.CancelOrderAsync(
                request.OrderId,
                ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedId>(order);

            return HttpResult.Ok(order, new SharedId(request.OrderId));
        }

        private OrderType GetTriggerOrderType(PlaceSpotTriggerOrderRequest request)
        {
            if (request.OrderSide == SharedOrderSide.Buy)
            {
                if (request.PriceDirection == SharedTriggerPriceDirection.PriceAbove)
                    return request.OrderPrice == null ? OrderType.StopMarket : OrderType.StopLimit;

                return request.OrderPrice == null ? OrderType.MarketIfTouched : OrderType.LimitIfTouched;
            }

            if (request.PriceDirection == SharedTriggerPriceDirection.PriceAbove)
                return request.OrderPrice == null ? OrderType.MarketIfTouched : OrderType.LimitIfTouched;

            return request.OrderPrice == null ? OrderType.StopMarket : OrderType.StopLimit;
        }

        private OrderType GetTriggerOrderType(PlaceFuturesTriggerOrderRequest request)
        {
            if (request.PositionSide == SharedPositionSide.Long)
            {
                if (request.OrderDirection == SharedTriggerOrderDirection.Enter)
                {
                    // Long enter
                    if (request.PriceDirection == SharedTriggerPriceDirection.PriceAbove)
                        return request.OrderPrice == null ? OrderType.StopMarket : OrderType.StopLimit;

                    return request.OrderPrice == null ? OrderType.MarketIfTouched : OrderType.LimitIfTouched;
                }

                // Long exit
                if (request.PriceDirection == SharedTriggerPriceDirection.PriceAbove)
                    return request.OrderPrice == null ? OrderType.MarketIfTouched : OrderType.LimitIfTouched;

                return request.OrderPrice == null ? OrderType.StopMarket : OrderType.StopLimit;
            }
            else
            {
                if (request.OrderDirection == SharedTriggerOrderDirection.Enter)
                {
                    // Short enter
                    if (request.PriceDirection == SharedTriggerPriceDirection.PriceAbove)
                        return request.OrderPrice == null ? OrderType.MarketIfTouched : OrderType.LimitIfTouched;

                    return request.OrderPrice == null ? OrderType.StopMarket : OrderType.StopLimit;
                }

                // Short exit
                if (request.PriceDirection == SharedTriggerPriceDirection.PriceAbove)
                    return request.OrderPrice == null ? OrderType.StopMarket : OrderType.StopLimit;

                return request.OrderPrice == null ? OrderType.MarketIfTouched : OrderType.LimitIfTouched;
            }
        }

        private ExecutionInstruction? GetExecutionInstruction(PlaceFuturesTriggerOrderRequest request)
        {
            return request.TriggerPriceType switch
            {
                null => null,
                SharedTriggerPriceType.LastPrice => ExecutionInstruction.LastPrice,
                SharedTriggerPriceType.IndexPrice => ExecutionInstruction.IndexPrice,
                _ => ExecutionInstruction.MarkPrice
            };
        }

        private OrderSide GetTriggerOrderSide(PlaceFuturesTriggerOrderRequest request)
        {
            if (request.PositionSide == SharedPositionSide.Long)
                return request.OrderDirection == SharedTriggerOrderDirection.Enter ? OrderSide.Buy : OrderSide.Sell;

            return request.OrderDirection == SharedTriggerOrderDirection.Enter ? OrderSide.Sell : OrderSide.Buy;
        }
        #endregion

        #region Spot Trigger Order Client
        PlaceSpotTriggerOrderOptions ISpotTriggerOrderRestClient.PlaceSpotTriggerOrderOptions { get; } = new PlaceSpotTriggerOrderOptions(_exchangeName, false);

        async Task<HttpResult<SharedId>> ISpotTriggerOrderRestClient.PlaceSpotTriggerOrderAsync(PlaceSpotTriggerOrderRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.PlaceSpotTriggerOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var result = await Trading.PlaceOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                request.OrderSide == SharedOrderSide.Buy ? OrderSide.Buy : OrderSide.Sell,
                GetTriggerOrderType(request),
                quantity: request.Quantity?.QuantityInBaseAsset.ToBitMEXAssetQuantity(request.Symbol!.BaseAsset),
                price: request.OrderPrice,
                clientOrderId: request.ClientOrderId,
                stopPrice: request.TriggerPrice,
                timeInForce: GetTimeInForce(request.TimeInForce),
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedId>(result);

            // Return
            return HttpResult.Ok(result, new SharedId(result.Data.OrderId.ToString()));
        }

        GetSpotTriggerOrderOptions ISpotTriggerOrderRestClient.GetSpotTriggerOrderOptions { get; } = new GetSpotTriggerOrderOptions(_exchangeName, true)
        {
            RequestNotes = "Only pending trigger orders can be requested, executed trigger orders are not available in the API"
        };
        async Task<HttpResult<SharedSpotTriggerOrder>> ISpotTriggerOrderRestClient.GetSpotTriggerOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetSpotTriggerOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotTriggerOrder>(Exchange, validationError);

            var orders = await Trading.GetOrdersAsync(request.Symbol!.GetSymbol(FormatSymbol),
                filter: new Dictionary<string, object>
                {
                    { "orderID", request.OrderId }
                },
                ct: ct).ConfigureAwait(false);
            if (!orders.Success)
                return HttpResult.Fail<SharedSpotTriggerOrder>(orders);

            var order = orders.Data.SingleOrDefault();
            if (order == null)
                return HttpResult.Fail<SharedSpotTriggerOrder>(orders, new ServerError(new ErrorInfo(ErrorType.UnknownOrder, "Order not found")));

            return HttpResult.Ok(orders, new SharedSpotTriggerOrder(
                ExchangeSymbolCache.ParseSymbol(_topicSpotId, EnvironmentName, null, order.Symbol),
                order.Symbol,
                order.OrderId,
                order.OrderType == OrderType.StopLimit ? SharedOrderType.Limit : SharedOrderType.Market,
                order.OrderSide == OrderSide.Buy ? SharedTriggerOrderDirection.Enter : SharedTriggerOrderDirection.Exit,
                ParseTriggerOrderStatus(order.Status),
                order.StopPrice ?? 0,
                order.Timestamp)
            {
                PlacedOrderId = order.OrderId,
                AveragePrice = order.AveragePrice,
                OrderPrice = order.Price,
                OrderQuantity = new SharedOrderQuantity(contractQuantity: order.Quantity),
                QuantityFilled = new SharedOrderQuantity(contractQuantity: order.QuantityFilled),
                TimeInForce = ParseTimeInForce(order.TimeInForce),
                UpdateTime = order.TransactTime,
                ClientOrderId = order.ClientOrderId
            });
        }

        private SharedTriggerOrderStatus ParseTriggerOrderStatus(OrderStatus status)
        {
            if (status == OrderStatus.Filled)
                return SharedTriggerOrderStatus.Filled;

            if (status == OrderStatus.Canceled || status == OrderStatus.Rejected)
                return SharedTriggerOrderStatus.CanceledOrRejected;

            if (status == OrderStatus.New || status == OrderStatus.PartiallyFilled)
                return SharedTriggerOrderStatus.Active;

            return SharedTriggerOrderStatus.Unknown;
        }

        CancelSpotTriggerOrderOptions ISpotTriggerOrderRestClient.CancelSpotTriggerOrderOptions { get; } = new CancelSpotTriggerOrderOptions(_exchangeName, true);
        async Task<HttpResult<SharedId>> ISpotTriggerOrderRestClient.CancelSpotTriggerOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.CancelSpotTriggerOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var order = await Trading.CancelOrderAsync(
                request.OrderId,
                ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedId>(order);

            return HttpResult.Ok(order, new SharedId(request.OrderId));
        }
        #endregion

        #region Tp/SL Client
        SetFuturesTpSlOptions IFuturesTpSlRestClient.SetFuturesTpSlOptions { get; } = new SetFuturesTpSlOptions(_exchangeName, true);
        async Task<HttpResult<SharedId>> IFuturesTpSlRestClient.SetFuturesTpSlAsync(SetTpSlRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.SetFuturesTpSlOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var result = await Trading.PlaceOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                request.PositionSide == SharedPositionSide.Long ? OrderSide.Sell : OrderSide.Buy,
                OrderType.MarketIfTouched,
                stopPrice: request.TriggerPrice,
                executionInstruction: ExecutionInstruction.Close,
                ct: ct).ConfigureAwait(false);

            if (!result.Success)
                return HttpResult.Fail<SharedId>(result);

            // Return
            return HttpResult.Ok(result, new SharedId(result.Data.OrderId.ToString()));
        }

        CancelFuturesTpSlOptions IFuturesTpSlRestClient.CancelFuturesTpSlOptions { get; } = new CancelFuturesTpSlOptions(_exchangeName, true)
        {
            RequiredOptionalParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(CancelTpSlRequest.OrderId), typeof(string), "Id of the tp/sl order", "123123")
            }
        };

        async Task<HttpResult<bool>> IFuturesTpSlRestClient.CancelFuturesTpSlAsync(CancelTpSlRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.CancelFuturesTpSlOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<bool>(Exchange, validationError);

            var result = await Trading.CancelOrderAsync(
                orderId: request.OrderId!,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<bool>(result);

            // Return
            return HttpResult.Ok(result, true);
        }

        #endregion
    }
}
