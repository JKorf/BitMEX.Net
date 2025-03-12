using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using BitMEX.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace BitMEX.Net.UnitTests
{
    [TestFixture()]
    public class BitMEXRestClientTests
    {
        [Test]
        public void CheckSignatureExample1()
        {
            var authProvider = new BitMEXAuthenticationProvider(new ApiCredentials("XXX", "XXX"));
            var client = (RestApiClient)new BitMEXRestClient().ExchangeApi;

            CryptoExchange.Net.Testing.TestHelpers.CheckSignature(
                client,
                authProvider,
                HttpMethod.Post,
                "/api/v3/order",
                (uriParams, bodyParams, headers) =>
                {
                    return headers["api-signature"].ToString();
                },
                "f03398fcf2c6e91f48d6b7f5e22de2d4996071613f0e76e130d5a381d5c894c5",
                new Dictionary<string, object>
                {
                    { "symbol", "LTCBTC" },
                },
                DateTimeConverter.ParseFromDouble(1499827319559),
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
