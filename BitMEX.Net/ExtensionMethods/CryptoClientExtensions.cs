using BitMEX.Net.Clients;
using BitMEX.Net.Interfaces.Clients;
using CryptoExchange.Net.Interfaces.Clients;

namespace CryptoExchange.Net.Interfaces
{
    /// <summary>
    /// Extensions for the ICryptoRestClient and ICryptoSocketClient interfaces
    /// </summary>
    public static class CryptoClientExtensions
    {
        /// <summary>
        /// Get the BitMEX REST Api client
        /// </summary>
        /// <param name="baseClient"></param>
        /// <returns></returns>
        public static IBitMEXRestClient BitMEX(this ICryptoRestClient baseClient) => baseClient.TryGet<IBitMEXRestClient>(() => new BitMEXRestClient());

        /// <summary>
        /// Get the BitMEX Websocket Api client
        /// </summary>
        /// <param name="baseClient"></param>
        /// <returns></returns>
        public static IBitMEXSocketClient BitMEX(this ICryptoSocketClient baseClient) => baseClient.TryGet<IBitMEXSocketClient>(() => new BitMEXSocketClient());
    }
}
