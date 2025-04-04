using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Text;
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
    internal partial class BitMEXSocketClientExchangeApi : IBitMEXSocketClientExchangeApiShared
    {
        private const string _topicSpotId = "BitMEXSpot";
        private const string _topicFuturesId = "BitMEXFutures";

        public string Exchange => "BitMEX";

        public TradingMode[] SupportedTradingModes => new[] { TradingMode.Spot, TradingMode.PerpetualLinear, TradingMode.DeliveryLinear, TradingMode.PerpetualInverse, TradingMode.DeliveryInverse };

        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();

        #region Spot Order client

        EndpointOptions<SubscribeSpotOrderRequest> ISpotOrderSocketClient.SubscribeSpotOrderOptions { get; } = new EndpointOptions<SubscribeSpotOrderRequest>(false);
        async Task<ExchangeResult<UpdateSubscription>> ISpotOrderSocketClient.SubscribeToSpotOrderUpdatesAsync(SubscribeSpotOrderRequest request, Action<ExchangeEvent<SharedSpotOrder[]>> handler, CancellationToken ct)
        {
            var validationError = ((ISpotOrderSocketClient)this).SubscribeSpotOrderOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeResult<UpdateSubscription>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult)
                return new ExchangeResult<UpdateSubscription>(Exchange, symbolInfoResult.Error!);

            var result = await SubscribeToOrderUpdatesAsync(
                update => {
                    var data = update.Data.Where(x => BitMEXUtils.GetSymbolType(x.Symbol) == SymbolType.Spot).ToList();
                    if (!data.Any())
                        return;

                    handler(update.AsExchangeEvent<SharedSpotOrder[]>(Exchange, data.Select(x =>
                    
                        new SharedSpotOrder(
                            ExchangeSymbolCache.ParseSymbol(_topicSpotId, x.Symbol),
                            x.Symbol,
                            x.OrderId,
                            ParseOrderType(x.OrderType, x.ExecutionInstruction),
                            x.OrderSide == Enums.OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                            ParseOrderStatus(x.Status),
                            x.Timestamp)
                        {
                            ClientOrderId = x.ClientOrderId,
                            OrderPrice = x.Price,
                            OrderQuantity = new SharedOrderQuantity(x.Quantity.ToSharedSymbolQuantity(x.Symbol)),
                            QuantityFilled = new SharedOrderQuantity(x.QuantityFilled.ToSharedSymbolQuantity(x.Symbol)),
                            UpdateTime = x.TransactTime,
                            TimeInForce = ParseTimeInForce(x.TimeInForce),
                            AveragePrice = x.AveragePrice,
                            TriggerPrice = x.StopPrice,
                            IsTriggerOrder = x.StopPrice != null
                        }
                    ).ToArray()));
                },
                ct: ct).ConfigureAwait(false);

            return new ExchangeResult<UpdateSubscription>(Exchange, result);
        }

        private SharedTimeInForce? ParseTimeInForce(TimeInForce timeInForce)
        {
            if (timeInForce == TimeInForce.GoodTillCancel) return SharedTimeInForce.GoodTillCanceled;
            if (timeInForce == TimeInForce.ImmediateOrCancel) return SharedTimeInForce.ImmediateOrCancel;
            if (timeInForce == TimeInForce.FillOrKill) return SharedTimeInForce.FillOrKill;

            return null;
        }

        private SharedOrderStatus ParseOrderStatus(OrderStatus status)
        {
            if (status == OrderStatus.New || status == OrderStatus.PartiallyFilled) return SharedOrderStatus.Open;
            if (status == OrderStatus.Rejected || status == OrderStatus.Canceled) return SharedOrderStatus.Canceled;
            return SharedOrderStatus.Filled;
        }

        private SharedOrderType ParseOrderType(OrderType orderType, ExecutionInstruction[]? executionInstruction)
        {
            if (orderType == OrderType.Market || orderType == OrderType.MarketIfTouched || orderType == OrderType.StopMarket) return SharedOrderType.Market;
            if (orderType == OrderType.Limit && executionInstruction?.Contains(ExecutionInstruction.PostOnly) == true) return SharedOrderType.LimitMaker;
            if (orderType == OrderType.Limit || orderType == OrderType.LimitIfTouched || orderType == OrderType.StopLimit) return SharedOrderType.Limit;
            return SharedOrderType.Other;
        }
        #endregion

        #region Balance client
        EndpointOptions<SubscribeBalancesRequest> IBalanceSocketClient.SubscribeBalanceOptions { get; } = new EndpointOptions<SubscribeBalancesRequest>(false);
        async Task<ExchangeResult<UpdateSubscription>> IBalanceSocketClient.SubscribeToBalanceUpdatesAsync(SubscribeBalancesRequest request, Action<ExchangeEvent<SharedBalance[]>> handler, CancellationToken ct)
        {
            var validationError = ((IBalanceSocketClient)this).SubscribeBalanceOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeResult<UpdateSubscription>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult)
                return new ExchangeResult<UpdateSubscription>(Exchange, symbolInfoResult.Error!);

            var result = await SubscribeToBalanceUpdatesAsync(
                update => handler(update.AsExchangeEvent<SharedBalance[]>(Exchange, update.Data.Select(x => 
                new SharedBalance(
                    BitMEXUtils.GetAssetFromCurrency(x.Currency), 
                    x.Quantity.ToSharedAssetQuantity(x.Currency),
                    (x.Quantity + x.PendingCredit).ToSharedAssetQuantity(x.Currency))).ToArray())),
                ct: ct).ConfigureAwait(false);

            return new ExchangeResult<UpdateSubscription>(Exchange, result);
        }

        #endregion

        #region User Trade client

        EndpointOptions<SubscribeUserTradeRequest> IUserTradeSocketClient.SubscribeUserTradeOptions { get; } = new EndpointOptions<SubscribeUserTradeRequest>(false);
        async Task<ExchangeResult<UpdateSubscription>> IUserTradeSocketClient.SubscribeToUserTradeUpdatesAsync(SubscribeUserTradeRequest request, Action<ExchangeEvent<SharedUserTrade[]>> handler, CancellationToken ct)
        {
            var validationError = ((IUserTradeSocketClient)this).SubscribeUserTradeOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeResult<UpdateSubscription>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult)
                return new ExchangeResult<UpdateSubscription>(Exchange, symbolInfoResult.Error!);

            var result = await SubscribeToUserTradeUpdatesAsync(
                update =>
                {
                    var data = update.Data
                    .Where(x => x.ExecutionType == ExecutionType.Trade)
                    .ToList();
                    if (!data.Any())
                        return;

                    handler(update.AsExchangeEvent<SharedUserTrade[]>(Exchange, data.Select(x =>
                        new SharedUserTrade(
                            ExchangeSymbolCache.ParseSymbol(_topicSpotId, x.Symbol) ?? ExchangeSymbolCache.ParseSymbol(_topicFuturesId, x.Symbol),
                            x.Symbol,
                            x.OrderId,
                            x.TradeId,
                            x.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                            x.Quantity?.ToSharedSymbolQuantity(x.Symbol) ?? 0,
                            x.LastTradePrice!.Value,
                            x.Timestamp)
                        {
                            Fee = x.Fee.ToSharedAssetQuantity(x.Currency),
                            Role = x.Role == TradeRole.Maker ? SharedRole.Maker : SharedRole.Taker
                        }
                    ).ToArray()));
                },
                ct: ct).ConfigureAwait(false);

            return new ExchangeResult<UpdateSubscription>(Exchange, result);
        }
        #endregion

        #region Book Ticker client

        EndpointOptions<SubscribeBookTickerRequest> IBookTickerSocketClient.SubscribeBookTickerOptions { get; } = new EndpointOptions<SubscribeBookTickerRequest>(false);
        async Task<ExchangeResult<UpdateSubscription>> IBookTickerSocketClient.SubscribeToBookTickerUpdatesAsync(SubscribeBookTickerRequest request, Action<ExchangeEvent<SharedBookTicker>> handler, CancellationToken ct)
        {
            var validationError = ((IBookTickerSocketClient)this).SubscribeBookTickerOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeResult<UpdateSubscription>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult)
                return new ExchangeResult<UpdateSubscription>(Exchange, symbolInfoResult.Error!);

            var symbol = request.Symbol.GetSymbol(FormatSymbol);
            var result = await SubscribeToBookTickerUpdatesAsync(symbol, update => handler(update.AsExchangeEvent(Exchange,
                new SharedBookTicker(
                    ExchangeSymbolCache.ParseSymbol(request.Symbol.TradingMode == TradingMode.Spot ? _topicSpotId : _topicFuturesId, update.Data.Symbol),
                    update.Data.Symbol,
                    update.Data.BestAskPrice,
                    request.Symbol.TradingMode != TradingMode.Spot ? update.Data.BestAskQuantity : update.Data.BestAskQuantity.ToSharedSymbolQuantity(symbol),
                    update.Data.BestBidPrice,
                    request.Symbol.TradingMode != TradingMode.Spot ? update.Data.BestBidQuantity : update.Data.BestBidQuantity.ToSharedSymbolQuantity(symbol)
                    ))), ct).ConfigureAwait(false);

            return new ExchangeResult<UpdateSubscription>(Exchange, result);
        }

        #endregion

        #region Order Book client
        SubscribeOrderBookOptions IOrderBookSocketClient.SubscribeOrderBookOptions { get; } = new SubscribeOrderBookOptions(false, new[] { 10 });
        async Task<ExchangeResult<UpdateSubscription>> IOrderBookSocketClient.SubscribeToOrderBookUpdatesAsync(SubscribeOrderBookRequest request, Action<ExchangeEvent<SharedOrderBook>> handler, CancellationToken ct)
        {
            var validationError = ((IOrderBookSocketClient)this).SubscribeOrderBookOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeResult<UpdateSubscription>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult)
                return new ExchangeResult<UpdateSubscription>(Exchange, symbolInfoResult.Error!);

            var symbol = request.Symbol.GetSymbol(FormatSymbol);
            var result = await SubscribeToOrderBookUpdatesAsync(symbol, update =>
            {
                var book = new SharedOrderBook(update.Data.Asks, update.Data.Bids);
                if (request.Symbol.TradingMode == TradingMode.Spot)
                {
                    foreach (var item in book.Asks)
                        item.Quantity = ((long)item.Quantity).ToSharedSymbolQuantity(symbol);
                    foreach (var item in book.Bids)
                        item.Quantity = ((long)item.Quantity).ToSharedSymbolQuantity(symbol);
                }

                handler(update.AsExchangeEvent(Exchange, book));
            }, ct).ConfigureAwait(false);

            return new ExchangeResult<UpdateSubscription>(Exchange, result);
        }
        #endregion

        #region Ticker client
        EndpointOptions<SubscribeTickerRequest> ITickerSocketClient.SubscribeTickerOptions { get; } = new EndpointOptions<SubscribeTickerRequest>(false);
        async Task<ExchangeResult<UpdateSubscription>> ITickerSocketClient.SubscribeToTickerUpdatesAsync(SubscribeTickerRequest request, Action<ExchangeEvent<SharedSpotTicker>> handler, CancellationToken ct)
        {
            var validationError = ((ITickerSocketClient)this).SubscribeTickerOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeResult<UpdateSubscription>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult)
                return new ExchangeResult<UpdateSubscription>(Exchange, symbolInfoResult.Error!);

            SharedSpotTicker? ticker = null;
            var symbol = request.Symbol.GetSymbol(FormatSymbol);
            var result = await SubscribeToSymbolUpdatesAsync(symbol, update =>
            {
                if (ticker == null)
                {
                    ticker = new SharedSpotTicker(
                        ExchangeSymbolCache.ParseSymbol(request.Symbol.TradingMode == TradingMode.Spot ? _topicSpotId : _topicFuturesId, update.Data.Symbol),
                        update.Data.Symbol,
                        update.Data.LastPrice,
                        update.Data.HighPrice,
                        update.Data.LowPrice,
                        request.Symbol.TradingMode != TradingMode.Spot ? update.Data.Volume24h ?? 0 : (update.Data.Volume24h ?? 0).ToSharedSymbolQuantity(symbol),
                        update.Data.LastChangePcnt * 100
                        );
                }
                else
                {
                    ticker.LastPrice = update.Data.LastPrice ?? ticker.LastPrice;
                    ticker.HighPrice = update.Data.HighPrice ?? ticker.HighPrice;
                    ticker.LowPrice = update.Data.LowPrice ?? ticker.LowPrice;
                    ticker.Volume = update.Data.Volume24h == null ? ticker.Volume : request.Symbol.TradingMode != TradingMode.Spot ? update.Data.Volume24h ?? 0 : (update.Data.Volume24h ?? 0).ToSharedSymbolQuantity(symbol);
                    ticker.ChangePercentage = update.Data.LastChangePcnt == null ? ticker.ChangePercentage : update.Data.LastChangePcnt * 100;
                }

                handler(update.AsExchangeEvent(Exchange, ticker));
            }, ct).ConfigureAwait(false);

            return new ExchangeResult<UpdateSubscription>(Exchange, result);
        }
        #endregion

        #region Trade client

        EndpointOptions<SubscribeTradeRequest> ITradeSocketClient.SubscribeTradeOptions { get; } = new EndpointOptions<SubscribeTradeRequest>(false);
        async Task<ExchangeResult<UpdateSubscription>> ITradeSocketClient.SubscribeToTradeUpdatesAsync(SubscribeTradeRequest request, Action<ExchangeEvent<SharedTrade[]>> handler, CancellationToken ct)
        {
            var validationError = ((ITradeSocketClient)this).SubscribeTradeOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeResult<UpdateSubscription>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult)
                return new ExchangeResult<UpdateSubscription>(Exchange, symbolInfoResult.Error!);

            var symbol = request.Symbol.GetSymbol(FormatSymbol);
            var result = await SubscribeToTradeUpdatesAsync(symbol, update =>
            {
                if (update.UpdateType == SocketUpdateType.Snapshot)
                    return;

                handler(update.AsExchangeEvent<SharedTrade[]>(Exchange, update.Data.Select(x =>
                    new SharedTrade(
                        x.Quantity.ToSharedSymbolQuantity(x.Symbol),
                        x.Price,
                        x.Timestamp)
                    {
                        Side = x.Side == OrderSide.Sell ? SharedOrderSide.Sell : SharedOrderSide.Buy,
                    }
                    ).ToArray()));
                }, ct).ConfigureAwait(false);

            return new ExchangeResult<UpdateSubscription>(Exchange, result);
        }

        #endregion

        #region Futures Order client

        EndpointOptions<SubscribeFuturesOrderRequest> IFuturesOrderSocketClient.SubscribeFuturesOrderOptions { get; } = new EndpointOptions<SubscribeFuturesOrderRequest>(false);
        async Task<ExchangeResult<UpdateSubscription>> IFuturesOrderSocketClient.SubscribeToFuturesOrderUpdatesAsync(SubscribeFuturesOrderRequest request, Action<ExchangeEvent<SharedFuturesOrder[]>> handler, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderSocketClient)this).SubscribeFuturesOrderOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeResult<UpdateSubscription>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult)
                return new ExchangeResult<UpdateSubscription>(Exchange, symbolInfoResult.Error!);

            var result = await SubscribeToOrderUpdatesAsync(
                update => {
                    var data = update.Data.Where(x => BitMEXUtils.GetSymbolType(x.Symbol) != SymbolType.Spot).ToList();
                    if (!data.Any())
                        return;

                    handler(update.AsExchangeEvent<SharedFuturesOrder[]>(Exchange, data.Select(x =>
                        new SharedFuturesOrder(
                            ExchangeSymbolCache.ParseSymbol(_topicFuturesId, x.Symbol),
                            x.Symbol,
                            x.OrderId,
                            ParseOrderType(x.OrderType, x.ExecutionInstruction),
                            x.OrderSide == Enums.OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                            ParseOrderStatus(x.Status),
                            x.Timestamp)
                        {
                            ClientOrderId = x.ClientOrderId,
                            OrderPrice = x.Price,
                            OrderQuantity = new SharedOrderQuantity(contractQuantity: x.Quantity),
                            QuantityFilled = new SharedOrderQuantity(contractQuantity: x.QuantityFilled),
                            UpdateTime = x.TransactTime,
                            TimeInForce = ParseTimeInForce(x.TimeInForce),
                            AveragePrice = x.AveragePrice,
                            ReduceOnly = x.ExecutionInstruction?.Contains(ExecutionInstruction.ReduceOnly) == true,
                            TriggerPrice = x.StopPrice,
                            IsTriggerOrder = x.StopPrice > 0,
                            IsCloseOrder = x.ExecutionInstruction?.Contains(ExecutionInstruction.Close) == true
                        }
                    ).ToArray()));
                },
                ct: ct).ConfigureAwait(false);

            return new ExchangeResult<UpdateSubscription>(Exchange, result);
        }
        #endregion

        #region Position client
        EndpointOptions<SubscribePositionRequest> IPositionSocketClient.SubscribePositionOptions { get; } = new EndpointOptions<SubscribePositionRequest>(false);
        async Task<ExchangeResult<UpdateSubscription>> IPositionSocketClient.SubscribeToPositionUpdatesAsync(SubscribePositionRequest request, Action<ExchangeEvent<SharedPosition[]>> handler, CancellationToken ct)
        {
            var validationError = ((IPositionSocketClient)this).SubscribePositionOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeResult<UpdateSubscription>(Exchange, validationError);

            var symbolInfoResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolInfoResult)
                return new ExchangeResult<UpdateSubscription>(Exchange, symbolInfoResult.Error!);

            var result = await SubscribeToPositionUpdatesAsync(
                update => handler(update.AsExchangeEvent<SharedPosition[]>(Exchange, update.Data.Where(x => x.Currency != null).Select(x => new SharedPosition(ExchangeSymbolCache.ParseSymbol(_topicFuturesId, x.Symbol), x.Symbol, Math.Abs(x.CurrentQuantity ?? 0), x.Timestamp)
                {
                    AverageOpenPrice = x.AverageEntryPrice,
                    PositionSide = x.CurrentQuantity < 0 ? SharedPositionSide.Short : SharedPositionSide.Long,
                    UnrealizedPnl = x.UnrealizedPnl.ToSharedAssetQuantity(x.Currency!),
                    Leverage = x.Leverage,
                    LiquidationPrice = x.LiquidationPrice
                }).ToArray())),
                ct: ct).ConfigureAwait(false);

            return new ExchangeResult<UpdateSubscription>(Exchange, result);
        }

        #endregion
    }
}
