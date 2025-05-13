using BitMEX.Net.Objects.Internal;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitMEX.Net.Objects.Sockets.Subscriptions
{
    internal class BitMEXInfoSubscription : SystemSubscription<InfoUpdate>
    {
        public override HashSet<string> ListenerIdentifiers { get; set; } = new HashSet<string>() { "info" };

        public BitMEXInfoSubscription(ILogger logger) : base(logger, false)
        {
        }

        public override CallResult HandleMessage(SocketConnection connection, DataEvent<InfoUpdate> message)
        {
            return CallResult.SuccessResult;
        }
    }
}
