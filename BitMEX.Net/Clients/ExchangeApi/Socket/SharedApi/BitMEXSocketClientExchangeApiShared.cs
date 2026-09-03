using CryptoExchange.Net.SharedApis;
using System;
using BitMEX.Net.Interfaces.Clients.ExchangeApi;
using System.Threading.Tasks;
using System.Threading;
using CryptoExchange.Net.Objects.Sockets;
using System.Linq;
using BitMEX.Net.Enums;
using BitMEX.Net.ExtensionMethods;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net;

namespace BitMEX.Net.Clients.ExchangeApi
{
    internal partial class BitMEXSocketClientExchangeSharedApi :
        SharedApiBase,
        IBitMEXSocketClientExchangeApiShared,
        IBitMEXSocketClientExchangeSharedApi
    {
        private readonly BitMEXSocketClientExchangeApi _api;

        private const string _topicSpotId = "BitMEXSpot";
        private const string _topicFuturesId = "BitMEXFutures";
        private const string _exchangeName = "BitMEX";

        public override SharedClientInfo Discover() => SharedUtils.GetClientInfo(BitMEXExchange.Metadata, this);

        public BitMEXSocketClientExchangeSharedApi(BitMEXSocketClientExchangeApi api)
            : base(
                  SharedTransport.Rest,
                  api.Exchange,
                  [TradingMode.Spot, TradingMode.PerpetualLinear, TradingMode.DeliveryLinear, TradingMode.PerpetualInverse, TradingMode.DeliveryInverse],
                  () => api.Authenticated,
                  api.FormatSymbol)
        {
            _api = api;

            SetCapabilities(
                SubscribeSpotOrderOptions,
                SubscribeBalanceOptions,
                SubscribeUserTradeOptions,
                SubscribeBookTickerOptions,
                SubscribeOrderBookOptions,
                SubscribeTickerOptions,
                SubscribeTradeOptions,
                SubscribeFuturesOrderOptions,
                SubscribePositionOptions
                );
        }
    }
}
