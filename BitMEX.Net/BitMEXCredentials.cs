using CryptoExchange.Net.Authentication;
using System;

namespace BitMEX.Net
{
    /// <summary>
    /// BitMEX credentials
    /// </summary>
    public class BitMEXCredentials : ApiCredentials
    {
        /// <summary>
        /// </summary>
        [Obsolete("Parameterless constructor is only for deserialization purposes and should not be used directly. Use parameterized constructor instead.")]
        public BitMEXCredentials() { }

        /// <summary>
        /// Create credentials using an HMAC key and secret
        /// </summary>
        /// <param name="apiKey">The API key</param>
        /// <param name="secret">The API secret</param>
        public BitMEXCredentials(string apiKey, string secret) : this(new HMACCredential(apiKey, secret)) { }

        /// <summary>
        /// Create credentials using HMAC credentials
        /// </summary>
        /// <param name="credential">The HMAC credentials</param>
        public BitMEXCredentials(HMACCredential credential) : base(credential) { }

        /// <inheritdoc />
#pragma warning disable CS0618 // Type or member is obsolete
        public override ApiCredentials Copy() => new BitMEXCredentials { CredentialPairs = CredentialPairs };
#pragma warning restore CS0618 // Type or member is obsolete
    }
}
