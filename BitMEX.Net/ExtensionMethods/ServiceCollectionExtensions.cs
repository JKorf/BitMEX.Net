using CryptoExchange.Net;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Http;
using BitMEX.Net;
using BitMEX.Net.Clients;
using BitMEX.Net.Interfaces;
using BitMEX.Net.Interfaces.Clients;
using BitMEX.Net.Objects.Options;
using BitMEX.Net.SymbolOrderBooks;
using System.Threading;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for DI
    /// </summary>
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// Add services such as the IBitMEXRestClient and IBitMEXSocketClient. Configures the services based on the provided configuration.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configuration">The configuration(section) containing the options</param>
        /// <returns></returns>
        public static IServiceCollection AddBitMEX(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var options = new BitMEXOptions();
            // Reset environment so we know if they're overridden
            options.Rest.Environment = null!;
            options.Socket.Environment = null!;
            configuration.Bind(options);

            if (options.Rest == null || options.Socket == null)
                throw new ArgumentException("Options null");

            var restEnvName = options.Rest.Environment?.Name ?? options.Environment?.Name ?? BitMEXEnvironment.Live.Name;
            var socketEnvName = options.Socket.Environment?.Name ?? options.Environment?.Name ?? BitMEXEnvironment.Live.Name;
            options.Rest.Environment = BitMEXEnvironment.GetEnvironmentByName(restEnvName) ?? options.Rest.Environment!;
            options.Rest.ApiCredentials = options.Rest.ApiCredentials ?? options.ApiCredentials;
            options.Socket.Environment = BitMEXEnvironment.GetEnvironmentByName(socketEnvName) ?? options.Socket.Environment!;
            options.Socket.ApiCredentials = options.Socket.ApiCredentials ?? options.ApiCredentials;


            services.AddSingleton(x => Options.Options.Create(options.Rest));
            services.AddSingleton(x => Options.Options.Create(options.Socket));

            return AddBitMEXCore(services, options.SocketClientLifeTime);
        }

        /// <summary>
        /// Add services such as the IBitMEXRestClient and IBitMEXSocketClient. Services will be configured based on the provided options.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="optionsDelegate">Set options for the BitMEX services</param>
        /// <returns></returns>
        public static IServiceCollection AddBitMEX(
            this IServiceCollection services,
            Action<BitMEXOptions>? optionsDelegate = null)
        {
            var options = new BitMEXOptions();
            // Reset environment so we know if they're overridden
            options.Rest.Environment = null!;
            options.Socket.Environment = null!;
            optionsDelegate?.Invoke(options);
            if (options.Rest == null || options.Socket == null)
                throw new ArgumentException("Options null");

            options.Rest.Environment = options.Rest.Environment ?? options.Environment ?? BitMEXEnvironment.Live;
            options.Rest.ApiCredentials = options.Rest.ApiCredentials ?? options.ApiCredentials;
            options.Socket.Environment = options.Socket.Environment ?? options.Environment ?? BitMEXEnvironment.Live;
            options.Socket.ApiCredentials = options.Socket.ApiCredentials ?? options.ApiCredentials;

            services.AddSingleton(x => Options.Options.Create(options.Rest));
            services.AddSingleton(x => Options.Options.Create(options.Socket));

            return AddBitMEXCore(services, options.SocketClientLifeTime);
        }

        private static IServiceCollection AddBitMEXCore(
            this IServiceCollection services,
            ServiceLifetime? socketClientLifeTime = null)
        {
            services.AddHttpClient<IBitMEXRestClient, BitMEXRestClient>((client, serviceProvider) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<BitMEXRestOptions>>().Value;
                client.Timeout = options.RequestTimeout;
                return new BitMEXRestClient(client, serviceProvider.GetRequiredService<ILoggerFactory>(), serviceProvider.GetRequiredService<IOptions<BitMEXRestOptions>>());
            }).ConfigurePrimaryHttpMessageHandler((serviceProvider) => {
                var options = serviceProvider.GetRequiredService<IOptions<BitMEXRestOptions>>().Value;
                return LibraryHelpers.CreateHttpClientMessageHandler(options.Proxy, options.HttpKeepAliveInterval);
            });
            services.Add(new ServiceDescriptor(typeof(IBitMEXSocketClient), x => { return new BitMEXSocketClient(x.GetRequiredService<IOptions<BitMEXSocketOptions>>(), x.GetRequiredService<ILoggerFactory>()); }, socketClientLifeTime ?? ServiceLifetime.Singleton));

            services.AddTransient<ICryptoRestClient, CryptoRestClient>();
            services.AddSingleton<ICryptoSocketClient, CryptoSocketClient>();
            services.AddTransient<IBitMEXOrderBookFactory, BitMEXOrderBookFactory>();
            services.AddTransient<IBitMEXTrackerFactory, BitMEXTrackerFactory>();
            services.AddSingleton<IBitMEXUserClientProvider, BitMEXUserClientProvider>(x =>
            new BitMEXUserClientProvider(
                x.GetRequiredService<HttpClient>(),
                x.GetRequiredService<ILoggerFactory>(),
                x.GetRequiredService<IOptions<BitMEXRestOptions>>(),
                x.GetRequiredService<IOptions<BitMEXSocketOptions>>()));

            services.RegisterSharedRestInterfaces(x => x.GetRequiredService<IBitMEXRestClient>().ExchangeApi.SharedClient);
            services.RegisterSharedSocketInterfaces(x => x.GetRequiredService<IBitMEXSocketClient>().ExchangeApi.SharedClient);

            return services;
        }
    }
}
