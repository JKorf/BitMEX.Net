# BitMEX.Net AI API Map

This map helps AI assistants route common user intents to the actual BitMEX.Net methods.

## Client Roots

| Need | Use |
| --- | --- |
| REST client | `new BitMEXRestClient(...)` |
| Socket client | `new BitMEXSocketClient(...)` |
| Public market data | `restClient.ExchangeApi.ExchangeData` |
| Authenticated account data | `restClient.ExchangeApi.Account` |
| Orders, executions, positions | `restClient.ExchangeApi.Trading` |
| Websocket subscriptions | `socketClient.ExchangeApi` |
| Shared REST abstraction | `restClient.ExchangeApi.SharedClient` |
| Shared socket abstraction | `socketClient.ExchangeApi.SharedClient` |
| Discover shared capabilities | `client.ExchangeApi.SharedClient.Discover()` |

## Market Data

| Intent | Method |
| --- | --- |
| Server time | `ExchangeData.GetServerTimeAsync()` |
| Active instruments | `ExchangeData.GetActiveSymbolsAsync()` |
| Instrument query | `ExchangeData.GetSymbolsAsync(...)` |
| Active intervals | `ExchangeData.GetActiveIntervalsAsync()` |
| Composite indexes | `ExchangeData.GetCompositeIndexesAsync(...)` |
| Indices | `ExchangeData.GetIndicesAsync()` |
| Symbol USD volumes | `ExchangeData.GetSymbolVolumesAsync()` |
| Public trades | `ExchangeData.GetTradesAsync(symbol, ...)` |
| Candles | `ExchangeData.GetKlinesAsync(symbol, BinPeriod.OneMinute, ...)` |
| Order book | `ExchangeData.GetOrderBookAsync(symbol, limit)` |
| Book ticker history | `ExchangeData.GetBookTickerHistoryAsync(symbol, ...)` |
| Aggregated quote candles | `ExchangeData.GetAggregatedBookTickerHistoryAsync(symbol, BinPeriod.OneMinute, ...)` |
| Funding history | `ExchangeData.GetFundingHistoryAsync(symbol, ...)` |
| Settlement history | `ExchangeData.GetSettlementHistoryAsync(symbol, ...)` |
| Liquidations | `ExchangeData.GetLiquidationsAsync(symbol, ...)` |
| Insurance history | `ExchangeData.GetInsuranceAsync(asset, ...)` |
| Public announcements | `ExchangeData.GetAnnouncementsAsync()` |
| Assets | `ExchangeData.GetAssetsAsync()` |
| Asset networks | `ExchangeData.GetAssetNetworksAsync()` |

## Account

| Intent | Method |
| --- | --- |
| Account info | `Account.GetAccountInfoAsync()` |
| Balances | `Account.GetBalancesAsync(asset)` |
| Balance history | `Account.GetBalanceHistoryAsync(...)` |
| Balance summary | `Account.GetBalanceSummaryAsync(...)` |
| Fees | `Account.GetFeesAsync()` |
| Deposit address | `Account.GetDepositAddressAsync(asset, network)` |
| Margin status | `Account.GetMarginStatusAsync(asset)` |
| Transfer between accounts | `Account.TransferAsync(asset, fromAccountId, toAccountId, quantity)` |
| Withdraw | `Account.WithdrawAsync(asset, network, quantity, ...)` |
| Cancel withdrawal | `Account.CancelWithdrawalAsync(transactId)` |
| Saved addresses | `Account.GetSavedAddressesAsync()` |
| API key info | `Account.GetApiKeyInfoAsync()` |

## Trading

| Intent | Method |
| --- | --- |
| Place order | `Trading.PlaceOrderAsync(...)` |
| Get orders | `Trading.GetOrdersAsync(...)` |
| Edit order | `Trading.EditOrderAsync(...)` |
| Cancel one order | `Trading.CancelOrderAsync(orderId: ...)` or `Trading.CancelOrderAsync(clientOrderId: ...)` |
| Cancel multiple orders | `Trading.CancelOrdersAsync(...)` |
| Cancel all orders | `Trading.CancelAllOrdersAsync(...)` |
| Dead-man switch | `Trading.CancelAllAfterAsync(timeout)` |
| User executions | `Trading.GetUserExecutionsAsync(...)` |
| User trades | `Trading.GetUserTradesAsync(...)` |
| Positions | `Trading.GetPositionsAsync(...)` |
| Cross leverage | `Trading.SetCrossMarginLeverageAsync(symbol, leverage)` |
| Isolated leverage | `Trading.SetIsolatedMarginLeverageAsync(symbol, leverage)` |

## Sockets

