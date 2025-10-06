# ![BitMEX.Net](https://raw.githubusercontent.com/JKorf/BitMEX.Net/main/BitMEX.Net/Icon/icon.png) BitMEX.Net  

[![.NET](https://img.shields.io/github/actions/workflow/status/JKorf/BitMEX.Net/dotnet.yml?style=for-the-badge)](https://github.com/JKorf/BitMEX.Net/actions/workflows/dotnet.yml) ![License](https://img.shields.io/github/license/JKorf/BitMEX.Net?style=for-the-badge)

BitMEX.Net is a client library for accessing the [BitMEX REST and Websocket API](https://www.bitmex.com/app/apiOverview). 

## Features
* Response data is mapped to descriptive models
* Input parameters and response values are mapped to discriptive enum values where possible
* Automatic websocket (re)connection management 
* Client side rate limiting 
* Client side order book implementation
* Support for managing different accounts
* Extensive logging
* Support for different environments
* Easy integration with other exchange client based on the CryptoExchange.Net base library
* Native AOT support

## Supported Frameworks
The library is targeting both `.NET Standard 2.0` and `.NET Standard 2.1` for optimal compatibility, as well as dotnet 8.0 and 9.0 to use the latest framework features.

|.NET implementation|Version Support|
|--|--|
|.NET Core|`2.0` and higher|
|.NET Framework|`4.6.1` and higher|
|Mono|`5.4` and higher|
|Xamarin.iOS|`10.14` and higher|
|Xamarin.Android|`8.0` and higher|
|UWP|`10.0.16299` and higher|
|Unity|`2018.1` and higher|

## Install the library

### NuGet 
[![NuGet version](https://img.shields.io/nuget/v/JKorf.BitMEX.net.svg?style=for-the-badge)](https://www.nuget.org/packages/JKorf.BitMEX.Net)  [![Nuget downloads](https://img.shields.io/nuget/dt/JKorf.BitMEX.Net.svg?style=for-the-badge)](https://www.nuget.org/packages/JKorf.BitMEX.Net)

	dotnet add package JKorf.BitMEX.Net
	
### GitHub packages
BitMEX.Net is available on [GitHub packages](https://github.com/JKorf/BitMEX.Net/pkgs/nuget/BitMEX.Net). You'll need to add `https://nuget.pkg.github.com/JKorf/index.json` as a NuGet package source.

### Download release
[![GitHub Release](https://img.shields.io/github/v/release/JKorf/BitMEX.Net?style=for-the-badge&label=GitHub)](https://github.com/JKorf/BitMEX.Net/releases)

The NuGet package files are added along side the source with the latest GitHub release which can found [here](https://github.com/JKorf/BitMEX.Net/releases).

## How to use
* REST Endpoints
	```csharp
	// Get the ETH/USDT ticker via rest request
	var restClient = new BitMEXRestClient();
	var tickerResult = await restClient.ExchangeApi.ExchangeData.GetSymbolsAsync("ETHUSDT");
	var lastPrice = tickerResult.Data.Single().LastPrice;
	```
* Websocket streams
	```csharp
	// Subscribe to ETH/USDT ticker updates via the websocket API
	var socketClient = new BitMEXSocketClient();
	var tickerSubscriptionResult = socketClient.ExchangeApi.SubscribeToSymbolUpdatesAsync("ETHUSDT", (update) =>
	{
		// If update.Data.LastPrice == null the price hasn't changed since last update
		if (update.Data.LastPrice != null)
		{ 
			var lastPrice = update.Data.LastPrice;
		}
	});
	```

For information on the clients, dependency injection, response processing and more see the [documentation](https://cryptoexchange.jkorf.dev?library=BitMEX.Net), or have a look at the examples [here](https://github.com/JKorf/BitMEX.Net/tree/main/Examples) or [here](https://github.com/JKorf/CryptoExchange.Net/tree/master/Examples).

### BitMEX Quantities
BitMEX handles quantities a bit differently than most exchange API's. Asset quantities like account balances are denoted in a base value.  
For example 9846 XBt instead of 0.00009846 BTC. The same logic is also applied to trading quantities, if you want to place a spot order for 0.1 BTC you'd need to specify a quantity of 10000000 XBt. Note that futures trading works with contracts.  
How many decimal places are used for each asset and symbol can be requested using the `restClient.ExchangeApi.ExchangeData.GetAssetsAsync` and `restClient.ExchangeApi.ExchangeData.GetActiveSymbolsAsync` endpoints.

The library offers an easy way of converting between these quantities with the `ToSharedAssetQuantity`, `ToSharedSymbolQuantity`, `ToBitMEXAssetQuantity` and `ToBitMEXSymbolQuantity` extension methods. For example:  
*Getting balance info*
```csharp
// Make sure to call this at least once at the start of the program to retrieve the conversion data
await BitMEXUtils.UpdateSymbolInfoAsync();

var balances = await bitMEXRestClient.ExchangeApi.Account.GetBalancesAsync();
foreach (var balance in balances.Data)
{
    var bitMEXQuantity = balance.Quantity; // The quantity as used in the BitMEX API, for example 1000000
    var bitMEXAssetName = balance.Currency; // The currency name as used in the BitMEX API, for example `gwei`
    var assetQuantity = balance.Quantity.ToSharedAssetQuantity(balance.Currency); // The quantity in the actual assset, for example 0.01
    var assetName = BitMEXUtils.GetAssetFromCurrency(balance.Currency); // The asset name, for example `ETH`
}
```

*Placing spot order*
```csharp
// Make sure to call this at least once at the start of the program to retrieve the conversion data
await BitMEXUtils.UpdateSymbolInfoAsync();

var symbols = await bitMEXRestClient.ExchangeApi.ExchangeData.GetActiveSymbolsAsync();
var ethUsdtSpotSymbol = symbols.Data.SingleOrDefault(x => x.BaseAsset == "ETH" && x.QuoteAsset == "USDT" && x.SymbolType == SymbolType.Spot);
var minQuantityInBaseUnit = ethUsdtSpotSymbol.LotSize; // For example 1000
var minQuantityInETH = ethUsdtSpotSymbol.LotSize.ToSharedSymbolQuantity(ethUsdtSpotSymbol.Symbol); // For example 0.001

var quantityToPlace = 0.1m;
var quantityBitMEX = quantityToPlace.ToBitMEXSymbolQuantity(ethUsdtSpotSymbol.Symbol); // For example 1000000
var result = await bitMEXRestClient.ExchangeApi.Trading.PlaceOrderAsync(ethUsdtSpotSymbol.Symbol, OrderSide.Buy, OrderType.Market, quantityBitMEX);
```


## CryptoExchange.Net
BitMEX.Net is based on the [CryptoExchange.Net](https://github.com/JKorf/CryptoExchange.Net) base library. Other exchange API implementations based on the CryptoExchange.Net base library are available and follow the same logic.

CryptoExchange.Net also allows for [easy access to different exchange API's](https://cryptoexchange.jkorf.dev/client-libs/shared).

|Exchange|Repository|Nuget|
|--|--|--|
|Aster|[JKorf/Aster.Net](https://github.com/JKorf/Aster.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.Aster.net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.Aster.Net)|
|Binance|[JKorf/Binance.Net](https://github.com/JKorf/Binance.Net)|[![Nuget version](https://img.shields.io/nuget/v/Binance.net.svg?style=flat-square)](https://www.nuget.org/packages/Binance.Net)|
|BingX|[JKorf/BingX.Net](https://github.com/JKorf/BingX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.BingX.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.BingX.Net)|
|Bitfinex|[JKorf/Bitfinex.Net](https://github.com/JKorf/Bitfinex.Net)|[![Nuget version](https://img.shields.io/nuget/v/Bitfinex.net.svg?style=flat-square)](https://www.nuget.org/packages/Bitfinex.Net)|
|Bitget|[JKorf/Bitget.Net](https://github.com/JKorf/Bitget.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.Bitget.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.Bitget.Net)|
|BitMart|[JKorf/BitMart.Net](https://github.com/JKorf/BitMart.Net)|[![Nuget version](https://img.shields.io/nuget/v/BitMart.net.svg?style=flat-square)](https://www.nuget.org/packages/BitMart.Net)|
|BloFin|[JKorf/BloFin.Net](https://github.com/JKorf/BloFin.Net)|[![Nuget version](https://img.shields.io/nuget/v/BloFin.net.svg?style=flat-square)](https://www.nuget.org/packages/BloFin.Net)|
|Bybit|[JKorf/Bybit.Net](https://github.com/JKorf/Bybit.Net)|[![Nuget version](https://img.shields.io/nuget/v/Bybit.net.svg?style=flat-square)](https://www.nuget.org/packages/Bybit.Net)|
|Coinbase|[JKorf/Coinbase.Net](https://github.com/JKorf/Coinbase.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.Coinbase.net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.Coinbase.Net)|
|CoinEx|[JKorf/CoinEx.Net](https://github.com/JKorf/CoinEx.Net)|[![Nuget version](https://img.shields.io/nuget/v/CoinEx.net.svg?style=flat-square)](https://www.nuget.org/packages/CoinEx.Net)|
|CoinGecko|[JKorf/CoinGecko.Net](https://github.com/JKorf/CoinGecko.Net)|[![Nuget version](https://img.shields.io/nuget/v/CoinGecko.net.svg?style=flat-square)](https://www.nuget.org/packages/CoinGecko.Net)|
|CoinW|[JKorf/CoinW.Net](https://github.com/JKorf/CoinW.Net)|[![Nuget version](https://img.shields.io/nuget/v/CoinW.net.svg?style=flat-square)](https://www.nuget.org/packages/CoinW.Net)|
|Crypto.com|[JKorf/CryptoCom.Net](https://github.com/JKorf/CryptoCom.Net)|[![Nuget version](https://img.shields.io/nuget/v/CryptoCom.net.svg?style=flat-square)](https://www.nuget.org/packages/CryptoCom.Net)|
|DeepCoin|[JKorf/DeepCoin.Net](https://github.com/JKorf/DeepCoin.Net)|[![Nuget version](https://img.shields.io/nuget/v/DeepCoin.net.svg?style=flat-square)](https://www.nuget.org/packages/DeepCoin.Net)|
|Gate.io|[JKorf/GateIo.Net](https://github.com/JKorf/GateIo.Net)|[![Nuget version](https://img.shields.io/nuget/v/GateIo.net.svg?style=flat-square)](https://www.nuget.org/packages/GateIo.Net)|
|HTX|[JKorf/HTX.Net](https://github.com/JKorf/HTX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.HTX.net.svg?style=flat-square)](https://www.nuget.org/packages/Jkorf.HTX.Net)|
|HyperLiquid|[JKorf/HyperLiquid.Net](https://github.com/JKorf/HyperLiquid.Net)|[![Nuget version](https://img.shields.io/nuget/v/HyperLiquid.Net.svg?style=flat-square)](https://www.nuget.org/packages/HyperLiquid.Net)|
|Gate.io|[JKorf/GateIo.Net](https://github.com/JKorf/GateIo.Net)|[![Nuget version](https://img.shields.io/nuget/v/GateIo.net.svg?style=flat-square)](https://www.nuget.org/packages/GateIo.Net)|
|Kraken|[JKorf/Kraken.Net](https://github.com/JKorf/Kraken.Net)|[![Nuget version](https://img.shields.io/nuget/v/KrakenExchange.net.svg?style=flat-square)](https://www.nuget.org/packages/KrakenExchange.Net)|
|Kucoin|[JKorf/Kucoin.Net](https://github.com/JKorf/Kucoin.Net)|[![Nuget version](https://img.shields.io/nuget/v/Kucoin.net.svg?style=flat-square)](https://www.nuget.org/packages/Kucoin.Net)|
|Mexc|[JKorf/Mexc.Net](https://github.com/JKorf/Mexc.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.Mexc.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.Mexc.Net)|
|OKX|[JKorf/OKX.Net](https://github.com/JKorf/OKX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.OKX.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.OKX.Net)|
|Toobit|[JKorf/Toobit.Net](https://github.com/JKorf/Toobit.Net)|[![Nuget version](https://img.shields.io/nuget/v/Toobit.net.svg?style=flat-square)](https://www.nuget.org/packages/Toobit.Net)|
|WhiteBit|[JKorf/WhiteBit.Net](https://github.com/JKorf/WhiteBit.Net)|[![Nuget version](https://img.shields.io/nuget/v/WhiteBit.net.svg?style=flat-square)](https://www.nuget.org/packages/WhiteBit.Net)|
|XT|[JKorf/XT.Net](https://github.com/JKorf/XT.Net)|[![Nuget version](https://img.shields.io/nuget/v/XT.net.svg?style=flat-square)](https://www.nuget.org/packages/XT.Net)|

When using multiple of these API's the [CryptoClients.Net](https://github.com/JKorf/CryptoClients.Net) package can be used which combines this and the other packages and allows easy access to all exchange API's.

## Discord
[![Nuget version](https://img.shields.io/discord/847020490588422145?style=for-the-badge)](https://discord.gg/MSpeEtSY8t)  
A Discord server is available [here](https://discord.gg/MSpeEtSY8t). For discussion and/or questions around the CryptoExchange.Net and implementation libraries, feel free to join.

## Supported functionality

### Rest API
|API|Supported|Location|
|--|--:|--|
|Address|✓|`restClient.ExchangeApi.Account`|
|AddressConfig|✓|`restClient.ExchangeApi.Account`|
|Announcement|✓|`restClient.ExchangeApi.ExchangeData`|
|ApiKey|✓|`restClient.ExchangeApi.Account`|
|Chat|X||
|Execution|✓|`restClient.ExchangeApi.Trading`|
|Funding|✓|`restClient.ExchangeApi.ExchangeData`|
|Guild|X||
|Instrument|✓|`restClient.ExchangeApi.ExchangeData`|
|Insurance|✓|`restClient.ExchangeApi.ExchangeData`|
|Leaderboard|X||
|Liquidation|✓|`restClient.ExchangeApi.ExchangeData`|
|Order|✓|`restClient.ExchangeApi.Trading`|
|OrderBook|✓|`restClient.ExchangeApi.ExchangeData`|
|Porl|X||
|Position|✓|`restClient.ExchangeApi.Trading`|
|Quote|✓|`restClient.ExchangeApi.ExchangeData`|
|ReferralCode|X||
|Schema|X||
|Settlement|✓|`restClient.ExchangeApi.ExchangeData`|
|Stats|✓|`restClient.ExchangeApi.ExchangeData`|
|Trade|✓|`restClient.ExchangeApi.ExchangeData`|
|User|✓|`restClient.ExchangeApi.Account`|
|UserAffiliates|X||
|UserEvent|✓|`restClient.ExchangeApi.Account`|
|Wallet|✓|`restClient.ExchangeApi.ExchangeData`|

### WebSocket API
|API|Supported|Location|
|--|--:|--|
|Public|✓|`restClient.ExchangeApi`|
|Private|✓|`restClient.ExchangeApi`|

## Support the project
Any support is greatly appreciated.

### Donate
Make a one time donation in a crypto currency of your choice. If you prefer to donate a currency not listed here please contact me.

**Btc**:  bc1q277a5n54s2l2mzlu778ef7lpkwhjhyvghuv8qf  
**Eth**:  0xcb1b63aCF9fef2755eBf4a0506250074496Ad5b7   
**USDT (TRX)**  TKigKeJPXZYyMVDgMyXxMf17MWYia92Rjd 

### Sponsor
Alternatively, sponsor me on Github using [Github Sponsors](https://github.com/sponsors/JKorf). 

## Release notes
* Version 2.8.0 - 30 Sep 2025
    * Updated CryptoExchange.Net version to 9.8.0, see https://github.com/JKorf/CryptoExchange.Net/releases/
    * Added ITrackerFactory to TrackerFactory implementation
    * Added ContractAddress mapping in Shared IAssetClient implementation
    * Added InvalidQuantity error mapping

* Version 2.7.0 - 01 Sep 2025
    * Updated CryptoExchange.Net version to 9.7.0, see https://github.com/JKorf/CryptoExchange.Net/releases/
    * HTTP REST requests will now use HTTP version 2.0 by default

* Version 2.6.0 - 25 Aug 2025
    * Updated CryptoExchange.Net version to 9.6.0, see https://github.com/JKorf/CryptoExchange.Net/releases/
    * Added ClearUserClients method to user client provider

* Version 2.5.1 - 21 Aug 2025
    * Added Invalid API key error parsing
    * Added unknown symbol websocket error mapping

* Version 2.5.0 - 20 Aug 2025
    * Updated CryptoExchange.Net to version 9.5.0, see https://github.com/JKorf/CryptoExchange.Net/releases/
    * Added improved error parsing
    * Added additional success check on restClient.ExchangeApi.Trading.CancelOrderAsync endpoint
    * Updated rest request sending too prevent duplicate parameter serialization

* Version 2.4.0 - 04 Aug 2025
    * Updated CryptoExchange.Net to version 9.4.0, see https://github.com/JKorf/CryptoExchange.Net/releases/
    * Added support for multi-symbol Shared socket subscriptions

* Version 2.3.0 - 23 Jul 2025
    * Updated CryptoExchange.Net to version 9.3.0, see https://github.com/JKorf/CryptoExchange.Net/releases/
    * Updated websocket message matching

* Version 2.2.1 - 16 Jul 2025
    * Updated CryptoExchange.Net to version 9.2.1, see https://github.com/JKorf/CryptoExchange.Net/releases/
    * Fixed issue with websocket ping response parsing

* Version 2.2.0 - 15 Jul 2025
    * Updated CryptoExchange.Net to version 9.2.0, see https://github.com/JKorf/CryptoExchange.Net/releases/
    * Removed some fields from restClient.ExchangeApi.Account.GetAccountInfoAsync response model no longer returned by API

* Version 2.1.0 - 02 Jun 2025
    * Updated CryptoExchange.Net to version 9.1.0, see https://github.com/JKorf/CryptoExchange.Net/releases/
    * Added (I)BitMEXUserClientProvider allowing for easy client management when handling multiple users

* Version 2.0.0 - 13 May 2025
    * Updated CryptoExchange.Net to version 9.0.0, see https://github.com/JKorf/CryptoExchange.Net/releases/
    * Added support for Native AOT compilation
    * Added RateLimitUpdated event
    * Added SharedSymbol response property to all Shared interfaces response models returning a symbol name
    * Added GenerateClientOrderId method to ExchangeApi Shared clients
    * Added OptionalExchangeParameters and Supported properties to EndpointOptions
    * Added IBookTickerRestClient implementation to ExchangeApi Shared client
    * Added ISpotOrderClientIdClient implementation to ExchangeApi Shared client
    * Added IFuturesOrderClientIdClient implementation to ExchangeApi Shared client
    * Added ISpotTriggerOrderRestClient implementation to ExchangeApi Shared client
    * Added IFuturesTpSlClient implementation to ExchangeApi Shared client
    * Added TriggerPrice, IsTriggerOrder and IsCloseOrder to SharedFuturesOrder model
    * Added TriggerPrice and IsTriggerOrder properties to SharedSpotOrder response model
    * Added QuoteVolume property mapping to SharedSpotTicker response model
    * Added All property to retrieve all available environment on BitMEXEnvironment
    * Added automatic mapping between XBT and BTC asset and symbol names when using the Shared implementations
    * Added missing Enum value ExecutionType enum
    * Refactored Shared clients quantity parameters and responses to use SharedQuantity
    * Updated all IEnumerable response and model types to array response types
    * Updated ExecutionInstruction property on BitMEXOrder model to array
    * Renamed BitMEXExchangeSymbolOrderBook to BitMEXSymbolOrderBook
    * Removed Newtonsoft.Json dependency
    * Removed legacy AddBitMEX(restOptions, socketOptions) DI overload
    * Fixed incorrect DataTradeMode on certain Shared interface responses
    * Fixed InvalidOperationException on empty socket user snapshot updates
    * Fixed some typos
    * Fixed error using Shared GetAssetsAsync endpoint

* Version 2.0.0-beta3 - 01 May 2025
    * Updated CryptoExchange.Net version to 9.0.0-beta5
    * Added property to retrieve all available API environments

* Version 2.0.0-beta2 - 23 Apr 2025
    * Updated CryptoExchange.Net to version 9.0.0-beta2
    * Added Shared spot ticker QuoteVolume mapping
    * Fixed incorrect DataTradeMode on responses
    * Fixed deserialization error in symbol subscription

* Version 2.0.0-beta1 - 22 Apr 2025
    * Updated CryptoExchange.Net to version 9.0.0-beta1, see https://github.com/JKorf/CryptoExchange.Net/releases/
    * Added support for Native AOT compilation
    * Added RateLimitUpdated event
    * Added SharedSymbol response property to all Shared interfaces response models returning a symbol name
    * Added GenerateClientOrderId method to ExchangeApi Shared clients
    * Added OptionalExchangeParameters and Supported properties to EndpointOptions
    * Added IBookTickerRestClient implementation to ExchangeApi Shared client
    * Added ISpotOrderClientIdClient implementation to ExchangeApi Shared client
    * Added IFuturesOrderClientIdClient implementation to ExchangeApi Shared client
    * Added ISpotTriggerOrderRestClient implementation to ExchangeApi Shared client
    * Added IFuturesTpSlClient implementation to ExchangeApi Shared client
    * Added TriggerPrice, IsTriggerOrder and IsCloseOrder to SharedFuturesOrder model
    * Added TriggerPrice and IsTriggerOrder properties to SharedSpotOrder response model
    * Refactored Shared clients quantity parameters and responses to use SharedQuantity
    * Updated all IEnumerable response and model types to array response types
    * Updated ExecutionInstruction property on BitMEXOrder model to array
    * Renamed BitMEXExchangeSymbolOrderBook to BitMEXSymbolOrderBook
    * Removed Newtonsoft.Json dependency
    * Removed legacy AddBitMEX(restOptions, socketOptions) DI overload
    * Fixed some typos
    * Fixed error using Shared GetAssetsAsync endpoint

* Version 1.1.0 - 11 Feb 2025
    * Updated CryptoExchange.Net to version 8.8.0, see https://github.com/JKorf/CryptoExchange.Net/releases/
    * Added setting of DataTime value on websocket DataEvent updates
    * Added DisplayName and ImageUrl to BitMEXExchange class
    * Fixed supported SharedKlineInterval not being specified correctly in the options
    * Fix Mono runtime exception on rest client construction using DI

* Version 1.0.0 - 07 Feb 2025
    * Initial release

