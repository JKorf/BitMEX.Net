using CryptoExchange.Net.Converters.SystemTextJson;
using BitMEX.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Order info
    /// </summary>
    [SerializationModel]
    public record BitMEXOrder
    {
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("orderID")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonPropertyName("clOrdID")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// Client order link id
        /// </summary>
        [JsonPropertyName("clOrdLinkID")]
        public string? ClientOrderLinkId { get; set; }
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
        /// Order side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide OrderSide { get; set; }
        /// <summary>
        /// Order quantity
        /// </summary>
        [JsonPropertyName("orderQty")]
        public long? Quantity { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal? Price { get; set; }
        /// <summary>
        /// Display quantity
        /// </summary>
        [JsonPropertyName("displayQty")]
        public decimal? DisplayQuantity { get; set; }
        /// <summary>
        /// Stop price
        /// </summary>
        [JsonPropertyName("stopPx")]
        public decimal? StopPrice { get; set; }
        /// <summary>
        /// Pegged order price type
        /// </summary>
        [JsonPropertyName("pegPriceType")]
        public PeggedPriceType? PeggedPriceType { get; set; }
        /// <summary>
        /// Currency
        /// </summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;
        /// <summary>
        /// Settlement currency
        /// </summary>
        [JsonPropertyName("settlCurrency")]
        public string? SettlementCurrency { get; set; }
        /// <summary>
        /// Order type
        /// </summary>
        [JsonPropertyName("ordType")]
        public OrderType OrderType { get; set; }
        /// <summary>
        /// Time in force
        /// </summary>
        [JsonPropertyName("timeInForce")]
        public TimeInForce TimeInForce { get; set; }
        /// <summary>
        /// Execution instructions
        /// </summary>
        [JsonPropertyName("execInst")]
        [JsonConverter(typeof(CommaSplitEnumConverter<ExecutionInstruction>))]
        public ExecutionInstruction[]? ExecutionInstruction { get; set; }
        /// <summary>
        /// Contingency type
        /// </summary>
        [JsonPropertyName("contingencyType")]
        public ContingencyType? ContingencyType { get; set; }
        /// <summary>
        /// Order status
        /// </summary>
        [JsonPropertyName("ordStatus")]
        public OrderStatus Status { get; set; }
        /// <summary>
        /// Triggered
        /// </summary>
        [JsonPropertyName("triggered")]
        public string? Triggered { get; set; }
        /// <summary>
        /// Working indicator
        /// </summary>
        [JsonPropertyName("workingIndicator")]
        public bool WorkingIndicator { get; set; }
        /// <summary>
        /// Order reject reason
        /// </summary>
        [JsonPropertyName("ordRejReason")]
        public string? OrderRejectReason { get; set; }
        /// <summary>
        /// Quantity remaining
        /// </summary>
        [JsonPropertyName("leavesQty")]
        public long QuantityRemaining { get; set; }
        /// <summary>
        /// Quantity filled
        /// </summary>
        [JsonPropertyName("cumQty")]
        public long QuantityFilled { get; set; }
        /// <summary>
        /// Average price
        /// </summary>
        [JsonPropertyName("avgPx")]
        public decimal? AveragePrice { get; set; }
        /// <summary>
        /// Transact time
        /// </summary>
        [JsonPropertyName("transactTime")]
        public DateTime TransactTime { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Peg offset value
        /// </summary>
        [JsonPropertyName("pegOffsetValue")]
        public long? PegOffsetValue { get; set; }
    }


}
