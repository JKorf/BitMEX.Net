using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Position info
    /// </summary>
    public record BitMEXPosition
    {
        /// <summary>
        /// Account id
        /// </summary>
        [JsonPropertyName("account")]
        public long AccountId { get; set; }
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Currency
        /// </summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;
        /// <summary>
        /// Underlying
        /// </summary>
        [JsonPropertyName("underlying")]
        public string Underlying { get; set; } = string.Empty;
        /// <summary>
        /// Quote asset
        /// </summary>
        [JsonPropertyName("quoteCurrency")]
        public string QuoteAsset { get; set; } = string.Empty;
        /// <summary>
        /// Fee
        /// </summary>
        [JsonPropertyName("commission")]
        public decimal Fee { get; set; }
        /// <summary>
        /// Initial margin requirement
        /// </summary>
        [JsonPropertyName("initMarginReq")]
        public decimal InitialMarginRequirement { get; set; }
        /// <summary>
        /// Maintenance margin requirement
        /// </summary>
        [JsonPropertyName("maintMarginReq")]
        public decimal MaintenanceMarginRequirement { get; set; }
        /// <summary>
        /// Risk limit
        /// </summary>
        [JsonPropertyName("riskLimit")]
        public decimal RiskLimit { get; set; }
        /// <summary>
        /// Leverage
        /// </summary>
        [JsonPropertyName("leverage")]
        public decimal Leverage { get; set; }
        /// <summary>
        /// Cross margin
        /// </summary>
        [JsonPropertyName("crossMargin")]
        public bool CrossMargin { get; set; }
        /// <summary>
        /// Deleverage percentile
        /// </summary>
        [JsonPropertyName("deleveragePercentile")]
        public decimal DeleveragePercentile { get; set; }
        /// <summary>
        /// Rebalanced profit and loss
        /// </summary>
        [JsonPropertyName("rebalancedPnl")]
        public decimal RebalancedPnl { get; set; }
        /// <summary>
        /// Previous realised profit and loss
        /// </summary>
        [JsonPropertyName("prevRealisedPnl")]
        public decimal PreviousRealisedPnl { get; set; }
        /// <summary>
        /// Previous unrealised profit and loss
        /// </summary>
        [JsonPropertyName("prevUnrealisedPnl")]
        public decimal PreviousUnrealisedPnl { get; set; }
        /// <summary>
        /// Opening quantity
        /// </summary>
        [JsonPropertyName("openingQty")]
        public decimal OpeningQuantity { get; set; }
        /// <summary>
        /// Open order buy quantity
        /// </summary>
        [JsonPropertyName("openOrderBuyQty")]
        public decimal OpenOrderBuyQuantity { get; set; }
        /// <summary>
        /// Open order buy cost
        /// </summary>
        [JsonPropertyName("openOrderBuyCost")]
        public decimal OpenOrderBuyCost { get; set; }
        /// <summary>
        /// Open order buy premium
        /// </summary>
        [JsonPropertyName("openOrderBuyPremium")]
        public decimal OpenOrderBuyPremium { get; set; }
        /// <summary>
        /// Open order sell quantity
        /// </summary>
        [JsonPropertyName("openOrderSellQty")]
        public decimal OpenOrderSellQuantity { get; set; }
        /// <summary>
        /// Open order sell cost
        /// </summary>
        [JsonPropertyName("openOrderSellCost")]
        public decimal OpenOrderSellCost { get; set; }
        /// <summary>
        /// Open order sell premium
        /// </summary>
        [JsonPropertyName("openOrderSellPremium")]
        public decimal OpenOrderSellPremium { get; set; }
        /// <summary>
        /// Current quantity
        /// </summary>
        [JsonPropertyName("currentQty")]
        public decimal CurrentQuantity { get; set; }
        /// <summary>
        /// Current cost
        /// </summary>
        [JsonPropertyName("currentCost")]
        public decimal CurrentCost { get; set; }
        /// <summary>
        /// Current commissions
        /// </summary>
        [JsonPropertyName("currentComm")]
        public decimal CurrentCommissions { get; set; }
        /// <summary>
        /// Realised cost
        /// </summary>
        [JsonPropertyName("realisedCost")]
        public decimal RealisedCost { get; set; }
        /// <summary>
        /// Unrealised cost
        /// </summary>
        [JsonPropertyName("unrealisedCost")]
        public decimal UnrealisedCost { get; set; }
        /// <summary>
        /// Gross open premium
        /// </summary>
        [JsonPropertyName("grossOpenPremium")]
        public decimal GrossOpenPremium { get; set; }
        /// <summary>
        /// Is open
        /// </summary>
        [JsonPropertyName("isOpen")]
        public bool IsOpen { get; set; }
        /// <summary>
        /// Mark price
        /// </summary>
        [JsonPropertyName("markPrice")]
        public decimal MarkPrice { get; set; }
        /// <summary>
        /// Mark value
        /// </summary>
        [JsonPropertyName("markValue")]
        public decimal MarkValue { get; set; }
        /// <summary>
        /// Risk value
        /// </summary>
        [JsonPropertyName("riskValue")]
        public decimal RiskValue { get; set; }
        /// <summary>
        /// Home notional
        /// </summary>
        [JsonPropertyName("homeNotional")]
        public decimal HomeNotional { get; set; }
        /// <summary>
        /// Foreign notional
        /// </summary>
        [JsonPropertyName("foreignNotional")]
        public decimal ForeignNotional { get; set; }
        /// <summary>
        /// Position status
        /// </summary>
        [JsonPropertyName("posState")]
        public string PositionStatus { get; set; } = string.Empty;
        /// <summary>
        /// Position cost
        /// </summary>
        [JsonPropertyName("posCost")]
        public decimal PositionCost { get; set; }
        /// <summary>
        /// Position cross
        /// </summary>
        [JsonPropertyName("posCross")]
        public decimal PositionCross { get; set; }
        /// <summary>
        /// Position commissions
        /// </summary>
        [JsonPropertyName("posComm")]
        public decimal PositionCommissions { get; set; }
        /// <summary>
        /// Position loss
        /// </summary>
        [JsonPropertyName("posLoss")]
        public decimal PositionLoss { get; set; }
        /// <summary>
        /// Position margin
        /// </summary>
        [JsonPropertyName("posMargin")]
        public decimal PositionMargin { get; set; }
        /// <summary>
        /// Position maintenance
        /// </summary>
        [JsonPropertyName("posMaint")]
        public decimal PositionMaintenance { get; set; }
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
        /// Realised profit and loss
        /// </summary>
        [JsonPropertyName("realisedPnl")]
        public decimal RealisedPnl { get; set; }
        /// <summary>
        /// Unrealised profit and loss
        /// </summary>
        [JsonPropertyName("unrealisedPnl")]
        public decimal UnrealisedPnl { get; set; }
        /// <summary>
        /// Unrealised profit and loss percentage
        /// </summary>
        [JsonPropertyName("unrealisedPnlPcnt")]
        public decimal UnrealisedPnlPcnt { get; set; }
        /// <summary>
        /// Unrealised roe percentage
        /// </summary>
        [JsonPropertyName("unrealisedRoePcnt")]
        public decimal UnrealisedRoePercentage { get; set; }
        /// <summary>
        /// Average cost price
        /// </summary>
        [JsonPropertyName("avgCostPrice")]
        public decimal AverageCostPrice { get; set; }
        /// <summary>
        /// Average entry price
        /// </summary>
        [JsonPropertyName("avgEntryPrice")]
        public decimal AverageEntryPrice { get; set; }
        /// <summary>
        /// Break even price
        /// </summary>
        [JsonPropertyName("breakEvenPrice")]
        public decimal BreakEvenPrice { get; set; }
        /// <summary>
        /// Margin call price
        /// </summary>
        [JsonPropertyName("marginCallPrice")]
        public decimal MarginCallPrice { get; set; }
        /// <summary>
        /// Liquidation price
        /// </summary>
        [JsonPropertyName("liquidationPrice")]
        public decimal LiquidationPrice { get; set; }
        /// <summary>
        /// Bankrupt price
        /// </summary>
        [JsonPropertyName("bankruptPrice")]
        public decimal BankruptPrice { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }


}
