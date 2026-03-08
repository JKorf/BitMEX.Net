using CryptoExchange.Net.Converters.SystemTextJson;
using BitMEX.Net.Enums;
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
        /// ["<c>execID</c>"] Trade id
        /// </summary>
        [JsonPropertyName("execID")]
        public string TradeId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>lastQty</c>"] Last trade quantity
        /// </summary>
        [JsonPropertyName("lastQty")]
        public long? LastTradeQuantity { get; set; }
        /// <summary>
        /// ["<c>lastPx</c>"] Last trade price
        /// </summary>
        [JsonPropertyName("lastPx")]
        public decimal? LastTradePrice { get; set; }
        /// <summary>
        /// ["<c>lastLiquidityInd</c>"] Last trade liquidity indicator
        /// </summary>
        [JsonPropertyName("lastLiquidityInd")]
        public string? LastTradeLiquidityIndicator { get; set; }
        /// <summary>
        /// ["<c>execType</c>"] Execution type
        /// </summary>
        [JsonPropertyName("execType")]
        public ExecutionType ExecutionType { get; set; }
        /// <summary>
        /// ["<c>commission</c>"] Fee
        /// </summary>
        [JsonPropertyName("commission")]
        public decimal FeePercentage { get; set; }
        /// <summary>
        /// ["<c>brokerCommission</c>"] Broker fee
        /// </summary>
        [JsonPropertyName("brokerCommission")]
        public decimal? BrokerFee { get; set; }
        /// <summary>
        /// ["<c>feeType</c>"] Role
        /// </summary>
        [JsonPropertyName("feeType")]
        public TradeRole? Role { get; set; }

        /// <summary>
        /// ["<c>tradePublishIndicator</c>"] Trade publish indicator
        /// </summary>
        [JsonPropertyName("tradePublishIndicator")]
        public string TradePublishIndicator { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>trdMatchID</c>"] Matching trade id
        /// </summary>
        [JsonPropertyName("trdMatchID")]
        public string TradeMatchId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>execCost</c>"] Value filled
        /// </summary>
        [JsonPropertyName("execCost")]
        public long ValueFilled { get; set; }
        /// <summary>
        /// ["<c>execComm</c>"] Fee
        /// </summary>
        [JsonPropertyName("execComm")]
        public long Fee { get; set; }
        /// <summary>
        /// ["<c>brokerExecComm</c>"] Broker exec comm
        /// </summary>
        [JsonPropertyName("brokerExecComm")]
        public decimal BrokerExecComm { get; set; }
        /// <summary>
        /// ["<c>homeNotional</c>"] Home notional
        /// </summary>
        [JsonPropertyName("homeNotional")]
        public decimal HomeNotional { get; set; }
        /// <summary>
        /// ["<c>foreignNotional</c>"] Foreign notional
        /// </summary>
        [JsonPropertyName("foreignNotional")]
        public decimal ForeignNotional { get; set; }
        /// <summary>
        /// ["<c>realisedPnl</c>"] Realised profit and loss
        /// </summary>
        [JsonPropertyName("realisedPnl")]
        public long RealisedPnl { get; set; }
        /// <summary>
        /// ["<c>trdType</c>"] Trade type
        /// </summary>
        [JsonPropertyName("trdType")]
        public string? TradeType { get; set; }
    }


}
