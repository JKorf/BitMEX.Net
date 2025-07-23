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

namespace BitMEX.Net.Objects.Sockets.Subscriptions
{
    /// <inheritdoc />
    internal class BitMEXRawSubscription<T> : Subscription<SocketResponse, SocketResponse>
    {
        private readonly Action<DataEvent<T>> _handler;

        private readonly string[] _topics;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="topics"></param>
        /// <param name="handler"></param>
        /// <param name="auth"></param>
        public BitMEXRawSubscription(ILogger logger, string[] topics, Action<DataEvent<T>> handler, bool auth) : base(logger, auth)
        {
            _handler = handler;
            _topics = topics;

            MessageMatcher = MessageMatcher.Create(topics.Select(x => new MessageHandlerLink<T>("upd" + x.Replace(":", ""), DoHandleMessage)).ToArray());
        }

        /// <inheritdoc />
        public override Query? GetSubQuery(SocketConnection connection)
        {
            return new BitMEXQuery<SocketResponse>(new SocketCommand
            {
                Operation = "subscribe",
                Parameters = _topics
            }, false, 1);
        }

        /// <inheritdoc />
        public override Query? GetUnsubQuery()
        {
            return new BitMEXQuery<SocketResponse>(new SocketCommand
            {
                Operation = "unsubscribe",
                Parameters = _topics
            }, false, 1);
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<T> message)
        {
            _handler.Invoke(message.As(message.Data, null!, null, SocketUpdateType.Update));
            return CallResult.SuccessResult;
        }
    }
}
