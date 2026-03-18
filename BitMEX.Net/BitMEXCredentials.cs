using CryptoExchange.Net.Authentication;

namespace BitMEX.Net
{
    /// <summary>
    /// BitMEX API credentials
    /// </summary>
    public class BitMEXCredentials : HMACCredential
    {
        /// <summary>
        /// Create new credentials providing only credentials in HMAC format
        /// </summary>
        /// <param name="key">API key</param>
        /// <param name="secret">API secret</param>
        public BitMEXCredentials(string key, string secret) : base(key, secret)
        {
        }
    }
}
