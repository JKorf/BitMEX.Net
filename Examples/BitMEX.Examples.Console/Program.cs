
using BitMEX.Net.Clients;

// REST
var restClient = new BitMEXRestClient();
var ticker = await restClient.ExchangeApi.ExchangeData.GetSymbolsAsync("ETHUSDT");
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

Console.ReadLine();
