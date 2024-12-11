namespace BitMEX.Net.Objects
{
    /// <summary>
    /// Api addresses
    /// </summary>
    public class BitMEXApiAddresses
    {
        /// <summary>
        /// The address used by the BitMEXRestClient for the API
        /// </summary>
        public string RestClientAddress { get; set; } = "";
        /// <summary>
        /// The address used by the BitMEXSocketClient for the websocket API
        /// </summary>
        public string SocketClientAddress { get; set; } = "";

        /// <summary>
        /// The default addresses to connect to the BitMEX API
        /// </summary>
        public static BitMEXApiAddresses Default = new BitMEXApiAddresses
        {
            RestClientAddress = "https://www.bitmex.com/",
            SocketClientAddress = "wss://ws.bitmex.com/"
        };
    }
}
