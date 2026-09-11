using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects.Options;
using CryptoExchange.Net.SharedApis;

namespace BitMEX.Net.Objects.Options
{
    /// <summary>
    /// BitMEX options
    /// </summary>
    public class BitMEXOptions : LibraryOptions<BitMEXRestOptions, BitMEXSocketOptions, BitMEXCredentials, BitMEXEnvironment>
    {
        /// <summary>
        /// Options for Shared API usage
        /// </summary>
        public SharedApiOptions SharedApi { get; set; } = new();
    }
}
