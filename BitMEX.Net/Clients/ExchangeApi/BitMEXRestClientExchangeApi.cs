using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

namespace BitMEX.Net.Clients.ExchangeApi
{
    /// <inheritdoc cref="IBitMEXRestClientExchangeApi" />
    internal partial class BitMEXRestClientExchangeApi : RestApiClient, IBitMEXRestClientExchangeApi
    {
        #region fields 
        internal static TimeSyncState _timeSyncState = new TimeSyncState("Exchange Api");

        public new BitMEXRestOptions ClientOptions => (BitMEXRestOptions)base.ClientOptions;

        protected override ErrorMapping ErrorMapping => BitMEXErrors.RestErrors;

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
        internal BitMEXRestClientExchangeApi(ILogger logger, HttpClient? httpClient, BitMEXRestOptions options)
            : base(logger, httpClient, options.Environment.RestClientAddress, options, options.ExchangeOptions)
        {
            Account = new BitMEXRestClientExchangeApiAccount(this);
            ExchangeData = new BitMEXRestClientExchangeApiExchangeData(logger, this);
            Trading = new BitMEXRestClientExchangeApiTrading(logger, this);
        }
        #endregion

        /// <inheritdoc />
        protected override IStreamMessageAccessor CreateAccessor() => new SystemTextJsonStreamMessageAccessor(SerializerOptions.WithConverters(BitMEXExchange._serializerContext));
        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(BitMEXExchange._serializerContext));

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
            if (parameters?.TryGetValue("filter", out var filter) == true)
                parameters["filter"] = (_serializer ??= (IStringMessageSerializer)CreateSerializer()).Serialize(filter);
            if (parameters?.TryGetValue("columns", out var columns) == true)
                parameters["columns"] = (_serializer ??= (IStringMessageSerializer)CreateSerializer()).Serialize(columns);

            var result = await base.SendAsync<T>(baseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);

            // Optional response checking

            return result;
        }

        /// <inheritdoc />
        protected override Error ParseErrorResponse(int httpStatusCode, KeyValuePair<string, string[]>[] responseHeaders, IMessageAccessor accessor, Exception? exception)
        {
            if (!accessor.IsValid)
            {
                if (httpStatusCode == 401)
                    return new ServerError(new ErrorInfo(ErrorType.Unauthorized, "Unauthorized"), exception: exception);

                return new ServerError(ErrorInfo.Unknown, exception: exception);
            }

            var message = accessor.GetValue<string>(MessagePath.Get().Property("error").Property("message"));
            var name = accessor.GetValue<string>(MessagePath.Get().Property("error").Property("name"));
            var details = accessor.GetValue<string?>(MessagePath.Get().Property("error").Property("details"));
            if (name == null)
                return new ServerError(ErrorInfo.Unknown, exception: exception);

            if (details != null && int.TryParse(details, out var intCode))
                return new ServerError(intCode, GetErrorInfo(intCode, message));

            return new ServerError(name, GetErrorInfo(name, message), exception: exception);
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
