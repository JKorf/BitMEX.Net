using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Balance info
    /// </summary>
    [SerializationModel]
    public record BitMEXBalance
    {
        /// <summary>
        /// Account id
        /// </summary>
        [JsonPropertyName("account")]
        public decimal AccountId { get; set; }
        /// <summary>
        /// Currency
        /// </summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;
        /// <summary>
        /// Deposited
        /// </summary>
        [JsonPropertyName("deposited")]
        public long Deposited { get; set; }
        /// <summary>
        /// Withdrawn
        /// </summary>
        [JsonPropertyName("withdrawn")]
        public long Withdrawn { get; set; }
        /// <summary>
        /// Transfer in
        /// </summary>
        [JsonPropertyName("transferIn")]
        public long TransferIn { get; set; }
        /// <summary>
        /// Transfer out
        /// </summary>
        [JsonPropertyName("transferOut")]
        public long TransferOut { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("amount")]
        public long Quantity { get; set; }
        /// <summary>
        /// Pending credit
        /// </summary>
        [JsonPropertyName("pendingCredit")]
        public long PendingCredit { get; set; }
        /// <summary>
        /// Pending debit
        /// </summary>
        [JsonPropertyName("pendingDebit")]
        public long PendingDebit { get; set; }
        /// <summary>
        /// Confirmed debit
        /// </summary>
        [JsonPropertyName("confirmedDebit")]
        public long ConfirmedDebit { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }


}
