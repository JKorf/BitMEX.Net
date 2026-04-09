using BitMEX.Net.Objects.Internal;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;
using Microsoft.Extensions.Logging;

namespace BitMEX.Net.Objects.Sockets.Subscriptions
{
    internal class BitMEXInfoSubscription : SystemSubscription
    {
        public BitMEXInfoSubscription(ILogger logger) : base(logger, false)
        {
            MessageRouter = MessageRouter.CreateWithoutHandler<InfoUpdate>("info");
        }
    }
}
