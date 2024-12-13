# ![BitMEX.Net](https://raw.githubusercontent.com/JKorf/BitMEX.Net/main/BitMEX.Net/Icon/icon.png) BitMEX.Net  

[![.NET](https://img.shields.io/github/actions/workflow/status/JKorf/BitMEX.Net/dotnet.yml?style=for-the-badge)](https://github.com/JKorf/BitMEX.Net/actions/workflows/dotnet.yml) ![License](https://img.shields.io/github/license/JKorf/BitMEX.Net?style=for-the-badge)

BitMEX.Net is a client library for accessing the [BitMEX REST and Websocket API](https://www.bitmex.com/app/apiOverview). 

## Features
* Response data is mapped to descriptive models
* Input parameters and response values are mapped to discriptive enum values where possible
* Automatic websocket (re)connection management 
* Client side rate limiting 
* Cient side order book implementation
* Extensive logging
* Support for different environments
* Easy integration with other exchange client based on the CryptoExchange.Net base library

## Supported Frameworks
The library is targeting both `.NET Standard 2.0` and `.NET Standard 2.1` for optimal compatibility

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

For information on the clients, dependency injection, response processing and more see the [documentation](https://jkorf.github.io/CryptoExchange.Net), or have a look at the examples [here](https://github.com/JKorf/BitMEX.Net/tree/main/Examples) or [here](https://github.com/JKorf/CryptoExchange.Net/tree/master/Examples).

## CryptoExchange.Net
BitMEX.Net is based on the [CryptoExchange.Net](https://github.com/JKorf/CryptoExchange.Net) base library. Other exchange API implementations based on the CryptoExchange.Net base library are available and follow the same logic.

CryptoExchange.Net also allows for [easy access to different exchange API's](https://jkorf.github.io/CryptoExchange.Net#idocs_shared).

|Exchange|Repository|Nuget|
|--|--|--|
|Binance|[JKorf/Binance.Net](https://github.com/JKorf/Binance.Net)|[![Nuget version](https://img.shields.io/nuget/v/Binance.net.svg?style=flat-square)](https://www.nuget.org/packages/Binance.Net)|
|BingX|[JKorf/BingX.Net](https://github.com/JKorf/BingX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.BingX.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.BingX.Net)|
|Bitfinex|[JKorf/Bitfinex.Net](https://github.com/JKorf/Bitfinex.Net)|[![Nuget version](https://img.shields.io/nuget/v/Bitfinex.net.svg?style=flat-square)](https://www.nuget.org/packages/Bitfinex.Net)|
|Bitget|[JKorf/Bitget.Net](https://github.com/JKorf/Bitget.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.Bitget.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.Bitget.Net)|
|BitMart|[JKorf/BitMart.Net](https://github.com/JKorf/BitMart.Net)|[![Nuget version](https://img.shields.io/nuget/v/BitMart.net.svg?style=flat-square)](https://www.nuget.org/packages/BitMart.Net)|
|Bybit|[JKorf/Bybit.Net](https://github.com/JKorf/Bybit.Net)|[![Nuget version](https://img.shields.io/nuget/v/Bybit.net.svg?style=flat-square)](https://www.nuget.org/packages/Bybit.Net)|
|Coinbase|[JKorf/Coinbase.Net](https://github.com/JKorf/Coinbase.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.Coinbase.net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.Coinbase.Net)|
|CoinEx|[JKorf/CoinEx.Net](https://github.com/JKorf/CoinEx.Net)|[![Nuget version](https://img.shields.io/nuget/v/CoinEx.net.svg?style=flat-square)](https://www.nuget.org/packages/CoinEx.Net)|
|CoinGecko|[JKorf/CoinGecko.Net](https://github.com/JKorf/CoinGecko.Net)|[![Nuget version](https://img.shields.io/nuget/v/CoinGecko.net.svg?style=flat-square)](https://www.nuget.org/packages/CoinGecko.Net)|
|Crypto.com|[JKorf/CryptoCom.Net](https://github.com/JKorf/CryptoCom.Net)|[![Nuget version](https://img.shields.io/nuget/v/CryptoCom.net.svg?style=flat-square)](https://www.nuget.org/packages/CryptoCom.Net)|
|Gate.io|[JKorf/GateIo.Net](https://github.com/JKorf/GateIo.Net)|[![Nuget version](https://img.shields.io/nuget/v/GateIo.net.svg?style=flat-square)](https://www.nuget.org/packages/GateIo.Net)|
|HTX|[JKorf/HTX.Net](https://github.com/JKorf/HTX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.HTX.net.svg?style=flat-square)](https://www.nuget.org/packages/Jkorf.HTX.Net)|
|Gate.io|[JKorf/GateIo.Net](https://github.com/JKorf/GateIo.Net)|[![Nuget version](https://img.shields.io/nuget/v/GateIo.net.svg?style=flat-square)](https://www.nuget.org/packages/GateIo.Net)|
|Kraken|[JKorf/Kraken.Net](https://github.com/JKorf/Kraken.Net)|[![Nuget version](https://img.shields.io/nuget/v/KrakenExchange.net.svg?style=flat-square)](https://www.nuget.org/packages/KrakenExchange.Net)|
|Kucoin|[JKorf/Kucoin.Net](https://github.com/JKorf/Kucoin.Net)|[![Nuget version](https://img.shields.io/nuget/v/Kucoin.net.svg?style=flat-square)](https://www.nuget.org/packages/Kucoin.Net)|
|Mexc|[JKorf/Mexc.Net](https://github.com/JKorf/Mexc.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.Mexc.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.Mexc.Net)|
|OKX|[JKorf/OKX.Net](https://github.com/JKorf/OKX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.OKX.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.OKX.Net)|
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
