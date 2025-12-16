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
}
