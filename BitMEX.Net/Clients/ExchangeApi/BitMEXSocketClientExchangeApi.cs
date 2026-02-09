using BitMEX.Net.Clients.MessageHandlers;
using BitMEX.Net.Enums;
using BitMEX.Net.Interfaces.Clients.ExchangeApi;
using BitMEX.Net.Objects.Internal;
using BitMEX.Net.Objects.Models;
using BitMEX.Net.Objects.Options;
using BitMEX.Net.Objects.Sockets;
using BitMEX.Net.Objects.Sockets.Subscriptions;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace BitMEX.Net.Clients.ExchangeApi
{
    /// <summary>
    /// Client providing access to the BitMEX Exchange websocket Api
    /// </summary>
    internal partial class BitMEXSocketClientExchangeApi : SocketApiClient, IBitMEXSocketClientExchangeApi
    {
        #region fields
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
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(BitMEXExchange._serializerContext));
        public override ISocketMessageHandler CreateMessageConverter(WebSocketMessageType messageType) => new BitMexSocketExchangeMessageHandler();

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new BitMEXAuthenticationProvider(credentials);

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<BitMEXTradeUpdate[]>> onMessage, CancellationToken ct = default)
            => SubscribeToTradeUpdatesAsync([symbol], onMessage, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BitMEXTradeUpdate[]>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DateTime, string?, SocketUpdate<BitMEXTradeUpdate[]>>((receiveTime, originalData, data) =>
            {
                DateTime? timestamp = data.Data.Length > 0 ? data.Data.Max(x => x.Timestamp) : null;
                if (data.Action == "update")
                    UpdateTimeOffset(timestamp!.Value);

                onMessage(
                    new DataEvent<BitMEXTradeUpdate[]>(Exchange, data.Data, receiveTime, originalData)
                        .WithUpdateType(data.Action == "partial" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithStreamId(data.Table)
                        .WithSymbol(data.Data.FirstOrDefault()?.Symbol)
                        .WithDataTimestamp(timestamp, GetTimeOffset())
                    );
            });

            var subscription = new BitMEXSubscription<BitMEXTradeUpdate[]>(_logger, this, "trade", symbols.ToArray(), handler, false);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, BinPeriod period, Action<DataEvent<BitMEXAggTrade[]>> onMessage, CancellationToken ct = default)
            => SubscribeToKlineUpdatesAsync([symbol], period, onMessage, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, BinPeriod period, Action<DataEvent<BitMEXAggTrade[]>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DateTime, string?, SocketUpdate<BitMEXAggTrade[]>>((receiveTime, originalData, data) =>
            {
                onMessage(
                    new DataEvent<BitMEXAggTrade[]>(Exchange, data.Data, receiveTime, originalData)
                        .WithUpdateType(data.Action == "partial" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithStreamId(data.Table)
                        .WithSymbol(data.Data.First().Symbol)
                    );
            });

            var subscription = new BitMEXSubscription<BitMEXAggTrade[]>(_logger, this, "tradeBin" + EnumConverter.GetString(period), symbols.ToArray(), handler, false);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToBookTickerUpdatesAsync(string symbol, Action<DataEvent<BitMEXBookTicker>> onMessage, CancellationToken ct = default)
            => SubscribeToBookTickerUpdatesAsync([symbol], onMessage, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToBookTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BitMEXBookTicker>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DateTime, string?, SocketUpdate<BitMEXBookTicker[]>>((receiveTime, originalData, data) =>
            {
                var item = data.Data.First();
                if (data.Action == "update")
                    UpdateTimeOffset(item.Timestamp);

                onMessage(
                    new DataEvent<BitMEXBookTicker>(Exchange, item, receiveTime, originalData)
                        .WithUpdateType(data.Action == "partial" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithStreamId(data.Table)
                        .WithSymbol(item.Symbol)
                        .WithDataTimestamp(item.Timestamp, GetTimeOffset())
                    );
            });

            var subscription = new BitMEXSubscription<BitMEXBookTicker[]>(_logger, this, "quote", symbols.ToArray(), handler, false);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToAggregatedBookTickerUpdatesAsync(string symbol, BinPeriod period, Action<DataEvent<BitMEXBookTicker>> onMessage, CancellationToken ct = default)
            => SubscribeToAggregatedBookTickerUpdatesAsync([symbol], period, onMessage, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAggregatedBookTickerUpdatesAsync(IEnumerable<string> symbols, BinPeriod period, Action<DataEvent<BitMEXBookTicker>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DateTime, string?, SocketUpdate<BitMEXBookTicker[]>>((receiveTime, originalData, data) =>
            {
                var item = data.Data.First();
                if (data.Action == "update")
                    UpdateTimeOffset(item.Timestamp);

                onMessage(
                    new DataEvent<BitMEXBookTicker>(Exchange, item, receiveTime, originalData)
                        .WithUpdateType(data.Action == "partial" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithStreamId(data.Table)
                        .WithSymbol(item.Symbol)
                        .WithDataTimestamp(item.Timestamp, GetTimeOffset())
                    );
            });

            var subscription = new BitMEXSubscription<BitMEXBookTicker[]>(_logger, this, "quoteBin", symbols.ToArray(), handler, false);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToSettlementUpdatesAsync(Action<DataEvent<BitMEXSettlementHistory[]>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DateTime, string?, SocketUpdate<BitMEXSettlementHistory[]>>((receiveTime, originalData, data) =>
            {
                var timestamp = data.Data.Max(x => x.Timestamp);
                if (data.Action == "update")
                    UpdateTimeOffset(timestamp);

                onMessage(
                    new DataEvent<BitMEXSettlementHistory[]>(Exchange, data.Data, receiveTime, originalData)
                        .WithUpdateType(data.Action == "partial" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithStreamId(data.Table)
                        .WithDataTimestamp(timestamp, GetTimeOffset())
                    );
            });

            var subscription = new BitMEXSubscription<BitMEXSettlementHistory[]>(_logger, this, "settlement", null, handler, false);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, Action<DataEvent<BitMEXOrderBookUpdate>> onMessage, CancellationToken ct = default)
            => SubscribeToOrderBookUpdatesAsync([symbol], onMessage, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BitMEXOrderBookUpdate>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DateTime, string?, SocketUpdate<BitMEXOrderBookUpdate[]>>((receiveTime, originalData, data) =>
            {
                var item = data.Data.First();
                if (data.Action == "update")
                    UpdateTimeOffset(item.Timestamp);

                onMessage(
                    new DataEvent<BitMEXOrderBookUpdate>(Exchange, item, receiveTime, originalData)
                        .WithUpdateType(data.Action == "partial" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithStreamId(data.Table)
                        .WithSymbol(item.Symbol)
                        .WithDataTimestamp(item.Timestamp, GetTimeOffset())
                    );
            });

            var subscription = new BitMEXSubscription<BitMEXOrderBookUpdate[]>(_logger, this, "orderBook10", symbols.ToArray(), handler, false);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToIncrementalOrderBookUpdatesAsync(string symbol, IncrementalBookLimit limit, Action<DataEvent<BitMEXOrderBookIncrementalUpdate>> onMessage, CancellationToken ct = default)
            => SubscribeToIncrementalOrderBookUpdatesAsync([symbol], limit, onMessage, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToIncrementalOrderBookUpdatesAsync(IEnumerable<string> symbols, IncrementalBookLimit limit, Action<DataEvent<BitMEXOrderBookIncrementalUpdate>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DateTime, string?, SocketUpdate<BitMEXOrderBookEntry[]>>((receiveTime, originalData, data) =>
            {
                var timestamp = data.Data.Max(x => x.Timestamp);
                if (data.Action == "update")
                    UpdateTimeOffset(timestamp);

                onMessage(
                    new DataEvent<BitMEXOrderBookIncrementalUpdate>(Exchange, new BitMEXOrderBookIncrementalUpdate
                    {
                        Action = EnumConverter.ParseString<BookUpdateType>(data.Action)!.Value,
                        Entries = data.Data
                    }, receiveTime, originalData)
                        .WithUpdateType(data.Action == "partial" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithStreamId(data.Table)
                        .WithSymbol(data.Data.First().Symbol)
                        .WithDataTimestamp(timestamp, GetTimeOffset())
                    );
            });

            var subscription = new BitMEXSubscription<BitMEXOrderBookEntry[]>(_logger, this, "orderBookL2" + (limit == IncrementalBookLimit.Top25 ? "_25" : ""), symbols.ToArray(), handler, false);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToLiquidationUpdatesAsync(Action<DataEvent<BitMEXLiquidation[]>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DateTime, string?, SocketUpdate<BitMEXLiquidation[]>>((receiveTime, originalData, data) =>
            {
                onMessage(
                    new DataEvent<BitMEXLiquidation[]>(Exchange, data.Data, receiveTime, originalData)
                        .WithUpdateType(data.Action == "partial" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithStreamId(data.Table)
                        .WithSymbol(data.Data.FirstOrDefault()?.Symbol)
                    );
            });

            var subscription = new BitMEXSubscription<BitMEXLiquidation[]>(_logger, this, "liquidation", null, handler, false);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToInsuranceUpdatesAsync(Action<DataEvent<BitMEXInsurance[]>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DateTime, string?, SocketUpdate<BitMEXInsurance[]>>((receiveTime, originalData, data) =>
            {
                var timestamp = data.Data.Max(x => x.Timestamp);
                if (data.Action == "update")
                    UpdateTimeOffset(timestamp);

                onMessage(
                    new DataEvent<BitMEXInsurance[]>(Exchange, data.Data, receiveTime, originalData)
                        .WithUpdateType(data.Action == "partial" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithStreamId(data.Table)
                        .WithDataTimestamp(timestamp, GetTimeOffset())
                    );
            });

            var subscription = new BitMEXSubscription<BitMEXInsurance[]>(_logger, this, "insurance", null, handler, false);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToSymbolUpdatesAsync(SymbolCategory? category, Action<DataEvent<BitMEXSymbolUpdate[]>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DateTime, string?, SocketUpdate<BitMEXSymbolUpdate[]>>((receiveTime, originalData, data) =>
            {
                var timestamp = data.Data.Max(x => x.Timestamp);
                if (data.Action == "update")
                    UpdateTimeOffset(timestamp);

                onMessage(
                    new DataEvent<BitMEXSymbolUpdate[]>(Exchange, data.Data, receiveTime, originalData)
                        .WithUpdateType(data.Action == "partial" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithStreamId(data.Table)
                        .WithDataTimestamp(timestamp, GetTimeOffset())
                    );
            });
            var subscription = new BitMEXSubscription<BitMEXSymbolUpdate[]>(_logger, this, "instrument", category == null ? null :[EnumConverter.GetString(category)], handler, false);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToSymbolUpdatesAsync(string symbol, Action<DataEvent<BitMEXSymbolUpdate>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DateTime, string?, SocketUpdate<BitMEXSymbolUpdate[]>>((receiveTime, originalData, data) =>
            {
                var item = data.Data.Single();
                if (data.Action == "update")
                    UpdateTimeOffset(item.Timestamp);

                onMessage(
                    new DataEvent<BitMEXSymbolUpdate>(Exchange, item, receiveTime, originalData)
                        .WithUpdateType(data.Action == "partial" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithStreamId(data.Table)
                        .WithSymbol(item.Symbol)
                        .WithDataTimestamp(item.Timestamp, GetTimeOffset())
                    );
            });
            var subscription = new BitMEXSubscription<BitMEXSymbolUpdate[]>(_logger, this, "instrument", [symbol], handler, false);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToSymbolUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BitMEXSymbolUpdate[]>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DateTime, string?, SocketUpdate<BitMEXSymbolUpdate[]>>((receiveTime, originalData, data) =>
            {
                var timestamp = data.Data.Max(x => x.Timestamp);
                if (data.Action == "update")
                    UpdateTimeOffset(timestamp);

                onMessage(
                    new DataEvent<BitMEXSymbolUpdate[]>(Exchange, data.Data, receiveTime, originalData)
                        .WithUpdateType(data.Action == "partial" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithStreamId(data.Table)
                        .WithDataTimestamp(timestamp, GetTimeOffset())
                    );
            });

            var subscription = new BitMEXSubscription<BitMEXSymbolUpdate[]>(_logger, this, "instrument", symbols.ToArray(), handler, false);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToFundingUpdatesAsync(string symbol, Action<DataEvent<BitMEXFundingRate>> onMessage, CancellationToken ct = default)
            => SubscribeToFundingUpdatesAsync([symbol], onMessage, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToFundingUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BitMEXFundingRate>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DateTime, string?, SocketUpdate<BitMEXFundingRate[]>>((receiveTime, originalData, data) =>
            {
                if (data.Data.Length == 0)
                    return;

                var timestamp = data.Data.Max(x => x.Timestamp);
                if (data.Action == "update")
                    UpdateTimeOffset(timestamp);

                onMessage(
                    new DataEvent<BitMEXFundingRate>(Exchange, data.Data.First(), receiveTime, originalData)
                        .WithUpdateType(data.Action == "partial" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithStreamId(data.Table)
                        .WithSymbol(data.Data.First().Symbol)
                        .WithDataTimestamp(timestamp, GetTimeOffset())
                    );
            });
            var subscription = new BitMEXSubscription<BitMEXFundingRate[]>(_logger, this, "funding", symbols.ToArray(), handler, false);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAnnouncementUpdatesAsync(Action<DataEvent<BitMEXAnnouncement[]>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DateTime, string?, SocketUpdate<BitMEXAnnouncement[]>>((receiveTime, originalData, data) =>
            {
                var timestamp = data.Data.Max(x => x.Timestamp);
                if (data.Action == "update")
                    UpdateTimeOffset(timestamp);

                onMessage(
                    new DataEvent<BitMEXAnnouncement[]>(Exchange, data.Data, receiveTime, originalData)
                        .WithUpdateType(data.Action == "partial" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithStreamId(data.Table)
                        .WithDataTimestamp(timestamp, GetTimeOffset())
                    );
            });
            var subscription = new BitMEXSubscription<BitMEXAnnouncement[]>(_logger, this, "announcement", null, handler, false);
            return await SubscribeAsync(BaseAddress.AppendPath("realtimePlatform"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToNotificationUpdatesAsync(Action<DataEvent<BitMEXNotification[]>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DateTime, string?, SocketUpdate<BitMEXNotification[]>>((receiveTime, originalData, data) =>
            {
                onMessage(
                    new DataEvent<BitMEXNotification[]>(Exchange, data.Data, receiveTime, originalData)
                        .WithUpdateType(data.Action == "partial" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithStreamId(data.Table)
                    );
            });
            var subscription = new BitMEXSubscription<BitMEXNotification[]>(_logger, this, "publicNotifications", null, handler, false);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(Action<DataEvent<BitMEXBalance[]>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DateTime, string?, SocketUpdate<BitMEXBalance[]>>((receiveTime, originalData, data) =>
            {
                DateTime? timestamp = data.Data.Any() ? data.Data.Max(x => x.Timestamp) : null;
                if (data.Action == "update")
                    UpdateTimeOffset(timestamp!.Value);

                onMessage(
                    new DataEvent<BitMEXBalance[]>(Exchange, data.Data, receiveTime, originalData)
                        .WithUpdateType(data.Action == "partial" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithStreamId(data.Table)
                        .WithDataTimestamp(timestamp, GetTimeOffset())
                    );
            });
            var subscription = new BitMEXSubscription<BitMEXBalance[]>(_logger, this, "wallet", null, handler, true);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTransactionUpdatesAsync(Action<DataEvent<BitMEXTransaction[]>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DateTime, string?, SocketUpdate<BitMEXTransaction[]>>((receiveTime, originalData, data) =>
            {
                DateTime? timestamp = data.Data.Any() ? data.Data.Max(x => x.Timestamp) : null;
                if (data.Action == "update")
                    UpdateTimeOffset(timestamp!.Value);

                onMessage(
                    new DataEvent<BitMEXTransaction[]>(Exchange, data.Data, receiveTime, originalData)
                        .WithUpdateType(data.Action == "partial" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithStreamId(data.Table)
                        .WithDataTimestamp(timestamp, GetTimeOffset())
                    );
            });

            var subscription = new BitMEXSubscription<BitMEXTransaction[]>(_logger, this, "transact", null, handler, true);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToPositionUpdatesAsync(Action<DataEvent<BitMEXPosition[]>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DateTime, string?, SocketUpdate<BitMEXPosition[]>>((receiveTime, originalData, data) =>
            {
                DateTime? timestamp = data.Data.Any() ? data.Data.Max(x => x.Timestamp) : null;
                if (data.Action == "update")
                    UpdateTimeOffset(timestamp!.Value);

                onMessage(
                    new DataEvent<BitMEXPosition[]>(Exchange, data.Data, receiveTime, originalData)
                        .WithUpdateType(data.Action == "partial" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithStreamId(data.Table)
                        .WithDataTimestamp(timestamp, GetTimeOffset())
                    );
            });
            var subscription = new BitMEXSubscription<BitMEXPosition[]>(_logger, this, "position", null, handler, true);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToMarginUpdatesAsync(Action<DataEvent<BitMEXMarginStatus>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DateTime, string?, SocketUpdate<BitMEXMarginStatus[]>>((receiveTime, originalData, data) =>
            {
                DateTime? timestamp = data.Data.Any() ? data.Data.Max(x => x.Timestamp) : null;
                if (data.Action == "update")
                    UpdateTimeOffset(timestamp!.Value);

                onMessage(
                    new DataEvent<BitMEXMarginStatus>(Exchange, data.Data.First(), receiveTime, originalData)
                        .WithUpdateType(data.Action == "partial" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithStreamId(data.Table)
                        .WithDataTimestamp(timestamp, GetTimeOffset())
                    );
            });
            var subscription = new BitMEXSubscription<BitMEXMarginStatus[]>(_logger, this, "margin", null, handler, true);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(Action<DataEvent<BitMEXOrder[]>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DateTime, string?, SocketUpdate<BitMEXOrder[]>>((receiveTime, originalData, data) =>
            {
                DateTime? timestamp = data.Data.Any() ? data.Data.Max(x => x.Timestamp) : null;
                if (data.Action == "update")
                    UpdateTimeOffset(timestamp!.Value);

                onMessage(
                    new DataEvent<BitMEXOrder[]>(Exchange, data.Data, receiveTime, originalData)
                        .WithUpdateType(data.Action == "partial" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithStreamId(data.Table)
                        .WithDataTimestamp(timestamp, GetTimeOffset())
                    );
            });
            var subscription = new BitMEXSubscription<BitMEXOrder[]>(_logger, this, "order", null, handler, true);
            return await SubscribeAsync(BaseAddress.AppendPath("realtime"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(Action<DataEvent<BitMEXExecution[]>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DateTime, string?, SocketUpdate<BitMEXExecution[]>>((receiveTime, originalData, data) =>
            {
                DateTime? timestamp = data.Data.Any() ? data.Data.Max(x => x.Timestamp) : null;
                if (data.Action == "update")
                    UpdateTimeOffset(timestamp!.Value);

                onMessage(
                    new DataEvent<BitMEXExecution[]>(Exchange, data.Data, receiveTime, originalData)
                        .WithUpdateType(data.Action == "partial" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithStreamId(data.Table)
                        .WithDataTimestamp(timestamp, GetTimeOffset())
                    );
            });
            var subscription = new BitMEXSubscription<BitMEXExecution[]>(_logger, this, "execution", null, handler, true);
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

        protected override Task<Uri?> GetReconnectUriAsync(ISocketConnection connection)
        {
            if (!connection.HasAuthenticatedSubscription)
                return Task.FromResult<Uri?>(null);
            
            return Task.FromResult(new Uri(GetAddress()!))!;
        }

        /// <inheritdoc />
        public IBitMEXSocketClientExchangeApiShared SharedClient => this;

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => BitMEXExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);
    }
}
