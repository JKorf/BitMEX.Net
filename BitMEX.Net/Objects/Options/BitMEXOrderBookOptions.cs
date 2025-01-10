using CryptoExchange.Net.Objects.Options;
using System;

namespace BitMEX.Net.Objects.Options
{
    /// <summary>
    /// Options for the BitMEX SymbolOrderBook
    /// </summary>
    public class BitMEXOrderBookOptions : OrderBookOptions
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        public static BitMEXOrderBookOptions Default { get; set; } = new BitMEXOrderBookOptions();

        /// <summary>
        /// The top amount of results to keep in sync. If for example limit=10 is used, the order book will contain the 10 best bids and 10 best asks. Leaving this null will sync the full order book
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// Whether or not quantities in the order book should be adjusted from the base unit. For example if false then 0.1 ETH would be 100000000
        /// </summary>
        public bool AdjustQuantities { get; set; } = true;

        /// <summary>
        /// After how much time we should consider the connection dropped if no data is received for this time after the initial subscriptions
        /// </summary>
        public TimeSpan? InitialDataTimeout { get; set; }

        internal BitMEXOrderBookOptions Copy()
        {
            var result = Copy<BitMEXOrderBookOptions>();
            result.Limit = Limit;
            result.InitialDataTimeout = InitialDataTimeout;
            return result;
        }
    }
}
