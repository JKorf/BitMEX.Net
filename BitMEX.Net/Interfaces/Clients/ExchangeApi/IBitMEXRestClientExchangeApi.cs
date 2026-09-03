using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces.Clients;
using System;

namespace BitMEX.Net.Interfaces.Clients.ExchangeApi
{
    /// <summary>
    /// BitMEX Exchange API endpoints
    /// </summary>
    public interface IBitMEXRestClientExchangeApi : IRestApiClient<BitMEXCredentials>, IDisposable
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
        /// [V1] Get the shared rest requests client. For new implementations prefer using <see cref="SharedApi"/>
        /// </summary>
        public IBitMEXRestClientExchangeApiShared SharedClient { get; }
        /// <summary>
        /// [V2] Gets the aggregate Shared API interface. Shared APIs provide a common,
        /// exchange-independent contract for accessing functionality across different
        /// exchange client libraries.
        /// </summary>
        public IBitMEXRestClientExchangeSharedApi SharedApi { get; }
    }
}
