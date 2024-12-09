using Microsoft.Extensions.Logging;
using System.Net.Http;
using System;
using CryptoExchange.Net.Authentication;
using BitMEX.Net.Interfaces.Clients;
using BitMEX.Net.Objects.Options;
using CryptoExchange.Net.Clients;
using Microsoft.Extensions.Options;
using BitMEX.Net.Interfaces.Clients.ExchangeApi;
using BitMEX.Net.Clients.ExchangeApi;

namespace BitMEX.Net.Clients
{
    /// <inheritdoc cref="IBitMEXRestClient" />
    public class BitMEXRestClient : BaseRestClient, IBitMEXRestClient
    {
        #region Api clients

        
         /// <inheritdoc />
        public IBitMEXRestClientExchangeApi ExchangeApi { get; }


        #endregion

        #region constructor/destructor

        /// <summary>
        /// Create a new instance of the BitMEXRestClient using provided options
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public BitMEXRestClient(Action<BitMEXRestOptions>? optionsDelegate = null)
            : this(null, null, Options.Create(ApplyOptionsDelegate(optionsDelegate)))
        {
        }

        /// <summary>
        /// Create a new instance of the BitMEXRestClient using provided options
        /// </summary>
        /// <param name="options">Option configuration</param>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="httpClient">Http client for this client</param>
        public BitMEXRestClient(HttpClient? httpClient, ILoggerFactory? loggerFactory, IOptions<BitMEXRestOptions> options) : base(loggerFactory, "BitMEX")
        {
            Initialize(options.Value);
            
            ExchangeApi = AddApiClient(new BitMEXRestClientExchangeApi(_logger, httpClient, options.Value));
        }

        #endregion

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public static void SetDefaultOptions(Action<BitMEXRestOptions> optionsDelegate)
        {
            BitMEXRestOptions.Default = ApplyOptionsDelegate(optionsDelegate);
        }

        /// <inheritdoc />
        public void SetApiCredentials(ApiCredentials credentials)
        {
            
            ExchangeApi.SetApiCredentials(credentials);

        }
    }
}
