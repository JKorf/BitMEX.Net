using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.JsonNet;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using BitMEX.Net.Clients;

namespace BitMEX.Net.UnitTests
{
    [TestFixture()]
    public class BitMEXRestClientTests
    {
        [Test]
        public void CheckSignatureExample1()
        {
            var authProvider = new BitMEXAuthenticationProvider(new ApiCredentials("XXX", "XXX"));
            var client = (RestApiClient)new BitMEXRestClient().SpotApi;

            CryptoExchange.Net.Testing.TestHelpers.CheckSignature(
                client,
                authProvider,
                HttpMethod.Post,
                "/api/v3/order",
                (uriParams, bodyParams, headers) =>
                {
                    return bodyParams["signature"].ToString();
                },
                "c8db56825ae71d6d79447849e617115f4a920fa2acdcab2b053c4b2838bd6b71",
                new Dictionary<string, object>
                {
                    { "symbol", "LTCBTC" },
                },
                DateTimeConverter.ParseFromLong(1499827319559),
                true,
                false);
        }

        [Test]
        public void CheckInterfaces()
        {
            CryptoExchange.Net.Testing.TestHelpers.CheckForMissingRestInterfaces<BitMEXRestClient>();
            CryptoExchange.Net.Testing.TestHelpers.CheckForMissingSocketInterfaces<BitMEXSocketClient>();
        }
    }
}
