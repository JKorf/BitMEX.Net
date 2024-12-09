using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Balance info
    /// </summary>
    public record BitMEXBalance
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
        /// Deposited
        /// </summary>
        [JsonPropertyName("deposited")]
        public decimal Deposited { get; set; }
        /// <summary>
        /// Withdrawn
        /// </summary>
        [JsonPropertyName("withdrawn")]
        public decimal Withdrawn { get; set; }
        /// <summary>
        /// Transfer in
        /// </summary>
        [JsonPropertyName("transferIn")]
        public decimal TransferIn { get; set; }
        /// <summary>
        /// Transfer out
        /// </summary>
        [JsonPropertyName("transferOut")]
        public decimal TransferOut { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Pending credit
        /// </summary>
        [JsonPropertyName("pendingCredit")]
        public decimal PendingCredit { get; set; }
        /// <summary>
        /// Pending debit
        /// </summary>
        [JsonPropertyName("pendingDebit")]
        public decimal PendingDebit { get; set; }
        /// <summary>
        /// Confirmed debit
        /// </summary>
        [JsonPropertyName("confirmedDebit")]
        public decimal ConfirmedDebit { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }


}
