---
description: "AI assistant guide for BitMEX.Net"
applyTo: "**/*"
---

# BitMEX.Net AI Coding Guide

BitMEX.Net is a CryptoExchange.Net-based client for the BitMEX REST and websocket API. Use this file as the first source of truth when generating code for this repository.

For multi-exchange code, use `CryptoExchange.Net.SharedApis` through the `.SharedClient` properties on the `ExchangeApi` surfaces. Use `.SharedClient.Discover()` to inspect supported shared features at runtime.

Shared spot and futures symbol clients expose `SpotSymbolCatalog` and `FuturesSymbolCatalog`. Their `GetSpotSymbolsAsync(...)` and `GetFuturesSymbolsAsync(...)` methods apply `GetSymbolsRequest` filters and return display names plus shared base/quote asset classifications. Commodity, fiat, and equity instruments are classified where BitMEX metadata identifies them; quote assets are classified as crypto stablecoins.

## Package And Client Shape

- NuGet package id: `JKorf.BitMEX.Net`
- Root namespace: `BitMEX.Net`
- REST client: `BitMEXRestClient`
- Socket client: `BitMEXSocketClient`
- Only API root: `ExchangeApi`
- REST sub-clients:
  - `restClient.ExchangeApi.ExchangeData`
  - `restClient.ExchangeApi.Account`
  - `restClient.ExchangeApi.Trading`
- Socket API:
  - `socketClient.ExchangeApi`
- Shared clients:
  - `restClient.ExchangeApi.SharedClient`
  - `socketClient.ExchangeApi.SharedClient`

Do not invent Binance/Bitget-style roots such as `SpotApi`, `SpotApiV3`, `UsdFuturesApi`, `FuturesApiV2`, `CoinFuturesApi`, or `PerpetualFuturesApi`. BitMEX.Net exposes one `ExchangeApi` root and the individual endpoints decide whether a symbol is spot, perpetual, delivery, index, or asset data.

## Credentials And Options

BitMEX credentials are HMAC key and secret only:

```csharp
var restClient = new BitMEXRestClient(options =>
{
    options.ApiCredentials = new BitMEXCredentials("API_KEY", "API_SECRET");
});
```

There is no passphrase/memo parameter. Do not generate `new BitMEXCredentials(key, secret, passphrase)`.

Useful options:

- `BitMEXRestOptions.Environment` defaults to `BitMEXEnvironment.Live`
- `BitMEXRestOptions.AutoTimestamp` defaults to `false`
- `BitMEXRestOptions.BrokerId` can be set when needed
- `BitMEXRestOptions.ExchangeOptions` configures REST API-level behavior
- `BitMEXSocketOptions.Environment` defaults to `BitMEXEnvironment.Live`
- `BitMEXSocketOptions.SocketSubscriptionsCombineTarget` defaults to `10`
- `BitMEXSocketOptions.ExchangeOptions` configures socket API-level behavior
- Use `BitMEXRestClient.SetDefaultOptions(...)` and `BitMEXSocketClient.SetDefaultOptions(...)` for application-wide defaults
- Use `services.AddBitMEX(...)` for dependency injection

## Symbol Rules

Use BitMEX-native symbols:

- Perpetual inverse example: `XBTUSD`
- Spot example: `ETH_USDT`
- Linear USDT perpetual examples often look like `ETHUSDT`
- Assets/currencies may use BitMEX currency codes such as `XBt`, `gwei`, or exchange-specific asset aliases in wallet endpoints

Do not rewrite symbols into:

- `BTCUSDT` when the desired BitMEX instrument is actually `XBTUSD`
- `BTC_USDT` unless you have confirmed that exact BitMEX spot symbol exists
- `BTC-USDT`
- `BTC/USDT`
- Bitfinex-style `tBTCUSD`

When in doubt, query `ExchangeData.GetActiveSymbolsAsync()` and use the returned `BitMEXSymbol.Symbol`.

## Quantity Rules

BitMEX quantities are not always human asset units.

