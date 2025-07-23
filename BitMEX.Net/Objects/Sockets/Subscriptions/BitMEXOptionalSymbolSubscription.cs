using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using System;
using BitMEX.Net.Objects.Internal;
using System.Linq;

namespace BitMEX.Net.Objects.Sockets.Subscriptions
{
    /// <inheritdoc />
    internal class BitMEXOptionalSymbolSubscription<T> : Subscription<SocketResponse, SocketResponse> where T : ISymbolModel
    {

        private readonly Action<DataEvent<T[]>> _handler;

        private readonly string[] _topics;
        private readonly string[]? _symbols;

        /// <summary>
        /// ctor
        /// </summary>
        public BitMEXOptionalSymbolSubscription(ILogger logger, string topic, string[]? filters, string[]? symbols, Action<DataEvent<T[]>> handler, bool auth) : base(logger, auth)
        {
            _handler = handler;
            _symbols = symbols;
            if (symbols != null)
                _topics = symbols.Select(x => topic + ":" + x).ToArray();
            else if(filters != null)
                _topics = filters.Select(x => topic + ":" + x).ToArray();
            else
                _topics = [topic];

            MessageMatcher = MessageMatcher.Create<SocketUpdate<T[]>>("upd" + topic, DoHandleMessage);
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
        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<SocketUpdate<T[]>> message)
        {
            if (_symbols?.Contains(message.Data.Data.First().Symbol) == false)
                return CallResult.SuccessResult;

            _handler.Invoke(message.As(message.Data.Data, message.Data.Table, null, message.Data.Action == "partial" ? SocketUpdateType.Snapshot : SocketUpdateType.Update));
            return CallResult.SuccessResult;
        }
    }
}
