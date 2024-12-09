using CryptoExchange.Net.Objects;
using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects.Sockets;
using BitMEX.Net.Objects.Models;

namespace BitMEX.Net.Interfaces.Clients.ExchangeApi
{
    /// <summary>
    /// BitMEX Exchange streams
    /// </summary>
    public interface IBitMEXSocketClientExchangeApi : ISocketApiClient, IDisposable
    {
        /// <summary>
        /// 
        /// <para><a href="XXX" /></para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToXXXUpdatesAsync(Action<DataEvent<BitMEXModel>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Get the shared socket requests client. This interface is shared with other exhanges to allow for a common implementation for different exchanges.
        /// </summary>
        public IBitMEXSocketClientExchangeApiShared SharedClient { get; }
    }
}
