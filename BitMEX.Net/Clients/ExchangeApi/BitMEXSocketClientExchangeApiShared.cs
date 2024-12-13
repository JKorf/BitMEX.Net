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

namespace BitMEX.Net.Clients.ExchangeApi
{
    internal partial class BitMEXSocketClientExchangeApi : IBitMEXSocketClientExchangeApiShared
    {
        public string Exchange => "BitMEX";

        public TradingMode[] SupportedTradingModes => new[] { TradingMode.Spot, TradingMode.PerpetualLinear, TradingMode.DeliveryLinear, TradingMode.PerpetualInverse, TradingMode.DeliveryInverse };

        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();

        #region Spot Order client

        EndpointOptions<SubscribeSpotOrderRequest> ISpotOrderSocketClient.SubscribeSpotOrderOptions { get; } = new EndpointOptions<SubscribeSpotOrderRequest>(false);
        async Task<ExchangeResult<UpdateSubscription>> ISpotOrderSocketClient.SubscribeToSpotOrderUpdatesAsync(SubscribeSpotOrderRequest request, Action<ExchangeEvent<IEnumerable<SharedSpotOrder>>> handler, CancellationToken ct)
        {
            var validationError = ((ISpotOrderSocketClient)this).SubscribeSpotOrderOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeResult<UpdateSubscription>(Exchange, validationError);

            await BitMEXExchange.UpdateScalesAsync(ct).ConfigureAwait(false);

            var result = await SubscribeToOrderUpdatesAsync(
                update => {
                    var data = update.Data.Where(x => BitMEXExchange.GetSymbolType(x.Symbol) == SymbolType.Spot).ToList();
                    if (!data.Any())
                        return;

                    handler(update.AsExchangeEvent<IEnumerable<SharedSpotOrder>>(Exchange, data.Select(x =>
                    
                        new SharedSpotOrder(
                            x.Symbol,
                            x.OrderId,
                            ParseOrderType(x.OrderType, x.ExecutionInstruction),
                            x.OrderSide == Enums.OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                            ParseOrderStatus(x.Status),
                            x.Timestamp)
                        {
                            ClientOrderId = x.ClientOrderId,
                            OrderPrice = x.Price,
                            Quantity = x.Quantity.ToSharedQuantity(BitMEXExchange.GetSymbolQuantityScale(x.Symbol)),
                            QuantityFilled = x.QuantityFilled.ToSharedQuantity(BitMEXExchange.GetSymbolQuantityScale(x.Symbol)),
                            UpdateTime = x.TransactTime,
                            TimeInForce = ParseTimeInForce(x.TimeInForce),
                            AveragePrice = x.AveragePrice
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

        private SharedOrderType ParseOrderType(OrderType orderType, ExecutionInstruction? executionInstruction)
        {
            if (orderType == OrderType.Market) return SharedOrderType.Market;
            if (orderType == OrderType.Limit && executionInstruction == ExecutionInstruction.PostOnly) return SharedOrderType.LimitMaker;
            if (orderType == OrderType.Limit) return SharedOrderType.Limit;
            return SharedOrderType.Other;
        }
        #endregion

        #region Balance client
        EndpointOptions<SubscribeBalancesRequest> IBalanceSocketClient.SubscribeBalanceOptions { get; } = new EndpointOptions<SubscribeBalancesRequest>(false);
        async Task<ExchangeResult<UpdateSubscription>> IBalanceSocketClient.SubscribeToBalanceUpdatesAsync(SubscribeBalancesRequest request, Action<ExchangeEvent<IEnumerable<SharedBalance>>> handler, CancellationToken ct)
        {
            var validationError = ((IBalanceSocketClient)this).SubscribeBalanceOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeResult<UpdateSubscription>(Exchange, validationError);

            await BitMEXExchange.UpdateScalesAsync(ct).ConfigureAwait(false);

            var result = await SubscribeToBalanceUpdatesAsync(
#warning check total
                update => handler(update.AsExchangeEvent<IEnumerable<SharedBalance>>(Exchange, update.Data.Select(x => 
                new SharedBalance(
                    BitMEXExchange.GetAssetFromCurrency(x.Currency), 
                    x.Quantity.ToSharedQuantity(BitMEXExchange.GetCurrencyScale(x.Currency)),
                    (x.Quantity + x.PendingCredit).ToSharedQuantity(BitMEXExchange.GetCurrencyScale(x.Currency)))).ToArray())),
                ct: ct).ConfigureAwait(false);

            return new ExchangeResult<UpdateSubscription>(Exchange, result);
        }

        #endregion

        #region User Trade client

        EndpointOptions<SubscribeUserTradeRequest> IUserTradeSocketClient.SubscribeUserTradeOptions { get; } = new EndpointOptions<SubscribeUserTradeRequest>(false);
        async Task<ExchangeResult<UpdateSubscription>> IUserTradeSocketClient.SubscribeToUserTradeUpdatesAsync(SubscribeUserTradeRequest request, Action<ExchangeEvent<IEnumerable<SharedUserTrade>>> handler, CancellationToken ct)
        {
            var validationError = ((IUserTradeSocketClient)this).SubscribeUserTradeOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeResult<UpdateSubscription>(Exchange, validationError);

            await BitMEXExchange.UpdateScalesAsync(ct).ConfigureAwait(false);

            var result = await SubscribeToUserTradeUpdatesAsync(
                update =>
                {
                    var data = update.Data
                    .Where(x => BitMEXExchange.GetSymbolType(x.Symbol) == SymbolType.Spot
                        && x.ExecutionType == ExecutionType.Trade)
                    .ToList();
                    if (!data.Any())
                        return;

                    handler(update.AsExchangeEvent<IEnumerable<SharedUserTrade>>(Exchange, data.Select(x =>
                        new SharedUserTrade(
                            x.Symbol,
                            x.OrderId,
                            x.TradeId,
                            x.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                            x.Quantity.ToSharedQuantity(BitMEXExchange.GetSymbolQuantityScale(x.Symbol)),
                            x.LastTradePrice!.Value,
                            x.Timestamp)
                        {
#warning fee needs to be converted for futures?
                            Fee = x.Fee.ToSharedQuantity(BitMEXExchange.GetCurrencyScale(x.Currency)),
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

            await BitMEXExchange.UpdateScalesAsync(ct).ConfigureAwait(false);

            var symbol = request.Symbol.GetSymbol(FormatSymbol);
            var result = await SubscribeToBookTickerUpdatesAsync(symbol, update => handler(update.AsExchangeEvent(Exchange,
                new SharedBookTicker(
                    update.Data.BestAskPrice,
                    request.Symbol.TradingMode != TradingMode.Spot ? update.Data.BestAskQuantity : update.Data.BestAskQuantity.ToSharedQuantity(BitMEXExchange.GetSymbolQuantityScale(symbol)),
                    update.Data.BestBidPrice,
                    request.Symbol.TradingMode != TradingMode.Spot ? update.Data.BestBidQuantity : update.Data.BestBidQuantity.ToSharedQuantity(BitMEXExchange.GetSymbolQuantityScale(symbol))
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

            await BitMEXExchange.UpdateScalesAsync(ct).ConfigureAwait(false);

            var symbol = request.Symbol.GetSymbol(FormatSymbol);
            var result = await SubscribeToOrderBookUpdatesAsync(symbol, update =>
            {
                var book = new SharedOrderBook(update.Data.Asks, update.Data.Bids);
                if (request.Symbol.TradingMode == TradingMode.Spot)
                {
                    foreach (var item in book.Asks)
                        item.Quantity = ((long)item.Quantity).ToSharedQuantity(BitMEXExchange.GetSymbolQuantityScale(symbol));
                    foreach (var item in book.Bids)
                        item.Quantity = ((long)item.Quantity).ToSharedQuantity(BitMEXExchange.GetSymbolQuantityScale(symbol));
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

            await BitMEXExchange.UpdateScalesAsync(ct).ConfigureAwait(false);

            SharedSpotTicker? ticker = null;
            var symbol = request.Symbol.GetSymbol(FormatSymbol);
            var result = await SubscribeToSymbolUpdatesAsync(symbol, update =>
            {
                if (ticker == null)
                {
                    ticker = new SharedSpotTicker(
                        update.Data.Symbol,
                        update.Data.LastPrice,
                        update.Data.HighPrice,
                        update.Data.LowPrice,
                        request.Symbol.TradingMode != TradingMode.Spot ? update.Data.Volume24h ?? 0 : (update.Data.Volume24h ?? 0).ToSharedQuantity(BitMEXExchange.GetSymbolQuantityScale(symbol)),
                        update.Data.LastChangePcnt * 100
                        );
                }
                else
                {
                    ticker.LastPrice = update.Data.LastPrice ?? ticker.LastPrice;
                    ticker.HighPrice = update.Data.HighPrice ?? ticker.HighPrice;
                    ticker.LowPrice = update.Data.LowPrice ?? ticker.LowPrice;
                    ticker.Volume = update.Data.Volume24h == null ? ticker.Volume : request.Symbol.TradingMode != TradingMode.Spot ? update.Data.Volume24h ?? 0 : (update.Data.Volume24h ?? 0).ToSharedQuantity(BitMEXExchange.GetSymbolQuantityScale(symbol));
                    ticker.ChangePercentage = update.Data.LastChangePcnt == null ? ticker.ChangePercentage : update.Data.LastChangePcnt * 100;
                }

                handler(update.AsExchangeEvent(Exchange, ticker));
            }, ct).ConfigureAwait(false);

            return new ExchangeResult<UpdateSubscription>(Exchange, result);
        }
        #endregion

        #region Trade client

        EndpointOptions<SubscribeTradeRequest> ITradeSocketClient.SubscribeTradeOptions { get; } = new EndpointOptions<SubscribeTradeRequest>(false);
        async Task<ExchangeResult<UpdateSubscription>> ITradeSocketClient.SubscribeToTradeUpdatesAsync(SubscribeTradeRequest request, Action<ExchangeEvent<IEnumerable<SharedTrade>>> handler, CancellationToken ct)
        {
            var validationError = ((ITradeSocketClient)this).SubscribeTradeOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeResult<UpdateSubscription>(Exchange, validationError);

            await BitMEXExchange.UpdateScalesAsync(ct).ConfigureAwait(false);

            var symbol = request.Symbol.GetSymbol(FormatSymbol);
            var result = await SubscribeToTradeUpdatesAsync(symbol, update =>
            {
                if (update.UpdateType == SocketUpdateType.Snapshot)
                    return;

                handler(update.AsExchangeEvent<IEnumerable<SharedTrade>>(Exchange, update.Data.Select(x =>
                    new SharedTrade(
                        x.Quantity.ToSharedQuantity(BitMEXExchange.GetSymbolQuantityScale(x.Symbol)),
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
        async Task<ExchangeResult<UpdateSubscription>> IFuturesOrderSocketClient.SubscribeToFuturesOrderUpdatesAsync(SubscribeFuturesOrderRequest request, Action<ExchangeEvent<IEnumerable<SharedFuturesOrder>>> handler, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderSocketClient)this).SubscribeFuturesOrderOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeResult<UpdateSubscription>(Exchange, validationError);

            var result = await SubscribeToOrderUpdatesAsync(
                update => {
                    var data = update.Data.Where(x => BitMEXExchange.GetSymbolType(x.Symbol) != SymbolType.Spot).ToList();
                    if (!data.Any())
                        return;

                    handler(update.AsExchangeEvent<IEnumerable<SharedFuturesOrder>>(Exchange, data.Select(x =>
                        new SharedFuturesOrder(
                            x.Symbol,
                            x.OrderId,
                            ParseOrderType(x.OrderType, x.ExecutionInstruction),
                            x.OrderSide == Enums.OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                            ParseOrderStatus(x.Status),
                            x.Timestamp)
                        {
                            ClientOrderId = x.ClientOrderId,
                            OrderPrice = x.Price,
                            Quantity = x.Quantity,
                            QuantityFilled = x.QuantityFilled,
                            UpdateTime = x.TransactTime,
                            TimeInForce = ParseTimeInForce(x.TimeInForce),
                            AveragePrice = x.AveragePrice,
                            ReduceOnly = x.ExecutionInstruction == ExecutionInstruction.ReduceOnly
                        }
                    ).ToArray()));
                },
                ct: ct).ConfigureAwait(false);

            return new ExchangeResult<UpdateSubscription>(Exchange, result);
        }
        #endregion

        #region Position client
        EndpointOptions<SubscribePositionRequest> IPositionSocketClient.SubscribePositionOptions { get; } = new EndpointOptions<SubscribePositionRequest>(false)
        {
            RequiredOptionalParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(SubscribePositionRequest.ListenKey), typeof(string), "The listenkey for starting the user stream", "123123123")
            }
        };
        async Task<ExchangeResult<UpdateSubscription>> IPositionSocketClient.SubscribeToPositionUpdatesAsync(SubscribePositionRequest request, Action<ExchangeEvent<IEnumerable<SharedPosition>>> handler, CancellationToken ct)
        {
            var validationError = ((IPositionSocketClient)this).SubscribePositionOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeResult<UpdateSubscription>(Exchange, validationError);

            var result = await SubscribeToPositionUpdatesAsync(
                update => handler(update.AsExchangeEvent<IEnumerable<SharedPosition>>(Exchange, update.Data.Select(x => new SharedPosition(x.Symbol, x.CurrentQuantity, x.Timestamp)
                {
                    AverageOpenPrice = x.AverageEntryPrice,
#warning check if correct
                    PositionSide = x.CurrentQuantity > 0 ? SharedPositionSide.Short : SharedPositionSide.Long,
                    UnrealizedPnl = x.UnrealizedPnl,
                    Leverage = x.Leverage,
                    LiquidationPrice = x.LiquidationPrice
                }).ToArray())),
                ct: ct).ConfigureAwait(false);

            return new ExchangeResult<UpdateSubscription>(Exchange, result);
        }

        #endregion
    }
}
