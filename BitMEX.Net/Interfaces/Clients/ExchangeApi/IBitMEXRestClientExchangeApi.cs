using CryptoExchange.Net.Interfaces.Clients;
using System;

namespace BitMEX.Net.Interfaces.Clients.ExchangeApi
{
    /// <summary>
    /// BitMEX Exchange API endpoints
    /// </summary>
    public interface IBitMEXRestClientExchangeApi : IRestApiClient, IDisposable
    {
        /// <summary>
        /// Endpoints related to account settings, info or actions
        /// </summary>
        /// <see cref="IBitMEXRestClientExchangeApiAccount"/>
        public IBitMEXRestClientExchangeApiAccount Account { get; }

        /// <summary>
        /// Endpoints related to retrieving market and system data
        /// </summary>
        /// <see cref="IBitMEXRestClientExchangeApiExchangeData"/>
        public IBitMEXRestClientExchangeApiExchangeData ExchangeData { get; }

        /// <summary>
        /// Endpoints related to orders and trades
        /// </summary>
        /// <see cref="IBitMEXRestClientExchangeApiTrading"/>
        public IBitMEXRestClientExchangeApiTrading Trading { get; }

        /// <summary>
        /// Get the shared rest requests client. This interface is shared with other exchanges to allow for a common implementation for different exchanges.
        /// </summary>
        public IBitMEXRestClientExchangeApiShared SharedClient { get; }
    }
}
