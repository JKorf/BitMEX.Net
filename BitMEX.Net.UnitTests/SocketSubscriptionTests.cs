using CryptoExchange.Net.Testing;
using NUnit.Framework;
using System.Threading.Tasks;
using BitMEX.Net.Clients;
using BitMEX.Net.Objects.Models;

namespace BitMEX.Net.UnitTests
{
    [TestFixture]
    public class SocketSubscriptionTests
    {
        [Test]
        public async Task ValidateSpotExchangeDataSubscriptions()
        {
            var client = new BitMEXSocketClient(opts =>
            {
                opts.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "456");
            });
            var tester = new SocketSubscriptionValidator<BitMEXSocketClient>(client, "Subscriptions/Spot", "XXX", stjCompare: true);
            //await tester.ValidateAsync<BitMEXModel>((client, handler) => client.SpotApi.SubscribeToXXXUpdatesAsync(handler), "XXX");
        }
    }
}
