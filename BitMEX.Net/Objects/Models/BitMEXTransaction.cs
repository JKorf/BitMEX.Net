using CryptoExchange.Net.Converters.SystemTextJson;
using BitMEX.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Transaction
    /// </summary>
    [SerializationModel]
    public record BitMEXTransaction
    {
        /// <summary>
        /// ["<c>transactID</c>"] Transaction id
        /// </summary>
        [JsonPropertyName("transactID")]
        public string TransactionId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>account</c>"] Account id
        /// </summary>
        [JsonPropertyName("account")]
        public long AccountId { get; set; }
        /// <summary>
        /// ["<c>currency</c>"] Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>network</c>"] Network
        /// </summary>
        [JsonPropertyName("network")]
        public string Network { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>transactionType</c>"] Transaction type
        /// </summary>
        [JsonPropertyName("transactionType")]
        public TransactionType TransactionType { get; set; }
        /// <summary>
        /// ["<c>amount</c>"] Quantity
        /// </summary>
        [JsonPropertyName("amount")]
        public long Quantity { get; set; }
        /// <summary>
        /// ["<c>walletBalance</c>"] Wallet balance
        /// </summary>
        [JsonPropertyName("walletBalance")]
        public long WalletBalance { get; set; }
        /// <summary>
        /// ["<c>fee</c>"] Fee
        /// </summary>
        [JsonPropertyName("fee")]
        public long Fee { get; set; }
        /// <summary>
        /// ["<c>transactStatus</c>"] Transaction status
        /// </summary>
        [JsonPropertyName("transactStatus")]
        public TransactionStatus TransactionStatus { get; set; }
        /// <summary>
        /// ["<c>address</c>"] Address
        /// </summary>
        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>tx</c>"] Transaction
        /// </summary>
        [JsonPropertyName("tx")]
        public string Transaction { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>orderID</c>"] Order id
        /// </summary>
        [JsonPropertyName("orderID")]
        public string? OrderId { get; set; }
        /// <summary>
        /// ["<c>text</c>"] Text
        /// </summary>
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>transactTime</c>"] Transaction time
        /// </summary>
        [JsonPropertyName("transactTime")]
        public DateTime TransactionTime { get; set; }
        /// <summary>
        /// ["<c>timestamp</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>memo</c>"] Memo
        /// </summary>
        [JsonPropertyName("memo")]
        public string? Memo { get; set; }
    }


}
