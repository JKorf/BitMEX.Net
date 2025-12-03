using BitMEX.Net.Objects.Internal;
using BitMEX.Net.Objects.Models;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
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
            AddTopicMapping<SocketUpdate<BitMEXTradeUpdate[]>>(x => x.Data.First().Symbol);
            AddTopicMapping<SocketUpdate<BitMEXAggTrade[]>>(x => x.Data.First().Symbol);
            AddTopicMapping<SocketUpdate<BitMEXBookTicker[]>>(x => x.Data.First().Symbol);
            AddTopicMapping<SocketUpdate<BitMEXSettlementHistory[]>>(x => x.Data.First().Symbol);
            AddTopicMapping<SocketUpdate<BitMEXOrderBookUpdate[]>>(x => x.Data.First().Symbol);
            AddTopicMapping<SocketUpdate<BitMEXOrderBookEntry[]>>(x => x.Data.First().Symbol);
            AddTopicMapping<SocketUpdate<BitMEXLiquidation[]>>(x => x.Data.First().Symbol);
            AddTopicMapping<SocketUpdate<BitMEXFundingRate[]>>(x => x.Data.First().Symbol);
        }

        protected override MessageEvaluator[] TypeEvaluators { get; } = [

            //new MessageEvaluator {
            //    Priority = 1,
            //    Fields = [
            //        new PropertyFieldReference("table") { Constraint = x => !_tableUpdatesWithoutSymbol.Contains(x!) },
            //        new PropertyFieldReference("symbol") { Depth = 3 },
            //    ],
            //    IdentifyMessageCallback = x => $"upd{x.FieldValue("table")}{x.FieldValue("symbol")}"
            //},

            new MessageEvaluator {
                Priority = 2,
                Fields = [
                    new PropertyFieldReference("table"),
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

        public override string? GetTypeIdentifier(ReadOnlySpan<byte> data, WebSocketMessageType? webSocketMessageType)
        {
            if (data.Length == 4)
                return "pong";

            return base.GetTypeIdentifier(data, webSocketMessageType);
        }
    }
}
