using BitMEX.Net.Interfaces.Clients;
using BitMEX.Net.Interfaces.Clients.ExchangeApi;
using BitMEX.Net.Objects.Options;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Options;

namespace BitMEX.Net.Clients
{
    /// <inheritdoc />
    public class BitMEXSharedApiClient : SharedApiClientBase, IBitMEXSharedApiClient
    {
        /// <inheritdoc />
        public IBitMEXRestClientExchangeSharedApi Rest { get; }
        /// <inheritdoc />
        public IBitMEXSocketClientExchangeSharedApi Socket { get; }

        /// <summary>
        /// ctor
        /// </summary>
        public BitMEXSharedApiClient(
            IBitMEXRestClient restClient,
            IBitMEXSocketClient socketClient,
            IOptions<BitMEXOptions> options)
            : base(options.Value.SharedApi.PreferredTransport,
                    restClient.ExchangeApi.SharedApi,
                    socketClient.ExchangeApi.SharedApi)
        {
            Rest = restClient.ExchangeApi.SharedApi;
            Socket = socketClient.ExchangeApi.SharedApi;
        }
    }
}
