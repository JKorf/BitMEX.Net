// 01-market-and-account.cs
//
// Demonstrates: BitMEX market data, authenticated balances and quantity conversion.
//
// Setup: dotnet add package JKorf.BitMEX.Net

using BitMEX.Net;
using BitMEX.Net.Clients;
using BitMEX.Net.Enums;
using BitMEX.Net.ExtensionMethods;

var client = new BitMEXRestClient(options =>
{
    options.ApiCredentials = new BitMEXCredentials("API_KEY", "API_SECRET");
});

const string symbol = "XBTUSD";

var symbols = await client.ExchangeApi.ExchangeData.GetActiveSymbolsAsync();
if (!symbols.Success)
{
    Console.WriteLine($"Active symbols failed: {symbols.Error}");
    return;
}

var instrument = symbols.Data.SingleOrDefault(x => x.Symbol == symbol);
if (instrument == null)
{
    Console.WriteLine($"{symbol} is not currently active on BitMEX.");
    return;
}

Console.WriteLine($"{instrument.Symbol}: last={instrument.LastPrice}, mark={instrument.MarkPrice}, funding={instrument.FundingRate}");

var orderBook = await client.ExchangeApi.ExchangeData.GetOrderBookAsync(symbol, 25);
if (!orderBook.Success)
{
    Console.WriteLine($"Order book failed: {orderBook.Error}");
    return;
}

Console.WriteLine($"{symbol} book levels: bids={orderBook.Data.Bids.Length}, asks={orderBook.Data.Asks.Length}");

var candles = await client.ExchangeApi.ExchangeData.GetKlinesAsync(
    symbol,
    BinPeriod.OneMinute,
    limit: 5,
    reverse: true);

if (candles.Success)
{
    foreach (var candle in candles.Data)
        Console.WriteLine($"{candle.Timestamp:u} open={candle.OpenPrice} close={candle.ClosePrice}");
}

var balances = await client.ExchangeApi.Account.GetBalancesAsync();
if (!balances.Success)
{
    Console.WriteLine($"Balances failed: {balances.Error}");
    return;
}

var conversionInfo = await BitMEXUtils.UpdateSymbolInfoAsync();
if (!conversionInfo.Success)
{
    Console.WriteLine($"Could not load BitMEX conversion metadata: {conversionInfo.Error}");
    return;
}

foreach (var balance in balances.Data.Take(5))
{
    var sharedQuantity = balance.Quantity.ToSharedAssetQuantity(balance.Currency);
    Console.WriteLine($"{balance.Currency}: bitmex={balance.Quantity}, shared={sharedQuantity}");
}
