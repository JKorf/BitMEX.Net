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
    internal class BitMEXOptionalSymbolSubscription<T> : Subscription<SocketResponse, SocketResponse> where T : ISymbolModel
    {
        /// <inheritdoc />
        public override HashSet<string> ListenerIdentifiers { get; set; }

        private readonly Action<DataEvent<T[]>> _handler;

        private readonly string[] _topics;
        private readonly string[]? _symbols;
        private readonly string[]? _filters;

        /// <inheritdoc />
        public override Type? GetMessageType(IMessageAccessor message)
        {
            return typeof(SocketUpdate<T[]>);
        }

        /// <summary>
        /// ctor
        /// </summary>
        public BitMEXOptionalSymbolSubscription(ILogger logger, string topic, string[]? filters, string[]? symbols, Action<DataEvent<T[]>> handler, bool auth) : base(logger, auth)
        {
            _handler = handler;
            _symbols = symbols;
            _filters = filters;
            if (symbols != null)
                _topics = symbols.Select(x => topic + ":" + x).ToArray();
            else if(filters != null)
                _topics = filters.Select(x => topic + ":" + x).ToArray();
            else
                _topics = [topic];

            ListenerIdentifiers = new HashSet<string>(["upd" + topic]);
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
        public override CallResult DoHandleMessage(SocketConnection connection, DataEvent<object> message)
        {
            var data = (SocketUpdate<T[]>)message.Data;
            if (_symbols?.Contains(data.Data.First().Symbol) == false)
                return CallResult.SuccessResult;

            _handler.Invoke(message.As(data.Data, data.Table, null, data.Action == "partial" ? SocketUpdateType.Snapshot : SocketUpdateType.Update));
            return CallResult.SuccessResult;
        }
    }
}
