using BitMEX.Net.Interfaces.Clients;
using BitMEX.Net.Objects.Options;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Net.Http;

namespace BitMEX.Net.Clients
{
    /// <inheritdoc />
    public class BitMEXUserClientProvider : UserClientProvider<
        IBitMEXRestClient,
        IBitMEXSocketClient,
        BitMEXRestOptions,
        BitMEXSocketOptions,
        BitMEXCredentials,
        BitMEXEnvironment
        >, IBitMEXUserClientProvider
    {
        /// <inheritdoc />
        public override string ExchangeName => BitMEXExchange.ExchangeName;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="optionsDelegate">Options to use for created clients</param>
        public BitMEXUserClientProvider(Action<BitMEXOptions>? optionsDelegate = null)
            : this(null, null, Options.Create(ApplyOptionsDelegate(optionsDelegate).Rest), Options.Create(ApplyOptionsDelegate(optionsDelegate).Socket))
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        public BitMEXUserClientProvider(
            HttpClient? httpClient,
            ILoggerFactory? loggerFactory,
            IOptions<BitMEXRestOptions> restOptions,
            IOptions<BitMEXSocketOptions> socketOptions)
            : base(httpClient, loggerFactory, restOptions, socketOptions)
        {
        }

        /// <inheritdoc />
        protected override IBitMEXRestClient ConstructRestClient(HttpClient client, ILoggerFactory? loggerFactory, IOptions<BitMEXRestOptions> options)
            => new BitMEXRestClient(client, loggerFactory, options);
        /// <inheritdoc />
        protected override IBitMEXSocketClient ConstructSocketClient(ILoggerFactory? loggerFactory, IOptions<BitMEXSocketOptions> options)
            => new BitMEXSocketClient(options, loggerFactory);
    }
}
