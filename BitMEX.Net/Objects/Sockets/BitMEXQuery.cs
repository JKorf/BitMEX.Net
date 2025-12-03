using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using System.Collections.Generic;
using BitMEX.Net.Objects.Models;
using BitMEX.Net.Objects.Internal;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Clients;
using System;

namespace BitMEX.Net.Objects.Sockets
{
    internal class BitMEXQuery<T> : Query<T>
    {
        private readonly SocketApiClient _client;

        public BitMEXQuery(SocketApiClient client, SocketCommand request, bool authenticated, int weight = 1) : base(request, authenticated, weight)
        {
            _client = client;
            RequiredResponses = request.Parameters.Length;

            MessageMatcher = MessageMatcher.Create<T>(request.Parameters, HandleMessage);
            MessageRouter = MessageRouter.CreateWithoutTopicFilter<T>(request.Parameters, HandleMessage);
        }

        public CallResult<T> HandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, T message)
        {
            if (message is SocketResponse resp && !string.IsNullOrEmpty(resp.Error))
            {
                if (resp.Error!.StartsWith("You are already subscribed to this topic"))
                    // Duplicate subscription, this is allowed by design
                    return new CallResult<T>(message, originalData, null);

                if (resp.Status != null)
                    return new CallResult<T>(new ServerError(resp.Status.Value, _client.GetErrorInfo(resp.Status.Value, resp.Error)), originalData);

                return new CallResult<T>(new ServerError(ErrorInfo.Unknown with { Message = resp.Error }), originalData);
            }

            return new CallResult<T>(message, originalData, null);
        }
    }
}
