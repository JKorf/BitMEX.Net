# BitMEX.Net AI-Friendly Examples

These examples are intentionally small console programs that AI assistants can copy from safely. They are compiled by `BitMEX.Net.UnitTests/Documentation/AiExampleCompileTests.cs`.

## Files

| File | Demonstrates |
| --- | --- |
| `01-market-and-account.cs` | Public market data, balances, result handling, and BitMEX quantity conversion |
| `02-trading-and-positions.cs` | Limit order placement, order query/cancel, positions, and leverage calls |
| `03-websocket.cs` | Public and private websocket subscriptions |
| `04-shared-client.cs` | CryptoExchange.Net shared client access from BitMEX.Net |
| `05-error-handling.cs` | Reusable error handling helpers for REST and sockets |

## BitMEX Shape To Remember

Use:

```csharp
restClient.ExchangeApi.ExchangeData
restClient.ExchangeApi.Account
restClient.ExchangeApi.Trading
socketClient.ExchangeApi
```

Do not use exchange roots from other libraries such as `SpotApi`, `UsdFuturesApi`, `FuturesApiV2`, `SpotApiV3`, `CoinFuturesApi`, or `PerpetualFuturesApi`.

BitMEX credentials use only API key and API secret:

```csharp
new BitMEXCredentials("API_KEY", "API_SECRET")
```

BitMEX symbols and quantities are exchange-specific. Prefer `GetActiveSymbolsAsync()` to discover exact symbols. Use `BitMEXUtils.UpdateSymbolInfoAsync()` before `ToBitMEXSymbolQuantity` or `ToSharedAssetQuantity`.
