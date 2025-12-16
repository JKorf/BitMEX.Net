using BitMEX.Net.Objects.Internal;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using Microsoft.Extensions.Logging;

namespace BitMEX.Net.Objects.Sockets.Subscriptions
{
    internal class BitMEXInfoSubscription : SystemSubscription
    {
        public BitMEXInfoSubscription(ILogger logger) : base(logger, false)
        {
            MessageMatcher = MessageMatcher.Create<InfoUpdate>("info");
            MessageRouter = MessageRouter.CreateWithoutHandler<InfoUpdate>("info");
        }
    }
}
