using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Text;
using BitMEX.Net.Interfaces.Clients.ExchangeApi;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using CryptoExchange.Net.Objects;
using System.Collections.Concurrent;
using BitMEX.Net.ExtensionMethods;
using BitMEX.Net.Enums;
using CryptoExchange.Net;

namespace BitMEX.Net.Clients.ExchangeApi
{
    internal partial class BitMEXRestClientExchangeApi : IBitMEXRestClientExchangeApiShared
    {
        public string Exchange => "BitMEX";

        public TradingMode[] SupportedTradingModes => new[] { TradingMode.Spot, TradingMode.PerpetualLinear, TradingMode.DeliveryLinear, TradingMode.PerpetualInverse, TradingMode.DeliveryInverse };

        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();


        #region Asset client
        EndpointOptions<GetAssetsRequest> IAssetsRestClient.GetAssetsOptions { get; } = new EndpointOptions<GetAssetsRequest>(true);

        async Task<ExchangeWebResult<IEnumerable<SharedAsset>>> IAssetsRestClient.GetAssetsAsync(GetAssetsRequest request, CancellationToken ct)
        {
            var validationError = ((IAssetsRestClient)this).GetAssetsOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedAsset>>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult)
                return new ExchangeWebResult<IEnumerable<SharedAsset>>(Exchange, symbolInfoResult.Error!);

            var assets = await ExchangeData.GetAssetsAsync(ct: ct).ConfigureAwait(false);
            if (!assets)
                return assets.AsExchangeResult<IEnumerable<SharedAsset>>(Exchange, null, default);

            return assets.AsExchangeResult<IEnumerable<SharedAsset>>(Exchange, TradingMode.Spot, assets.Data.Select(x =>
            {
                return new SharedAsset(x.Asset)
                {
                    FullName = x.Name,
                    Networks = x.Networks.Select(n => new SharedAssetNetwork(n.Asset)
                        {
                            DepositEnabled = n.DepositEnabled,
                            MinWithdrawQuantity = x.MinWithdrawalQuantity.ToSharedAssetQuantity(x.Currency),
                            MaxWithdrawQuantity = x.MaxWithdrawalQuantity.ToSharedAssetQuantity(x.Currency),
                            WithdrawEnabled = n.WithdrawalEnabled,
                            WithdrawFee = n.WithdrawalFee.ToSharedAssetQuantity(x.Currency) ?? n.MinFee.ToSharedAssetQuantity(x.Currency),
                        }
                    ).ToArray()
                };
            }).ToArray());
        }

        EndpointOptions<GetAssetRequest> IAssetsRestClient.GetAssetOptions { get; } = new EndpointOptions<GetAssetRequest>(false);
        async Task<ExchangeWebResult<SharedAsset>> IAssetsRestClient.GetAssetAsync(GetAssetRequest request, CancellationToken ct)
        {
            var validationError = ((IAssetsRestClient)this).GetAssetOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedAsset>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult)
                return new ExchangeWebResult<SharedAsset>(Exchange, symbolInfoResult.Error!);

            var assets = await ExchangeData.GetAssetsAsync(ct: ct).ConfigureAwait(false);
            if (!assets)
                return assets.AsExchangeResult<SharedAsset>(Exchange, null, default);

            var asset = assets.Data.SingleOrDefault(x => x.Asset == request.Asset);
            if (asset == null)
                return assets.AsExchangeError<SharedAsset>(Exchange, new ServerError("Not found"));

