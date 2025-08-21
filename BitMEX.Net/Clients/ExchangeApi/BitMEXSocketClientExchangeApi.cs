using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using BitMEX.Net.Interfaces.Clients.ExchangeApi;
using BitMEX.Net.Objects.Models;
using BitMEX.Net.Objects.Options;
using BitMEX.Net.Objects.Sockets.Subscriptions;
using CryptoExchange.Net;
using BitMEX.Net.Objects.Sockets;
using System.Collections.Generic;
using System.Linq;
using BitMEX.Net.Enums;
using BitMEX.Net.Objects.Internal;
using System.Net.WebSockets;
using CryptoExchange.Net.Objects.Errors;

namespace BitMEX.Net.Clients.ExchangeApi
{
    /// <summary>
    /// Client providing access to the BitMEX Exchange websocket Api
    /// </summary>
    internal partial class BitMEXSocketClientExchangeApi : SocketApiClient, IBitMEXSocketClientExchangeApi
    {
        #region fields
        private static readonly MessagePath _subscribePath = MessagePath.Get().Property("subscribe");
        private static readonly MessagePath _tablePath = MessagePath.Get().Property("table");
        private static readonly MessagePath _symbolPath = MessagePath.Get().Property("data").Index(0).Property("symbol");
        private static readonly MessagePath _infoPath = MessagePath.Get().Property("info");
        private static readonly MessagePath _errorPath = MessagePath.Get().Property("error");
        private static readonly MessagePath _argsPath = MessagePath.Get().Property("request").Property("args");

        protected override ErrorMapping ErrorMapping => BitMEXErrors.SocketErrors;
        #endregion

        #region constructor/destructor

        /// <summary>
        /// ctor
        /// </summary>
        internal BitMEXSocketClientExchangeApi(ILogger logger, BitMEXSocketOptions options) :
            base(logger, options.Environment.SocketClientAddress!, options, options.ExchangeOptions)
        {
            AddSystemSubscription(new BitMEXInfoSubscription(_logger));

            ProcessUnparsableMessages = true;

            RegisterPeriodicQuery("Ping", TimeSpan.FromSeconds(5), x => new PingQuery(), (connection, result) =>
            {
                if (!result)
                {
                    // Ping timeout, reconnect
                    _logger.LogWarning("[Sckt {SocketId}] Ping response timeout, reconnecting", connection.SocketId);
                    _ = connection.TriggerReconnectAsync();
                }
            });
        }
        #endregion

