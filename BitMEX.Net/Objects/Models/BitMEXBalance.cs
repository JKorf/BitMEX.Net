using CryptoExchange.Net.Converters.SystemTextJson;
using System;
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
        /// ["<c>account</c>"] Account id
        /// </summary>
        [JsonPropertyName("account")]
        public decimal AccountId { get; set; }
        /// <summary>
        /// ["<c>currency</c>"] Currency
        /// </summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>deposited</c>"] Deposited
        /// </summary>
        [JsonPropertyName("deposited")]
        public long Deposited { get; set; }
        /// <summary>
        /// ["<c>withdrawn</c>"] Withdrawn
        /// </summary>
        [JsonPropertyName("withdrawn")]
        public long Withdrawn { get; set; }
        /// <summary>
        /// ["<c>transferIn</c>"] Transfer in
        /// </summary>
        [JsonPropertyName("transferIn")]
        public long TransferIn { get; set; }
        /// <summary>
        /// ["<c>transferOut</c>"] Transfer out
        /// </summary>
        [JsonPropertyName("transferOut")]
        public long TransferOut { get; set; }
        /// <summary>
        /// ["<c>amount</c>"] Quantity
        /// </summary>
        [JsonPropertyName("amount")]
        public long Quantity { get; set; }
        /// <summary>
        /// ["<c>pendingCredit</c>"] Pending credit
        /// </summary>
        [JsonPropertyName("pendingCredit")]
        public long PendingCredit { get; set; }
        /// <summary>
        /// ["<c>pendingDebit</c>"] Pending debit
        /// </summary>
        [JsonPropertyName("pendingDebit")]
        public long PendingDebit { get; set; }
        /// <summary>
        /// ["<c>confirmedDebit</c>"] Confirmed debit
        /// </summary>
        [JsonPropertyName("confirmedDebit")]
        public long ConfirmedDebit { get; set; }
        /// <summary>
        /// ["<c>timestamp</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }


}
