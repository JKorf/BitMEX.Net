using BitMEX.Net.Interfaces.Clients.ExchangeApi;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.Objects.Options;

namespace BitMEX.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the BitMEX websocket API
    /// </summary>
    public interface IBitMEXSocketClient : ISocketClient<BitMEXCredentials>
    {
        /// <summary>
        /// Exchange API endpoints
        /// </summary>
        /// <see cref="IBitMEXSocketClientExchangeApi"/>
        public IBitMEXSocketClientExchangeApi ExchangeApi { get; }
    }
}
