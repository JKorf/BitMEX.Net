using CryptoExchange.Net.Authentication;
using System;

namespace BitMEX.Net
{
    /// <summary>
    /// BitMEX API credentials
    /// </summary>
    public class BitMEXCredentials : HMACCredential
    {
        /// <summary>
        /// Create new credentials
        /// </summary>
        public BitMEXCredentials() { }

        /// <summary>
        /// Create new credentials providing credentials in HMAC format
        /// </summary>
        /// <param name="key">API key</param>
        /// <param name="secret">API secret</param>
        public BitMEXCredentials(string key, string secret) : base(key, secret)
        {
        }

        /// <summary>
        /// Create new credentials providing HMAC credentials
        /// </summary>
        /// <param name="credential">HMAC Credentials</param>
        public BitMEXCredentials(HMACCredential credential) : base(credential.Key, credential.Secret)
        {
        }

        /// <summary>
        /// Specify the HMAC credentials
        /// </summary>
        /// <param name="key">API key</param>
        /// <param name="secret">API secret</param>
        public BitMEXCredentials WithHMAC(string key, string secret)
        {
            if (!string.IsNullOrEmpty(Key)) throw new InvalidOperationException("Credentials already set");

            Key = key;
            Secret = secret;
            return this;
        }
    }
}
