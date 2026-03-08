using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Balance summary
    /// </summary>
    [SerializationModel]
    public record BitMEXBalanceSummary
    {
        /// <summary>
        /// ["<c>account</c>"] Account id
        /// </summary>
        [JsonPropertyName("account")]
        public decimal AccountId { get; set; }
        /// <summary>
        /// ["<c>currency</c>"] Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>transactType</c>"] Transaction type
        /// </summary>
        [JsonPropertyName("transactType")]
        public string TransactType { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>symbol</c>"] Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>amount</c>"] Quantity
        /// </summary>
        [JsonPropertyName("amount")]
        public long Quantity { get; set; }
        /// <summary>
        /// ["<c>pendingDebit</c>"] Pending debit
        /// </summary>
        [JsonPropertyName("pendingDebit")]
        public long PendingDebit { get; set; }
        /// <summary>
        /// ["<c>realisedPnl</c>"] Realised profit and loss
        /// </summary>
        [JsonPropertyName("realisedPnl")]
        public long RealisedPnl { get; set; }
        /// <summary>
        /// ["<c>walletBalance</c>"] Wallet balance
        /// </summary>
        [JsonPropertyName("walletBalance")]
        public long WalletBalance { get; set; }
        /// <summary>
        /// ["<c>unrealisedPnl</c>"] Unrealised profit and loss
        /// </summary>
        [JsonPropertyName("unrealisedPnl")]
        public long UnrealisedPnl { get; set; }
        /// <summary>
        /// ["<c>marginBalance</c>"] Margin balance
        /// </summary>
        [JsonPropertyName("marginBalance")]
        public long MarginBalance { get; set; }
    }


}
