using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Text;

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
