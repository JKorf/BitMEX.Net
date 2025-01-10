using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using BitMEX.Net.Clients;
using BitMEX.Net.Objects.Options;
using System.Threading;

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
        public async Task TestAccount()
        {
            await RunAndCheckResult(client => client.ExchangeApi.Account.GetUserEventsAsync(default, default, default), false);
            await RunAndCheckResult(client => client.ExchangeApi.Account.GetAccountInfoAsync(default), false);
            await RunAndCheckResult(client => client.ExchangeApi.Account.GetFeesAsync(default), false);
            await RunAndCheckResult(client => client.ExchangeApi.Account.GetMarginStatusAsync(default, default), false);
            await RunAndCheckResult(client => client.ExchangeApi.Account.GetQuoteFillRatioAsync(default, default), false);
            await RunAndCheckResult(client => client.ExchangeApi.Account.GetTradingVolumeAsync(default), false);
            await RunAndCheckResult(client => client.ExchangeApi.Account.GetBalancesAsync(default, default), false);
            await RunAndCheckResult(client => client.ExchangeApi.Account.GetBalanceHistoryAsync(default, default, default, default, default, default), false);
            await RunAndCheckResult(client => client.ExchangeApi.Account.GetBalanceSummaryAsync(default, default, default, default), false);
            await RunAndCheckResult(client => client.ExchangeApi.Account.GetSavedAddressesAsync(default), false);
            await RunAndCheckResult(client => client.ExchangeApi.Account.GetAddressBookSettingsAsync(default), false);
            await RunAndCheckResult(client => client.ExchangeApi.Account.GetApiKeyInfoAsync(default), false);
        }

        [Test]
        public async Task TestExchangeData()
        {
            await RunAndCheckResult(client => client.ExchangeApi.ExchangeData.GetServerTimeAsync(default), false);
            await RunAndCheckResult(client => client.ExchangeApi.ExchangeData.GetActiveSymbolsAsync(default), false);
            await RunAndCheckResult(client => client.ExchangeApi.ExchangeData.GetSymbolsAsync(default, default, default, default, default, default, default, default, default, default), false);
            await RunAndCheckResult(client => client.ExchangeApi.ExchangeData.GetActiveIntervalsAsync(default), false);
            await RunAndCheckResult(client => client.ExchangeApi.ExchangeData.GetCompositeIndexesAsync(".BXBT", default, default, default, default, default, default, default, default), false);
            await RunAndCheckResult(client => client.ExchangeApi.ExchangeData.GetIndicesAsync(default), false);
            await RunAndCheckResult(client => client.ExchangeApi.ExchangeData.GetSymbolVolumesAsync(default), false);
            await RunAndCheckResult(client => client.ExchangeApi.ExchangeData.GetTradesAsync("ETHUSDT", default, default, default, default, default, default, default, default, default), false);
            await RunAndCheckResult(client => client.ExchangeApi.ExchangeData.GetKlinesAsync("ETHUSDT", Enums.BinPeriod.OneDay, default, default, default, default, default, default, default, default, default, default), false);
            await RunAndCheckResult(client => client.ExchangeApi.ExchangeData.GetExchangeStatsAsync(default), false);
            await RunAndCheckResult(client => client.ExchangeApi.ExchangeData.GetExchangeStatHistoryAsync(default), false);
            await RunAndCheckResult(client => client.ExchangeApi.ExchangeData.GetExchangeStatHistoryUSDAsync(default), false);
            await RunAndCheckResult(client => client.ExchangeApi.ExchangeData.GetSettlementHistoryAsync("ETH_USDT", default, default, default, default, default, default, default, default, default), false);
            await RunAndCheckResult(client => client.ExchangeApi.ExchangeData.GetBookTickerHistoryAsync("ETH_USDT", default, default, default, default, default, default, default, default, default), false);
            await RunAndCheckResult(client => client.ExchangeApi.ExchangeData.GetAggregatedBookTickerHistoryAsync("ETH_USDT", Enums.BinPeriod.OneDay, default, default, default, default, default, default, default, default, default, default), false);
            await RunAndCheckResult(client => client.ExchangeApi.ExchangeData.GetOrderBookAsync("ETH_USDT", 5, default), false);
            await RunAndCheckResult(client => client.ExchangeApi.ExchangeData.GetInsuranceAsync(default, default, default, default, default, default, default, default, default, default), false);
            await RunAndCheckResult(client => client.ExchangeApi.ExchangeData.GetFundingHistoryAsync(default, default, default, default, default, default, default, default, default, default), false);
            await RunAndCheckResult(client => client.ExchangeApi.ExchangeData.GetAnnouncementsAsync(default, default), false);
            await RunAndCheckResult(client => client.ExchangeApi.ExchangeData.GetUrgentAnnouncementsAsync(default, default), false);
            await RunAndCheckResult(client => client.ExchangeApi.ExchangeData.GetAssetsAsync(default), false);
            await RunAndCheckResult(client => client.ExchangeApi.ExchangeData.GetAssetNetworksAsync(default), false);
            await RunAndCheckResult(client => client.ExchangeApi.ExchangeData.GetLiquidationsAsync(default, default, default, default, default, default, default, default, default, default), false);
        }

        [Test]
        public async Task TestTrading()
        {
            await RunAndCheckResult(client => client.ExchangeApi.Trading.GetUserExecutionsAsync(default, default, default, default, default, default, default, default, default, default), false);
            await RunAndCheckResult(client => client.ExchangeApi.Trading.GetOrdersAsync(default, default, default, default, default, default, default, default, default, default), false);
            await RunAndCheckResult(client => client.ExchangeApi.Trading.GetUserTradesAsync(default, default, default, default, default, default, default, default, default, default, default), false);
            await RunAndCheckResult(client => client.ExchangeApi.Trading.GetPositionsAsync(default, default, default, default), false);
        }
    }
}
