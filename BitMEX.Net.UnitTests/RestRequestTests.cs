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
        public async Task ValidateAccountCalls()
        {
            var client = new BitMEXRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "456");
            });
            var tester = new RestRequestValidator<BitMEXRestClient>(client, "Endpoints/Exchange/Account", "https://www.bitmex.com", IsAuthenticated, stjCompare: true);
            await tester.ValidateAsync(client => client.ExchangeApi.Account.GetUserEventsAsync(), "GetUserEvents", nestedJsonProperty: "userEvents");
            await tester.ValidateAsync(client => client.ExchangeApi.Account.GetAccountInfoAsync(), "GetAccountInfo", ignoreProperties: new List<string> { "notifications", "orderBookBinning" });
            await tester.ValidateAsync(client => client.ExchangeApi.Account.GetFeesAsync(), "GetFees");
            await tester.ValidateAsync(client => client.ExchangeApi.Account.GetBalancesAsync(), "GetBalances");
        }

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
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetLiquidationsAsync(), "GetLiquidations");
        }

        [Test]
        public async Task ValidateTradingCalls()
        {
            var client = new BitMEXRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "456");
            });
            var tester = new RestRequestValidator<BitMEXRestClient>(client, "Endpoints/Exchange/Trading", "https://www.bitmex.com", IsAuthenticated, stjCompare: true);
            await tester.ValidateAsync(client => client.ExchangeApi.Trading.PlaceOrderAsync("ETHUSDT", Enums.OrderSide.Buy, Enums.OrderType.LimitIfTouched), "PlaceOrder", ignoreProperties: new List<string> { "text" });
            await tester.ValidateAsync(client => client.ExchangeApi.Trading.EditOrderAsync("123"), "EditOrder", ignoreProperties: new List<string> { "text" });
            await tester.ValidateAsync(client => client.ExchangeApi.Trading.GetOrdersAsync("123"), "GetOrders", ignoreProperties: new List<string> { "text" });
            await tester.ValidateAsync(client => client.ExchangeApi.Trading.CancelOrdersAsync(["123"]), "CancelOrders", ignoreProperties: new List<string> { "text" });
            await tester.ValidateAsync(client => client.ExchangeApi.Trading.CancelAllOrdersAsync(), "CancelAllOrders", ignoreProperties: new List<string> { "text" });
            await tester.ValidateAsync(client => client.ExchangeApi.Trading.GetUserTradesAsync(), "GetUserTrades", ignoreProperties: new List<string> { "text" });
            await tester.ValidateAsync(client => client.ExchangeApi.Trading.GetPositionsAsync(), "GetPositions");
        }

        private bool IsAuthenticated(WebCallResult result)
        {
            return result.RequestHeaders.Any(x => x.Key == "api-signature");
        }
    }
}