| Intent | Method |
| --- | --- |
| Trades | `SubscribeToTradeUpdatesAsync(symbol, ...)` |
| Klines | `SubscribeToKlineUpdatesAsync(symbol, BinPeriod.OneMinute, ...)` |
| Book ticker | `SubscribeToBookTickerUpdatesAsync(symbol, ...)` |
| Aggregated book ticker | `SubscribeToAggregatedBookTickerUpdatesAsync(symbol, BinPeriod.OneMinute, ...)` |
| Top order book | `SubscribeToOrderBookUpdatesAsync(symbol, ...)` |
| Incremental book | `SubscribeToIncrementalOrderBookUpdatesAsync(symbol, IncrementalBookLimit.Top25, ...)` |
| Symbol updates | `SubscribeToSymbolUpdatesAsync(symbol, ...)` or `SubscribeToSymbolUpdatesAsync(SymbolCategory.Spot, ...)` |
| Funding | `SubscribeToFundingUpdatesAsync(symbol, ...)` |
| Liquidations | `SubscribeToLiquidationUpdatesAsync(...)` |
| Private balances | `SubscribeToBalanceUpdatesAsync(...)` |
| Private orders | `SubscribeToOrderUpdatesAsync(...)` |
| Private positions | `SubscribeToPositionUpdatesAsync(...)` |
| Private executions | `SubscribeToUserTradeUpdatesAsync(...)` |

## Shared APIs

Use SharedApis for exchange-agnostic code across BitMEX.Net and other CryptoExchange.Net exchange libraries.

| Intent | Pattern |
| --- | --- |
| Shared REST client | `new BitMEXRestClient().ExchangeApi.SharedClient` |
| Shared socket client | `new BitMEXSocketClient().ExchangeApi.SharedClient` |
| Discover shared capabilities | `client.ExchangeApi.SharedClient.Discover()` |
| Shared assets | `IAssetsRestClient.GetAssetsAsync(new GetAssetsRequest())` |
| Shared spot symbols | `ISpotSymbolRestClient.GetSpotSymbolsAsync(new GetSymbolsRequest())` |
| Shared futures symbols | `IFuturesSymbolRestClient.GetFuturesSymbolsAsync(new GetSymbolsRequest())` |
| Shared order book socket | `IOrderBookSocketClient.SubscribeToOrderBookUpdatesAsync(...)` |

Shared REST calls return `HttpResult<T>` / `HttpResult`. Shared socket subscriptions return `WebSocketResult<UpdateSubscription>`. Shared non-I/O symbol/cache helpers such as symbol support checks return `ExchangeCallResult<T>`.

For shared socket subscriptions, keep the concrete socket client and unsubscribe with `await socketClient.UnsubscribeAsync(subscription.Data)`.

## Result Handling

| Situation | Pattern |
| --- | --- |
| REST success check | `if (!result.Success) { Console.WriteLine(result.Error); return; }` |
| Socket subscription success check | `WebSocketResult<UpdateSubscription> sub = await ...; if (!sub.Success) { Console.WriteLine(sub.Error); return; }` |
| Read REST data | Read `result.Data` only after `result.Success` |
| Shared helper data | Read `ExchangeCallResult<T>.Data` only after `result.Success` |

## Symbol And Quantity Cheatsheet

| Topic | BitMEX.Net guidance |
| --- | --- |
| Classic inverse perp | `XBTUSD` |
| Spot example | `ETH_USDT` |
| Discover symbols | `ExchangeData.GetActiveSymbolsAsync()` |
| Kline enum | `BinPeriod` |
| Spot/asset conversion | `BitMEXUtils.UpdateSymbolInfoAsync()` plus extension methods |
| Credential shape | `new BitMEXCredentials(key, secret)` |

## Avoid / Replace

| Avoid | Use |
| --- | --- |
| `SpotApi` | `ExchangeApi` |
| `UsdFuturesApi` | `ExchangeApi` |
| `FuturesApiV2` | `ExchangeApi` |
| `SpotApiV3` | `ExchangeApi` |
| `GetOpenOrdersAsync` | `GetOrdersAsync` with filters when needed |
| `GetTickerAsync` | `GetSymbolsAsync` or `GetActiveSymbolsAsync` for current instrument fields |
| `KlineInterval.OneMinute` | `BinPeriod.OneMinute` |
| `OrderType.Stop` | `OrderType.StopMarket` |
| `BTC-USDT`, `BTC/USDT`, `tBTCUSD` | BitMEX-native symbols such as `XBTUSD` |
| `BitMEXCredentials(key, secret, passphrase)` | `BitMEXCredentials(key, secret)` |
