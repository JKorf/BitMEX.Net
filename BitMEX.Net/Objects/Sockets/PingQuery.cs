using CryptoExchange.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitMEX.Net.Objects.Sockets
{
    internal class PingQuery : Query<string>
    {
        public PingQuery() : base("ping", false, 0)
        {
            MessageMatcher = MessageMatcher.Create<string>("pong");
            RequestTimeout = TimeSpan.FromSeconds(5);
        }

    }
}
