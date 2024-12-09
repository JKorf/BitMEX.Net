using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using BitMEX.Net.Interfaces.Clients;
using BitMEX.Net.Objects.Options;
using BitMEX.Net.Interfaces.Clients.ExchangeApi;
using BitMEX.Net.Clients.ExchangeApi;

namespace BitMEX.Net.Clients
{
    /// <inheritdoc cref="IBitMEXSocketClient" />
    public class BitMEXSocketClient : BaseSocketClient, IBitMEXSocketClient
    {
        #region fields
        #endregion

        #region Api clients

        
         /// <inheritdoc />
        public IBitMEXSocketClientExchangeApi ExchangeApi { get; }


        #endregion

        #region constructor/destructor

        /// <summary>
        /// Create a new instance of BitMEXSocketClient
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public BitMEXSocketClient(Action<BitMEXSocketOptions>? optionsDelegate = null)
            : this(Options.Create(ApplyOptionsDelegate(optionsDelegate)), null)
        {
        }

        /// <summary>
        /// Create a new instance of BitMEXSocketClient
        /// </summary>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="options">Option configuration</param>
        public BitMEXSocketClient(IOptions<BitMEXSocketOptions> options, ILoggerFactory? loggerFactory = null) : base(loggerFactory, "BitMEX")
        {
            Initialize(options.Value);

            
            ExchangeApi = AddApiClient(new BitMEXSocketClientExchangeApi(_logger, options.Value));

        }
        #endregion

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public static void SetDefaultOptions(Action<BitMEXSocketOptions> optionsDelegate)
        {
            BitMEXSocketOptions.Default = ApplyOptionsDelegate(optionsDelegate);
        }

        /// <inheritdoc />
        public void SetApiCredentials(ApiCredentials credentials)
        {
            
            ExchangeApi.SetApiCredentials(credentials);

        }
    }
}
