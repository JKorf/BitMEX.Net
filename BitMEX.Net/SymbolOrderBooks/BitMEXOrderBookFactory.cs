using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.OrderBook;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using BitMEX.Net.Interfaces;
using BitMEX.Net.Interfaces.Clients;
using BitMEX.Net.Objects.Options;

namespace BitMEX.Net.SymbolOrderBooks
{
    /// <summary>
    /// BitMEX order book factory
    /// </summary>
    public class BitMEXOrderBookFactory : IBitMEXOrderBookFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving logging and clients</param>
        public BitMEXOrderBookFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;            
            
            Exchange = new OrderBookFactory<BitMEXOrderBookOptions>(Create, Create);
        }

        
         /// <inheritdoc />
        public IOrderBookFactory<BitMEXOrderBookOptions> Exchange { get; }


        /// <inheritdoc />
        public ISymbolOrderBook Create(SharedSymbol symbol, Action<BitMEXOrderBookOptions>? options = null)
        {
            var symbolName = BitMEXExchange.FormatSymbol(symbol.BaseAsset, symbol.QuoteAsset, symbol.TradingMode, symbol.DeliverTime);
            return Create(symbolName, options);
        }

        
         /// <inheritdoc />
        public ISymbolOrderBook Create(string symbol, Action<BitMEXOrderBookOptions>? options = null)
            => new BitMEXExchangeSymbolOrderBook(symbol, options, 
                                                          _serviceProvider.GetRequiredService<ILoggerFactory>(),
                                                          _serviceProvider.GetRequiredService<IBitMEXSocketClient>());


    }
}
