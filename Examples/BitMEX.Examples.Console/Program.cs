
using BitMEX.Net.Clients;

// REST
var restClient = new BitMEXRestClient();
var ticker = await restClient.ExchangeApi.ExchangeData.GetSymbolsAsync("ETHUSDT");
if (!ticker.Success)
{
    Console.WriteLine($"Failed to get ticker: {ticker.Error}");
    return;
}

Console.WriteLine($"Rest client ticker price for ETHUSDT: {ticker.Data.First().LastPrice}");

Console.WriteLine();
Console.WriteLine("Press enter to start websocket subscription");
Console.ReadLine();

// Websocket
var socketClient = new BitMEXSocketClient();
var subscription = await socketClient.ExchangeApi.SubscribeToSymbolUpdatesAsync("ETHUSDT", update =>
{
    if (update.Data.LastPrice != null)
        Console.WriteLine($"Websocket client ticker price for ETHUSDT: {update.Data.LastPrice}");
});

if (!subscription.Success)
{
    Console.WriteLine($"Failed to subscribe to symbol updates: {subscription.Error}");
    return;
}

Console.ReadLine();
