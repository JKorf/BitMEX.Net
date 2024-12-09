using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Transaction
    /// </summary>
    public record BitMEXTransaction
    {
        /// <summary>
        /// Transaction id
        /// </summary>
        [JsonPropertyName("transactID")]
        public string TransactId { get; set; } = string.Empty;
        /// <summary>
        /// Account id
        /// </summary>
        [JsonPropertyName("account")]
        public decimal AccountId { get; set; }
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Network
        /// </summary>
        [JsonPropertyName("network")]
        public string Network { get; set; } = string.Empty;
        /// <summary>
        /// Transact type
        /// </summary>
        [JsonPropertyName("transactType")]
        public string TransactType { get; set; } = string.Empty;
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Wallet balance
        /// </summary>
        [JsonPropertyName("walletBalance")]
        public decimal WalletBalance { get; set; }
        /// <summary>
        /// Fee
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// Transaction status
        /// </summary>
        [JsonPropertyName("transactStatus")]
        public string TransactStatus { get; set; } = string.Empty;
        /// <summary>
        /// Address
        /// </summary>
        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;
        /// <summary>
        /// Transaction
        /// </summary>
        [JsonPropertyName("tx")]
        public string Transaction { get; set; } = string.Empty;
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("orderID")]
        public string? OrderId { get; set; }
        /// <summary>
        /// Text
        /// </summary>
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
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
        /// Memo
        /// </summary>
        [JsonPropertyName("memo")]
        public string? Memo { get; set; }
    }


}