- Wallet balances and some spot quantities are in BitMEX base units such as `XBt` or `gwei`.
- Perpetual and delivery contract quantities are contract counts.
- Spot order quantities passed to `PlaceOrderAsync` use the BitMEX API base-unit quantity.
- Use `BitMEXUtils.UpdateSymbolInfoAsync()` before using conversion helpers.
- Use extension methods from `BitMEX.Net.ExtensionMethods`:
  - `ToBitMEXAssetQuantity(asset)`
  - `ToBitMEXSymbolQuantity(symbol)`
  - `ToSharedAssetQuantity(assetOrCurrency)`
  - `ToSharedSymbolQuantity(symbol)`

Example:

```csharp
await BitMEXUtils.UpdateSymbolInfoAsync();

var humanEthQuantity = 0.01m;
var bitmexQuantity = humanEthQuantity.ToBitMEXSymbolQuantity("ETH_USDT");
```

## REST Endpoint Routing

Market data and public exchange data:

- `GetServerTimeAsync()`
- `GetActiveSymbolsAsync()`
- `GetSymbolsAsync(...)`
- `GetActiveIntervalsAsync()`
- `GetCompositeIndexesAsync(...)`
- `GetIndicesAsync()`
- `GetSymbolVolumesAsync()`
- `GetTradesAsync(...)`
- `GetKlinesAsync(symbol, BinPeriod.OneMinute, ...)`
- `GetExchangeStatsAsync()`
- `GetExchangeStatHistoryAsync()`
- `GetExchangeStatHistoryUSDAsync()`
- `GetSettlementHistoryAsync(...)`
- `GetBookTickerHistoryAsync(...)`
- `GetAggregatedBookTickerHistoryAsync(...)`
- `GetOrderBookAsync(symbol, limit)`
- `GetInsuranceAsync(...)`
- `GetFundingHistoryAsync(...)`
- `GetAnnouncementsAsync()`
- `GetUrgentAnnouncementsAsync()`
- `GetAssetsAsync()`
- `GetAssetNetworksAsync()`
- `GetLiquidationsAsync(...)`

Account endpoints:

- `GetUserEventsAsync(...)`
- `GetAccountInfoAsync()`
- `GetFeesAsync()`
- `GetDepositAddressAsync(asset, network)`
- `GetMarginStatusAsync(...)`
- `GetQuoteFillRatioAsync(...)`
- `GetQuoteValueRatioAsync(accountId)`
- `GetTradingVolumeAsync()`
- `GetBalancesAsync(...)`
- `GetBalanceHistoryAsync(...)`
- `GetBalanceSummaryAsync(...)`
- `TransferAsync(asset, fromAccountId, toAccountId, quantity)`
- `WithdrawAsync(asset, network, quantity, ...)`
- `CancelWithdrawalAsync(transactId)`
- `SetIsolatedMarginAsync(symbol, enabled)`
- `SetRiskLimitAsync(symbol, riskLimit)`
- `TransferMarginAsync(symbol, quantity)`
- `GetSavedAddressesAsync()`
- `AddSavedAddressAsync(...)`
- `GetAddressBookSettingsAsync()`
- `GetApiKeyInfoAsync()`

Trading endpoints:

- `PlaceOrderAsync(symbol, OrderSide.Buy, OrderType.Limit, quantity, price, ...)`
- `GetOrdersAsync(...)`
- `EditOrderAsync(...)`
- `CancelOrderAsync(orderId: ..., clientOrderId: ...)`
- `CancelOrdersAsync(orderIds: ..., clientOrderIds: ...)`
- `CancelAllOrdersAsync(...)`
- `CancelAllAfterAsync(timeout)`
- `GetExecutionHistoryByDayAsync(symbol, day)`
- `GetUserExecutionsAsync(...)`
- `GetUserTradesAsync(...)`
- `GetPositionsAsync(...)`
- `SetCrossMarginLeverageAsync(symbol, leverage)`
- `SetIsolatedMarginLeverageAsync(symbol, leverage)`

## Websocket Routing

Public socket subscriptions:

