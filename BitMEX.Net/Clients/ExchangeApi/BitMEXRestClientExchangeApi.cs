using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BitMEX.Net.Interfaces.Clients.ExchangeApi;
using BitMEX.Net.Objects.Options;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using BitMEX.Net.Clients.MessageHandlers;
using System.Net.Http.Headers;

namespace BitMEX.Net.Clients.ExchangeApi
{
    /// <inheritdoc cref="IBitMEXRestClientExchangeApi" />
    internal partial class BitMEXRestClientExchangeApi : RestApiClient<BitMEXEnvironment, BitMEXAuthenticationProvider, BitMEXCredentials>, IBitMEXRestClientExchangeApi
    {
        #region fields 
        public new BitMEXRestOptions ClientOptions => (BitMEXRestOptions)base.ClientOptions;

        protected override ErrorMapping ErrorMapping => BitMEXErrors.RestErrors;

        protected override IRestMessageHandler MessageHandler { get; } = new BitMexRestMessageHandler(BitMEXErrors.RestErrors);

        private IStringMessageSerializer? _serializer;
        #endregion

        #region Api clients
        /// <inheritdoc />
        public IBitMEXRestClientExchangeApiAccount Account { get; }
        /// <inheritdoc />
        public IBitMEXRestClientExchangeApiExchangeData ExchangeData { get; }
        /// <inheritdoc />
        public IBitMEXRestClientExchangeApiTrading Trading { get; }
        /// <inheritdoc />
        public string ExchangeName => "BitMEX";
        #endregion

        #region constructor/destructor
        internal BitMEXRestClientExchangeApi(ILoggerFactory? loggerFactory, HttpClient? httpClient, BitMEXRestOptions options)
            : base(loggerFactory, BitMEXExchange.Metadata.Id, httpClient, options.Environment.RestClientAddress, options, options.ExchangeOptions)
        {
            Account = new BitMEXRestClientExchangeApiAccount(this);
            ExchangeData = new BitMEXRestClientExchangeApiExchangeData(_logger, this);
            Trading = new BitMEXRestClientExchangeApiTrading(_logger, this);
        }
        #endregion

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(BitMEXExchange._serializerContext));

        /// <inheritdoc />
        protected override BitMEXAuthenticationProvider CreateAuthenticationProvider(BitMEXCredentials credentials)
            => new BitMEXAuthenticationProvider(credentials);

        internal async Task<HttpResult> SendAsync(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            return await base.SendAsync<Unit>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
        }

        internal async Task<HttpResult<T>> SendAsync<T>(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null) where T : class
        {
            if (parameters?.TryGetValue("filter", out var filter) == true)
                parameters["filter"] = (_serializer ??= (IStringMessageSerializer)CreateSerializer()).Serialize(filter);
            if (parameters?.TryGetValue("columns", out var columns) == true)
                parameters["columns"] = (_serializer ??= (IStringMessageSerializer)CreateSerializer()).Serialize(columns);

            return await base.SendAsync<T>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override Task<HttpResult<DateTime>> GetServerTimestampAsync()
            => throw new NotImplementedException();

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null) 
            => BitMEXExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);

        /// <inheritdoc />
        public IBitMEXRestClientExchangeApiShared SharedClient => this;

    }
}
