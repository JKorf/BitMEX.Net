using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using BitMEX.Net.Clients;
using BitMEX.Net.Objects.Options;

namespace BitMEX.Net.UnitTests
{
    [NonParallelizable]
    public class BitMEXRestIntegrationTests : RestIntergrationTest<BitMEXRestClient>
    {
        public override bool Run { get; set; } = true;

        public override BitMEXRestClient GetClient(ILoggerFactory loggerFactory)
        {
            var key = Environment.GetEnvironmentVariable("APIKEY");
            var sec = Environment.GetEnvironmentVariable("APISECRET");

            Authenticated = key != null && sec != null;
            return new BitMEXRestClient(null, loggerFactory, Options.Create(new BitMEXRestOptions
            {
                AutoTimestamp = false,
                OutputOriginalData = true,
                ApiCredentials = Authenticated ? new CryptoExchange.Net.Authentication.ApiCredentials(key, sec) : null
            }));
        }

        [Test]
        public async Task TestErrorResponseParsing()
        {
            if (!ShouldRun())
                return;

            var result = await CreateClient().ExchangeApi.ExchangeData.GetTradesAsync("ETHUSDT", limit: 100000);

            Assert.That(result.Success, Is.False);
            Assert.That(result.Error.Code, Is.EqualTo(400));
            Assert.That(result.Error.Message.StartsWith("ValidationError"), Is.True);
        }

        [Test]
        public async Task TestSpotExchangeData()
        {
            //await RunAndCheckResult(client => client.SpotApi.ExchangeData.PingAsync(CancellationToken.None), false);
        }
    }
}
