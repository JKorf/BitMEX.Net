// 03-websocket.cs
//
// Demonstrates: BitMEX public and private websocket subscriptions.
//
// Setup: dotnet add package JKorf.BitMEX.Net

using BitMEX.Net;
using BitMEX.Net.Clients;
using BitMEX.Net.Enums;

var socketClient = new BitMEXSocketClient(options =>
{
    options.ApiCredentials = new BitMEXCredentials("API_KEY", "API_SECRET");
});

var tradeSubscription = await socketClient.ExchangeApi.SubscribeToTradeUpdatesAsync(
    "XBTUSD",
    update =>
    {
        foreach (var trade in update.Data)
            Console.WriteLine($"{trade.Symbol} trade: {trade.Quantity} @ {trade.Price}");
    });

if (!tradeSubscription.Success)
{
    Console.WriteLine($"Trade subscription failed: {tradeSubscription.Error}");
    return;
}

var klineSubscription = await socketClient.ExchangeApi.SubscribeToKlineUpdatesAsync(
    "XBTUSD",
    BinPeriod.OneMinute,
    update =>
    {
        foreach (var candle in update.Data)
            Console.WriteLine($"{candle.Symbol} 1m close: {candle.ClosePrice}");
    });

if (!klineSubscription.Success)
{
    Console.WriteLine($"Kline subscription failed: {klineSubscription.Error}");
    await socketClient.UnsubscribeAsync(tradeSubscription.Data);
    return;
}

var orderSubscription = await socketClient.ExchangeApi.SubscribeToOrderUpdatesAsync(update =>
{
    foreach (var order in update.Data)
        Console.WriteLine($"Order update: {order.OrderId} {order.Status}");
});

if (!orderSubscription.Success)
{
    Console.WriteLine($"Private order subscription failed: {orderSubscription.Error}");
    await socketClient.UnsubscribeAsync(tradeSubscription.Data);
    await socketClient.UnsubscribeAsync(klineSubscription.Data);
    return;
}

Console.WriteLine("Listening. Press Enter to unsubscribe.");
Console.ReadLine();

await socketClient.UnsubscribeAsync(tradeSubscription.Data);
await socketClient.UnsubscribeAsync(klineSubscription.Data);
await socketClient.UnsubscribeAsync(orderSubscription.Data);
