using CryptoExchange.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitMEX.Net.Objects.Sockets
{
    internal class PingQuery : Query<string>
    {
        public override HashSet<string> ListenerIdentifiers { get; set; } = new HashSet<string>() { "pong" };

        public PingQuery() : base("ping", false, 0)
        {
            RequestTimeout = TimeSpan.FromSeconds(5);
        }

    }
}
