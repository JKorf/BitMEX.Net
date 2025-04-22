using BitMEX.Net.Objects.Internal;
using BitMEX.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Converters
{
    [JsonSerializable(typeof(Dictionary<string, BitMEXTradeFee>))]
    [JsonSerializable(typeof(InfoUpdate))]
    [JsonSerializable(typeof(BitMEXServerTime))]

    // End manual defined attributes

    [JsonSerializable(typeof(SocketUpdate<BitMEXOrderBookEntry[]>))]
    [JsonSerializable(typeof(SocketUpdate<BitMEXTradeUpdate[]>))]
    [JsonSerializable(typeof(SocketUpdate<BitMEXAggTrade[]>))]
    [JsonSerializable(typeof(SocketUpdate<BitMEXBookTicker[]>))]
    [JsonSerializable(typeof(SocketUpdate<BitMEXSettlementHistory[]>))]
    [JsonSerializable(typeof(SocketUpdate<BitMEXOrderBookUpdate[]>))]
    [JsonSerializable(typeof(SocketUpdate<BitMEXLiquidation[]>))]
    [JsonSerializable(typeof(SocketUpdate<BitMEXInsurance[]>))]
    [JsonSerializable(typeof(SocketUpdate<BitMEXFundingRate[]>))]
    [JsonSerializable(typeof(SocketUpdate<BitMEXAnnouncement[]>))]
    [JsonSerializable(typeof(SocketUpdate<BitMEXNotification[]>))]
    [JsonSerializable(typeof(SocketUpdate<BitMEXBalance[]>))]
    [JsonSerializable(typeof(SocketUpdate<BitMEXTransaction[]>))]
    [JsonSerializable(typeof(SocketUpdate<BitMEXPosition[]>))]
    [JsonSerializable(typeof(SocketUpdate<BitMEXMarginStatus[]>))]
    [JsonSerializable(typeof(SocketUpdate<BitMEXOrder[]>))]
    [JsonSerializable(typeof(SocketUpdate<BitMEXExecution[]>))]
    [JsonSerializable(typeof(IDictionary<string, object>))]
    [JsonSerializable(typeof(BitMEXSymbolUpdate[]))]
    [JsonSerializable(typeof(BitMEXAccountInfo[]))]
    [JsonSerializable(typeof(BitMEXAccountInfoPreferences[]))]
    [JsonSerializable(typeof(BitMEXAddress[]))]
    [JsonSerializable(typeof(BitMEXAddressBookConfig[]))]
    [JsonSerializable(typeof(BitMEXApiKey[]))]
    [JsonSerializable(typeof(BitMEXAsset[]))]
    [JsonSerializable(typeof(BitMEXAssetNetwork[]))]
    [JsonSerializable(typeof(BitMEXBalanceSummary[]))]
    [JsonSerializable(typeof(BitMEXCompositeIndex[]))]
    [JsonSerializable(typeof(BitMEXExchangeStat[]))]
    [JsonSerializable(typeof(BitMEXExchangeStatHistory[]))]
    [JsonSerializable(typeof(BitMEXExchangeStatHistoryUsd[]))]
    [JsonSerializable(typeof(BitMEXFillRatio[]))]
    [JsonSerializable(typeof(BitMEXIndex[]))]
    [JsonSerializable(typeof(BitMEXIntervals[]))]
    [JsonSerializable(typeof(BitMEXNetwork[]))]
    [JsonSerializable(typeof(BitMEXOrderBook[]))]
    [JsonSerializable(typeof(BitMEXOrderBookIncrementalUpdate[]))]
    [JsonSerializable(typeof(BitMEXSymbol[]))]
    [JsonSerializable(typeof(BitMEXSymbolVolume[]))]
    [JsonSerializable(typeof(BitMEXTrade[]))]
    [JsonSerializable(typeof(BitMEXTradeFee[]))]
    [JsonSerializable(typeof(BitMEXUsdVolume[]))]
    [JsonSerializable(typeof(BitMEXUserEventWrapper[]))]
    [JsonSerializable(typeof(BitMEXUserEvent[]))]
    [JsonSerializable(typeof(BitMEXValueRatio[]))]
    [JsonSerializable(typeof(SocketResponse))]
    [JsonSerializable(typeof(SocketCommand))]
    [JsonSerializable(typeof(string))]
    [JsonSerializable(typeof(int?))]
    [JsonSerializable(typeof(int))]
    [JsonSerializable(typeof(long?))]
    [JsonSerializable(typeof(long))]
    [JsonSerializable(typeof(decimal?))]
    [JsonSerializable(typeof(decimal))]
    [JsonSerializable(typeof(DateTime))]
    [JsonSerializable(typeof(DateTime?))]
    internal partial class BitMEXSourceGenerationContext : JsonSerializerContext
    {
    }
}
