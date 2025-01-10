using BitMEX.Net.Converter;
using BitMEX.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Symbol info
    /// </summary>
    public record BitMEXSymbol
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Base asset
        /// </summary>
        [JsonPropertyName("rootSymbol")]
        public string BaseAsset { get; set; } = string.Empty;
        /// <summary>
        /// Status
        /// </summary>
        [JsonPropertyName("state")]
        public SymbolStatus Status { get; set; }
        /// <summary>
        /// Symbol type
        /// </summary>
        [JsonPropertyName("typ")]
        public SymbolType SymbolType { get; set; }
        /// <summary>
        /// Listing time
        /// </summary>
        [JsonPropertyName("listing")]
        public DateTime ListingTime { get; set; }
        /// <summary>
        /// Front time
        /// </summary>
        [JsonPropertyName("front")]
        public DateTime? FrontTime { get; set; }
        /// <summary>
        /// Expiry time
        /// </summary>
        [JsonPropertyName("expiry")]
        public DateTime? ExpiryTime { get; set; }
        /// <summary>
        /// Settle time
        /// </summary>
        [JsonPropertyName("settle")]
        public DateTime? SettleTime { get; set; }
        /// <summary>
        /// Listed settle
        /// </summary>
        [JsonPropertyName("listedSettle")]
        public DateTime? ListedSettle { get; set; }
        /// <summary>
        /// Position asset
        /// </summary>
        [JsonPropertyName("positionCurrency")]
        public string? PositionAsset { get; set; }
        /// <summary>
        /// Underlying asset
        /// </summary>
        [JsonPropertyName("underlying")]
        public string Underlying { get; set; } = string.Empty;
        /// <summary>
        /// Quote asset
        /// </summary>
        [JsonPropertyName("quoteCurrency")]
        public string QuoteAsset { get; set; } = string.Empty;
        /// <summary>
        /// Underlying symbol
        /// </summary>
        [JsonPropertyName("underlyingSymbol")]
        public string? UnderlyingSymbol { get; set; } = string.Empty;
        /// <summary>
        /// Reference
        /// </summary>
        [JsonPropertyName("reference")]
        public string Reference { get; set; } = string.Empty;
        /// <summary>
        /// Reference symbol
        /// </summary>
        [JsonPropertyName("referenceSymbol")]
        public string? ReferenceSymbol { get; set; } = string.Empty;
        /// <summary>
        /// Calc interval
        /// </summary>
        [JsonPropertyName("calcInterval")]
        public DateTime? CalcInterval { get; set; }
        /// <summary>
        /// Publish interval
        /// </summary>
        [JsonPropertyName("publishInterval")]
        [JsonConverter(typeof(IntervalConverter))]
        public TimeSpan? PublishInterval { get; set; }
        /// <summary>
        /// Publish time
        /// </summary>
        [JsonPropertyName("publishTime")]
        public DateTime? PublishTime { get; set; }
        /// <summary>
        /// Max order quantity
        /// </summary>
        [JsonPropertyName("maxOrderQty")]
        public long MaxOrderQuantity { get; set; }
        /// <summary>
        /// Max price
        /// </summary>
        [JsonPropertyName("maxPrice")]
        public decimal MaxPrice { get; set; }
        /// <summary>
        /// Lot size
        /// </summary>
        [JsonPropertyName("lotSize")]
        public long LotSize { get; set; }
        /// <summary>
        /// Price step
        /// </summary>
        [JsonPropertyName("tickSize")]
        public decimal PriceStep { get; set; }
        /// <summary>
        /// Multiplier
        /// </summary>
        [JsonPropertyName("multiplier")]
        public decimal Multiplier { get; set; }
        /// <summary>
        /// Settlement asset
        /// </summary>
        [JsonPropertyName("settlCurrency")]
        public string? SettlementAsset { get; set; }
        /// <summary>
        /// Underlying to position multiplier
        /// </summary>
        [JsonPropertyName("underlyingToPositionMultiplier")]
        public decimal? UnderlyingToPositionMultiplier { get; set; }
        /// <summary>
        /// Underlying to settle multiplier
        /// </summary>
        [JsonPropertyName("underlyingToSettleMultiplier")]
        public decimal? UnderlyingToSettleMultiplier { get; set; }
        /// <summary>
        /// Quote to settle multiplier
        /// </summary>
        [JsonPropertyName("quoteToSettleMultiplier")]
        public decimal? QuoteToSettleMultiplier { get; set; }
        /// <summary>
        /// Is quanto
        /// </summary>
        [JsonPropertyName("isQuanto")]
        public bool IsQuanto { get; set; }
        /// <summary>
        /// Is inverse
        /// </summary>
        [JsonPropertyName("isInverse")]
        public bool IsInverse { get; set; }
        /// <summary>
        /// Initial margin
        /// </summary>
        [JsonPropertyName("initMargin")]
        public decimal InitialMargin { get; set; }
        /// <summary>
        /// Maintenance margin
        /// </summary>
        [JsonPropertyName("maintMargin")]
        public decimal MaintenanceMargin { get; set; }
        /// <summary>
        /// Risk limit
        /// </summary>
        [JsonPropertyName("riskLimit")]
        public decimal? RiskLimit { get; set; }
        /// <summary>
        /// Risk step
        /// </summary>
        [JsonPropertyName("riskStep")]
        public decimal? RiskStep { get; set; }
        /// <summary>
        /// Limit
        /// </summary>
        [JsonPropertyName("limit")]
        public decimal? Limit { get; set; }
        /// <summary>
        /// Taxed
        /// </summary>
        [JsonPropertyName("taxed")]
        public bool Taxed { get; set; }
        /// <summary>
        /// Deleverage
        /// </summary>
        [JsonPropertyName("deleverage")]
        public bool Deleverage { get; set; }
        /// <summary>
        /// Maker fee
        /// </summary>
        [JsonPropertyName("makerFee")]
        public decimal MakerFee { get; set; }
        /// <summary>
        /// Taker fee
        /// </summary>
        [JsonPropertyName("takerFee")]
        public decimal TakerFee { get; set; }
        /// <summary>
        /// Settlement fee
        /// </summary>
        [JsonPropertyName("settlementFee")]
        public decimal? SettlementFee { get; set; }
        /// <summary>
        /// Funding base symbol
        /// </summary>
        [JsonPropertyName("fundingBaseSymbol")]
        public string? FundingBaseSymbol { get; set; }
        /// <summary>
        /// Funding quote symbol
        /// </summary>
        [JsonPropertyName("fundingQuoteSymbol")]
        public string? FundingQuoteSymbol { get; set; }
        /// <summary>
        /// Funding premium symbol
        /// </summary>
        [JsonPropertyName("fundingPremiumSymbol")]
        public string? FundingPremiumSymbol { get; set; }
        /// <summary>
        /// Funding timestamp
        /// </summary>
        [JsonPropertyName("fundingTimestamp")]
        public DateTime? FundingTimestamp { get; set; }
        /// <summary>
        /// Funding interval
        /// </summary>
        [JsonPropertyName("fundingInterval")]
        [JsonConverter(typeof(IntervalConverter))]
        public TimeSpan? FundingInterval { get; set; }
        /// <summary>
        /// Funding rate
        /// </summary>
        [JsonPropertyName("fundingRate")]
        public decimal? FundingRate { get; set; }
        /// <summary>
        /// Indicative funding rate
        /// </summary>
        [JsonPropertyName("indicativeFundingRate")]
        public decimal? IndicativeFundingRate { get; set; }
        /// <summary>
        /// Rebalance timestamp
        /// </summary>
        [JsonPropertyName("rebalanceTimestamp")]
        public DateTime? RebalanceTimestamp { get; set; }
        /// <summary>
        /// Rebalance interval
        /// </summary>
        [JsonPropertyName("rebalanceInterval")]
        [JsonConverter(typeof(IntervalConverter))]
        public TimeSpan? RebalanceInterval { get; set; }
        /// <summary>
        /// Prev close price
        /// </summary>
        [JsonPropertyName("prevClosePrice")]
        public decimal PrevClosePrice { get; set; }
        /// <summary>
        /// Limit down price
        /// </summary>
        [JsonPropertyName("limitDownPrice")]
        public decimal? LimitDownPrice { get; set; }
        /// <summary>
        /// Limit up price
        /// </summary>
        [JsonPropertyName("limitUpPrice")]
        public decimal? LimitUpPrice { get; set; }
        /// <summary>
        /// Total volume
        /// </summary>
        [JsonPropertyName("totalVolume")]
        public long TotalVolume { get; set; }
        /// <summary>
        /// Volume
        /// </summary>
        [JsonPropertyName("volume")]
        public long Volume { get; set; }
        /// <summary>
        /// Volume 24 hours
        /// </summary>
        [JsonPropertyName("volume24h")]
        public long Volume24h { get; set; }
        /// <summary>
        /// Previous total turnover
        /// </summary>
        [JsonPropertyName("prevTotalTurnover")]
        public long PrevTotalTurnover { get; set; }
        /// <summary>
        /// Total turnover
        /// </summary>
        [JsonPropertyName("totalTurnover")]
        public long TotalTurnover { get; set; }
        /// <summary>
        /// Turnover
        /// </summary>
        [JsonPropertyName("turnover")]
        public long Turnover { get; set; }
        /// <summary>
        /// Turnover24h
        /// </summary>
        [JsonPropertyName("turnover24h")]
        public long Turnover24h { get; set; }
        /// <summary>
        /// Home notional24h
        /// </summary>
        [JsonPropertyName("homeNotional24h")]
        public decimal HomeNotional24h { get; set; }
        /// <summary>
        /// Foreign notional24h
        /// </summary>
        [JsonPropertyName("foreignNotional24h")]
        public decimal ForeignNotional24h { get; set; }
        /// <summary>
        /// Previous price24h
        /// </summary>
        [JsonPropertyName("prevPrice24h")]
        public decimal PrevPrice24h { get; set; }
        /// <summary>
        /// Volume weighted average price
        /// </summary>
        [JsonPropertyName("vwap")]
        public decimal VolumeWeightedAveragePrice { get; set; }
        /// <summary>
        /// High price
        /// </summary>
        [JsonPropertyName("highPrice")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// Low price
        /// </summary>
        [JsonPropertyName("lowPrice")]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// Last price
        /// </summary>
        [JsonPropertyName("lastPrice")]
        public decimal LastPrice { get; set; }
        /// <summary>
        /// Last price protected
        /// </summary>
        [JsonPropertyName("lastPriceProtected")]
        public decimal LastPriceProtected { get; set; }
        /// <summary>
        /// Last tick direction
        /// </summary>
        [JsonPropertyName("lastTickDirection")]
        public TickDirection? LastTickDirection { get; set; }
        /// <summary>
        /// Last change percentage
        /// </summary>
        [JsonPropertyName("lastChangePcnt")]
        public decimal? LastChangePercentage { get; set; }
        /// <summary>
        /// Best bid price
        /// </summary>
        [JsonPropertyName("bidPrice")]
        public decimal? BestBidPrice { get; set; }
        /// <summary>
        /// Mid price
        /// </summary>
        [JsonPropertyName("midPrice")]
        public decimal? MidPrice { get; set; }
        /// <summary>
        /// Best ask price
        /// </summary>
        [JsonPropertyName("askPrice")]
        public decimal? BestAskPrice { get; set; }
        /// <summary>
        /// Impact bid price
        /// </summary>
        [JsonPropertyName("impactBidPrice")]
        public decimal? ImpactBidPrice { get; set; }
        /// <summary>
        /// Impact mid price
        /// </summary>
        [JsonPropertyName("impactMidPrice")]
        public decimal? ImpactMidPrice { get; set; }
        /// <summary>
        /// Impact ask price
        /// </summary>
        [JsonPropertyName("impactAskPrice")]
        public decimal? ImpactAskPrice { get; set; }
        /// <summary>
        /// Has liquidity
        /// </summary>
        [JsonPropertyName("hasLiquidity")]
        public bool HasLiquidity { get; set; }
        /// <summary>
        /// Open interest
        /// </summary>
        [JsonPropertyName("openInterest")]
        public long? OpenInterest { get; set; }
        /// <summary>
        /// Open value
        /// </summary>
        [JsonPropertyName("openValue")]
        public long? OpenValue { get; set; }
        /// <summary>
        /// Fair method
        /// </summary>
        [JsonPropertyName("fairMethod")]
        public FairMethod? FairMethod { get; set; }
        /// <summary>
        /// Fair basis rate
        /// </summary>
        [JsonPropertyName("fairBasisRate")]
        public decimal? FairBasisRate { get; set; }
        /// <summary>
        /// Fair basis
        /// </summary>
        [JsonPropertyName("fairBasis")]
        public decimal? FairBasis { get; set; }
        /// <summary>
        /// Fair price
        /// </summary>
        [JsonPropertyName("fairPrice")]
        public decimal? FairPrice { get; set; }
        /// <summary>
        /// Mark method
        /// </summary>
        [JsonPropertyName("markMethod")]
        public MarkMethod MarkMethod { get; set; }
        /// <summary>
        /// Mark price
        /// </summary>
        [JsonPropertyName("markPrice")]
        public decimal MarkPrice { get; set; }
        /// <summary>
        /// Indicative settle price
        /// </summary>
        [JsonPropertyName("indicativeSettlePrice")]
        public decimal? IndicativeSettlePrice { get; set; }
        /// <summary>
        /// Settled price adjustment rate
        /// </summary>
        [JsonPropertyName("settledPriceAdjustmentRate")]
        public decimal? SettledPriceAdjustmentRate { get; set; }
        /// <summary>
        /// Settled price
        /// </summary>
        [JsonPropertyName("settledPrice")]
        public decimal? SettledPrice { get; set; }
        /// <summary>
        /// Instant pnl
        /// </summary>
        [JsonPropertyName("instantPnl")]
        public bool InstantPnl { get; set; }
        /// <summary>
        /// Min tick
        /// </summary>
        [JsonPropertyName("minTick")]
        public decimal MinTick { get; set; }
        /// <summary>
        /// Funding base rate
        /// </summary>
        [JsonPropertyName("fundingBaseRate")]
        public decimal? FundingBaseRate { get; set; }
        /// <summary>
        /// Funding quote rate
        /// </summary>
        [JsonPropertyName("fundingQuoteRate")]
        public decimal? FundingQuoteRate { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
