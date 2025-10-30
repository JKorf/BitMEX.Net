using CryptoExchange.Net.Objects.Options;

namespace BitMEX.Net.Objects.Options
{
    /// <summary>
    /// Options for the BitMEXRestClient
    /// </summary>
    public class BitMEXRestOptions : RestExchangeOptions<BitMEXEnvironment>
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        internal static BitMEXRestOptions Default { get; set; } = new BitMEXRestOptions()
        {
            Environment = BitMEXEnvironment.Live,
            AutoTimestamp = false
        };

        /// <summary>
        /// ctor
        /// </summary>
        public BitMEXRestOptions()
        {
            Default?.Set(this);
        }

        /// <summary>
        /// Broker id
        /// </summary>
        public string? BrokerId { get; set; }
        
         /// <summary>
        /// Exchange API options
        /// </summary>
        public RestApiOptions ExchangeOptions { get; private set; } = new RestApiOptions();


        internal BitMEXRestOptions Set(BitMEXRestOptions targetOptions)
        {
            targetOptions = base.Set<BitMEXRestOptions>(targetOptions);
            
            targetOptions.ExchangeOptions = ExchangeOptions.Set(targetOptions.ExchangeOptions);

            return targetOptions;
        }
    }
}
