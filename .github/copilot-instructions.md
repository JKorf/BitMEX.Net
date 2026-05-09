# BitMEX.Net Copilot Instructions

Generate code against the actual BitMEX.Net client shape.

Before making API-shape decisions, also read:

- `AGENTS.md` for the full repository-specific AI coding guide
- `llms.txt` for concise Bitstamp.Net context
- `llms-full.txt` for detailed endpoint routing, pitfalls and examples
- `docs/ai-api-map.md` for intent-to-method mapping

## Correct Client Roots

```csharp
var restClient = new BitMEXRestClient();
var socketClient = new BitMEXSocketClient();

await restClient.ExchangeApi.ExchangeData.GetActiveSymbolsAsync();
await restClient.ExchangeApi.Account.GetBalancesAsync();
await restClient.ExchangeApi.Trading.GetOrdersAsync("XBTUSD");
await socketClient.ExchangeApi.SubscribeToTradeUpdatesAsync("XBTUSD", update => { });
```

BitMEX.Net does not expose `SpotApi`, `UsdFuturesApi`, `FuturesApiV2`, `SpotApiV3`, `CoinFuturesApi`, or `PerpetualFuturesApi`.

## Credentials

```csharp
options.ApiCredentials = new BitMEXCredentials("API_KEY", "API_SECRET");
```

There is no passphrase.

## BitMEX-Specific Rules

- Use `ExchangeApi.ExchangeData`, `ExchangeApi.Account`, and `ExchangeApi.Trading`.
- Use `BinPeriod.OneMinute`, `BinPeriod.FiveMinutes`, `BinPeriod.OneHour`, or `BinPeriod.OneDay` for klines.
- Use BitMEX-native symbols such as `XBTUSD`, `ETH_USDT`, and symbols returned by `GetActiveSymbolsAsync`.
- Use `BitMEXUtils.UpdateSymbolInfoAsync()` before quantity conversion helpers.
- Use `ToBitMEXSymbolQuantity`, `ToBitMEXAssetQuantity`, `ToSharedSymbolQuantity`, and `ToSharedAssetQuantity` from `BitMEX.Net.ExtensionMethods`.
- Check `result.Success` before reading `result.Data`.
- Use named optional parameters for `CancelOrderAsync(orderId: ...)` and `CancelOrderAsync(clientOrderId: ...)`.

## Frequent Endpoint Mapping

| Intent | BitMEX.Net member |
| --- | --- |
| Active instruments | `restClient.ExchangeApi.ExchangeData.GetActiveSymbolsAsync()` |
| Symbol metadata | `restClient.ExchangeApi.ExchangeData.GetSymbolsAsync(...)` |
| Klines | `restClient.ExchangeApi.ExchangeData.GetKlinesAsync(symbol, BinPeriod.OneMinute, ...)` |
| Order book | `restClient.ExchangeApi.ExchangeData.GetOrderBookAsync(symbol, limit)` |
| Funding history | `restClient.ExchangeApi.ExchangeData.GetFundingHistoryAsync(symbol)` |
| Balances | `restClient.ExchangeApi.Account.GetBalancesAsync()` |
| Account info | `restClient.ExchangeApi.Account.GetAccountInfoAsync()` |
| Fees | `restClient.ExchangeApi.Account.GetFeesAsync()` |
| Place order | `restClient.ExchangeApi.Trading.PlaceOrderAsync(...)` |
| Orders | `restClient.ExchangeApi.Trading.GetOrdersAsync(...)` |
| Positions | `restClient.ExchangeApi.Trading.GetPositionsAsync(...)` |
| Trades stream | `socketClient.ExchangeApi.SubscribeToTradeUpdatesAsync(...)` |
| Order book stream | `socketClient.ExchangeApi.SubscribeToOrderBookUpdatesAsync(...)` |
| Private order stream | `socketClient.ExchangeApi.SubscribeToOrderUpdatesAsync(...)` |

## Avoid

- `BTC-USDT`, `BTC/USDT`, `tBTCUSD`
- `new BitMEXCredentials(key, secret, passphrase)`
- `GetTickerAsync` or `GetOpenOrdersAsync`; use `GetSymbolsAsync`/`GetOrdersAsync` as appropriate
- Human spot quantities without conversion to BitMEX base units
