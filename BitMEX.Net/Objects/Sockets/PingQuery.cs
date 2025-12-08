using CryptoExchange.Net.Sockets;
using System;

namespace BitMEX.Net.Objects.Sockets
{
    internal class PingQuery : Query<string>
    {
        public PingQuery() : base("ping", false, 0)
        {
            RequestTimeout = TimeSpan.FromSeconds(5);

            MessageMatcher = MessageMatcher.Create<string>("pong");
            MessageRouter = MessageRouter.CreateWithoutHandler<string>("pong");
        }

    }
}