- `SubscribeToTradeUpdatesAsync(symbol, ...)`
- `SubscribeToKlineUpdatesAsync(symbol, BinPeriod.OneMinute, ...)`
- `SubscribeToBookTickerUpdatesAsync(symbol, ...)`
- `SubscribeToAggregatedBookTickerUpdatesAsync(symbol, BinPeriod.OneMinute, ...)`
- `SubscribeToSettlementUpdatesAsync(...)`
- `SubscribeToOrderBookUpdatesAsync(symbol, ...)`
- `SubscribeToIncrementalOrderBookUpdatesAsync(symbol, IncrementalBookLimit.Top25, ...)`
- `SubscribeToLiquidationUpdatesAsync(...)`
- `SubscribeToInsuranceUpdatesAsync(...)`
- `SubscribeToSymbolUpdatesAsync(...)`
- `SubscribeToFundingUpdatesAsync(symbol, ...)`
- `SubscribeToAnnouncementUpdatesAsync(...)`
- `SubscribeToNotificationUpdatesAsync(...)`

Private socket subscriptions:

- `SubscribeToBalanceUpdatesAsync(...)`
- `SubscribeToTransactionUpdatesAsync(...)`
- `SubscribeToPositionUpdatesAsync(...)`
- `SubscribeToMarginUpdatesAsync(...)`
- `SubscribeToOrderUpdatesAsync(...)`
- `SubscribeToUserTradeUpdatesAsync(...)`

Always check `subscription.Success` before using `subscription.Data`. Unsubscribe with `await socketClient.UnsubscribeAsync(subscription.Data)`.

## Result Handling

REST calls return `HttpResult<T>` and socket subscriptions return `WebSocketResult<UpdateSubscription>`. Shared non-I/O symbol/cache helpers return `ExchangeCallResult<T>`.

```csharp
var result = await client.ExchangeApi.ExchangeData.GetActiveSymbolsAsync();
if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}

foreach (var symbol in result.Data)
    Console.WriteLine(symbol.Symbol);
```

Never assume `Data` is populated when `Success` is false.

## Local Order Book And Trackers

BitMEX.Net includes:

- `BitMEXOrderBookFactory`
- `BitMEXSymbolOrderBook`
- `BitMEXTrackerFactory`
- `BitMEXUserSpotDataTracker`
- `BitMEXUserFuturesDataTracker`

Use these when code needs maintained local order book state or user data tracking instead of manually combining snapshots and websocket deltas.

## Common Pitfalls

- Do not use `SpotApi`, `FuturesApi`, `UsdFuturesApi`, or versioned API roots.
- Do not add a credentials passphrase.
- Do not convert BitMEX symbols to another exchange format.
- Do not place spot orders with human asset quantities unless you converted them to BitMEX base units.
- Do not use `KlineInterval`; BitMEX.Net uses `BinPeriod`.
- Do not use `OrderType.Stop`; use `OrderType.StopMarket`.
- Use `ExecutionInstruction.PostOnly`, `ReduceOnly`, `Close`, `MarkPrice`, `IndexPrice`, or `LastPrice` where appropriate.
- `GetOrdersAsync` is the order query endpoint; there is no separate `GetOpenOrdersAsync` method in this library.
- `CancelOrderAsync` accepts `orderId` and `clientOrderId` optional named parameters.

## Source Files To Inspect Before Changing API Usage

- `BitMEX.Net/Interfaces/Clients/ExchangeApi/IBitMEXRestClientExchangeApiExchangeData.cs`
- `BitMEX.Net/Interfaces/Clients/ExchangeApi/IBitMEXRestClientExchangeApiAccount.cs`
- `BitMEX.Net/Interfaces/Clients/ExchangeApi/IBitMEXRestClientExchangeApiTrading.cs`
- `BitMEX.Net/Interfaces/Clients/ExchangeApi/IBitMEXSocketClientExchangeApi.cs`
- `BitMEX.Net/Objects/Options/BitMEXRestOptions.cs`
- `BitMEX.Net/Objects/Options/BitMEXSocketOptions.cs`
- `BitMEX.Net/BitMEXCredentials.cs`
- `BitMEX.Net/BitMEXUtils.cs`
- `BitMEX.Net/ExtensionMethods/BitMEXExtensionMethods.cs`
- `Examples/ai-friendly`
