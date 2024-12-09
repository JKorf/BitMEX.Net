using BitMEX.Net.Interfaces.Clients.ExchangeApi;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces;

namespace BitMEX.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the BitMEX websocket API
    /// </summary>
    public interface IBitMEXSocketClient : ISocketClient
    {
        
        /// <summary>
        /// Exchange API endpoints
        /// </summary>
        public IBitMEXSocketClientExchangeApi ExchangeApi { get; }


        /// <summary>
        /// Set the API credentials for this client. All Api clients in this client will use the new credentials, regardless of earlier set options.
        /// </summary>
        /// <param name="credentials">The credentials to set</param>
        void SetApiCredentials(ApiCredentials credentials);
    }
}
