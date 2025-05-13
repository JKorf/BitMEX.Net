using CryptoExchange.Net.Testing;
using NUnit.Framework;
using System.Threading.Tasks;
using BitMEX.Net.Clients;
using BitMEX.Net.Objects.Models;
using System.Collections;
using System.Collections.Generic;

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
            var tester = new SocketSubscriptionValidator<BitMEXSocketClient>(client, "Subscriptions/Exchange", "wss://ws.bitmex.com/");
            await tester.ValidateAsync<BitMEXTradeUpdate[]>((client, handler) => client.ExchangeApi.SubscribeToTradeUpdatesAsync("ETH_USDT", handler), "Trades", nestedJsonProperty: "data", ignoreProperties: ["trdType"]);
            await tester.ValidateAsync<BitMEXAggTrade[]>((client, handler) => client.ExchangeApi.SubscribeToKlineUpdatesAsync("ETH_USDT", Enums.BinPeriod.FiveMinutes, handler), "Klines", nestedJsonProperty: "data");
            await tester.ValidateAsync<BitMEXBookTicker>((client, handler) => client.ExchangeApi.SubscribeToBookTickerUpdatesAsync("ETH_USDT", handler), "BookTicker", nestedJsonProperty: "data");
        }
    }
}
