using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using System.Collections.Generic;

namespace BitMEX.Net
{
    internal class BitMEXAuthenticationProvider : AuthenticationProvider
    {
        private static readonly IMessageSerializer _serializer = new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(BitMEXExchange._serializerContext));

        public BitMEXAuthenticationProvider(ApiCredentials credentials) : base(credentials)
        {
        }

        public override void ProcessRequest(RestApiClient apiClient, RestRequestConfiguration request)
        {
            if (!request.Authenticated)
                return;

            var timestamp = GetTimestamp(apiClient);

            var expires = DateTimeConverter.ConvertToSeconds(timestamp.AddSeconds(5))!;
            request.Headers ??= new Dictionary<string, string>();
            request.Headers.Add("api-expires", expires.Value.ToString());
            request.Headers.Add("api-key", ApiKey);

            var body = (request.BodyParameters == null || request.BodyParameters.Count == 0) ? "" : GetSerializedBody(_serializer, request.BodyParameters);
            var queryParams = request.GetQueryString(true);
            if (!string.IsNullOrEmpty(queryParams))
                queryParams = $"?{queryParams}";

            var signStr = $"{request.Method.ToString().ToUpperInvariant()}{request.Path}{queryParams}{expires}{body}";

            request.Headers.Add("api-signature", SignHMACSHA256(signStr, SignOutputType.Hex));

            request.SetBodyContent(body);
            request.SetQueryString(queryParams);
        }

        public string GetSignature(string method, string pathAndQuery, long expires, string body)
        {
            var signStr = $"{method}{pathAndQuery}{expires}{body}";
            return SignHMACSHA256(signStr, SignOutputType.Hex).ToLowerInvariant();
        }
    }
}
