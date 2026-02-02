using BitMEX.Net.Interfaces.Clients;
using BitMEX.Net.Objects.Options;
using CryptoExchange.Net.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Net.Http;

namespace BitMEX.Net.Clients
{
    /// <inheritdoc />
    public class BitMEXUserClientProvider : IBitMEXUserClientProvider
    {
        private static ConcurrentDictionary<string, IBitMEXRestClient> _restClients = new ConcurrentDictionary<string, IBitMEXRestClient>();
        private static ConcurrentDictionary<string, IBitMEXSocketClient> _socketClients = new ConcurrentDictionary<string, IBitMEXSocketClient>();

        private readonly IOptions<BitMEXRestOptions> _restOptions;
        private readonly IOptions<BitMEXSocketOptions> _socketOptions;
        private readonly HttpClient _httpClient;
        private readonly ILoggerFactory? _loggerFactory;

        /// <inheritdoc />
        public string ExchangeName => BitMEXExchange.ExchangeName;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="optionsDelegate">Options to use for created clients</param>
        public BitMEXUserClientProvider(Action<BitMEXOptions>? optionsDelegate = null)
            : this(null, null, Options.Create(ApplyOptionsDelegate(optionsDelegate).Rest), Options.Create(ApplyOptionsDelegate(optionsDelegate).Socket))
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        public BitMEXUserClientProvider(
            HttpClient? httpClient,
            ILoggerFactory? loggerFactory,
            IOptions<BitMEXRestOptions> restOptions,
            IOptions<BitMEXSocketOptions> socketOptions)
        {
            _httpClient = httpClient ?? new HttpClient();
            _loggerFactory = loggerFactory;
            _restOptions = restOptions;
            _socketOptions = socketOptions;
        }

        /// <inheritdoc />
        public void InitializeUserClient(string userIdentifier, ApiCredentials credentials, BitMEXEnvironment? environment = null)
        {
            CreateRestClient(userIdentifier, credentials, environment);
            CreateSocketClient(userIdentifier, credentials, environment);
        }

        /// <inheritdoc />
        public void ClearUserClients(string userIdentifier)
        {
            _restClients.TryRemove(userIdentifier, out _);
            _socketClients.TryRemove(userIdentifier, out _);
        }

        /// <inheritdoc />
        public IBitMEXRestClient GetRestClient(string userIdentifier, ApiCredentials? credentials = null, BitMEXEnvironment? environment = null)
        {
            if (!_restClients.TryGetValue(userIdentifier, out var client) || client.Disposed)
                client = CreateRestClient(userIdentifier, credentials, environment);

            return client;
        }

        /// <inheritdoc />
        public IBitMEXSocketClient GetSocketClient(string userIdentifier, ApiCredentials? credentials = null, BitMEXEnvironment? environment = null)
        {
            if (!_socketClients.TryGetValue(userIdentifier, out var client) || client.Disposed)
                client = CreateSocketClient(userIdentifier, credentials, environment);

            return client;
        }

        private IBitMEXRestClient CreateRestClient(string userIdentifier, ApiCredentials? credentials, BitMEXEnvironment? environment)
        {
            var clientRestOptions = SetRestEnvironment(environment);
            var client = new BitMEXRestClient(_httpClient, _loggerFactory, clientRestOptions);
            if (credentials != null)
            {
                client.SetApiCredentials(credentials);
                _restClients.TryAdd(userIdentifier, client);
            }
            return client;
        }

        private IBitMEXSocketClient CreateSocketClient(string userIdentifier, ApiCredentials? credentials, BitMEXEnvironment? environment)
        {
            var clientSocketOptions = SetSocketEnvironment(environment);
            var client = new BitMEXSocketClient(clientSocketOptions!, _loggerFactory);
            if (credentials != null)
            {
                client.SetApiCredentials(credentials);
                _socketClients.TryAdd(userIdentifier, client);
            }
            return client;
        }

        private IOptions<BitMEXRestOptions> SetRestEnvironment(BitMEXEnvironment? environment)
        {
            if (environment == null)
                return _restOptions;

            var newRestClientOptions = new BitMEXRestOptions();
            var restOptions = _restOptions.Value.Set(newRestClientOptions);
            newRestClientOptions.Environment = environment;
            return Options.Create(newRestClientOptions);
        }

        private IOptions<BitMEXSocketOptions> SetSocketEnvironment(BitMEXEnvironment? environment)
        {
            if (environment == null)
                return _socketOptions;

            var newSocketClientOptions = new BitMEXSocketOptions();
            var restOptions = _socketOptions.Value.Set(newSocketClientOptions);
            newSocketClientOptions.Environment = environment;
            return Options.Create(newSocketClientOptions);
        }

        private static T ApplyOptionsDelegate<T>(Action<T>? del) where T : new()
        {
            var opts = new T();
            del?.Invoke(opts);
            return opts;
        }
    }
}
