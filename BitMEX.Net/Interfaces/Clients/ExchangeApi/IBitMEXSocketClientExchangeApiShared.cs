using CryptoExchange.Net.SharedApis;

namespace BitMEX.Net.Interfaces.Clients.ExchangeApi
{
    /// <summary>
    /// Shared interface for Exchange socket API usage
    /// </summary>
    public interface IBitMEXSocketClientExchangeApiShared :
        ISpotOrderSocketClient,
        IUserTradeSocketClient,
        IBalanceSocketClient,
        IBookTickerSocketClient,
        IOrderBookSocketClient,
        ITickerSocketClient,
        ITradeSocketClient,
        IFuturesOrderSocketClient,
        IPositionSocketClient
    {
    }

    /// <summary>
    /// Shared API interface. Shared APIs provide a common,
    /// exchange-independent contract for accessing functionality across different
    /// exchange client libraries.
    /// </summary>
    public interface IBitMEXSocketClientExchangeSharedApi :
        ISubscribeSpotOrdersSocket,
        ISubscribeUserTradesSocket,
        ISubscribeBalancesSocket,
        ISubscribeBookTickerSocket,
        ISubscribeOrderBookSocket,
        ISubscribeTickerSocket,
        ISubscribeTradesSocket,
        ISubscribeFuturesOrdersSocket,
        ISubscribePositionsSocket
    { }
}
