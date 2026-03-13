using CryptoExchange.Net.Authentication;

namespace BitMEX.Net
{
    /// <summary>
    /// BitMEX credentials
    /// </summary>
    public class BitMEXCredentials : ApiCredentials
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="apiKey">The API key</param>
        /// <param name="secret">The API secret</param>
        public BitMEXCredentials(string apiKey, string secret) : this(new HMACCredential(apiKey, secret)) { }
       
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="credential">The HMAC credentials</param>
        public BitMEXCredentials(HMACCredential credential) : base(credential) { }

        /// <inheritdoc />
        public override ApiCredentials Copy() => new BitMEXCredentials(Hmac!);
    }
}
