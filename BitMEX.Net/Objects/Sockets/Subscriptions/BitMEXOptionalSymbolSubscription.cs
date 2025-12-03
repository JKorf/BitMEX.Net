//using CryptoExchange.Net.Interfaces;
//using CryptoExchange.Net.Objects;
//using CryptoExchange.Net.Objects.Sockets;
//using CryptoExchange.Net.Sockets;
//using Microsoft.Extensions.Logging;
//using System;
//using BitMEX.Net.Objects.Internal;
//using System.Linq;
//using CryptoExchange.Net.Clients;

//namespace BitMEX.Net.Objects.Sockets.Subscriptions
//{
//    /// <inheritdoc />
//    internal class BitMEXOptionalSymbolSubscription<T> : Subscription<SocketResponse, SocketResponse> where T : ISymbolModel
//    {
//        private readonly SocketApiClient _client;
//        private readonly Action<DateTime, string?, SocketUpdate<T[]>> _handler;

//        private readonly string[] _topics;
//        private readonly string[]? _symbols;

//        /// <summary>
//        /// ctor
//        /// </summary>
//        public BitMEXOptionalSymbolSubscription(ILogger logger, SocketApiClient client, string topic, string[]? filters, string[]? symbols, Action<DateTime, string?, SocketUpdate<T[]>> handler, bool auth) : base(logger, auth)
//        {
//            _client = client;
//            _handler = handler;
//            _symbols = symbols;
//            if (symbols != null)
//                _topics = symbols.Select(x => topic + ":" + x).ToArray();
//            else if(filters != null)
//                _topics = filters.Select(x => topic + ":" + x).ToArray();
//            else
//                _topics = [topic];

//            MessageMatcher = MessageMatcher.Create<SocketUpdate<T[]>>("upd" + topic, DoHandleMessage);
//            MessageRouter = MessageRouter.Create<SocketUpdate<T[]>>("upd" + topic, DoHandleMessage);
//        }

//        /// <inheritdoc />
//        protected override Query? GetSubQuery(SocketConnection connection)
//        {
//            return new BitMEXQuery<SocketResponse>(_client, new SocketCommand
//            {
//                Operation = "subscribe",
//                Parameters = _topics
//            }, false, 1);
//        }

//        /// <inheritdoc />
//        protected override Query? GetUnsubQuery(SocketConnection connection)
//        {
//            return new BitMEXQuery<SocketResponse>(_client, new SocketCommand
//            {
//                Operation = "unsubscribe",
//                Parameters = _topics
//            }, false, 1);
//        }

//        /// <inheritdoc />
//        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, SocketUpdate<T[]> message)
//        {
//            if (_symbols?.Contains(message.Data.First().Symbol) == false)
//                return CallResult.SuccessResult;

//            _handler.Invoke(receiveTime, originalData, message);
//            return CallResult.SuccessResult;
//        }
//    }
//}
