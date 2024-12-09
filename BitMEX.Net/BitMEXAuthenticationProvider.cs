using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;

namespace BitMEX.Net
{
    internal class BitMEXAuthenticationProvider : AuthenticationProvider
    {
        private static readonly IMessageSerializer _serializer = new SystemTextJsonMessageSerializer();

        public BitMEXAuthenticationProvider(ApiCredentials credentials) : base(credentials)
        {
        }

        public override void AuthenticateRequest(
            RestApiClient apiClient,
            Uri uri,
            HttpMethod method,
            ref IDictionary<string, object>? uriParameters,
            ref IDictionary<string, object>? bodyParameters,
            ref Dictionary<string, string>? headers,
            bool auth,
            ArrayParametersSerialization arraySerialization,
            HttpMethodParameterPosition parameterPosition,
            RequestBodyFormat requestBodyFormat)
        {
            headers = new Dictionary<string, string>() { };

            if (!auth)
                return;

            var timestamp = GetTimestamp(apiClient);
            // Receive window option
            var expires = DateTimeConverter.ConvertToSeconds(timestamp.AddSeconds(5)).ToString()!;
            headers.Add("api-expires", expires);
            headers.Add("api-key", ApiKey);
            var body = bodyParameters == null ? "" : GetSerializedBody(_serializer, bodyParameters);
            var query = uriParameters?.Any() != true ? "" : ("?" + uriParameters.CreateParamString(false, arraySerialization));
            var signStr = $"{method.ToString().ToUpperInvariant()}{uri.AbsolutePath}{query}{expires}{body}";
            headers.Add("api-signature", SignHMACSHA256(signStr, SignOutputType.Hex).ToLowerInvariant());
        }
    }
}
