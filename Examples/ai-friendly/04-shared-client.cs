// 04-shared-client.cs
//
// Demonstrates: accessing CryptoExchange.Net shared clients from BitMEX.Net.
//
// Setup: dotnet add package JKorf.BitMEX.Net

using BitMEX.Net;
using BitMEX.Net.Clients;
using CryptoExchange.Net.SharedApis;

var client = new BitMEXRestClient(options =>
{
    options.ApiCredentials = new BitMEXCredentials("API_KEY", "API_SECRET");
});

var shared = client.ExchangeApi.SharedClient;
Console.WriteLine($"Exchange: {shared.Exchange}");
Console.WriteLine($"Trading modes: {string.Join(", ", shared.SupportedTradingModes)}");

var info = shared.Discover();
var supportedFeatures = info.Features
    .Where(x => x.Supported)
    .Select(x => x.EndpointName);
Console.WriteLine($"{info.Exchange} {info.TypeName}: {string.Join(", ", supportedFeatures)}");

var assets = await shared.GetAssetsAsync(new GetAssetsRequest());
if (!assets.Success)
{
    Console.WriteLine($"Shared asset request failed: {assets.Error}");
    return;
}

foreach (var asset in assets.Data.Take(5))
    Console.WriteLine($"{asset.Name}: networks={asset.Networks.Length}");

// Native BitMEX endpoints remain available beside the shared abstraction.
var nativeSymbols = await client.ExchangeApi.ExchangeData.GetActiveSymbolsAsync();
if (nativeSymbols.Success)
{
    foreach (var symbol in nativeSymbols.Data.Take(5))
        Console.WriteLine($"{symbol.Symbol}: {symbol.SymbolType}");
}
