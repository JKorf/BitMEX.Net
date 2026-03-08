using CryptoExchange.Net.Converters.SystemTextJson;
using BitMEX.Net.Converters;
using BitMEX.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Symbol info
    /// </summary>
    [SerializationModel]
    public record BitMEXSymbol
    {
        /// <summary>
        /// ["<c>symbol</c>"] Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>rootSymbol</c>"] Base asset
        /// </summary>
        [JsonPropertyName("rootSymbol")]
        public string BaseAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>state</c>"] Status
        /// </summary>
        [JsonPropertyName("state")]
        public SymbolStatus Status { get; set; }
        /// <summary>
        /// ["<c>typ</c>"] Symbol type
        /// </summary>
        [JsonPropertyName("typ")]
        public SymbolType SymbolType { get; set; }
        /// <summary>
        /// ["<c>listing</c>"] Listing time
        /// </summary>
        [JsonPropertyName("listing")]
        public DateTime ListingTime { get; set; }
        /// <summary>
        /// ["<c>front</c>"] Front time
        /// </summary>
        [JsonPropertyName("front")]
        public DateTime? FrontTime { get; set; }
        /// <summary>
        /// ["<c>expiry</c>"] Expiry time
        /// </summary>
        [JsonPropertyName("expiry")]
        public DateTime? ExpiryTime { get; set; }
        /// <summary>
        /// ["<c>settle</c>"] Settle time
        /// </summary>
        [JsonPropertyName("settle")]
        public DateTime? SettleTime { get; set; }
        /// <summary>
        /// ["<c>listedSettle</c>"] Listed settle
        /// </summary>
        [JsonPropertyName("listedSettle")]
        public DateTime? ListedSettle { get; set; }
        /// <summary>
        /// ["<c>positionCurrency</c>"] Position asset
        /// </summary>
        [JsonPropertyName("positionCurrency")]
        public string? PositionAsset { get; set; }
        /// <summary>
        /// ["<c>underlying</c>"] Underlying asset
        /// </summary>
        [JsonPropertyName("underlying")]
        public string Underlying { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>quoteCurrency</c>"] Quote asset
        /// </summary>
        [JsonPropertyName("quoteCurrency")]
        public string QuoteAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>underlyingSymbol</c>"] Underlying symbol
        /// </summary>
        [JsonPropertyName("underlyingSymbol")]
        public string? UnderlyingSymbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>reference</c>"] Reference
        /// </summary>
        [JsonPropertyName("reference")]
        public string Reference { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>referenceSymbol</c>"] Reference symbol
        /// </summary>
        [JsonPropertyName("referenceSymbol")]
        public string? ReferenceSymbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>calcInterval</c>"] Calc interval
        /// </summary>
        [JsonPropertyName("calcInterval")]
        public DateTime? CalcInterval { get; set; }
        /// <summary>
        /// ["<c>publishInterval</c>"] Publish interval
        /// </summary>
        [JsonPropertyName("publishInterval")]
        [JsonConverter(typeof(IntervalConverter))]
        public TimeSpan? PublishInterval { get; set; }
        /// <summary>
        /// ["<c>publishTime</c>"] Publish time
        /// </summary>
        [JsonPropertyName("publishTime")]
        public DateTime? PublishTime { get; set; }
        /// <summary>
        /// ["<c>maxOrderQty</c>"] Max order quantity
        /// </summary>
        [JsonPropertyName("maxOrderQty")]
        public long MaxOrderQuantity { get; set; }
        /// <summary>
        /// ["<c>maxPrice</c>"] Max price
        /// </summary>
        [JsonPropertyName("maxPrice")]
        public decimal MaxPrice { get; set; }
        /// <summary>
        /// ["<c>lotSize</c>"] Lot size
        /// </summary>
        [JsonPropertyName("lotSize")]
        public long LotSize { get; set; }
        /// <summary>
        /// ["<c>tickSize</c>"] Price step
        /// </summary>
        [JsonPropertyName("tickSize")]
        public decimal PriceStep { get; set; }
        /// <summary>
        /// ["<c>multiplier</c>"] Multiplier
        /// </summary>
        [JsonPropertyName("multiplier")]
        public decimal Multiplier { get; set; }
        /// <summary>
        /// ["<c>settlCurrency</c>"] Settlement asset
        /// </summary>
        [JsonPropertyName("settlCurrency")]
        public string? SettlementAsset { get; set; }
        /// <summary>
        /// ["<c>underlyingToPositionMultiplier</c>"] Underlying to position multiplier
        /// </summary>
        [JsonPropertyName("underlyingToPositionMultiplier")]
        public decimal? UnderlyingToPositionMultiplier { get; set; }
        /// <summary>
        /// ["<c>underlyingToSettleMultiplier</c>"] Underlying to settle multiplier
        /// </summary>
        [JsonPropertyName("underlyingToSettleMultiplier")]
        public decimal? UnderlyingToSettleMultiplier { get; set; }
        /// <summary>
        /// ["<c>quoteToSettleMultiplier</c>"] Quote to settle multiplier
        /// </summary>
        [JsonPropertyName("quoteToSettleMultiplier")]
        public decimal? QuoteToSettleMultiplier { get; set; }
        /// <summary>
        /// ["<c>isQuanto</c>"] Is quanto
        /// </summary>
        [JsonPropertyName("isQuanto")]
        public bool IsQuanto { get; set; }
        /// <summary>
        /// ["<c>isInverse</c>"] Is inverse
        /// </summary>
        [JsonPropertyName("isInverse")]
        public bool IsInverse { get; set; }
        /// <summary>
        /// ["<c>initMargin</c>"] Initial margin
        /// </summary>
        [JsonPropertyName("initMargin")]
        public decimal InitialMargin { get; set; }
        /// <summary>
        /// ["<c>maintMargin</c>"] Maintenance margin
        /// </summary>
        [JsonPropertyName("maintMargin")]
        public decimal MaintenanceMargin { get; set; }
        /// <summary>
        /// ["<c>riskLimit</c>"] Risk limit
        /// </summary>
        [JsonPropertyName("riskLimit")]
        public decimal? RiskLimit { get; set; }
        /// <summary>
        /// ["<c>riskStep</c>"] Risk step
        /// </summary>
        [JsonPropertyName("riskStep")]
        public decimal? RiskStep { get; set; }
        /// <summary>
        /// ["<c>limit</c>"] Limit
        /// </summary>
        [JsonPropertyName("limit")]
        public decimal? Limit { get; set; }
        /// <summary>
        /// ["<c>taxed</c>"] Taxed
        /// </summary>
        [JsonPropertyName("taxed")]
        public bool Taxed { get; set; }
        /// <summary>
        /// ["<c>deleverage</c>"] Deleverage
        /// </summary>
        [JsonPropertyName("deleverage")]
        public bool Deleverage { get; set; }
        /// <summary>
        /// ["<c>makerFee</c>"] Maker fee
        /// </summary>
        [JsonPropertyName("makerFee")]
        public decimal MakerFee { get; set; }
        /// <summary>
        /// ["<c>takerFee</c>"] Taker fee
        /// </summary>
        [JsonPropertyName("takerFee")]
        public decimal TakerFee { get; set; }
        /// <summary>
        /// ["<c>settlementFee</c>"] Settlement fee
        /// </summary>
        [JsonPropertyName("settlementFee")]
        public decimal? SettlementFee { get; set; }
        /// <summary>
        /// ["<c>fundingBaseSymbol</c>"] Funding base symbol
        /// </summary>
        [JsonPropertyName("fundingBaseSymbol")]
        public string? FundingBaseSymbol { get; set; }
        /// <summary>
        /// ["<c>fundingQuoteSymbol</c>"] Funding quote symbol
        /// </summary>
        [JsonPropertyName("fundingQuoteSymbol")]
        public string? FundingQuoteSymbol { get; set; }
        /// <summary>
        /// ["<c>fundingPremiumSymbol</c>"] Funding premium symbol
        /// </summary>
        [JsonPropertyName("fundingPremiumSymbol")]
        public string? FundingPremiumSymbol { get; set; }
        /// <summary>
        /// ["<c>fundingTimestamp</c>"] Funding timestamp
        /// </summary>
        [JsonPropertyName("fundingTimestamp")]
        public DateTime? FundingTimestamp { get; set; }
        /// <summary>
        /// ["<c>fundingInterval</c>"] Funding interval
        /// </summary>
        [JsonPropertyName("fundingInterval")]
        [JsonConverter(typeof(IntervalConverter))]
        public TimeSpan? FundingInterval { get; set; }
        /// <summary>
        /// ["<c>fundingRate</c>"] Funding rate
        /// </summary>
        [JsonPropertyName("fundingRate")]
        public decimal? FundingRate { get; set; }
        /// <summary>
        /// ["<c>indicativeFundingRate</c>"] Indicative funding rate
        /// </summary>
        [JsonPropertyName("indicativeFundingRate")]
        public decimal? IndicativeFundingRate { get; set; }
        /// <summary>
        /// ["<c>rebalanceTimestamp</c>"] Rebalance timestamp
        /// </summary>
        [JsonPropertyName("rebalanceTimestamp")]
        public DateTime? RebalanceTimestamp { get; set; }
        /// <summary>
        /// ["<c>rebalanceInterval</c>"] Rebalance interval
        /// </summary>
        [JsonPropertyName("rebalanceInterval")]
        [JsonConverter(typeof(IntervalConverter))]
        public TimeSpan? RebalanceInterval { get; set; }
        /// <summary>
        /// ["<c>prevClosePrice</c>"] Prev close price
        /// </summary>
        [JsonPropertyName("prevClosePrice")]
        public decimal PrevClosePrice { get; set; }
        /// <summary>
        /// ["<c>limitDownPrice</c>"] Limit down price
        /// </summary>
        [JsonPropertyName("limitDownPrice")]
        public decimal? LimitDownPrice { get; set; }
        /// <summary>
        /// ["<c>limitUpPrice</c>"] Limit up price
        /// </summary>
        [JsonPropertyName("limitUpPrice")]
        public decimal? LimitUpPrice { get; set; }
        /// <summary>
        /// ["<c>totalVolume</c>"] Total volume
        /// </summary>
        [JsonPropertyName("totalVolume")]
        public long TotalVolume { get; set; }
        /// <summary>
        /// ["<c>volume</c>"] Volume
        /// </summary>
        [JsonPropertyName("volume")]
        public long Volume { get; set; }
        /// <summary>
        /// ["<c>volume24h</c>"] Volume 24 hours
        /// </summary>
        [JsonPropertyName("volume24h")]
        public long Volume24h { get; set; }
        /// <summary>
        /// ["<c>prevTotalTurnover</c>"] Previous total turnover
        /// </summary>
        [JsonPropertyName("prevTotalTurnover")]
        public long PrevTotalTurnover { get; set; }
        /// <summary>
        /// ["<c>totalTurnover</c>"] Total turnover
        /// </summary>
        [JsonPropertyName("totalTurnover")]
        public long TotalTurnover { get; set; }
        /// <summary>
        /// ["<c>turnover</c>"] Turnover
        /// </summary>
        [JsonPropertyName("turnover")]
        public long Turnover { get; set; }
        /// <summary>
        /// ["<c>turnover24h</c>"] Turnover24h
        /// </summary>
        [JsonPropertyName("turnover24h")]
        public long Turnover24h { get; set; }
        /// <summary>
        /// ["<c>homeNotional24h</c>"] Home notional24h
        /// </summary>
        [JsonPropertyName("homeNotional24h")]
        public decimal HomeNotional24h { get; set; }
        /// <summary>
        /// ["<c>foreignNotional24h</c>"] Foreign notional24h
        /// </summary>
        [JsonPropertyName("foreignNotional24h")]
        public decimal ForeignNotional24h { get; set; }
        /// <summary>
        /// ["<c>prevPrice24h</c>"] Previous price24h
        /// </summary>
        [JsonPropertyName("prevPrice24h")]
        public decimal PrevPrice24h { get; set; }
        /// <summary>
        /// ["<c>vwap</c>"] Volume weighted average price
        /// </summary>
        [JsonPropertyName("vwap")]
        public decimal VolumeWeightedAveragePrice { get; set; }
        /// <summary>
        /// ["<c>highPrice</c>"] High price
        /// </summary>
        [JsonPropertyName("highPrice")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// ["<c>lowPrice</c>"] Low price
        /// </summary>
        [JsonPropertyName("lowPrice")]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// ["<c>lastPrice</c>"] Last price
        /// </summary>
        [JsonPropertyName("lastPrice")]
        public decimal LastPrice { get; set; }
        /// <summary>
        /// ["<c>lastPriceProtected</c>"] Last price protected
        /// </summary>
        [JsonPropertyName("lastPriceProtected")]
        public decimal LastPriceProtected { get; set; }
        /// <summary>
        /// ["<c>lastTickDirection</c>"] Last tick direction
        /// </summary>
        [JsonPropertyName("lastTickDirection")]
        public TickDirection? LastTickDirection { get; set; }
        /// <summary>
        /// ["<c>lastChangePcnt</c>"] Last change percentage
        /// </summary>
        [JsonPropertyName("lastChangePcnt")]
        public decimal? LastChangePercentage { get; set; }
        /// <summary>
        /// ["<c>bidPrice</c>"] Best bid price
        /// </summary>
        [JsonPropertyName("bidPrice")]
        public decimal? BestBidPrice { get; set; }
        /// <summary>
        /// ["<c>midPrice</c>"] Mid price
        /// </summary>
        [JsonPropertyName("midPrice")]
        public decimal? MidPrice { get; set; }
        /// <summary>
        /// ["<c>askPrice</c>"] Best ask price
        /// </summary>
        [JsonPropertyName("askPrice")]
        public decimal? BestAskPrice { get; set; }
        /// <summary>
        /// ["<c>impactBidPrice</c>"] Impact bid price
        /// </summary>
        [JsonPropertyName("impactBidPrice")]
        public decimal? ImpactBidPrice { get; set; }
        /// <summary>
        /// ["<c>impactMidPrice</c>"] Impact mid price
        /// </summary>
        [JsonPropertyName("impactMidPrice")]
        public decimal? ImpactMidPrice { get; set; }
        /// <summary>
        /// ["<c>impactAskPrice</c>"] Impact ask price
        /// </summary>
        [JsonPropertyName("impactAskPrice")]
        public decimal? ImpactAskPrice { get; set; }
        /// <summary>
        /// ["<c>hasLiquidity</c>"] Has liquidity
        /// </summary>
        [JsonPropertyName("hasLiquidity")]
        public bool HasLiquidity { get; set; }
        /// <summary>
        /// ["<c>openInterest</c>"] Open interest
        /// </summary>
        [JsonPropertyName("openInterest")]
        public long? OpenInterest { get; set; }
        /// <summary>
        /// ["<c>openValue</c>"] Open value
        /// </summary>
        [JsonPropertyName("openValue")]
        public long? OpenValue { get; set; }
        /// <summary>
        /// ["<c>fairMethod</c>"] Fair method
        /// </summary>
        [JsonPropertyName("fairMethod")]
        public FairMethod? FairMethod { get; set; }
        /// <summary>
        /// ["<c>fairBasisRate</c>"] Fair basis rate
        /// </summary>
        [JsonPropertyName("fairBasisRate")]
        public decimal? FairBasisRate { get; set; }
        /// <summary>
        /// ["<c>fairBasis</c>"] Fair basis
        /// </summary>
        [JsonPropertyName("fairBasis")]
        public decimal? FairBasis { get; set; }
        /// <summary>
        /// ["<c>fairPrice</c>"] Fair price
        /// </summary>
        [JsonPropertyName("fairPrice")]
        public decimal? FairPrice { get; set; }
        /// <summary>
        /// ["<c>markMethod</c>"] Mark method
        /// </summary>
        [JsonPropertyName("markMethod")]
        public MarkMethod MarkMethod { get; set; }
        /// <summary>
        /// ["<c>markPrice</c>"] Mark price
        /// </summary>
        [JsonPropertyName("markPrice")]
        public decimal MarkPrice { get; set; }
        /// <summary>
        /// ["<c>indicativeSettlePrice</c>"] Indicative settle price
        /// </summary>
        [JsonPropertyName("indicativeSettlePrice")]
        public decimal? IndicativeSettlePrice { get; set; }
        /// <summary>
        /// ["<c>settledPriceAdjustmentRate</c>"] Settled price adjustment rate
        /// </summary>
        [JsonPropertyName("settledPriceAdjustmentRate")]
        public decimal? SettledPriceAdjustmentRate { get; set; }
        /// <summary>
        /// ["<c>settledPrice</c>"] Settled price
        /// </summary>
        [JsonPropertyName("settledPrice")]
        public decimal? SettledPrice { get; set; }
        /// <summary>
        /// ["<c>instantPnl</c>"] Instant pnl
        /// </summary>
        [JsonPropertyName("instantPnl")]
        public bool InstantPnl { get; set; }
        /// <summary>
        /// ["<c>minTick</c>"] Min tick
        /// </summary>
        [JsonPropertyName("minTick")]
        public decimal MinTick { get; set; }
        /// <summary>
        /// ["<c>fundingBaseRate</c>"] Funding base rate
        /// </summary>
        [JsonPropertyName("fundingBaseRate")]
        public decimal? FundingBaseRate { get; set; }
        /// <summary>
        /// ["<c>fundingQuoteRate</c>"] Funding quote rate
        /// </summary>
        [JsonPropertyName("fundingQuoteRate")]
        public decimal? FundingQuoteRate { get; set; }
        /// <summary>
        /// ["<c>timestamp</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
