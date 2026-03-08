using CryptoExchange.Net.Converters.SystemTextJson;
using BitMEX.Net.Enums;
using System;
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
        /// ["<c>orderID</c>"] Order id
        /// </summary>
        [JsonPropertyName("orderID")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>clOrdID</c>"] Client order id
        /// </summary>
        [JsonPropertyName("clOrdID")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// ["<c>clOrdLinkID</c>"] Client order link id
        /// </summary>
        [JsonPropertyName("clOrdLinkID")]
        public string? ClientOrderLinkId { get; set; }
        /// <summary>
        /// ["<c>account</c>"] Account id
        /// </summary>
        [JsonPropertyName("account")]
        public long AccountId { get; set; }
        /// <summary>
        /// ["<c>symbol</c>"] Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>side</c>"] Order side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide OrderSide { get; set; }
        /// <summary>
        /// ["<c>orderQty</c>"] Order quantity
        /// </summary>
        [JsonPropertyName("orderQty")]
        public long? Quantity { get; set; }
        /// <summary>
        /// ["<c>price</c>"] Price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal? Price { get; set; }
        /// <summary>
        /// ["<c>displayQty</c>"] Display quantity
        /// </summary>
        [JsonPropertyName("displayQty")]
        public decimal? DisplayQuantity { get; set; }
        /// <summary>
        /// ["<c>stopPx</c>"] Stop price
        /// </summary>
        [JsonPropertyName("stopPx")]
        public decimal? StopPrice { get; set; }
        /// <summary>
        /// ["<c>pegPriceType</c>"] Pegged order price type
        /// </summary>
        [JsonPropertyName("pegPriceType")]
        public PeggedPriceType? PeggedPriceType { get; set; }
        /// <summary>
        /// ["<c>currency</c>"] Currency
        /// </summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>settlCurrency</c>"] Settlement currency
        /// </summary>
        [JsonPropertyName("settlCurrency")]
        public string? SettlementCurrency { get; set; }
        /// <summary>
        /// ["<c>ordType</c>"] Order type
        /// </summary>
        [JsonPropertyName("ordType")]
        public OrderType OrderType { get; set; }
        /// <summary>
        /// ["<c>timeInForce</c>"] Time in force
        /// </summary>
        [JsonPropertyName("timeInForce")]
        public TimeInForce TimeInForce { get; set; }
        /// <summary>
        /// ["<c>execInst</c>"] Execution instructions
        /// </summary>
        [JsonPropertyName("execInst")]
        [JsonConverter(typeof(CommaSplitEnumConverter<ExecutionInstruction>))]
        public ExecutionInstruction[]? ExecutionInstruction { get; set; }
        /// <summary>
        /// ["<c>contingencyType</c>"] Contingency type
        /// </summary>
        [JsonPropertyName("contingencyType")]
        public ContingencyType? ContingencyType { get; set; }
        /// <summary>
        /// ["<c>ordStatus</c>"] Order status
        /// </summary>
        [JsonPropertyName("ordStatus")]
        public OrderStatus Status { get; set; }
        /// <summary>
        /// ["<c>triggered</c>"] Triggered
        /// </summary>
        [JsonPropertyName("triggered")]
        public string? Triggered { get; set; }
        /// <summary>
        /// ["<c>workingIndicator</c>"] Working indicator
        /// </summary>
        [JsonPropertyName("workingIndicator")]
        public bool WorkingIndicator { get; set; }
        /// <summary>
        /// ["<c>ordRejReason</c>"] Order reject reason
        /// </summary>
        [JsonPropertyName("ordRejReason")]
        public string? OrderRejectReason { get; set; }
        /// <summary>
        /// ["<c>leavesQty</c>"] Quantity remaining
        /// </summary>
        [JsonPropertyName("leavesQty")]
        public long QuantityRemaining { get; set; }
        /// <summary>
        /// ["<c>cumQty</c>"] Quantity filled
        /// </summary>
        [JsonPropertyName("cumQty")]
        public long QuantityFilled { get; set; }
        /// <summary>
        /// ["<c>avgPx</c>"] Average price
        /// </summary>
        [JsonPropertyName("avgPx")]
        public decimal? AveragePrice { get; set; }
        /// <summary>
        /// ["<c>transactTime</c>"] Transact time
        /// </summary>
        [JsonPropertyName("transactTime")]
        public DateTime TransactTime { get; set; }
        /// <summary>
        /// ["<c>timestamp</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// ["<c>pegOffsetValue</c>"] Peg offset value
        /// </summary>
        [JsonPropertyName("pegOffsetValue")]
        public long? PegOffsetValue { get; set; }

        /// <summary>
        /// ["<c>error</c>"] Error message
        /// </summary>
        [JsonPropertyName("error")]
        public string? Error { get; set; }
    }


}