            return assets.AsExchangeResult<SharedAsset>(Exchange, TradingMode.Spot, 
                new SharedAsset(asset.Asset)
                {
                    FullName = asset.Name,
                    Networks = asset.Networks.Select(n => new SharedAssetNetwork(n.Asset)
                    {
                        DepositEnabled = n.DepositEnabled,
                        MinWithdrawQuantity = asset.MinWithdrawalQuantity.ToSharedAssetQuantity(n.Asset),
                        MaxWithdrawQuantity = asset.MaxWithdrawalQuantity.ToSharedAssetQuantity(n.Asset),
                        WithdrawEnabled = n.WithdrawalEnabled,
                        WithdrawFee = n.WithdrawalFee.ToSharedAssetQuantity(n.Asset) ?? n.MinFee.ToSharedAssetQuantity(n.Asset),
                    }
                    ).ToArray()
                }
            );
        }

        #endregion

        #region Balance Client
        EndpointOptions<GetBalancesRequest> IBalanceRestClient.GetBalancesOptions { get; } = new EndpointOptions<GetBalancesRequest>(true);

        async Task<ExchangeWebResult<IEnumerable<SharedBalance>>> IBalanceRestClient.GetBalancesAsync(GetBalancesRequest request, CancellationToken ct)
        {
            var validationError = ((IBalanceRestClient)this).GetBalancesOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedBalance>>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult)
                return new ExchangeWebResult<IEnumerable<SharedBalance>>(Exchange, symbolInfoResult.Error!);

            var result = await Account.GetBalancesAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<IEnumerable<SharedBalance>>(Exchange, null, default);

            return result.AsExchangeResult<IEnumerable<SharedBalance>>(Exchange, TradingMode.Spot, result.Data.Select(x => 
            new SharedBalance(BitMEXUtils.GetAssetFromCurrency(x.Currency), x.Quantity.ToSharedAssetQuantity(x.Currency), (x.Quantity + x.PendingCredit).ToSharedAssetQuantity(x.Currency))).ToArray());
        }

        #endregion

        #region Deposit client

        EndpointOptions<GetDepositAddressesRequest> IDepositRestClient.GetDepositAddressesOptions { get; } = new EndpointOptions<GetDepositAddressesRequest>(true)
        {
            RequiredOptionalParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(GetDepositAddressesRequest.Network), typeof(string), "Network to use", "btc")
            }
        };
        async Task<ExchangeWebResult<IEnumerable<SharedDepositAddress>>> IDepositRestClient.GetDepositAddressesAsync(GetDepositAddressesRequest request, CancellationToken ct)
        {
            var validationError = ((IDepositRestClient)this).GetDepositAddressesOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedDepositAddress>>(Exchange, validationError);

            var depositAddresses = await Account.GetDepositAddressAsync(request.Asset, request.Network!, ct: ct).ConfigureAwait(false);
            if (!depositAddresses)
                return depositAddresses.AsExchangeResult<IEnumerable<SharedDepositAddress>>(Exchange, null, default);

            return depositAddresses.AsExchangeResult<IEnumerable<SharedDepositAddress>>(Exchange, TradingMode.Spot, new[] { new SharedDepositAddress(request.Asset, depositAddresses.Data) });
        }

        GetDepositsOptions IDepositRestClient.GetDepositsOptions { get; } = new GetDepositsOptions(SharedPaginationSupport.Descending, false, 10000)
        {
            RequestNotes = "Due to the API not offering a filter on deposit type less results may be returned per page"
        };
        async Task<ExchangeWebResult<IEnumerable<SharedDeposit>>> IDepositRestClient.GetDepositsAsync(GetDepositsRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((IDepositRestClient)this).GetDepositsOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedDeposit>>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult)
                return new ExchangeWebResult<IEnumerable<SharedDeposit>>(Exchange, symbolInfoResult.Error!);

            // Determine page token
            int? offset = null;
            if (pageToken is OffsetToken offsetToken)
                offset = offsetToken.Offset;

            // Get data
            var deposits = await Account.GetBalanceHistoryAsync(
                request.Asset,
                limit: request.Limit ?? 100,
                offset: offset,
                ct: ct).ConfigureAwait(false);
            if (!deposits)
                return deposits.AsExchangeResult<IEnumerable<SharedDeposit>>(Exchange, null, default);

            // Determine next token
            OffsetToken? nextToken = null;
            if (deposits.Data.Count() == (request.Limit ?? 100))
                nextToken = new OffsetToken((offset ?? 0) + deposits.Data.Count());

            return deposits.AsExchangeResult<IEnumerable<SharedDeposit>>(Exchange, TradingMode.Spot, 
                deposits.Data.Where(x => x.TransactionType == TransactionType.Deposit).Select(x => new SharedDeposit(BitMEXUtils.GetAssetFromCurrency(x.Currency), x.Quantity.ToSharedAssetQuantity(x.Currency), x.TransactionStatus == TransactionStatus.Completed, x.TransactionTime)
            {
                Network = x.Network,
                TransactionId = x.Transaction,
                Tag = x.Memo,
                Id = x.TransactionId
            }).ToArray(), nextToken);
        }

        #endregion

        #region Fee Client
        EndpointOptions<GetFeeRequest> IFeeRestClient.GetFeeOptions { get; } = new EndpointOptions<GetFeeRequest>(true);

        async Task<ExchangeWebResult<SharedFee>> IFeeRestClient.GetFeesAsync(GetFeeRequest request, CancellationToken ct)
        {
            var validationError = ((IFeeRestClient)this).GetFeeOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedFee>(Exchange, validationError);

            // Get data
            var result = await Account.GetFeesAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedFee>(Exchange, null, default);

            if(!result.Data.TryGetValue(request.Symbol.GetSymbol(FormatSymbol), out var fees))
                return result.AsExchangeError<SharedFee>(Exchange, new ServerError("Not found"));

            // Return
            return result.AsExchangeResult(Exchange, request.Symbol.TradingMode, new SharedFee(fees.MakerFee * 100, fees.TakerFee * 100));
        }
        #endregion

        #region Klines Client

        GetKlinesOptions IKlineRestClient.GetKlinesOptions { get; } = new GetKlinesOptions(SharedPaginationSupport.Descending, true, 1000, false,
            SharedKlineInterval.OneMinute,
            SharedKlineInterval.FiveMinutes,
            SharedKlineInterval.OneHour,
            SharedKlineInterval.OneDay);

        async Task<ExchangeWebResult<IEnumerable<SharedKline>>> IKlineRestClient.GetKlinesAsync(GetKlinesRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var interval = (Enums.BinPeriod)request.Interval;
            if (!Enum.IsDefined(typeof(Enums.BinPeriod), interval))
                return new ExchangeWebResult<IEnumerable<SharedKline>>(Exchange, new ArgumentError("Interval not supported"));

            var validationError = ((IKlineRestClient)this).GetKlinesOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedKline>>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult)
                return new ExchangeWebResult<IEnumerable<SharedKline>>(Exchange, symbolInfoResult.Error!);

            // Determine pagination
            int offset = 0;
            if (pageToken is OffsetToken offsetToken)
                offset = offsetToken.Offset;

            var limit = request.Limit ?? 100;
            bool includeNow = request.EndTime == null && offset == 0;

            // Get data
            var symbol = request.Symbol.GetSymbol(FormatSymbol);
            var result = await ExchangeData.GetKlinesAsync(
                symbol,
                period: interval,
                partial: includeNow == true ? true : null,
                startTime: request.StartTime,
                endTime: request.EndTime,
                offset: offset,
                limit: limit,
                ct: ct
                ).ConfigureAwait(false);
            if (!result)
                return new ExchangeWebResult<IEnumerable<SharedKline>>(Exchange, TradingMode.Spot, result.As<IEnumerable<SharedKline>>(default));

            // Get next token
            OffsetToken? nextToken = null;
            if (result.Data.Count() == limit)
                nextToken = new OffsetToken(offset + (includeNow ? limit - 1 : limit));            

            return result.AsExchangeResult<IEnumerable<SharedKline>>(Exchange, request.Symbol.TradingMode, 
                result.Data.Select(x => new SharedKline(x.Timestamp.AddSeconds(-(int)interval), x.ClosePrice, x.HighPrice, x.LowPrice, x.OpenPrice, x.Volume.ToSharedSymbolQuantity(symbol))).ToArray(), nextToken);
        }

        #endregion

        #region Order Book client
        GetOrderBookOptions IOrderBookRestClient.GetOrderBookOptions { get; } = new GetOrderBookOptions(1, 5000, false);
        async Task<ExchangeWebResult<SharedOrderBook>> IOrderBookRestClient.GetOrderBookAsync(GetOrderBookRequest request, CancellationToken ct)
        {
            var validationError = ((IOrderBookRestClient)this).GetOrderBookOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedOrderBook>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult)
                return new ExchangeWebResult<SharedOrderBook>(Exchange, symbolInfoResult.Error!);

            var result = await ExchangeData.GetOrderBookAsync(
                request.Symbol.GetSymbol(FormatSymbol),
                limit: request.Limit ?? 20,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedOrderBook>(Exchange, null, default);

            var book = new SharedOrderBook(result.Data.Asks, result.Data.Bids);
            if (request.Symbol.TradingMode == TradingMode.Spot)
            {
                foreach (var item in book.Asks)
                    item.Quantity = ((long)item.Quantity).ToSharedAssetQuantity(request.Symbol.BaseAsset);
                foreach (var item in book.Bids)
                    item.Quantity = ((long)item.Quantity).ToSharedAssetQuantity(request.Symbol.BaseAsset);
            }

            return result.AsExchangeResult(Exchange, TradingMode.Spot, book);
        }

        #endregion

        #region Recent Trades client
        GetRecentTradesOptions IRecentTradeRestClient.GetRecentTradesOptions { get; } = new GetRecentTradesOptions(1000, false);

        async Task<ExchangeWebResult<IEnumerable<SharedTrade>>> IRecentTradeRestClient.GetRecentTradesAsync(GetRecentTradesRequest request, CancellationToken ct)
        {
            var validationError = ((IRecentTradeRestClient)this).GetRecentTradesOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedTrade>>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult)
                return new ExchangeWebResult<IEnumerable<SharedTrade>>(Exchange, symbolInfoResult.Error!);

            // Get data
            var symbol = request.Symbol.GetSymbol(FormatSymbol);
            var result = await ExchangeData.GetTradesAsync(
                symbol,
                limit: request.Limit,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<IEnumerable<SharedTrade>>(Exchange, null, default);

            // Return
            return result.AsExchangeResult<IEnumerable<SharedTrade>>(Exchange, TradingMode.Spot, result.Data.Select(x => 
            new SharedTrade(x.Quantity.ToSharedSymbolQuantity(symbol), x.Price, x.Timestamp)
            {
                Side = x.Side == OrderSide.Sell ? SharedOrderSide.Sell : SharedOrderSide.Buy,
            }).ToArray());
        }
        #endregion

        #region Trade History client
        GetTradeHistoryOptions ITradeHistoryRestClient.GetTradeHistoryOptions { get; } = new GetTradeHistoryOptions(SharedPaginationSupport.Ascending, true, 1000, false);

        async Task<ExchangeWebResult<IEnumerable<SharedTrade>>> ITradeHistoryRestClient.GetTradeHistoryAsync(GetTradeHistoryRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((ITradeHistoryRestClient)this).GetTradeHistoryOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedTrade>>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult)
                return new ExchangeWebResult<IEnumerable<SharedTrade>>(Exchange, symbolInfoResult.Error!);

            int offset = 0;
            if (pageToken is OffsetToken token)
                offset = token.Offset;

            var limit = request.Limit ?? 1000;

            // Get data
            var symbol = request.Symbol.GetSymbol(FormatSymbol);
            var result = await ExchangeData.GetTradesAsync(
                symbol,
                startTime: request.StartTime,
                endTime: request.EndTime,
                limit: limit,
                offset: offset,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<IEnumerable<SharedTrade>>(Exchange, null, default);

            OffsetToken? nextToken = null;
            if (result.Data.Count() == limit)
                nextToken = new OffsetToken(offset + limit);

            // Return
            return result.AsExchangeResult<IEnumerable<SharedTrade>>(Exchange, TradingMode.Spot, result.Data.Select(x => new SharedTrade(x.Quantity.ToSharedSymbolQuantity(symbol), x.Price, x.Timestamp)
            {
                Side = x.Side == OrderSide.Sell ? SharedOrderSide.Sell : SharedOrderSide.Buy,
            }).ToArray(), nextToken);
        }
        #endregion

        #region Withdrawal client

        GetWithdrawalsOptions IWithdrawalRestClient.GetWithdrawalsOptions { get; } = new GetWithdrawalsOptions(SharedPaginationSupport.Descending, true, 1000)
        {
            RequestNotes = "Due to the API not offering a filter on withdrawal type less results may be returned per page"
        };
        async Task<ExchangeWebResult<IEnumerable<SharedWithdrawal>>> IWithdrawalRestClient.GetWithdrawalsAsync(GetWithdrawalsRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((IWithdrawalRestClient)this).GetWithdrawalsOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedWithdrawal>>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult)
                return new ExchangeWebResult<IEnumerable<SharedWithdrawal>>(Exchange, symbolInfoResult.Error!);

            // Determine page token
            int? offset = null;
            if (pageToken is OffsetToken offsetToken)
                offset = offsetToken.Offset;

            // Get data
            var withdrawals = await Account.GetBalanceHistoryAsync(
                request.Asset,
                limit: request.Limit ?? 100,
                offset: offset,
                ct: ct).ConfigureAwait(false);
            if (!withdrawals)
                return withdrawals.AsExchangeResult<IEnumerable<SharedWithdrawal>>(Exchange, null, default);

            // Determine next token
            OffsetToken? nextToken = null;
            if (withdrawals.Data.Count() == (request.Limit ?? 100))
                nextToken = new OffsetToken((offset ?? 0) + withdrawals.Data.Count());

            return withdrawals.AsExchangeResult<IEnumerable<SharedWithdrawal>>(Exchange, TradingMode.Spot, withdrawals.Data.Where(x => x.TransactionType == TransactionType.Withdrawal).Select(x => 
            new SharedWithdrawal(BitMEXUtils.GetAssetFromCurrency(x.Currency), x.Address, x.Quantity.ToSharedAssetQuantity(x.Currency), x.TransactionStatus == TransactionStatus.Completed, x.Timestamp)
            {
                Network = x.Network,
                Tag = x.Memo,
                TransactionId = x.Transaction,
                Fee = x.Fee.ToSharedAssetQuantity(x.Currency),
                Id = x.TransactionId
            }).ToArray());
        }

        #endregion

        #region Withdraw client

        WithdrawOptions IWithdrawRestClient.WithdrawOptions { get; } = new WithdrawOptions()
        {
            RequiredOptionalParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(WithdrawRequest.Network), typeof(string), "Network to use", "btc")
            }
        };
        async Task<ExchangeWebResult<SharedId>> IWithdrawRestClient.WithdrawAsync(WithdrawRequest request, CancellationToken ct)
        {
            var validationError = ((IWithdrawRestClient)this).WithdrawOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult)
                return new ExchangeWebResult<SharedId>(Exchange, symbolInfoResult.Error!);

            // Get data
            var withdrawal = await Account.WithdrawAsync(
                request.Asset,
                network: request.Network!,
                request.Quantity.ToBitMEXAssetQuantity(request.Asset),
                request.Address,
                memo: request.AddressTag,
                ct: ct).ConfigureAwait(false);
            if (!withdrawal)
                return withdrawal.AsExchangeResult<SharedId>(Exchange, null, default);

            return withdrawal.AsExchangeResult(Exchange, TradingMode.Spot, new SharedId(withdrawal.Data.TransactionId));
        }

        #endregion

        #region Spot Symbol client
        EndpointOptions<GetSymbolsRequest> ISpotSymbolRestClient.GetSpotSymbolsOptions { get; } = new EndpointOptions<GetSymbolsRequest>(false);

        async Task<ExchangeWebResult<IEnumerable<SharedSpotSymbol>>> ISpotSymbolRestClient.GetSpotSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotSymbolRestClient)this).GetSpotSymbolsOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedSpotSymbol>>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult)
                return new ExchangeWebResult<IEnumerable<SharedSpotSymbol>>(Exchange, symbolInfoResult.Error!);

            var result = await ExchangeData.GetActiveSymbolsAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<IEnumerable<SharedSpotSymbol>>(Exchange, null, default);

            return result.AsExchangeResult<IEnumerable<SharedSpotSymbol>>(Exchange, TradingMode.Spot, result.Data.Where(x => x.SymbolType == SymbolType.Spot).Select(s => 
            new SharedSpotSymbol(s.BaseAsset, s.QuoteAsset, s.Symbol, s.Status == SymbolStatus.Open)
            {
                MinTradeQuantity = s.LotSize.ToSharedSymbolQuantity(s.Symbol),
                MaxTradeQuantity = s.MaxOrderQuantity.ToSharedSymbolQuantity(s.Symbol),
                QuantityStep = s.LotSize.ToSharedSymbolQuantity(s.Symbol),
                PriceStep = s.PriceStep
            }).ToArray());
        }

        #endregion

        #region Spot Ticker client

        EndpointOptions<GetTickerRequest> ISpotTickerRestClient.GetSpotTickerOptions { get; } = new EndpointOptions<GetTickerRequest>(false);
        async Task<ExchangeWebResult<SharedSpotTicker>> ISpotTickerRestClient.GetSpotTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotTickerRestClient)this).GetSpotTickerOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedSpotTicker>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult)
                return new ExchangeWebResult<SharedSpotTicker>(Exchange, symbolInfoResult.Error!);

            var result = await ExchangeData.GetActiveSymbolsAsync(ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedSpotTicker>(Exchange, null, default);

            var symbol = result.Data.SingleOrDefault(x => x.Symbol == request.Symbol.GetSymbol(FormatSymbol));
            if (symbol == null)
                return result.AsExchangeError<SharedSpotTicker>(Exchange, new ServerError("Not found"));

            return result.AsExchangeResult(Exchange, TradingMode.Spot, new SharedSpotTicker(
                symbol.Symbol,
                symbol.LastPrice,
                symbol.HighPrice,
                symbol.LowPrice,
                symbol.Volume24h.ToSharedSymbolQuantity(symbol.Symbol),
                Math.Round(symbol.PrevPrice24h == 0 ? 0 : symbol.LastPrice / symbol.PrevPrice24h * 100 - 100, 3)
                ));
        }

        EndpointOptions<GetTickersRequest> ISpotTickerRestClient.GetSpotTickersOptions { get; } = new EndpointOptions<GetTickersRequest>(false);
        async Task<ExchangeWebResult<IEnumerable<SharedSpotTicker>>> ISpotTickerRestClient.GetSpotTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotTickerRestClient)this).GetSpotTickersOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedSpotTicker>>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult)
                return new ExchangeWebResult<IEnumerable<SharedSpotTicker>>(Exchange, symbolInfoResult.Error!);

            var result = await ExchangeData.GetActiveSymbolsAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<IEnumerable<SharedSpotTicker>>(Exchange, null, default);

            return result.AsExchangeResult<IEnumerable<SharedSpotTicker>>(Exchange, TradingMode.Spot, result.Data.Where(x => x.SymbolType == SymbolType.Spot).Select(x =>
            new SharedSpotTicker(
                x.Symbol,
                x.LastPrice,
                x.HighPrice,
                x.LowPrice,
                x.Volume24h.ToSharedSymbolQuantity(x.Symbol),
                Math.Round(x.PrevPrice24h == 0 ? 0 : x.LastPrice / x.PrevPrice24h * 100 - 100, 3))).ToArray());
        }

        #endregion

        #region Spot Order Client
        SharedFeeDeductionType ISpotOrderRestClient.SpotFeeDeductionType => SharedFeeDeductionType.AddToCost;
        SharedFeeAssetType ISpotOrderRestClient.SpotFeeAssetType => SharedFeeAssetType.QuoteAsset;
        IEnumerable<SharedOrderType> ISpotOrderRestClient.SpotSupportedOrderTypes { get; } = new[] { SharedOrderType.Limit, SharedOrderType.Market, SharedOrderType.LimitMaker };
        IEnumerable<SharedTimeInForce> ISpotOrderRestClient.SpotSupportedTimeInForce { get; } = new[] { SharedTimeInForce.GoodTillCanceled, SharedTimeInForce.ImmediateOrCancel, SharedTimeInForce.FillOrKill };
        SharedQuantitySupport ISpotOrderRestClient.SpotSupportedOrderQuantity { get; } = new SharedQuantitySupport(
                SharedQuantityType.BaseAsset,
                SharedQuantityType.BaseAsset,
                SharedQuantityType.BaseAsset,
                SharedQuantityType.BaseAsset);

        PlaceSpotOrderOptions ISpotOrderRestClient.PlaceSpotOrderOptions { get; } = new PlaceSpotOrderOptions();
        async Task<ExchangeWebResult<SharedId>> ISpotOrderRestClient.PlaceSpotOrderAsync(PlaceSpotOrderRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).PlaceSpotOrderOptions.ValidateRequest(
                Exchange,
                request,
                request.Symbol.TradingMode,
                SupportedTradingModes,
                ((ISpotOrderRestClient)this).SpotSupportedOrderTypes,
                ((ISpotOrderRestClient)this).SpotSupportedTimeInForce,
                ((ISpotOrderRestClient)this).SpotSupportedOrderQuantity);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult)
                return new ExchangeWebResult<SharedId>(Exchange, symbolInfoResult.Error!);

            var result = await Trading.PlaceOrderAsync(
                request.Symbol.GetSymbol(FormatSymbol),
                request.Side == SharedOrderSide.Buy ? Enums.OrderSide.Buy : Enums.OrderSide.Sell,
                request.OrderType == SharedOrderType.Limit ? Enums.OrderType.Limit : Enums.OrderType.Market,
                quantity: request.Quantity.ToBitMEXAssetQuantity(request.Symbol.BaseAsset),
                price: request.Price,
                timeInForce: GetTimeInForce(request.TimeInForce, request.OrderType),
                executionInstruction: request.OrderType == SharedOrderType.LimitMaker ? ExecutionInstruction.PostOnly : null,
                clientOrderId: request.ClientOrderId,
                ct: ct).ConfigureAwait(false);

            if (!result)
                return result.AsExchangeResult<SharedId>(Exchange, null, default);

            return result.AsExchangeResult(Exchange, TradingMode.Spot, new SharedId(result.Data.OrderId));
        }

        EndpointOptions<GetOrderRequest> ISpotOrderRestClient.GetSpotOrderOptions { get; } = new EndpointOptions<GetOrderRequest>(true);
        async Task<ExchangeWebResult<SharedSpotOrder>> ISpotOrderRestClient.GetSpotOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).GetSpotOrderOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedSpotOrder>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult)
                return new ExchangeWebResult<SharedSpotOrder>(Exchange, symbolInfoResult.Error!);

            var orders = await Trading.GetOrdersAsync(request.Symbol.GetSymbol(FormatSymbol), 
                filter: new Dictionary<string, object>
                {
                    { "orderID", request.OrderId }
                },
                ct: ct).ConfigureAwait(false);
            if (!orders)
                return orders.AsExchangeResult<SharedSpotOrder>(Exchange, null, default);

            var order = orders.Data.SingleOrDefault();
            if (order == null)
                return orders.AsExchangeError<SharedSpotOrder>(Exchange, new ServerError("Not found"));

            return orders.AsExchangeResult(Exchange, TradingMode.Spot, new SharedSpotOrder(
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
                Quantity = order.Quantity.ToSharedSymbolQuantity(order.Symbol),
                QuantityFilled = order.QuantityFilled.ToSharedSymbolQuantity(order.Symbol),
                TimeInForce = ParseTimeInForce(order.TimeInForce),
                UpdateTime = order.TransactTime
            });
        }

        EndpointOptions<GetOpenOrdersRequest> ISpotOrderRestClient.GetOpenSpotOrdersOptions { get; } = new EndpointOptions<GetOpenOrdersRequest>(true);
        async Task<ExchangeWebResult<IEnumerable<SharedSpotOrder>>> ISpotOrderRestClient.GetOpenSpotOrdersAsync(GetOpenOrdersRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).GetOpenSpotOrdersOptions.ValidateRequest(Exchange, request, request.Symbol?.TradingMode ?? request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedSpotOrder>>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult)
                return new ExchangeWebResult<IEnumerable<SharedSpotOrder>>(Exchange, symbolInfoResult.Error!);

            var symbol = request.Symbol?.GetSymbol(FormatSymbol);
            var orders = await Trading.GetOrdersAsync(symbol,
                filter: new Dictionary<string, object>
                {
                    { "open", true }
                },
                ct: ct).ConfigureAwait(false);
            if (!orders)
                return orders.AsExchangeResult<IEnumerable<SharedSpotOrder>>(Exchange, null, default);

            var data = orders.Data.Where(x => BitMEXUtils.GetSymbolType(x.Symbol) == SymbolType.Spot);
            return orders.AsExchangeResult<IEnumerable<SharedSpotOrder>>(Exchange, TradingMode.Spot, data.Select(x => new SharedSpotOrder(
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
                Quantity = x.Quantity.ToSharedSymbolQuantity(x.Symbol),
                QuantityFilled = x.QuantityFilled.ToSharedSymbolQuantity(x.Symbol),
                TimeInForce = ParseTimeInForce(x.TimeInForce),
                UpdateTime = x.TransactTime
            }).ToArray());
        }

        PaginatedEndpointOptions<GetClosedOrdersRequest> ISpotOrderRestClient.GetClosedSpotOrdersOptions { get; } = new PaginatedEndpointOptions<GetClosedOrdersRequest>(SharedPaginationSupport.Ascending, true, 500, true);
        async Task<ExchangeWebResult<IEnumerable<SharedSpotOrder>>> ISpotOrderRestClient.GetClosedSpotOrdersAsync(GetClosedOrdersRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).GetClosedSpotOrdersOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedSpotOrder>>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult)
                return new ExchangeWebResult<IEnumerable<SharedSpotOrder>>(Exchange, symbolInfoResult.Error!);

            // Determine page token
            int offset = 0;
            if (pageToken is OffsetToken offsetToken)
                offset = offsetToken.Offset;

            var limit = request.Limit ?? 500;

            // Get data
            var orders = await Trading.GetOrdersAsync(request.Symbol.GetSymbol(FormatSymbol),
                filter: new Dictionary<string, object>
                {
                    { "ordStatus", new string[] {"Canceled", "Filled" } }
                },
                startTime: request.StartTime,
                endTime: request.EndTime,
                limit: limit,
                offset: offset,
                ct: ct).ConfigureAwait(false);
            if (!orders)
                return orders.AsExchangeResult<IEnumerable<SharedSpotOrder>>(Exchange, null, default);

            // Get next token
            OffsetToken? nextToken = null;
            if (orders.Data.Any())
                nextToken = new OffsetToken(offset + limit);

            return orders.AsExchangeResult<IEnumerable<SharedSpotOrder>>(Exchange, TradingMode.Spot, orders.Data.Select(x => new SharedSpotOrder(
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
                Quantity = x.Quantity.ToSharedSymbolQuantity(x.Symbol),
                QuantityFilled = x.QuantityFilled.ToSharedSymbolQuantity(x.Symbol),
                TimeInForce = ParseTimeInForce(x.TimeInForce),
                UpdateTime = x.TransactTime
            }).ToArray(), nextToken);
        }

        EndpointOptions<GetOrderTradesRequest> ISpotOrderRestClient.GetSpotOrderTradesOptions { get; } = new EndpointOptions<GetOrderTradesRequest>(true);
        async Task<ExchangeWebResult<IEnumerable<SharedUserTrade>>> ISpotOrderRestClient.GetSpotOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).GetSpotOrderTradesOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedUserTrade>>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult)
                return new ExchangeWebResult<IEnumerable<SharedUserTrade>>(Exchange, symbolInfoResult.Error!);

            var trades = await Trading.GetUserTradesAsync(symbol: request.Symbol.GetSymbol(FormatSymbol),
                filter: new Dictionary<string, object>
                {
                    { "orderID", request.OrderId }
                },
                ct: ct).ConfigureAwait(false);
            if (!trades)
                return trades.AsExchangeResult<IEnumerable<SharedUserTrade>>(Exchange, null, default);

            return trades.AsExchangeResult<IEnumerable<SharedUserTrade>>(Exchange, TradingMode.Spot, trades.Data.Select(x => new SharedUserTrade(
                x.Symbol,
                x.OrderId,
                x.TradeId,
                x.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                x.Quantity.ToSharedSymbolQuantity(x.Symbol),
                x.LastTradePrice!.Value,
                x.Timestamp)
            {
                Fee = x.Fee.ToSharedAssetQuantity(x.Currency),
                Role = x.Role == TradeRole.Maker ? SharedRole.Maker : SharedRole.Taker
            }).ToArray());
        }

        PaginatedEndpointOptions<GetUserTradesRequest> ISpotOrderRestClient.GetSpotUserTradesOptions { get; } = new PaginatedEndpointOptions<GetUserTradesRequest>(SharedPaginationSupport.Descending, true, 1000, true);
        async Task<ExchangeWebResult<IEnumerable<SharedUserTrade>>> ISpotOrderRestClient.GetSpotUserTradesAsync(GetUserTradesRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).GetSpotUserTradesOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedUserTrade>>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult)
                return new ExchangeWebResult<IEnumerable<SharedUserTrade>>(Exchange, symbolInfoResult.Error!);

            // Determine page token
            int offset = 0;
            if (pageToken is OffsetToken offsetToken)
                offset = offsetToken.Offset;

            var limit = request.Limit ?? 500;

            // Get data
            var trades = await Trading.GetUserTradesAsync(symbol: request.Symbol.GetSymbol(FormatSymbol),
                startTime: request.StartTime,
                endTime: request.EndTime,
                limit: limit,
                offset: offset,
                ct: ct
                ).ConfigureAwait(false);
            if (!trades)
                return trades.AsExchangeResult<IEnumerable<SharedUserTrade>>(Exchange, null, default);

            // Get next token
            OffsetToken? nextToken = null;
            if (trades.Data.Count() == limit)
                nextToken = new OffsetToken(offset + limit);

            return trades.AsExchangeResult<IEnumerable<SharedUserTrade>>(Exchange, TradingMode.Spot, trades.Data.Select(x => new SharedUserTrade(
                x.Symbol,
                x.OrderId,
                x.TradeId,
                x.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                x.Quantity.ToSharedSymbolQuantity(x.Symbol),
                x.LastTradePrice!.Value,
                x.Timestamp)
            {
                Fee = x.Fee.ToSharedAssetQuantity(x.Currency),
                Role = x.Role == TradeRole.Maker ? SharedRole.Maker : SharedRole.Taker
            }).ToArray(), nextToken);
        }

        EndpointOptions<CancelOrderRequest> ISpotOrderRestClient.CancelSpotOrderOptions { get; } = new EndpointOptions<CancelOrderRequest>(true);
        async Task<ExchangeWebResult<SharedId>> ISpotOrderRestClient.CancelSpotOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).CancelSpotOrderOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

            var order = await Trading.CancelOrderAsync(request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order)
                return order.AsExchangeResult<SharedId>(Exchange, null, default);

            return order.AsExchangeResult(Exchange, TradingMode.Spot, new SharedId(order.Data.OrderId));
        }

        private Enums.TimeInForce? GetTimeInForce(SharedTimeInForce? tif, SharedOrderType type)
        {
            if (tif == SharedTimeInForce.FillOrKill) return TimeInForce.FillOrKill;
            if (tif == SharedTimeInForce.ImmediateOrCancel) return TimeInForce.ImmediateOrCancel;
            if (tif == SharedTimeInForce.GoodTillCanceled) return TimeInForce.GoodTillCancel;

            return null;
        }

        private SharedOrderStatus ParseOrderStatus(OrderStatus status)
        {
            if (status == OrderStatus.New) return SharedOrderStatus.Open;
            if (status == OrderStatus.Canceled || status == OrderStatus.Rejected) return SharedOrderStatus.Canceled;
            return SharedOrderStatus.Filled;
        }

        private SharedOrderType ParseOrderType(OrderType type, ExecutionInstruction? executionInstruction)
        {
            if (type == OrderType.Market) return SharedOrderType.Market;
            if (type == OrderType.Limit && executionInstruction == ExecutionInstruction.PostOnly) return SharedOrderType.LimitMaker;
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

        #region Funding Rate client
        GetFundingRateHistoryOptions IFundingRateRestClient.GetFundingRateHistoryOptions { get; } = new GetFundingRateHistoryOptions(SharedPaginationSupport.Descending, true, 500, false);

        async Task<ExchangeWebResult<IEnumerable<SharedFundingRate>>> IFundingRateRestClient.GetFundingRateHistoryAsync(GetFundingRateHistoryRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((IFundingRateRestClient)this).GetFundingRateHistoryOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedFundingRate>>(Exchange, validationError);

            int offset = 0;
            if (pageToken is OffsetToken token)
                offset = token.Offset;

            var limit = request.Limit ?? 500;

            // Get data
            var result = await ExchangeData.GetFundingHistoryAsync(
                request.Symbol.GetSymbol(FormatSymbol),
                startTime: request.StartTime,
                endTime: request.EndTime,
                limit: limit,
                offset: offset,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<IEnumerable<SharedFundingRate>>(Exchange, null, default);

            OffsetToken? nextToken = null;
            if (result.Data.Count() == limit)
                nextToken = new OffsetToken(offset + limit);

            // Return
            return result.AsExchangeResult<IEnumerable<SharedFundingRate>>(Exchange, request.Symbol.TradingMode, result.Data.Select(x => new SharedFundingRate(x.FundingRate, x.Timestamp)).ToArray(), nextToken);
        }
        #endregion

        #region Futures Symbol client

        EndpointOptions<GetSymbolsRequest> IFuturesSymbolRestClient.GetFuturesSymbolsOptions { get; } = new EndpointOptions<GetSymbolsRequest>(false);
        async Task<ExchangeWebResult<IEnumerable<SharedFuturesSymbol>>> IFuturesSymbolRestClient.GetFuturesSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesSymbolRestClient)this).GetFuturesSymbolsOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedFuturesSymbol>>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult)
                return new ExchangeWebResult<IEnumerable<SharedFuturesSymbol>>(Exchange, symbolInfoResult.Error!);

            var result = await ExchangeData.GetActiveSymbolsAsync(ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<IEnumerable<SharedFuturesSymbol>>(Exchange, null, default);

            // Quanto not supported as it doesn't currently fit in the response models
            var data = result.Data.Where(x => x.SymbolType != SymbolType.Spot && !x.IsQuanto);
            if (request.TradingMode != null) 
            {
                data = data.Where(x => request.TradingMode.Value.IsPerpetual() ? x.SymbolType == SymbolType.PerpetualContract : x.SymbolType == SymbolType.Futures);
                data = data.Where(x => request.TradingMode.Value.IsInverse() ? x.IsInverse : !x.IsInverse);
            }
            
            return result.AsExchangeResult<IEnumerable<SharedFuturesSymbol>>(Exchange, request.TradingMode == null ? SupportedTradingModes : new[] { request.TradingMode.Value }, 
                data.Select(s => new SharedFuturesSymbol(
                    s.SymbolType == SymbolType.PerpetualContract ? 
                            (s.IsInverse ? SharedSymbolType.PerpetualInverse : SharedSymbolType.PerpetualLinear): 
                            (s.IsInverse ? SharedSymbolType.DeliveryInverse : SharedSymbolType.DeliveryLinear),
                    s.BaseAsset,
                    s.QuoteAsset,
                    s.Symbol,
                    s.Status == SymbolStatus.Open)
            {
                MinTradeQuantity = s.LotSize,
                MaxTradeQuantity = s.MaxOrderQuantity,
                QuantityStep = s.LotSize,
                PriceStep = s.PriceStep,
                ContractSize = s.IsInverse ? (s.Multiplier / s.UnderlyingToSettleMultiplier) : (1 / s.UnderlyingToPositionMultiplier),
                DeliveryTime = s.SettleTime
            }).ToArray());
        }

        #endregion

        #region Futures Ticker client

        EndpointOptions<GetTickerRequest> IFuturesTickerRestClient.GetFuturesTickerOptions { get; } = new EndpointOptions<GetTickerRequest>(false);
        async Task<ExchangeWebResult<SharedFuturesTicker>> IFuturesTickerRestClient.GetFuturesTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesTickerRestClient)this).GetFuturesTickerOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedFuturesTicker>(Exchange, validationError);

            var resultTicker = await ExchangeData.GetSymbolsAsync(request.Symbol.GetSymbol(FormatSymbol), ct: ct).ConfigureAwait(false);
            if (!resultTicker)
                return resultTicker.AsExchangeResult<SharedFuturesTicker>(Exchange, null, default);

            var symbol = resultTicker.Data.SingleOrDefault();
            if (symbol == null)
                return resultTicker.AsExchangeError<SharedFuturesTicker>(Exchange, new ServerError("Not found"));

            return resultTicker.AsExchangeResult(Exchange, request.Symbol.TradingMode, 
                new SharedFuturesTicker(symbol.Symbol, symbol.LastPrice, symbol.HighPrice, symbol.LowPrice, symbol.Volume, symbol.LastChangePercentage)
            {
                MarkPrice = symbol.MarkPrice,
                FundingRate = symbol.FundingRate,
                NextFundingTime = symbol.FundingTimestamp
            });
        }

        EndpointOptions<GetTickersRequest> IFuturesTickerRestClient.GetFuturesTickersOptions { get; } = new EndpointOptions<GetTickersRequest>(false);
        async Task<ExchangeWebResult<IEnumerable<SharedFuturesTicker>>> IFuturesTickerRestClient.GetFuturesTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesTickerRestClient)this).GetFuturesTickersOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedFuturesTicker>>(Exchange, validationError);

            var resultTickers = await ExchangeData.GetActiveSymbolsAsync(ct: ct).ConfigureAwait(false);
            if (!resultTickers)
                return resultTickers.AsExchangeResult<IEnumerable<SharedFuturesTicker>>(Exchange, null, default);

            var data = resultTickers.Data.Where(x => x.SymbolType != SymbolType.Spot);
            if (request.TradingMode != null)
            {
                data = data.Where(x => request.TradingMode.Value.IsPerpetual() ? x.SymbolType == SymbolType.PerpetualContract : x.SymbolType == SymbolType.Futures);
                data = data.Where(x => request.TradingMode.Value.IsInverse() ? x.IsInverse : !x.IsInverse);
            }

            return resultTickers.AsExchangeResult<IEnumerable<SharedFuturesTicker>>(Exchange, request.TradingMode == null ? SupportedTradingModes : new[] { request.TradingMode.Value }, data.Select(x =>
            {
                var markPrice = resultTickers.Data.Single(p => p.Symbol == x.Symbol);
                return new SharedFuturesTicker(x.Symbol, x.LastPrice, x.HighPrice, x.LowPrice, x.Volume, x.LastChangePercentage)
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

        EndpointOptions<GetLeverageRequest> ILeverageRestClient.GetLeverageOptions { get; } = new EndpointOptions<GetLeverageRequest>(true);
        async Task<ExchangeWebResult<SharedLeverage>> ILeverageRestClient.GetLeverageAsync(GetLeverageRequest request, CancellationToken ct)
        {
            var validationError = ((ILeverageRestClient)this).GetLeverageOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedLeverage>(Exchange, validationError);

            var result = await Trading.GetPositionsAsync(filter: new Dictionary<string, object>{
                { "symbol", request.Symbol.GetSymbol(FormatSymbol) }
            },
            columns: new[] { "leverage" },
            ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedLeverage>(Exchange, null, default);

            if (!result.Data.Any())
                return result.AsExchangeError<SharedLeverage>(Exchange, new ServerError("Position not found, no leverage set yet"));

            var position = result.Data.First();
            return result.AsExchangeResult(Exchange, request.Symbol.TradingMode, new SharedLeverage(position.Leverage)
            {
                MarginMode = position.CrossMargin ? SharedMarginMode.Cross : SharedMarginMode.Isolated
            });
        }

        SetLeverageOptions ILeverageRestClient.SetLeverageOptions { get; } = new SetLeverageOptions()
        {
            RequiredOptionalParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(SetLeverageRequest.MarginMode), typeof(SharedMarginMode), "Margin mode to change leverage for", SharedMarginMode.Isolated)
            }
        };
        async Task<ExchangeWebResult<SharedLeverage>> ILeverageRestClient.SetLeverageAsync(SetLeverageRequest request, CancellationToken ct)
        {
            var validationError = ((ILeverageRestClient)this).SetLeverageOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedLeverage>(Exchange, validationError);

            if (request.MarginMode == SharedMarginMode.Isolated)
            {
                var result = await Trading.SetIsolatedMarginLeverageAsync(symbol: request.Symbol.GetSymbol(FormatSymbol), request.Leverage, ct: ct).ConfigureAwait(false);
                if (!result)
                    return result.AsExchangeResult<SharedLeverage>(Exchange, null, default);

                return result.AsExchangeResult(Exchange, request.Symbol.TradingMode, new SharedLeverage(result.Data.Leverage) { MarginMode = request.MarginMode });
            }
            else
            {
                var result = await Trading.SetCrossMarginLeverageAsync(symbol: request.Symbol.GetSymbol(FormatSymbol), request.Leverage, ct: ct).ConfigureAwait(false);
                if (!result)
                    return result.AsExchangeResult<SharedLeverage>(Exchange, null, default);

                return result.AsExchangeResult(Exchange, request.Symbol.TradingMode, new SharedLeverage(result.Data.Leverage) { MarginMode = request.MarginMode });
            }

        }
        #endregion

        #region Open Interest client

        EndpointOptions<GetOpenInterestRequest> IOpenInterestRestClient.GetOpenInterestOptions { get; } = new EndpointOptions<GetOpenInterestRequest>(true);
        async Task<ExchangeWebResult<SharedOpenInterest>> IOpenInterestRestClient.GetOpenInterestAsync(GetOpenInterestRequest request, CancellationToken ct)
        {
            var validationError = ((IOpenInterestRestClient)this).GetOpenInterestOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedOpenInterest>(Exchange, validationError);

            var result = await ExchangeData.GetSymbolsAsync(filter: new Dictionary<string, object>{
                { "symbol", request.Symbol.GetSymbol(FormatSymbol) }
            }, columns: ["openInterest"], ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedOpenInterest>(Exchange, null, default);

            var symbol = result.Data.SingleOrDefault();
            if (symbol == null)
                return result.AsExchangeError<SharedOpenInterest>(Exchange, new ServerError("Symbol not found"));

            return result.AsExchangeResult(Exchange, request.Symbol.TradingMode, new SharedOpenInterest(symbol.OpenInterest ?? 0));
        }

        #endregion

        #region Futures Order Client

        SharedFeeDeductionType IFuturesOrderRestClient.FuturesFeeDeductionType => SharedFeeDeductionType.AddToCost;
        SharedFeeAssetType IFuturesOrderRestClient.FuturesFeeAssetType => SharedFeeAssetType.QuoteAsset;

        IEnumerable<SharedOrderType> IFuturesOrderRestClient.FuturesSupportedOrderTypes { get; } = new[] { SharedOrderType.Limit, SharedOrderType.Market };
        IEnumerable<SharedTimeInForce> IFuturesOrderRestClient.FuturesSupportedTimeInForce { get; } = new[] { SharedTimeInForce.GoodTillCanceled, SharedTimeInForce.ImmediateOrCancel, SharedTimeInForce.FillOrKill };
        SharedQuantitySupport IFuturesOrderRestClient.FuturesSupportedOrderQuantity { get; } = new SharedQuantitySupport(
                SharedQuantityType.Contracts,
                SharedQuantityType.Contracts,
                SharedQuantityType.Contracts,
                SharedQuantityType.Contracts);

        PlaceFuturesOrderOptions IFuturesOrderRestClient.PlaceFuturesOrderOptions { get; } = new PlaceFuturesOrderOptions();
        async Task<ExchangeWebResult<SharedId>> IFuturesOrderRestClient.PlaceFuturesOrderAsync(PlaceFuturesOrderRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).PlaceFuturesOrderOptions.ValidateRequest(
                Exchange,
                request,
                request.Symbol.TradingMode,
                SupportedTradingModes,
                ((IFuturesOrderRestClient)this).FuturesSupportedOrderTypes,
                ((IFuturesOrderRestClient)this).FuturesSupportedTimeInForce,
                ((IFuturesOrderRestClient)this).FuturesSupportedOrderQuantity);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

            var result = await Trading.PlaceOrderAsync(
                request.Symbol.GetSymbol(FormatSymbol),
                request.Side == SharedOrderSide.Buy ? Enums.OrderSide.Buy : Enums.OrderSide.Sell,
                request.OrderType == SharedOrderType.Limit ? Enums.OrderType.Limit : Enums.OrderType.Market,
                quantity: (long?)request.Quantity,
                price: request.Price,
                timeInForce: GetTimeInForce(request.TimeInForce, request.OrderType),
                executionInstruction: request.OrderType == SharedOrderType.LimitMaker ? ExecutionInstruction.PostOnly : request.ReduceOnly == true ? ExecutionInstruction.ReduceOnly: null,
                clientOrderId: request.ClientOrderId,
                ct: ct).ConfigureAwait(false);

            if (!result)
                return result.AsExchangeResult<SharedId>(Exchange, null, default);

            return result.AsExchangeResult(Exchange, request.Symbol.TradingMode, new SharedId(result.Data.OrderId));
        }

        EndpointOptions<GetOrderRequest> IFuturesOrderRestClient.GetFuturesOrderOptions { get; } = new EndpointOptions<GetOrderRequest>(true);
        async Task<ExchangeWebResult<SharedFuturesOrder>> IFuturesOrderRestClient.GetFuturesOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).GetFuturesOrderOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedFuturesOrder>(Exchange, validationError);

            var orders = await Trading.GetOrdersAsync(request.Symbol.GetSymbol(FormatSymbol),
                filter: new Dictionary<string, object>
                {
                    { "orderID", request.OrderId }
                },
                ct: ct).ConfigureAwait(false);
            if (!orders)
                return orders.AsExchangeResult<SharedFuturesOrder>(Exchange, null, default);

            var order = orders.Data.SingleOrDefault();
            if (order == null)
                return orders.AsExchangeError<SharedFuturesOrder>(Exchange, new ServerError("Not found"));

            return orders.AsExchangeResult(Exchange, TradingMode.Spot, new SharedFuturesOrder(
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
                Quantity = order.Quantity,
                QuantityFilled = order.QuantityFilled,
                TimeInForce = ParseTimeInForce(order.TimeInForce),
                UpdateTime = order.TransactTime,
                ReduceOnly = order.ExecutionInstruction == ExecutionInstruction.ReduceOnly
            });
        }

        EndpointOptions<GetOpenOrdersRequest> IFuturesOrderRestClient.GetOpenFuturesOrdersOptions { get; } = new EndpointOptions<GetOpenOrdersRequest>(true);
        async Task<ExchangeWebResult<IEnumerable<SharedFuturesOrder>>> IFuturesOrderRestClient.GetOpenFuturesOrdersAsync(GetOpenOrdersRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).GetOpenFuturesOrdersOptions.ValidateRequest(Exchange, request, request.Symbol?.TradingMode ?? request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedFuturesOrder>>(Exchange, validationError);

            var symbol = request.Symbol?.GetSymbol(FormatSymbol);
            var orders = await Trading.GetOrdersAsync(symbol,
                filter: new Dictionary<string, object>
                {
                    { "ordStatus", new [] { "New", "PartiallyFilled" } }
                },
                ct: ct).ConfigureAwait(false);
            if (!orders)
                return orders.AsExchangeResult<IEnumerable<SharedFuturesOrder>>(Exchange, null, default);

            var data = orders.Data.Where(x => BitMEXUtils.GetSymbolType(x.Symbol) != SymbolType.Spot);
            return orders.AsExchangeResult<IEnumerable<SharedFuturesOrder>>(Exchange, TradingMode.Spot, data.Select(x => new SharedFuturesOrder(
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
                Quantity = x.Quantity,
                QuantityFilled = x.QuantityFilled,
                TimeInForce = ParseTimeInForce(x.TimeInForce),
                UpdateTime = x.TransactTime,
                ReduceOnly = x.ExecutionInstruction == ExecutionInstruction.ReduceOnly
            }).ToArray());
        }

        PaginatedEndpointOptions<GetClosedOrdersRequest> IFuturesOrderRestClient.GetClosedFuturesOrdersOptions { get; } = new PaginatedEndpointOptions<GetClosedOrdersRequest>(SharedPaginationSupport.Descending, true, 1000, true);
        async Task<ExchangeWebResult<IEnumerable<SharedFuturesOrder>>> IFuturesOrderRestClient.GetClosedFuturesOrdersAsync(GetClosedOrdersRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).GetClosedFuturesOrdersOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedFuturesOrder>>(Exchange, validationError);

            // Determine page token
            int offset = 0;
            if (pageToken is OffsetToken offsetToken)
                offset = offsetToken.Offset;

            var limit = request.Limit ?? 500;

            // Get data
            var orders = await Trading.GetOrdersAsync(request.Symbol.GetSymbol(FormatSymbol),
                filter: new Dictionary<string, object>
                {
                    { "ordStatus", new string[] {"Canceled", "Filled" } }
                },
                startTime: request.StartTime,
                endTime: request.EndTime,
                limit: limit,
                offset: offset,
                ct: ct).ConfigureAwait(false);
            if (!orders)
                return orders.AsExchangeResult<IEnumerable<SharedFuturesOrder>>(Exchange, null, default);

            // Get next token
            OffsetToken? nextToken = null;
            if (orders.Data.Any())
                nextToken = new OffsetToken(offset + limit);

            return orders.AsExchangeResult<IEnumerable<SharedFuturesOrder>>(Exchange, TradingMode.Spot, orders.Data.Select(x => new SharedFuturesOrder(
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
                Quantity = x.Quantity,
                QuantityFilled = x.QuantityFilled,
                TimeInForce = ParseTimeInForce(x.TimeInForce),
                UpdateTime = x.TransactTime,
                ReduceOnly = x.ExecutionInstruction == ExecutionInstruction.ReduceOnly
            }).ToArray(), nextToken);
        }

        EndpointOptions<GetOrderTradesRequest> IFuturesOrderRestClient.GetFuturesOrderTradesOptions { get; } = new EndpointOptions<GetOrderTradesRequest>(true);
        async Task<ExchangeWebResult<IEnumerable<SharedUserTrade>>> IFuturesOrderRestClient.GetFuturesOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).GetFuturesOrderTradesOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedUserTrade>>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult)
                return new ExchangeWebResult<IEnumerable<SharedUserTrade>>(Exchange, symbolInfoResult.Error!);

            var trades = await Trading.GetUserTradesAsync(symbol: request.Symbol.GetSymbol(FormatSymbol),
                filter: new Dictionary<string, object>
                {
                    { "orderID", request.OrderId }
                },
                ct: ct).ConfigureAwait(false);
            if (!trades)
                return trades.AsExchangeResult<IEnumerable<SharedUserTrade>>(Exchange, null, default);

            return trades.AsExchangeResult<IEnumerable<SharedUserTrade>>(Exchange, TradingMode.Spot, trades.Data.Select(x => new SharedUserTrade(
                x.Symbol,
                x.OrderId,
                x.TradeId,
                x.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                x.Quantity,
                x.LastTradePrice!.Value,
                x.Timestamp)
            {
                Fee = x.Fee.ToSharedAssetQuantity(x.SettlementCurrency ?? x.Currency),
                Role = x.Role == TradeRole.Maker ? SharedRole.Maker : SharedRole.Taker
            }).ToArray());
        }

        PaginatedEndpointOptions<GetUserTradesRequest> IFuturesOrderRestClient.GetFuturesUserTradesOptions { get; } = new PaginatedEndpointOptions<GetUserTradesRequest>(SharedPaginationSupport.Descending, true, 1000, true);
        async Task<ExchangeWebResult<IEnumerable<SharedUserTrade>>> IFuturesOrderRestClient.GetFuturesUserTradesAsync(GetUserTradesRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).GetFuturesUserTradesOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedUserTrade>>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult)
                return new ExchangeWebResult<IEnumerable<SharedUserTrade>>(Exchange, symbolInfoResult.Error!);

            // Determine page token
            long? fromId = null;
            if (pageToken is FromIdToken fromIdToken)
                fromId = long.Parse(fromIdToken.FromToken);

            // Determine page token
            int offset = 0;
            if (pageToken is OffsetToken offsetToken)
                offset = offsetToken.Offset;

            var limit = request.Limit ?? 500;

            // Get data
            var trades = await Trading.GetUserTradesAsync(symbol: request.Symbol.GetSymbol(FormatSymbol),
                startTime: request.StartTime,
                endTime: request.EndTime,
                limit: limit,
                offset: offset,
                ct: ct
                ).ConfigureAwait(false);
            if (!trades)
                return trades.AsExchangeResult<IEnumerable<SharedUserTrade>>(Exchange, null, default);

            // Get next token
            OffsetToken? nextToken = null;
            if (trades.Data.Count() == limit)
                nextToken = new OffsetToken(offset + limit);

            return trades.AsExchangeResult<IEnumerable<SharedUserTrade>>(Exchange, TradingMode.Spot, trades.Data.Select(x => new SharedUserTrade(
                x.Symbol,
                x.OrderId,
                x.TradeId,
                x.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                x.Quantity,
                x.LastTradePrice!.Value,
                x.Timestamp)
            {
                Fee = x.Fee.ToSharedAssetQuantity(x.SettlementCurrency ?? x.Currency),
                Role = x.Role == TradeRole.Maker ? SharedRole.Maker : SharedRole.Taker
            }).ToArray(), nextToken);
        }

        EndpointOptions<CancelOrderRequest> IFuturesOrderRestClient.CancelFuturesOrderOptions { get; } = new EndpointOptions<CancelOrderRequest>(true);
        async Task<ExchangeWebResult<SharedId>> IFuturesOrderRestClient.CancelFuturesOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).CancelFuturesOrderOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

            var order = await Trading.CancelOrderAsync(request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order)
                return order.AsExchangeResult<SharedId>(Exchange, null, default);

            return order.AsExchangeResult(Exchange, request.Symbol.TradingMode, new SharedId(request.OrderId));
        }

        EndpointOptions<GetPositionsRequest> IFuturesOrderRestClient.GetPositionsOptions { get; } = new EndpointOptions<GetPositionsRequest>(true);
        async Task<ExchangeWebResult<IEnumerable<SharedPosition>>> IFuturesOrderRestClient.GetPositionsAsync(GetPositionsRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).GetPositionsOptions.ValidateRequest(Exchange, request, request.Symbol?.TradingMode ?? request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedPosition>>(Exchange, validationError);

            var result = await Trading.GetPositionsAsync(filter: request.Symbol == null ? null : new Dictionary<string, object>{
                { "symbol", request.Symbol.GetSymbol(FormatSymbol) }
            }, ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<IEnumerable<SharedPosition>>(Exchange, null, default);

            var data = result.Data;
            if (request.TradingMode != null)
                data = data.Where(x => request.TradingMode.Value.IsPerpetual() ? BitMEXUtils.GetSymbolType(x.Symbol) == SymbolType.PerpetualContract : BitMEXUtils.GetSymbolType(x.Symbol) == SymbolType.Futures);
            
            var resultTypes = request.Symbol == null && request.TradingMode == null ? SupportedTradingModes : request.Symbol != null ? new[] { request.Symbol.TradingMode } : new[] { request.TradingMode!.Value };
            return result.AsExchangeResult<IEnumerable<SharedPosition>>(Exchange, resultTypes, data.Where(x => x.Currency != null).Select(x => new SharedPosition(x.Symbol, Math.Abs(x.CurrentQuantity ?? 0), x.Timestamp)
            {
                UnrealizedPnl = x.UnrealizedPnl.ToSharedAssetQuantity(x.Currency!),
                LiquidationPrice = x.LiquidationPrice == 0 ? null : x.LiquidationPrice,
                Leverage = x.Leverage,
                AverageOpenPrice = x.AverageEntryPrice,
                PositionSide = x.CurrentQuantity < 0 ? SharedPositionSide.Short : SharedPositionSide.Long
            }).ToArray());
        }

        EndpointOptions<ClosePositionRequest> IFuturesOrderRestClient.ClosePositionOptions { get; } = new EndpointOptions<ClosePositionRequest>(true)
        {
            RequiredOptionalParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(ClosePositionRequest.PositionSide), typeof(SharedPositionSide), "The position side to close", SharedPositionSide.Long),
            }
        };
        async Task<ExchangeWebResult<SharedId>> IFuturesOrderRestClient.ClosePositionAsync(ClosePositionRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).ClosePositionOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

            var symbol = request.Symbol.GetSymbol(FormatSymbol); 
            var result = await Trading.PlaceOrderAsync(
                symbol,
                request.PositionSide == SharedPositionSide.Long ? OrderSide.Sell : OrderSide.Buy,
                OrderType.Market,
                executionInstruction: ExecutionInstruction.Close,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedId>(Exchange, null, default);

            return result.AsExchangeResult(Exchange, request.Symbol.TradingMode, new SharedId(result.Data.OrderId));
        }

        #endregion
    }
}
