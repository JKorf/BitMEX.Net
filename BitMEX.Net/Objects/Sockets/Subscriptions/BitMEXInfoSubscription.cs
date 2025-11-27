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
    internal class BitMEXInfoSubscription : SystemSubscription
    {
        public BitMEXInfoSubscription(ILogger logger) : base(logger, false)
        {
            MessageMatcher = MessageMatcher.Create<InfoUpdate>("info");
            MessageRouter = MessageRouter.Create<InfoUpdate>("info");
        }
    }
}
