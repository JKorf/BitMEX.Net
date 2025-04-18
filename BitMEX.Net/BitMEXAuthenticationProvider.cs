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
        private static readonly IMessageSerializer _serializer = new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(BitMEXExchange._serializerContext));

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
            var expires = DateTimeConverter.ConvertToSeconds(timestamp.AddSeconds(5))!;
            headers.Add("api-expires", expires.Value.ToString());
            headers.Add("api-key", ApiKey);
            var body = bodyParameters == null ? "" : GetSerializedBody(_serializer, bodyParameters);
            var query = uriParameters?.Any() != true ? uri.AbsolutePath : uri.SetParameters(uriParameters, arraySerialization).PathAndQuery;
            var signStr = GetSignature(method.ToString().ToUpperInvariant(), query, expires.Value, body);
            headers.Add("api-signature", signStr);
        }

        public string GetSignature(string method, string pathAndQuery, long expires, string body)
        {
            var signStr = $"{method}{pathAndQuery}{expires}{body}";
            return SignHMACSHA256(signStr, SignOutputType.Hex).ToLowerInvariant();
        }
    }
}