        /// <inheritdoc />
        protected override IByteMessageAccessor CreateAccessor(WebSocketMessageType type) => new SystemTextJsonByteMessageAccessor(SerializerOptions.WithConverters(BitMEXExchange._serializerContext));
        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(BitMEXExchange._serializerContext));

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new BitMEXAuthenticationProvider(credentials);

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<BitMEXTradeUpdate[]>> onMessage, CancellationToken ct = default)
            => SubscribeToTradeUpdatesAsync([symbol], onMessage, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BitMEXTradeUpdate[]>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BitMEXSubscription<BitMEXTradeUpdate[]>(_logger, this, symbols.Select(x => "trade:" + x).ToArray(), x => onMessage(
                x.WithSymbol(x.Data.First().Symbol)
                .WithDataTimestamp(x.Data.Max(x => x.Timestamp))), false);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, BinPeriod period, Action<DataEvent<BitMEXAggTrade[]>> onMessage, CancellationToken ct = default)
            => SubscribeToKlineUpdatesAsync([symbol], period, onMessage, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, BinPeriod period, Action<DataEvent<BitMEXAggTrade[]>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BitMEXSubscription<BitMEXAggTrade[]>(_logger, this, symbols.Select(x => "tradeBin" + EnumConverter.GetString(period) + ":" + x).ToArray(), x => onMessage(
                x.WithSymbol(x.Data.First().Symbol)
                .WithDataTimestamp(x.Data.Max(x => x.Timestamp))), false);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToBookTickerUpdatesAsync(string symbol, Action<DataEvent<BitMEXBookTicker>> onMessage, CancellationToken ct = default)
            => SubscribeToBookTickerUpdatesAsync([symbol], onMessage, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToBookTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BitMEXBookTicker>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BitMEXSubscription<BitMEXBookTicker[]>(_logger, this, symbols.Select(x => "quote:" + x).ToArray(), x => onMessage(
                x.As(x.Data.First())
                .WithSymbol(x.Data.First().Symbol)
                .WithDataTimestamp(x.Data.Max(x => x.Timestamp))), false);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToAggregatedBookTickerUpdatesAsync(string symbol, BinPeriod period, Action<DataEvent<BitMEXBookTicker>> onMessage, CancellationToken ct = default)
            => SubscribeToAggregatedBookTickerUpdatesAsync([symbol], period, onMessage, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAggregatedBookTickerUpdatesAsync(IEnumerable<string> symbols, BinPeriod period, Action<DataEvent<BitMEXBookTicker>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BitMEXSubscription<BitMEXBookTicker[]>(_logger, this, symbols.Select(x => "quoteBin" + EnumConverter.GetString(period) + ":" + x).ToArray(), x => onMessage(
                x.As(x.Data.First())
                .WithSymbol(x.Data.First().Symbol)
                .WithDataTimestamp(x.Data.Max(x => x.Timestamp))), false);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToSettlementUpdatesAsync(Action<DataEvent<BitMEXSettlementHistory[]>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BitMEXSubscription<BitMEXSettlementHistory[]>(_logger, this, ["settlement"], x => onMessage(x
                .WithDataTimestamp(x.Data.Max(x => x.Timestamp))), false);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, Action<DataEvent<BitMEXOrderBookUpdate>> onMessage, CancellationToken ct = default)
            => SubscribeToOrderBookUpdatesAsync([symbol], onMessage, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BitMEXOrderBookUpdate>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BitMEXSubscription<BitMEXOrderBookUpdate[]>(_logger, this, symbols.Select(x => "orderBook10:" + x).ToArray(), x => onMessage(
                x.As(x.Data.First())
                .WithSymbol(x.Data.First().Symbol)
                .WithDataTimestamp(x.Data.Max(x => x.Timestamp))), false);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToIncrementalOrderBookUpdatesAsync(string symbol, IncrementalBookLimit limit, Action<DataEvent<BitMEXOrderBookIncrementalUpdate>> onMessage, CancellationToken ct = default)
            => SubscribeToIncrementalOrderBookUpdatesAsync([symbol], limit, onMessage, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToIncrementalOrderBookUpdatesAsync(IEnumerable<string> symbols, IncrementalBookLimit limit, Action<DataEvent<BitMEXOrderBookIncrementalUpdate>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BitMEXRawSubscription<SocketUpdate<BitMEXOrderBookEntry[]>>(_logger, this, symbols.Select(x => "orderBookL2" + (limit == IncrementalBookLimit.Top25 ? "_25": "") + ":" + x).ToArray(), x => onMessage(x.As(new BitMEXOrderBookIncrementalUpdate
            {
                Action = EnumConverter.ParseString<BookUpdateType>(x.Data.Action)!.Value,
                Entries = x.Data.Data
            })
                .WithSymbol(x.Data.Data.First().Symbol)
                .WithUpdateType(x.Data.Action == "partial" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                .WithDataTimestamp(x.Data.Data.Max(x => x.Timestamp))), false);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToLiquidationUpdatesAsync(Action<DataEvent<BitMEXLiquidation[]>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BitMEXSubscription<BitMEXLiquidation[]>(_logger, this, ["liquidation"], x => onMessage(
                x.WithSymbol(x.Data.FirstOrDefault()?.Symbol!)), false);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToInsuranceUpdatesAsync(Action<DataEvent<BitMEXInsurance[]>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BitMEXSubscription<BitMEXInsurance[]>(_logger, this, ["insurance"], x => onMessage(x
                .WithDataTimestamp(x.Data.Max(x => x.Timestamp))), false);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToSymbolUpdatesAsync(SymbolCategory? category, Action<DataEvent<BitMEXSymbolUpdate[]>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BitMEXOptionalSymbolSubscription<BitMEXSymbolUpdate>(_logger, this, "instrument", category == null ? null :[EnumConverter.GetString(category)], null, x => onMessage(x
                .WithDataTimestamp(x.Data.Max(x => x.Timestamp))), false);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToSymbolUpdatesAsync(string symbol, Action<DataEvent<BitMEXSymbolUpdate>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BitMEXOptionalSymbolSubscription<BitMEXSymbolUpdate>(_logger, this, "instrument", null, [symbol], x => onMessage(
                x.As(x.Data.Single())
                .WithDataTimestamp(x.Data.Max(x => x.Timestamp))), false);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToSymbolUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BitMEXSymbolUpdate[]>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BitMEXOptionalSymbolSubscription<BitMEXSymbolUpdate>(_logger, this, "instrument", null, symbols.ToArray(), x => onMessage(x
                .WithDataTimestamp(x.Data.Max(x => x.Timestamp))), false);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToFundingUpdatesAsync(string symbol, Action<DataEvent<BitMEXFundingRate>> onMessage, CancellationToken ct = default)
            => SubscribeToFundingUpdatesAsync([symbol], onMessage, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToFundingUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BitMEXFundingRate>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BitMEXSubscription<BitMEXFundingRate[]>(_logger, this, symbols.Select(x => "funding:" + x).ToArray(), x => onMessage(
                x.As(x.Data.First())
                .WithSymbol(x.Data.First().Symbol)
                .WithDataTimestamp(x.Data.Max(x => x.Timestamp))), false);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAnnouncementUpdatesAsync(Action<DataEvent<BitMEXAnnouncement[]>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BitMEXSubscription<BitMEXAnnouncement[]>(_logger, this, ["announcement"], x => onMessage(x.WithDataTimestamp(x.Data.Max(x => x.Timestamp))), false);
            return await SubscribeAsync(BaseAddress.AppendPath("realtimePlatform"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToNotificationUpdatesAsync(Action<DataEvent<BitMEXNotification[]>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BitMEXSubscription<BitMEXNotification[]>(_logger, this, ["publicNotifications"], onMessage, false);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(Action<DataEvent<BitMEXBalance[]>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BitMEXSubscription<BitMEXBalance[]>(_logger, this, ["wallet"], x => onMessage(x.WithDataTimestamp(x.Data.Any() ? x.Data.Max(x => x.Timestamp) : null)), true);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTransactionUpdatesAsync(Action<DataEvent<BitMEXTransaction[]>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BitMEXSubscription<BitMEXTransaction[]>(_logger, this, ["transact"], x => onMessage(x.WithDataTimestamp(x.Data.Any() ? x.Data.Max(x => x.Timestamp) : null)), true);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToPositionUpdatesAsync(Action<DataEvent<BitMEXPosition[]>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BitMEXSubscription<BitMEXPosition[]>(_logger, this, ["position"], x => onMessage(x.WithDataTimestamp(x.Data.Any() ? x.Data.Max(x => x.Timestamp) : null)), true);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToMarginUpdatesAsync(Action<DataEvent<BitMEXMarginStatus>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BitMEXSubscription<BitMEXMarginStatus[]>(_logger, this, ["margin"], x => onMessage(x.As(x.Data.First())
                .WithDataTimestamp(x.Data.Max(x => x.Timestamp))), true);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(Action<DataEvent<BitMEXOrder[]>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BitMEXSubscription<BitMEXOrder[]>(_logger, this, ["order"], x => onMessage(x.WithDataTimestamp(x.Data.Any() ? x.Data.Max(x => x.Timestamp) : null)), true);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(Action<DataEvent<BitMEXExecution[]>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BitMEXSubscription<BitMEXExecution[]>(_logger, this, ["execution"], x => onMessage(x.WithDataTimestamp(x.Data.Any() ? x.Data.Max(x => x.Timestamp) : null)), true);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        protected override Task<CallResult<string?>> GetConnectionUrlAsync(string address, bool authentication)
        {
            if (!authentication)
                return Task.FromResult(new CallResult<string?>(address));

            return Task.FromResult(new CallResult<string?>(GetAddress()!));
        }

        private string? GetAddress()
        {
            if (AuthenticationProvider == null)
                return null;

            var provider = (BitMEXAuthenticationProvider)AuthenticationProvider;
            var expires = DateTimeConverter.ConvertToSeconds(DateTime.UtcNow.AddSeconds(5));
            var queryParams = $"api-key={provider.ApiKey}&api-signature={provider.GetSignature("GET", "/realtime", expires.Value, "")}&api-expires={expires}";
            return BaseAddress.AppendPath("realtime?" + queryParams);
        }

        protected override Task<Uri?> GetReconnectUriAsync(SocketConnection connection)
        {
            if (!connection.Subscriptions.Any(x => x.Authenticated))
                return Task.FromResult<Uri?>(null);
            
            return Task.FromResult(new Uri(GetAddress()!))!;
        }

        protected override Task<Query?> GetAuthenticationRequestAsync(SocketConnection connection) => Task.FromResult<Query?>(null);

        /// <inheritdoc />
        public override string? GetListenerIdentifier(IMessageAccessor message)
        {
            if (!message.IsValid)
                return "pong";

            var table = message.GetValue<string>(_tablePath);
            if (table != null)
            {
                if (table.Equals("settlement")
                    || table.Equals("liquidation")
                    || table.Equals("execution")
                    || table.Equals("order")
                    || table.Equals("position")
                    || table.Equals("instrument")
                    || table.Equals("announcement")
                    || table.Equals("publicNotifications")
                    || table.Equals("insurance"))
                {
                    return "upd" + table;
                }

                var symbol = message.GetValue<string>(_symbolPath);
                return "upd" + table + symbol;
            }

            var sub = message.GetValue<string>(_subscribePath);
            if (sub != null)
                return sub;

            var info = message.GetValue<string>(_infoPath);
            if (info != null)
                return "info";

            var error = message.GetValue<string>(_errorPath);
            if (error != null)
            {
                // Check if the request is present in the error
                var subArgs = message.GetValues<string>(_argsPath);
                if (subArgs != null && subArgs.Any())
                    return string.Join("", subArgs);
            }

            return null;
        }

        /// <inheritdoc />
        public IBitMEXSocketClientExchangeApiShared SharedClient => this;

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => BitMEXExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);
    }
}
