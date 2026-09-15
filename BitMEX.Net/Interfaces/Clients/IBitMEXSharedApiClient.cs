using BitMEX.Net.Interfaces.Clients.ExchangeApi;
using CryptoExchange.Net.SharedApis;

namespace BitMEX.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for the shared REST and WebSocket API implementations of BitMEX
    /// </summary>
    public interface IBitMEXSharedApiClient : ISharedApiClientBase
    {
        /// <summary>
        /// REST shared API implementations
        /// </summary>
        IBitMEXRestClientExchangeSharedApi Rest { get; }

        /// <summary>
        /// WebSocket shared API implementations
        /// </summary>
        IBitMEXSocketClientExchangeSharedApi Socket { get; }
    }
}
