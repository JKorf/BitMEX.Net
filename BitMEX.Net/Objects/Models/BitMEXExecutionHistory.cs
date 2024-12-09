using BitMEX.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Execution history
    /// </summary>
    public record BitMEXExecutionHistory : BitMEXOrder
    {
        /// <summary>
        /// Trade id
        /// </summary>
        [JsonPropertyName("execID")]
        public string TradeId { get; set; } = string.Empty;
        /// <summary>
        /// Last trade quantity
        /// </summary>
        [JsonPropertyName("lastQty")]
        public decimal? LastTradeQuantity { get; set; }
        /// <summary>
        /// Last trade price
        /// </summary>
        [JsonPropertyName("lastPx")]
        public decimal? LastTradePrice { get; set; }
        /// <summary>
        /// Last trade role
        /// </summary>
        [JsonPropertyName("lastLiquidityInd")]
        public TradeRole LastTradeRole { get; set; }
        /// <summary>
        /// Pegged order offset value
        /// </summary>
        [JsonPropertyName("pegOffsetValue")]
        public decimal PeggedOffsetValue { get; set; }
        /// <summary>
        /// Execution type
        /// </summary>
        [JsonPropertyName("execType")]
        public string ExecType { get; set; } = string.Empty;
#warning ExecType?
        /// <summary>
        /// Fee
        /// </summary>
        [JsonPropertyName("commission")]
        public decimal Fee { get; set; }
        /// <summary>
        /// Broker fee
        /// </summary>
        [JsonPropertyName("brokerCommission")]
        public decimal BrokerFee { get; set; }
        /// <summary>
        /// Fee type
        /// </summary>
        [JsonPropertyName("feeType")]
        public string FeeType { get; set; } = string.Empty;
#warning Fee type?
        /// <summary>
        /// Trade publish indicator
        /// </summary>
        [JsonPropertyName("tradePublishIndicator")]
        public string TradePublishIndicator { get; set; } = string.Empty;
        /// <summary>
        /// Matching trade id
        /// </summary>
        [JsonPropertyName("trdMatchID")]
        public string TradeMatchId { get; set; } = string.Empty;
        /// <summary>
        /// Execution cost
        /// </summary>
        [JsonPropertyName("execCost")]
        public decimal ExecutionCost { get; set; }
        /// <summary>
        /// Exec comm
        /// </summary>
        [JsonPropertyName("execComm")]
        public decimal ExecComm { get; set; }
        /// <summary>
        /// Broker exec comm
        /// </summary>
        [JsonPropertyName("brokerExecComm")]
        public decimal BrokerExecComm { get; set; }
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
        /// Realised profit and loss
        /// </summary>
        [JsonPropertyName("realisedPnl")]
        public decimal RealisedPnl { get; set; }
        /// <summary>
        /// Trade type
        /// </summary>
        [JsonPropertyName("trdType")]
        public string? TradeType { get; set; }
#warning trade type?
    }


}
