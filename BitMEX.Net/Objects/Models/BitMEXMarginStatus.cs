using CryptoExchange.Net.Converters.SystemTextJson;
using System;
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
        /// ["<c>account</c>"] Account id
        /// </summary>
        [JsonPropertyName("account")]
        public long AccountId { get; set; }
        /// <summary>
        /// ["<c>currency</c>"] Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>riskLimit</c>"] Risk limit
        /// </summary>
        [JsonPropertyName("riskLimit")]
        public decimal RiskLimit { get; set; }
        /// <summary>
        /// ["<c>state</c>"] Status
        /// </summary>
        [JsonPropertyName("state")]
        public string? Status { get; set; }
        /// <summary>
        /// ["<c>amount</c>"] Quantity
        /// </summary>
        [JsonPropertyName("amount")]
        public long Quantity { get; set; }
        /// <summary>
        /// ["<c>prevRealisedPnl</c>"] Previous realised profit and loss
        /// </summary>
        [JsonPropertyName("prevRealisedPnl")]
        public long? PrevRealisedPnl { get; set; }
        /// <summary>
        /// ["<c>grossComm</c>"] Gross comm
        /// </summary>
        [JsonPropertyName("grossComm")]
        public long GrossComm { get; set; }
        /// <summary>
        /// ["<c>grossOpenCost</c>"] Gross open cost
        /// </summary>
        [JsonPropertyName("grossOpenCost")]
        public long GrossOpenCost { get; set; }
        /// <summary>
        /// ["<c>grossOpenPremium</c>"] Gross open premium
        /// </summary>
        [JsonPropertyName("grossOpenPremium")]
        public long GrossOpenPremium { get; set; }
        /// <summary>
        /// ["<c>grossExecCost</c>"] Gross execution cost
        /// </summary>
        [JsonPropertyName("grossExecCost")]
        public long? GrossExecCost { get; set; }
        /// <summary>
        /// ["<c>grossMarkValue</c>"] Gross mark value
        /// </summary>
        [JsonPropertyName("grossMarkValue")]
        public long GrossMarkValue { get; set; }
        /// <summary>
        /// ["<c>riskValue</c>"] Risk value
        /// </summary>
        [JsonPropertyName("riskValue")]
        public long RiskValue { get; set; }
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
        /// ["<c>targetExcessMargin</c>"] Target excess margin
        /// </summary>
        [JsonPropertyName("targetExcessMargin")]
        public long TargetExcessMargin { get; set; }
        /// <summary>
        /// ["<c>realisedPnl</c>"] Realised profit and loss
        /// </summary>
        [JsonPropertyName("realisedPnl")]
        public long RealisedPnl { get; set; }
        /// <summary>
        /// ["<c>unrealisedPnl</c>"] Unrealised profit and loss
        /// </summary>
        [JsonPropertyName("unrealisedPnl")]
        public long UnrealisedPnl { get; set; }
        /// <summary>
        /// ["<c>walletBalance</c>"] Wallet balance
        /// </summary>
        [JsonPropertyName("walletBalance")]
        public long WalletBalance { get; set; }
        /// <summary>
        /// ["<c>marginBalance</c>"] Margin balance
        /// </summary>
        [JsonPropertyName("marginBalance")]
        public long MarginBalance { get; set; }
        /// <summary>
        /// ["<c>marginLeverage</c>"] Margin leverage
        /// </summary>
        [JsonPropertyName("marginLeverage")]
        public decimal MarginLeverage { get; set; }
        /// <summary>
        /// ["<c>marginUsedPcnt</c>"] Margin used percentage
        /// </summary>
        [JsonPropertyName("marginUsedPcnt")]
        public decimal MarginUsedPercentage { get; set; }
        /// <summary>
        /// ["<c>excessMargin</c>"] Excess margin
        /// </summary>
        [JsonPropertyName("excessMargin")]
        public decimal ExcessMargin { get; set; }
        /// <summary>
        /// ["<c>availableMargin</c>"] Available margin
        /// </summary>
        [JsonPropertyName("availableMargin")]
        public long AvailableMargin { get; set; }
        /// <summary>
        /// ["<c>withdrawableMargin</c>"] Withdrawable margin
        /// </summary>
        [JsonPropertyName("withdrawableMargin")]
        public long WithdrawableMargin { get; set; }
        /// <summary>
        /// ["<c>makerFeeDiscount</c>"] Maker fee discount
        /// </summary>
        [JsonPropertyName("makerFeeDiscount")]
        public decimal? MakerFeeDiscount { get; set; }
        /// <summary>
        /// ["<c>takerFeeDiscount</c>"] Taker fee discount
        /// </summary>
        [JsonPropertyName("takerFeeDiscount")]
        public decimal? TakerFeeDiscount { get; set; }
        /// <summary>
        /// ["<c>timestamp</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>foreignMarginBalance</c>"] Foreign margin balance
        /// </summary>
        [JsonPropertyName("foreignMarginBalance")]
        public long ForeignMarginBalance { get; set; }
        /// <summary>
        /// ["<c>foreignRequirement</c>"] Foreign requirement
        /// </summary>
        [JsonPropertyName("foreignRequirement")]
        public long ForeignRequirement { get; set; }
    }


}
