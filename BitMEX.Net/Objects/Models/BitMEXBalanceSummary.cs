using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Balance summary
    /// </summary>
    public record BitMEXBalanceSummary
    {
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
        /// Transaction type
        /// </summary>
        [JsonPropertyName("transactType")]
        public string TransactType { get; set; } = string.Empty;
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Pending debit
        /// </summary>
        [JsonPropertyName("pendingDebit")]
        public decimal PendingDebit { get; set; }
        /// <summary>
        /// Realised profit and loss
        /// </summary>
        [JsonPropertyName("realisedPnl")]
        public decimal RealisedPnl { get; set; }
        /// <summary>
        /// Wallet balance
        /// </summary>
        [JsonPropertyName("walletBalance")]
        public decimal WalletBalance { get; set; }
        /// <summary>
        /// Unrealised profit and loss
        /// </summary>
        [JsonPropertyName("unrealisedPnl")]
        public decimal UnrealisedPnl { get; set; }
        /// <summary>
        /// Margin balance
        /// </summary>
        [JsonPropertyName("marginBalance")]
        public decimal MarginBalance { get; set; }
    }


}
