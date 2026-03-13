using CryptoExchange.Net.Objects.Options;

namespace BitMEX.Net.Objects.Options
{
    /// <summary>
    /// Options for the BitMEXSocketClient
    /// </summary>
    public class BitMEXSocketOptions : SocketExchangeOptions<BitMEXEnvironment, BitMEXCredentials>
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        internal static BitMEXSocketOptions Default { get; set; } = new BitMEXSocketOptions()
        {
            Environment = BitMEXEnvironment.Live,
            SocketSubscriptionsCombineTarget = 10
        };

        /// <summary>
        /// ctor
        /// </summary>
        public BitMEXSocketOptions()
        {
            Default?.Set(this);
        }
        
        /// <summary>
        /// Exchange API options
        /// </summary>
        public SocketApiOptions<BitMEXCredentials> ExchangeOptions { get; private set; } = new SocketApiOptions<BitMEXCredentials>();

        internal BitMEXSocketOptions Set(BitMEXSocketOptions targetOptions)
        {
            targetOptions = base.Set<BitMEXSocketOptions>(targetOptions);            
            targetOptions.ExchangeOptions = ExchangeOptions.Set(targetOptions.ExchangeOptions);
            return targetOptions;
        }
    }
}
