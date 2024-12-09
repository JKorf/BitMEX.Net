using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BitMEX.Net.Clients;

namespace BitMEX.Net.UnitTests
{
    [TestFixture]
    public class RestRequestTests
    {
        [Test]
        public async Task ValidateExchangeDataAccountCalls()
        {
            var client = new BitMEXRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "456");
            });
            var tester = new RestRequestValidator<BitMEXRestClient>(client, "Endpoints/Spot/ExchangeData", "XXX", IsAuthenticated, stjCompare: true);
            //await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetServerTimeAsync(), "GetServerTime");
           
        }

        private bool IsAuthenticated(WebCallResult result)
        {
            return result.RequestUrl?.Contains("signature") == true || result.RequestBody?.Contains("signature=") == true;
        }
    }
}
