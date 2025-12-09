using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using BitMEX.Net.Clients;
using BitMEX.Net.Objects.Options;
using System.Threading;
using BitMEX.Net.SymbolOrderBooks;

namespace BitMEX.Net.UnitTests
{
    [NonParallelizable]
    public class BitMEXRestIntegrationTests : RestIntegrationTest<BitMEXRestClient>
    {
        public override bool Run { get; set; } = true;

        public override BitMEXRestClient GetClient(ILoggerFactory loggerFactory, bool useNewDeserialization)
        {
            var key = Environment.GetEnvironmentVariable("APIKEY");
            var sec = Environment.GetEnvironmentVariable("APISECRET");

            Authenticated = key != null && sec != null;
            return new BitMEXRestClient(null, loggerFactory, Options.Create(new BitMEXRestOptions
            {
                AutoTimestamp = false,
                OutputOriginalData = true,
                UseUpdatedDeserialization = useNewDeserialization,
                ApiCredentials = Authenticated ? new CryptoExchange.Net.Authentication.ApiCredentials(key, sec) : null
            }));
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task TestErrorResponseParsing(bool useNewDeserialization)
        {
            if (!ShouldRun())
                return;

            var result = await CreateClient(useNewDeserialization).ExchangeApi.ExchangeData.GetTradesAsync("ETHUSDT", limit: 100000);

            Assert.That(result.Success, Is.False);
            Assert.That(result.Error.ErrorCode, Is.EqualTo("ValidationError"));
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task TestAccount(bool useUpdatedDeserialization)
        {
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.Account.GetUserEventsAsync(default, default, default), true);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.Account.GetAccountInfoAsync(default), true);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.Account.GetFeesAsync(default), true);
            //await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.Account.GetMarginStatusAsync(default, default), true);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.Account.GetQuoteFillRatioAsync(default, default), true);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.Account.GetTradingVolumeAsync(default), true);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.Account.GetBalancesAsync(default, default), true);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.Account.GetBalanceHistoryAsync(default, default, default, default, default, default), true);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.Account.GetBalanceSummaryAsync(default, default, default, default), true);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.Account.GetSavedAddressesAsync(default), true);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.Account.GetAddressBookSettingsAsync(default), true);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.Account.GetApiKeyInfoAsync(default), true);
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task TestExchangeData(bool useUpdatedDeserialization)
        {
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.ExchangeData.GetServerTimeAsync(default), false);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.ExchangeData.GetActiveSymbolsAsync(default), false);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.ExchangeData.GetSymbolsAsync(default, default, default, default, default, default, default, default, default, default), false);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.ExchangeData.GetActiveIntervalsAsync(default), false);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.ExchangeData.GetCompositeIndexesAsync(".BXBT", default, default, default, default, default, default, default, default), false);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.ExchangeData.GetIndicesAsync(default), false);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.ExchangeData.GetSymbolVolumesAsync(default), false);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.ExchangeData.GetTradesAsync("ETHUSDT", default, default, default, default, default, default, default, default, default), false);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.ExchangeData.GetKlinesAsync("ETHUSDT", Enums.BinPeriod.OneDay, default, default, default, default, default, default, default, default, default, default), false);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.ExchangeData.GetExchangeStatsAsync(default), false);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.ExchangeData.GetExchangeStatHistoryAsync(default), false);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.ExchangeData.GetExchangeStatHistoryUSDAsync(default), false);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.ExchangeData.GetSettlementHistoryAsync("ETH_USDT", default, default, default, default, default, default, default, default, default), false);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.ExchangeData.GetBookTickerHistoryAsync("ETH_USDT", default, default, default, default, default, default, default, default, default), false);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.ExchangeData.GetAggregatedBookTickerHistoryAsync("ETH_USDT", Enums.BinPeriod.OneDay, default, default, default, default, default, default, default, default, default, default), false);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.ExchangeData.GetOrderBookAsync("ETH_USDT", 5, default), false);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.ExchangeData.GetInsuranceAsync(default, default, default, default, default, default, default, default, default, default), false);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.ExchangeData.GetFundingHistoryAsync(default, default, default, default, default, default, default, default, default, default), false);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.ExchangeData.GetAnnouncementsAsync(default, default), false);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.ExchangeData.GetUrgentAnnouncementsAsync(default, default), false);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.ExchangeData.GetAssetsAsync(default), false);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.ExchangeData.GetAssetNetworksAsync(default), false);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.ExchangeData.GetLiquidationsAsync(default, default, default, default, default, default, default, default, default, default), false);
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task TestTrading(bool useUpdatedDeserialization)
        {
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.Trading.GetUserExecutionsAsync(default, default, default, default, default, default, default, default, default, default), true);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.Trading.GetOrdersAsync(default, default, default, default, default, default, default, default, default, default), true);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.Trading.GetUserTradesAsync(default, default, default, default, default, default, default, default, default, default, default), true);
            await RunAndCheckResult(useUpdatedDeserialization, client => client.ExchangeApi.Trading.GetPositionsAsync(default, default, default, default), true);
        }

        [Test]
        public async Task TestOrderBooks()
        {
            await TestOrderBook(new BitMEXSymbolOrderBook("ETH_USDT"));
        }
    }
}
