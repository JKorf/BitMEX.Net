using BitMEX.Net;
using BitMEX.Net.Clients;
using BitMEX.Net.Enums;
using BitMEX.Net.ExtensionMethods;

const string symbol = "ETHUSDT";

// Replace with valid credentials or order placement will always fail
var apiKey = "APIKEY";
var apiSecret = "APISECRET";

Console.WriteLine("BitMEX.Net order placement example");
Console.WriteLine();
Console.WriteLine("This example can place real orders when valid credentials are configured.");
Console.WriteLine();

var client = new BitMEXRestClient(options =>
{
    options.ApiCredentials = new BitMEXCredentials(apiKey, apiSecret);
});

Console.WriteLine($"Placing limit buy order for {symbol}...");

var ticker = await client.ExchangeApi.ExchangeData.GetSymbolsAsync(symbol);
if (!ticker.Success)
{
    Console.WriteLine($"Failed to get ticker: {ticker.Error}");
    return;
}

await BitMEXUtils.UpdateSymbolInfoAsync();
var quantityToPlace = 0.01m;
var quantityBitMEX = quantityToPlace.ToBitMEXSymbolQuantity(symbol); // For example 1000000

var symbolInfo = ticker.Data.Single();
var safePrice = Math.Round(symbolInfo.LastPrice * 0.95m, 2);
var order = await client.ExchangeApi.Trading.PlaceOrderAsync(
    symbol: symbol,
    orderSide: OrderSide.Buy,
    orderType: OrderType.Limit,
    quantity: quantityBitMEX,
    price: safePrice,
    timeInForce: TimeInForce.GoodTillCancel);

if (!order.Success)
{
    Console.WriteLine($"Failed to place order: {order.Error}");
    return;
}

Console.WriteLine($"Placed order {order.Data.OrderId}, status: {order.Data.Status}");

var orderStatus = await client.ExchangeApi.Trading.GetOrdersAsync(
    symbol: symbol,
    filter: new Dictionary<string, object>
    {
        { "orderID", order.Data.OrderId }
    },
    limit: 1);

if (orderStatus.Success)
{
    var status = orderStatus.Data.SingleOrDefault();
    Console.WriteLine(status == null
        ? "Order was not returned by the status lookup"
        : $"Order status: {status.Status}, filled: {status.QuantityFilled}, remaining: {status.QuantityRemaining}");
}
else
{
    Console.WriteLine($"Failed to query order: {orderStatus.Error}");
}

var cancel = await client.ExchangeApi.Trading.CancelOrderAsync(order.Data.OrderId);
Console.WriteLine(cancel.Success
    ? $"Cancelled order {order.Data.OrderId}"
    : $"Failed to cancel order: {cancel.Error}");
