using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BitMEX.Net.Clients;
using System.Linq;

namespace BitMEX.Net.UnitTests
{
    [TestFixture]
    public class RestRequestTests
    {
        [Test]
        public async Task ValidateExchangeDataCalls()
        {
            var client = new BitMEXRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "456");
            });
            var tester = new RestRequestValidator<BitMEXRestClient>(client, "Endpoints/Exchange/ExchangeData", "https://www.bitmex.com", IsAuthenticated, stjCompare: true);
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetActiveSymbolsAsync(), "GetActiveSymbols");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetActiveIntervalsAsync(), "GetActiveIntervals");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetSymbolVolumesAsync(), "GetSymbolVolumes");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetTradesAsync("ETHUSDT"), "GetTrades");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetKlinesAsync("ETHUSDT", Enums.BinPeriod.OneDay), "GetKlines");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetExchangeStatsAsync(), "GetExchangeStats");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetSettlementHistoryAsync("ETHUSDT"), "GetSettlementHistory");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetBookTickerHistoryAsync("ETHUSDT"), "GetBookTickerHistory");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetAggregatedBookTickerHistoryAsync("ETHUSDT", Enums.BinPeriod.FiveMinutes), "GetAggregatedBookTickerHistory");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetOrderBookAsync("ETHUSDT", 25), "GetOrderBook");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetInsuranceAsync("XBT"), "GetInsurance");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetFundingHistoryAsync("ETHUSDT"), "GetFundingHistory");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetAnnouncementsAsync(), "GetAnnouncements");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetAssetsAsync(), "GetAssets");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetAssetNetworksAsync(), "GetAssetNetworks");

        }

        private bool IsAuthenticated(WebCallResult result)
        {
            return result.RequestHeaders.Any(x => x.Key == "api-signature");
        }
    }
}
