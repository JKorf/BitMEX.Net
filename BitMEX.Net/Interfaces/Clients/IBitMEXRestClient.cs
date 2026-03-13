using BitMEX.Net.Interfaces.Clients.ExchangeApi;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.Objects.Options;

namespace BitMEX.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the BitMEX Rest API. 
    /// </summary>
    public interface IBitMEXRestClient : IRestClient<BitMEXCredentials>
    {

        /// <summary>
        /// Exchange API endpoints
        /// </summary>
        /// <see cref="IBitMEXRestClientExchangeApi"/>
        public IBitMEXRestClientExchangeApi ExchangeApi { get; }

    }
}
