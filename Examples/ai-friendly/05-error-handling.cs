// 05-error-handling.cs
//
// Demonstrates: reusable BitMEX.Net REST and socket result handling.
//
// Setup: dotnet add package JKorf.BitMEX.Net

using BitMEX.Net.Clients;
using BitMEX.Net.Enums;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;

var restClient = new BitMEXRestClient();
var socketClient = new BitMEXSocketClient();

var book = await restClient.ExchangeApi.ExchangeData.GetOrderBookAsync("XBTUSD", 25);
if (!EnsureSuccess(book, "load order book"))
    return;

Console.WriteLine($"Best bid: {book.Data.Bids.FirstOrDefault()?.Price}");

var subscription = await socketClient.ExchangeApi.SubscribeToIncrementalOrderBookUpdatesAsync(
    "XBTUSD",
    IncrementalBookLimit.Top25,
    update =>
    {
        Console.WriteLine($"Incremental book update: {update.Data.Action}");
    });

if (!EnsureSuccessSocket(subscription, "subscribe to incremental book"))
    return;

await socketClient.UnsubscribeAsync(subscription.Data);

static bool EnsureSuccess<T>(HttpResult<T> result, string action)
{
    if (result.Success)
        return true;

    Console.WriteLine($"Could not {action}: {result.Error}");
    return false;
}

static bool EnsureSuccessSocket<T>(WebSocketResult<T> result, string action)
{
    if (result.Success)
        return true;

    Console.WriteLine($"Could not {action}: {result.Error}");
    return false;
}
