using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text.Json;

namespace BitMEX.Net.Clients.ExchangeApi
{
    internal class BitMexSocketClientExchangeApiMessageConverter : JsonSocketMessageHandler
    {
        public override JsonSerializerOptions Options { get; } = SerializerOptions.WithConverters(BitMEXExchange._serializerContext);

        private static readonly HashSet<string> _tableUpdatesWithoutSymbol = [
            "settlement",
            "liquidation",
            "execution",
            "order",
            "position",
            "instrument",
            "announcement",
            "publicNotifications",
            "insurance"
            ];

        protected override MessageEvaluator[] MessageEvaluators { get; } = [

            new MessageEvaluator {
                Priority = 1,
                Fields = [
                    new PropertyFieldReference("table") { Constraint = x => !_tableUpdatesWithoutSymbol.Contains(x) },
                    new PropertyFieldReference("symbol") { Depth = 3 },
                ],
                IdentifyMessageCallback = x => $"upd{x.FieldValue("table")}{x.FieldValue("symbol")}"
            },

            new MessageEvaluator {
                Priority = 2,
                Fields = [
                    new PropertyFieldReference("table") { Constraint = x => _tableUpdatesWithoutSymbol.Contains(x) },
                ],
                IdentifyMessageCallback = x => $"upd{x.FieldValue("table")}"
            },

            new MessageEvaluator {
                Priority = 3,
                ForceIfFound = true,
                Fields = [
                    new PropertyFieldReference("info"),
                ],
                StaticIdentifier = "info"
            },

            new MessageEvaluator {
                Priority = 4,
                ForceIfFound = true,
                Fields = [
                    new PropertyFieldReference("subscribe"),
                ],
                IdentifyMessageCallback = x => x.FieldValue("subscribe")
            },

            // TODO
            new MessageEvaluator {
                Priority = 5,
                ForceIfFound = true,
                Fields = [
                    new PropertyFieldReference("error"),
                ],
                StaticIdentifier = "subscribe"
            },

        ];

        public override string? GetMessageIdentifier(ReadOnlySpan<byte> data, WebSocketMessageType? webSocketMessageType)
        {
            if (data.Length == 4)
                return "pong";

            return base.GetMessageIdentifier(data, webSocketMessageType);
        }
    }
}
