using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using BitMEX.Net.Interfaces.Clients.ExchangeApi;
using BitMEX.Net.Objects.Models;
using BitMEX.Net.Objects.Options;
using BitMEX.Net.Objects.Sockets.Subscriptions;

namespace BitMEX.Net.Clients.ExchangeApi
{
    /// <summary>
    /// Client providing access to the BitMEX Exchange websocket Api
    /// </summary>
    internal partial class BitMEXSocketClientExchangeApi : SocketApiClient, IBitMEXSocketClientExchangeApi
    {
        #region fields
        private static readonly MessagePath _idPath = MessagePath.Get().Property("id");
        #endregion

        #region constructor/destructor

        /// <summary>
        /// ctor
        /// </summary>
        internal BitMEXSocketClientExchangeApi(ILogger logger, BitMEXSocketOptions options) :
            base(logger, options.Environment.SocketClientAddress!, options, options.ExchangeOptions)
        {
        }
        #endregion

        /// <inheritdoc />
        protected override IByteMessageAccessor CreateAccessor() => new SystemTextJsonByteMessageAccessor();
        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer();

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new BitMEXAuthenticationProvider(credentials);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToXXXUpdatesAsync(Action<DataEvent<BitMEXModel>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BitMEXSubscription<BitMEXModel>(_logger, new [] { "XXX" }, onMessage, false);
            return await SubscribeAsync(subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override string? GetListenerIdentifier(IMessageAccessor message)
        {
            return message.GetValue<string>(_idPath);
        }

        /// <inheritdoc />
        protected override Task<Query?> GetAuthenticationRequestAsync(SocketConnection connection) => Task.FromResult<Query?>(null);

        /// <inheritdoc />
        public IBitMEXSocketClientExchangeApiShared SharedClient => this;

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => BitMEXExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);
    }
}
