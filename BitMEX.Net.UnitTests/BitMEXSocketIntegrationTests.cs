using BitMEX.Net.Clients;
using BitMEX.Net.Objects.Models;
using BitMEX.Net.Objects.Options;
using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace BitMEX.Net.UnitTests
{
    [NonParallelizable]
    internal class BitMEXSocketIntegrationTests : SocketIntegrationTest<BitMEXSocketClient>
    {
        public override bool Run { get; set; } = false;

        public BitMEXSocketIntegrationTests()
        {
        }

        public override BitMEXSocketClient GetClient(ILoggerFactory loggerFactory)
        {
            var key = Environment.GetEnvironmentVariable("APIKEY");
            var sec = Environment.GetEnvironmentVariable("APISECRET");

            Authenticated = key != null && sec != null;
            return new BitMEXSocketClient(Options.Create(new BitMEXSocketOptions
            {
                OutputOriginalData = true,
                ApiCredentials = Authenticated ? new CryptoExchange.Net.Authentication.ApiCredentials(key, sec) : null
            }), loggerFactory);
        }

        [Test]
        public async Task TestSubscriptions()
        {
            await RunAndCheckUpdate<BitMEXBalance[]>((client, updateHandler) => client.ExchangeApi.SubscribeToBalanceUpdatesAsync(default , default), false, true);
            await RunAndCheckUpdate<BitMEXBookTicker>((client, updateHandler) => client.ExchangeApi.SubscribeToBookTickerUpdatesAsync("ETHUSDT", updateHandler, default), true, false);
        } 
    }
}
