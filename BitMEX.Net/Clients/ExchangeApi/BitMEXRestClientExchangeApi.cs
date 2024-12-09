using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Interfaces.CommonClients;
using BitMEX.Net.Interfaces.Clients.ExchangeApi;
using BitMEX.Net.Objects.Options;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Converters.MessageParsing;

namespace BitMEX.Net.Clients.ExchangeApi
{
    /// <inheritdoc cref="IBitMEXRestClientExchangeApi" />
    internal partial class BitMEXRestClientExchangeApi : RestApiClient, IBitMEXRestClientExchangeApi
    {
        #region fields 
        internal static TimeSyncState _timeSyncState = new TimeSyncState("Exchange Api");
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
        internal BitMEXRestClientExchangeApi(ILogger logger, HttpClient? httpClient, BitMEXRestOptions options)
            : base(logger, httpClient, options.Environment.RestClientAddress, options, options.ExchangeOptions)
        {
            Account = new BitMEXRestClientExchangeApiAccount(this);
            ExchangeData = new BitMEXRestClientExchangeApiExchangeData(logger, this);
            Trading = new BitMEXRestClientExchangeApiTrading(logger, this);
        }
        #endregion

        /// <inheritdoc />
        protected override IStreamMessageAccessor CreateAccessor() => new SystemTextJsonStreamMessageAccessor();
        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer();

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new BitMEXAuthenticationProvider(credentials);

        internal Task<WebCallResult> SendAsync(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
            => SendToAddressAsync(BaseAddress, definition, parameters, cancellationToken, weight);

        internal async Task<WebCallResult> SendToAddressAsync(string baseAddress, RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await base.SendAsync(baseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);

            // Optional response checking

            return result;
        }

        internal Task<WebCallResult<T>> SendAsync<T>(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null) where T : class
            => SendToAddressAsync<T>(BaseAddress, definition, parameters, cancellationToken, weight);

        internal async Task<WebCallResult<T>> SendToAddressAsync<T>(string baseAddress, RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null) where T : class
        {
            var result = await base.SendAsync<T>(baseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);

            // Optional response checking

            return result;
        }

        /// <inheritdoc />
        protected override Error ParseErrorResponse(int httpStatusCode, IEnumerable<KeyValuePair<string, IEnumerable<string>>> responseHeaders, IMessageAccessor accessor)
        {
            if (!accessor.IsJson)
                return new ServerError(accessor.GetOriginalString());

            var message = accessor.GetValue<string>(MessagePath.Get().Property("error").Property("message"));
            var name = accessor.GetValue<string>(MessagePath.Get().Property("error").Property("name"));
            if (name == null)
                return new ServerError(accessor.GetOriginalString());

            return new ServerError($"{name} - {message}");
        }

        /// <inheritdoc />
        protected override Task<WebCallResult<DateTime>> GetServerTimestampAsync()
            => throw new NotImplementedException();

        /// <inheritdoc />
        public override TimeSyncInfo? GetTimeSyncInfo()
            => new TimeSyncInfo(_logger, ApiOptions.AutoTimestamp ?? ClientOptions.AutoTimestamp, ApiOptions.TimestampRecalculationInterval ?? ClientOptions.TimestampRecalculationInterval, _timeSyncState);

        /// <inheritdoc />
        public override TimeSpan? GetTimeOffset()
            => _timeSyncState.TimeOffset;

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null) 
            => BitMEXExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);

        /// <inheritdoc />
        public IBitMEXRestClientExchangeApiShared SharedClient => this;

    }
}
