using BitMEX.Net;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Converters.SystemTextJson.MessageConverters;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace BitMEX.Net.Clients.MessageHandlers
{
    internal class BitMexRestMessageHandler : JsonRestMessageHandler
    {
        private readonly ErrorMapping _errorMapping;

        public override JsonSerializerOptions Options { get; } = SerializerOptions.WithConverters(BitMEXExchange._serializerContext);

        public BitMexRestMessageHandler(ErrorMapping errorMapping)
        {
            _errorMapping = errorMapping;
        }

        public override async ValueTask<Error> ParseErrorResponse(
            int httpStatusCode,
            HttpResponseHeaders responseHeaders,
            Stream responseStream)
        {
            var (error, document) = await GetJsonDocument(responseStream).ConfigureAwait(false);
            if (error != null)
                return error;

            if(!document!.RootElement.TryGetProperty("error", out var errorProp))
                return new ServerError(ErrorInfo.Unknown);

            var errorName = errorProp.TryGetProperty("name", out var codeProp) ? codeProp.GetString() : null;
            var errorMsg = errorProp.TryGetProperty("message", out var msgProp) ? msgProp.GetString() : null;
            var errorDetails = errorProp.TryGetProperty("details", out var messageProp) ? messageProp.GetString() : null;

            if (errorName == null)
                return new ServerError(ErrorInfo.Unknown);

            if (errorDetails != null && int.TryParse(errorDetails, out var intCode))
                return new ServerError(intCode, _errorMapping.GetErrorInfo(errorDetails, errorMsg));

            return new ServerError(errorName, _errorMapping.GetErrorInfo(errorName, errorMsg));
        }
    }
}
