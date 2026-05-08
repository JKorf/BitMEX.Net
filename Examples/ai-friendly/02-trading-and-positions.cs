// 02-trading-and-positions.cs
//
// Demonstrates: BitMEX order placement, order querying, cancellation, positions and leverage.
//
// Setup: dotnet add package JKorf.BitMEX.Net

using BitMEX.Net;
using BitMEX.Net.Clients;
using BitMEX.Net.Enums;

var client = new BitMEXRestClient(options =>
{
    options.ApiCredentials = new BitMEXCredentials("API_KEY", "API_SECRET");
});

const string symbol = "XBTUSD";

var order = await client.ExchangeApi.Trading.PlaceOrderAsync(
    symbol: symbol,
    orderSide: OrderSide.Buy,
    orderType: OrderType.Limit,
    quantity: 100,
    price: 1m,
    timeInForce: TimeInForce.GoodTillCancel,
    executionInstruction: ExecutionInstruction.PostOnly,
    clientOrderId: $"ai-example-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}");

if (!order.Success)
{
    Console.WriteLine($"Order rejected: {order.Error}");
    return;
}

Console.WriteLine($"Placed order {order.Data.OrderId} on {order.Data.Symbol}");

var orders = await client.ExchangeApi.Trading.GetOrdersAsync(
    symbol: symbol,
    filter: new Dictionary<string, object>
    {
        ["orderID"] = order.Data.OrderId
    });

if (orders.Success)
    Console.WriteLine($"Matching order rows: {orders.Data.Length}");

var cancel = await client.ExchangeApi.Trading.CancelOrderAsync(orderId: order.Data.OrderId);
Console.WriteLine(cancel.Success
    ? $"Canceled order {cancel.Data.OrderId}"
    : $"Cancel failed: {cancel.Error}");

var positions = await client.ExchangeApi.Trading.GetPositionsAsync(
    filter: new Dictionary<string, object>
    {
        ["symbol"] = symbol
    });

if (positions.Success)
{
    foreach (var position in positions.Data)
        Console.WriteLine($"{position.Symbol}: quantity={position.CurrentQuantity}, leverage={position.Leverage}, open={position.IsOpen}");
}

// Example only: changing leverage affects account risk settings.
var leverage = await client.ExchangeApi.Trading.SetCrossMarginLeverageAsync(symbol, leverage: 2m);
Console.WriteLine(leverage.Success
    ? $"Cross leverage set for {leverage.Data.Symbol}"
    : $"Leverage update failed: {leverage.Error}");
