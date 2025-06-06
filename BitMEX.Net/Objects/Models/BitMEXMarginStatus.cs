using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Margin status
    /// </summary>
    [SerializationModel]
    public record BitMEXMarginStatus
    {
        /// <summary>
        /// Account id
        /// </summary>
        [JsonPropertyName("account")]
        public long AccountId { get; set; }
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Risk limit
        /// </summary>
        [JsonPropertyName("riskLimit")]
        public decimal RiskLimit { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        [JsonPropertyName("state")]
        public string? Status { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("amount")]
        public long Quantity { get; set; }
        /// <summary>
        /// Previous realised profit and loss
        /// </summary>
        [JsonPropertyName("prevRealisedPnl")]
        public long? PrevRealisedPnl { get; set; }
        /// <summary>
        /// Gross comm
        /// </summary>
        [JsonPropertyName("grossComm")]
        public long GrossComm { get; set; }
        /// <summary>
        /// Gross open cost
        /// </summary>
        [JsonPropertyName("grossOpenCost")]
        public long GrossOpenCost { get; set; }
        /// <summary>
        /// Gross open premium
        /// </summary>
        [JsonPropertyName("grossOpenPremium")]
        public long GrossOpenPremium { get; set; }
        /// <summary>
        /// Gross execution cost
        /// </summary>
        [JsonPropertyName("grossExecCost")]
        public long? GrossExecCost { get; set; }
        /// <summary>
        /// Gross mark value
        /// </summary>
        [JsonPropertyName("grossMarkValue")]
        public long GrossMarkValue { get; set; }
        /// <summary>
        /// Risk value
        /// </summary>
        [JsonPropertyName("riskValue")]
        public long RiskValue { get; set; }
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
        /// Target excess margin
        /// </summary>
        [JsonPropertyName("targetExcessMargin")]
        public long TargetExcessMargin { get; set; }
        /// <summary>
        /// Realised profit and loss
        /// </summary>
        [JsonPropertyName("realisedPnl")]
        public long RealisedPnl { get; set; }
        /// <summary>
        /// Unrealised profit and loss
        /// </summary>
        [JsonPropertyName("unrealisedPnl")]
        public long UnrealisedPnl { get; set; }
        /// <summary>
        /// Wallet balance
        /// </summary>
        [JsonPropertyName("walletBalance")]
        public long WalletBalance { get; set; }
        /// <summary>
        /// Margin balance
        /// </summary>
        [JsonPropertyName("marginBalance")]
        public long MarginBalance { get; set; }
        /// <summary>
        /// Margin leverage
        /// </summary>
        [JsonPropertyName("marginLeverage")]
        public decimal MarginLeverage { get; set; }
        /// <summary>
        /// Margin used percentage
        /// </summary>
        [JsonPropertyName("marginUsedPcnt")]
        public decimal MarginUsedPercentage { get; set; }
        /// <summary>
        /// Excess margin
        /// </summary>
        [JsonPropertyName("excessMargin")]
        public decimal ExcessMargin { get; set; }
        /// <summary>
        /// Available margin
        /// </summary>
        [JsonPropertyName("availableMargin")]
        public long AvailableMargin { get; set; }
        /// <summary>
        /// Withdrawable margin
        /// </summary>
        [JsonPropertyName("withdrawableMargin")]
        public long WithdrawableMargin { get; set; }
        /// <summary>
        /// Maker fee discount
        /// </summary>
        [JsonPropertyName("makerFeeDiscount")]
        public decimal? MakerFeeDiscount { get; set; }
        /// <summary>
        /// Taker fee discount
        /// </summary>
        [JsonPropertyName("takerFeeDiscount")]
        public decimal? TakerFeeDiscount { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Foreign margin balance
        /// </summary>
        [JsonPropertyName("foreignMarginBalance")]
        public long ForeignMarginBalance { get; set; }
        /// <summary>
        /// Foreign requirement
        /// </summary>
        [JsonPropertyName("foreignRequirement")]
        public long ForeignRequirement { get; set; }
    }


}
