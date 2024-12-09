using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using System;
using BitMEX.Net.Objects.Options;

namespace BitMEX.Net.Interfaces
{
    /// <summary>
    /// BitMEX local order book factory
    /// </summary>
    public interface IBitMEXOrderBookFactory
    {
        /// <summary>
        /// Exchange order book factory methods
        /// </summary>
        IOrderBookFactory<BitMEXOrderBookOptions> Exchange { get; }


        /// <summary>
        /// Create a SymbolOrderBook for the symbol
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="options">Book options</param>
        /// <returns></returns>
        ISymbolOrderBook Create(SharedSymbol symbol, Action<BitMEXOrderBookOptions>? options = null);

        
        /// <summary>
        /// Create a new Exchange local order book instance
        /// </summary>
        ISymbolOrderBook Create(string symbol, Action<BitMEXOrderBookOptions>? options = null);

    }
}