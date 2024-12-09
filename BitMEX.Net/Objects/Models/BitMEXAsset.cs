using BitMEX.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Asset info
    /// </summary>
    public record BitMEXAsset
    {
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("asset")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Currency
        /// </summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;
        /// <summary>
        /// Major currency
        /// </summary>
        [JsonPropertyName("majorCurrency")]
        public string MajorCurrency { get; set; } = string.Empty;
        /// <summary>
        /// Name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Asset type
        /// </summary>
        [JsonPropertyName("currencyType")]
        public AssetType AssetType { get; set; }
        /// <summary>
        /// Scale
        /// </summary>
        [JsonPropertyName("scale")]
        public decimal Scale { get; set; }
        /// <summary>
        /// Enabled
        /// </summary>
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }
        /// <summary>
        /// Is margin asset
        /// </summary>
        [JsonPropertyName("isMarginCurrency")]
        public bool IsMarginAsset { get; set; }
        /// <summary>
        /// Memo required
        /// </summary>
        [JsonPropertyName("memoRequired")]
        public bool MemoRequired { get; set; }
        /// <summary>
        /// Networks
        /// </summary>
        [JsonPropertyName("networks")]
        public IEnumerable<BitMEXAssetNetwork> Networks { get; set; } = Array.Empty<BitMEXAssetNetwork>();
    }

    /// <summary>
    /// Network info
    /// </summary>
    public record BitMEXAssetNetwork
    {
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("asset")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Token address
        /// </summary>
        [JsonPropertyName("tokenAddress")]
        public string? TokenAddress { get; set; }
        /// <summary>
        /// Deposit enabled
        /// </summary>
        [JsonPropertyName("depositEnabled")]
        public bool DepositEnabled { get; set; }
        /// <summary>
        /// Withdrawal enabled
        /// </summary>
        [JsonPropertyName("withdrawalEnabled")]
        public bool WithdrawalEnabled { get; set; }
        /// <summary>
        /// Withdrawal fee
        /// </summary>
        [JsonPropertyName("withdrawalFee")]
        public decimal? WithdrawalFee { get; set; }
        /// <summary>
        /// Min fee
        /// </summary>
        [JsonPropertyName("minFee")]
        public decimal? MinFee { get; set; }
        /// <summary>
        /// Max fee
        /// </summary>
        [JsonPropertyName("maxFee")]
        public decimal? MaxFee { get; set; }
    }


}
