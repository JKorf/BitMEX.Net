using BitMEX.Net.Clients;
using BitMEX.Net.Objects.Models;
using BitMEX.Net.Objects.Options;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BitMEX.Net.UnitTests
{
    [TestFixture]
    public class SocketSubscriptionTests
    {

        [TestCase(false)]
        [TestCase(true)]
        public async Task ValidateSpotExchangeDataSubscriptions(bool newDeserialization)
        {
            var logger = new LoggerFactory();
            logger.AddProvider(new TraceLoggerProvider());

            var client = new BitMEXSocketClient(Options.Create(new BitMEXSocketOptions
            {
                ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "456", "789"),
                OutputOriginalData = true,
                UseUpdatedDeserialization = newDeserialization
            }), logger);
            var tester = new SocketSubscriptionValidator<BitMEXSocketClient>(client, "Subscriptions/Exchange", "wss://ws.bitmex.com/");
            await tester.ValidateAsync<BitMEXTradeUpdate[]>((client, handler) => client.ExchangeApi.SubscribeToTradeUpdatesAsync("ETH_USDT", handler), "Trades", nestedJsonProperty: "data", ignoreProperties: ["trdType"]);
            await tester.ValidateAsync<BitMEXAggTrade[]>((client, handler) => client.ExchangeApi.SubscribeToKlineUpdatesAsync("ETH_USDT", Enums.BinPeriod.FiveMinutes, handler), "Klines", nestedJsonProperty: "data");
            await tester.ValidateAsync<BitMEXBookTicker>((client, handler) => client.ExchangeApi.SubscribeToBookTickerUpdatesAsync("ETH_USDT", handler), "BookTicker", nestedJsonProperty: "data");
        }
    }
}
