using BitMEX.Net.Objects.Internal;
using BitMEX.Net.Objects.Models;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;
using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text.Json;

namespace BitMEX.Net.Clients.MessageHandlers
{
    internal class BitMexSocketExchangeMessageHandler : JsonSocketMessageHandler
    {
        public override JsonSerializerOptions Options { get; } = SerializerOptions.WithConverters(BitMEXExchange._serializerContext);

        public BitMexSocketExchangeMessageHandler()
        {   
            AddTopicMapping<SocketUpdate<BitMEXTradeUpdate[]>>(x => x.Data.FirstOrDefault()?.Symbol ?? (x.Filter?.TryGetValue("symbol", out var symbol) == true ? (string)symbol : null));
            AddTopicMapping<SocketUpdate<BitMEXAggTrade[]>>(x => x.Data.First().Symbol);
            AddTopicMapping<SocketUpdate<BitMEXBookTicker[]>>(x => x.Data.First().Symbol);
            AddTopicMapping<SocketUpdate<BitMEXSettlementHistory[]>>(x => x.Data.FirstOrDefault()?.Symbol ?? (x.Filter?.TryGetValue("symbol", out var symbol) == true ? (string)symbol : null));
            AddTopicMapping<SocketUpdate<BitMEXOrderBookUpdate[]>>(x => x.Data.First().Symbol);
            AddTopicMapping<SocketUpdate<BitMEXOrderBookEntry[]>>(x => x.Data.First().Symbol);
            AddTopicMapping<SocketUpdate<BitMEXLiquidation[]>>(x => x.Data.FirstOrDefault()?.Symbol ?? (x.Filter?.TryGetValue("symbol", out var symbol) == true ? (string)symbol : null));
            AddTopicMapping<SocketUpdate<BitMEXFundingRate[]>>(x => x.Data.FirstOrDefault()?.Symbol ?? (x.Filter?.TryGetValue("symbol", out var symbol) == true ? (string)symbol : null));
            AddTopicMapping<SocketUpdate<BitMEXSymbolUpdate[]>>(x => x.Data.FirstOrDefault()?.Symbol ?? (x.Filter?.TryGetValue("symbol", out var symbol) == true ? (string)symbol : null));
        }

        protected override MessageTypeDefinition[] TypeEvaluators { get; } = [

            new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("table"),
                ],
                TypeIdentifierCallback = x => $"upd{x.FieldValue("table")}"
            },

            new MessageTypeDefinition {
                ForceIfFound = true,
                Fields = [
                    new PropertyFieldReference("info"),
                ],
                StaticIdentifier = "info"
            },

            new MessageTypeDefinition {
                ForceIfFound = true,
                Fields = [
                    new PropertyFieldReference("subscribe"),
                ],
                TypeIdentifierCallback = x => x.FieldValue("subscribe")!
            },

            new MessageTypeDefinition {
                ForceIfFound = true,
                Fields = [
                    new PropertyFieldReference("unsubscribe"),
                ],
                TypeIdentifierCallback = x => x.FieldValue("unsubscribe")!
            },

            new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("error"),
                    new PropertyFieldReference("args") { ArrayValues = true, Depth = 2 }
                ],
                TypeIdentifierCallback = x => string.Join("", x.FieldValue("args")!.Split(','))
            },

        ];

        public override string? GetTypeIdentifier(ReadOnlySpan<byte> data, WebSocketMessageType? webSocketMessageType)
        {
            if (data.Length == 4)
                return "pong";

            return base.GetTypeIdentifier(data, webSocketMessageType);
        }
    }
}
