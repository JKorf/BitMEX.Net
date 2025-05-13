using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text;
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
        /// Network
        /// </summary>
        [JsonPropertyName("network")]
        public string Network { get; set; } = string.Empty;
        /// <summary>
        /// Name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Network symbol
        /// </summary>
        [JsonPropertyName("networkSymbol")]
        public string NetworkSymbol { get; set; } = string.Empty;
        /// <summary>
        /// Transaction explorer
        /// </summary>
        [JsonPropertyName("transactionExplorer")]
        public string TransactionExplorer { get; set; } = string.Empty;
        /// <summary>
        /// Token explorer
        /// </summary>
        [JsonPropertyName("tokenExplorer")]
        public string TokenExplorer { get; set; } = string.Empty;
        /// <summary>
        /// Deposit confirmations
        /// </summary>
        [JsonPropertyName("depositConfirmations")]
        public int? DepositConfirmations { get; set; }
        /// <summary>
        /// Enabled
        /// </summary>
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }
    }


}
