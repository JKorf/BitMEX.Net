using BitMEX.Net.Clients;
using BitMEX.Net.Interfaces;
using BitMEX.Net.Interfaces.Clients;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Trackers.Klines;
using CryptoExchange.Net.Trackers.Trades;
using CryptoExchange.Net.Trackers.UserData;
using CryptoExchange.Net.Trackers.UserData.Interfaces;
using CryptoExchange.Net.Trackers.UserData.Objects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;

namespace BitMEX.Net
{
    /// <inheritdoc />
    public class BitMEXTrackerFactory : IBitMEXTrackerFactory
    {
        private readonly IServiceProvider? _serviceProvider;

        /// <summary>
        /// ctor
        /// </summary>
        public BitMEXTrackerFactory()
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving logging and clients</param>
        public BitMEXTrackerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public bool CanCreateKlineTracker(SharedSymbol symbol, SharedKlineInterval interval) => false;

        /// <inheritdoc />
        public bool CanCreateTradeTracker(SharedSymbol symbol) => true;

        /// <inheritdoc />
        public IKlineTracker CreateKlineTracker(SharedSymbol symbol, SharedKlineInterval interval, int? limit = null, TimeSpan? period = null) => throw new NotImplementedException();

        /// <inheritdoc />
        public ITradeTracker CreateTradeTracker(SharedSymbol symbol, int? limit = null, TimeSpan? period = null)
        {
            var restClient = _serviceProvider?.GetRequiredService<IBitMEXRestClient>() ?? new BitMEXRestClient();
            var socketClient = _serviceProvider?.GetRequiredService<IBitMEXSocketClient>() ?? new BitMEXSocketClient();

            IRecentTradeRestClient? sharedRestClient = restClient.ExchangeApi.SharedClient;
            ITradeSocketClient sharedSocketClient = socketClient.ExchangeApi.SharedClient;

            return new TradeTracker(
                _serviceProvider?.GetRequiredService<ILoggerFactory>().CreateLogger(restClient.Exchange),
                sharedRestClient,
                null,
                sharedSocketClient,
                symbol,
                limit,
                period
                );
        }

        /// <inheritdoc />
        public IUserSpotDataTracker CreateUserSpotDataTracker(SpotUserDataTrackerConfig? config = null)
        {
            var restClient = _serviceProvider?.GetRequiredService<IBitMEXRestClient>() ?? new BitMEXRestClient();
            var socketClient = _serviceProvider?.GetRequiredService<IBitMEXSocketClient>() ?? new BitMEXSocketClient();
            return new BitMEXUserSpotDataTracker(
                _serviceProvider?.GetRequiredService<ILogger<BitMEXUserSpotDataTracker>>() ?? new NullLogger<BitMEXUserSpotDataTracker>(),
                restClient,
                socketClient,
                null,
                config
                );
        }

        /// <inheritdoc />
        public IUserSpotDataTracker CreateUserSpotDataTracker(string userIdentifier, ApiCredentials credentials, SpotUserDataTrackerConfig? config = null, BitMEXEnvironment? environment = null)
        {
            var clientProvider = _serviceProvider?.GetRequiredService<IBitMEXUserClientProvider>() ?? new BitMEXUserClientProvider();
            var restClient = clientProvider.GetRestClient(userIdentifier, credentials, environment);
            var socketClient = clientProvider.GetSocketClient(userIdentifier, credentials, environment);
            return new BitMEXUserSpotDataTracker(
                _serviceProvider?.GetRequiredService<ILogger<BitMEXUserSpotDataTracker>>() ?? new NullLogger<BitMEXUserSpotDataTracker>(),
                restClient,
                socketClient,
                userIdentifier,
                config
                );
        }

        /// <inheritdoc />
        public IUserFuturesDataTracker CreateUserFuturesDataTracker(FuturesUserDataTrackerConfig? config = null)
        {
            var restClient = _serviceProvider?.GetRequiredService<IBitMEXRestClient>() ?? new BitMEXRestClient();
            var socketClient = _serviceProvider?.GetRequiredService<IBitMEXSocketClient>() ?? new BitMEXSocketClient();
            return new BitMEXUserFuturesDataTracker(
                _serviceProvider?.GetRequiredService<ILogger<BitMEXUserFuturesDataTracker>>() ?? new NullLogger<BitMEXUserFuturesDataTracker>(),
                restClient,
                socketClient,
                null,
                config
                );
        }

        /// <inheritdoc />
        public IUserFuturesDataTracker CreateUserFuturesDataTracker(string userIdentifier, ApiCredentials credentials, FuturesUserDataTrackerConfig? config = null, BitMEXEnvironment? environment = null)
        {
            var clientProvider = _serviceProvider?.GetRequiredService<IBitMEXUserClientProvider>() ?? new BitMEXUserClientProvider();
            var restClient = clientProvider.GetRestClient(userIdentifier, credentials, environment);
            var socketClient = clientProvider.GetSocketClient(userIdentifier, credentials, environment);
            return new BitMEXUserFuturesDataTracker(
                _serviceProvider?.GetRequiredService<ILogger<BitMEXUserFuturesDataTracker>>() ?? new NullLogger<BitMEXUserFuturesDataTracker>(),
                restClient,
                socketClient,
                userIdentifier,
                config
                );
        }
    }
}
