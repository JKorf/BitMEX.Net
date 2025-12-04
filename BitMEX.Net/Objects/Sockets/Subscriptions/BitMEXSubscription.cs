using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using BitMEX.Net.Objects.Models;
using BitMEX.Net.Objects.Internal;
using System.Linq;
using CryptoExchange.Net.Clients;

namespace BitMEX.Net.Objects.Sockets.Subscriptions
{
    /// <inheritdoc />
    internal class BitMEXSubscription<T> : Subscription
    {
        private readonly SocketApiClient _client;
        private readonly Action<DateTime, string?, SocketUpdate<T>> _handler;

        private readonly string[] _topics;

        /// <summary>
        /// ctor
        /// </summary>
        public BitMEXSubscription(ILogger logger, SocketApiClient client, string topic, string[]? symbols, Action<DateTime, string?, SocketUpdate<T>> handler, bool auth) : base(logger, auth)
        {
            _client = client;
            _handler = handler;
            _topics = symbols == null ? [topic] : symbols.Select(x => $"{topic}:{x}").ToArray();


            MessageMatcher = MessageMatcher.Create(_topics.Select(x => new MessageHandlerLink<SocketUpdate<T>>("upd" + x.Replace(":", ""), DoHandleMessage)).ToArray());
            MessageRouter = MessageRouter.CreateWithOptionalTopicFilters<SocketUpdate<T>>("upd" + topic, symbols, DoHandleMessage);
        }
            
        /// <inheritdoc />
        protected override Query? GetSubQuery(SocketConnection connection)
        {
            return new BitMEXQuery<SocketResponse>(_client, new SocketCommand
            {
                Operation = "subscribe",
                Parameters = _topics
            }, false, 1);
        }

        /// <inheritdoc />
        protected override Query? GetUnsubQuery(SocketConnection connection)
        {
            return new BitMEXQuery<SocketResponse>(_client, new SocketCommand
            {
                Operation = "unsubscribe",
                Parameters = _topics
            }, false, 1);
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, SocketUpdate<T> message)
        {
            _handler.Invoke(receiveTime, originalData, message);
            return CallResult.SuccessResult;
        }
    }
}
