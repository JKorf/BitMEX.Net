using CryptoExchange.Net.Converters.SystemTextJson;
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
    [SerializationModel]
    public record BitMEXExecution : BitMEXOrder
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
        public long? LastTradeQuantity { get; set; }
        /// <summary>
        /// Last trade price
        /// </summary>
        [JsonPropertyName("lastPx")]
        public decimal? LastTradePrice { get; set; }
        /// <summary>
        /// Last trade liquidity indicator
        /// </summary>
        [JsonPropertyName("lastLiquidityInd")]
        public string? LastTradeLiquidityIndicator { get; set; }
        /// <summary>
        /// Execution type
        /// </summary>
        [JsonPropertyName("execType")]
        public ExecutionType ExecutionType { get; set; }
        /// <summary>
        /// Fee
        /// </summary>
        [JsonPropertyName("commission")]
        public decimal FeePercentage { get; set; }
        /// <summary>
        /// Broker fee
        /// </summary>
        [JsonPropertyName("brokerCommission")]
        public decimal? BrokerFee { get; set; }
        /// <summary>
        /// Role
        /// </summary>
        [JsonPropertyName("feeType")]
        public TradeRole? Role { get; set; }

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
        /// Value filled
        /// </summary>
        [JsonPropertyName("execCost")]
        public long ValueFilled { get; set; }
        /// <summary>
        /// Fee
        /// </summary>
        [JsonPropertyName("execComm")]
        public long Fee { get; set; }
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
        public long RealisedPnl { get; set; }
        /// <summary>
        /// Trade type
        /// </summary>
        [JsonPropertyName("trdType")]
        public string? TradeType { get; set; }
    }


}
