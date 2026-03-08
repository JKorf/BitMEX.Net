using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Network info
    /// </summary>
    [SerializationModel]
    public record BitMEXNetwork
    {
        /// <summary>
        /// ["<c>network</c>"] Network
        /// </summary>
        [JsonPropertyName("network")]
        public string Network { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>name</c>"] Name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>currency</c>"] Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>networkSymbol</c>"] Network symbol
        /// </summary>
        [JsonPropertyName("networkSymbol")]
        public string NetworkSymbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>transactionExplorer</c>"] Transaction explorer
        /// </summary>
        [JsonPropertyName("transactionExplorer")]
        public string TransactionExplorer { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>tokenExplorer</c>"] Token explorer
        /// </summary>
        [JsonPropertyName("tokenExplorer")]
        public string TokenExplorer { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>depositConfirmations</c>"] Deposit confirmations
        /// </summary>
        [JsonPropertyName("depositConfirmations")]
        public int? DepositConfirmations { get; set; }
        /// <summary>
        /// ["<c>enabled</c>"] Enabled
        /// </summary>
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }
    }


}
