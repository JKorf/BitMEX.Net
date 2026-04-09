using CryptoExchange.Net.Converters.SystemTextJson;
using BitMEX.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Asset info
    /// </summary>
    [SerializationModel]
    public record BitMEXAsset
    {
        /// <summary>
        /// Asset id
        /// </summary>
        [JsonPropertyName("assetID")]
        public long? AssetId { get; set; }
        /// <summary>
        /// ["<c>asset</c>"] Asset
        /// </summary>
        [JsonPropertyName("asset")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>currency</c>"] Currency
        /// </summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>majorCurrency</c>"] Major currency
        /// </summary>
        [JsonPropertyName("majorCurrency")]
        public string MajorCurrency { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>name</c>"] Name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>currencyType</c>"] Asset type
        /// </summary>
        [JsonPropertyName("currencyType")]
        public AssetType AssetType { get; set; }
        /// <summary>
        /// ["<c>scale</c>"] Scale
        /// </summary>
        [JsonPropertyName("scale")]
        public int Scale { get; set; }
        /// <summary>
        /// ["<c>enabled</c>"] Enabled
        /// </summary>
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }
        /// <summary>
        /// ["<c>isMarginCurrency</c>"] Is margin asset
        /// </summary>
        [JsonPropertyName("isMarginCurrency")]
        public bool IsMarginAsset { get; set; }
        /// <summary>
        /// ["<c>memoRequired</c>"] Memo required
        /// </summary>
        [JsonPropertyName("memoRequired")]
        public bool MemoRequired { get; set; }
        /// <summary>
        /// ["<c>minDepositAmount</c>"] Min deposit quantity
        /// </summary>
        [JsonPropertyName("minDepositAmount")]
        public long? MinDepositQuantity { get; set; }
        /// <summary>
        /// ["<c>minWithdrawalAmount</c>"] Min withdrawal quantity
        /// </summary>
        [JsonPropertyName("minWithdrawalAmount")]
        public long? MinWithdrawalQuantity { get; set; }
        /// <summary>
        /// ["<c>maxWithdrawalAmount</c>"] Max withdrawal quantity
        /// </summary>
        [JsonPropertyName("maxWithdrawalAmount")]
        public long? MaxWithdrawalQuantity { get; set; }
        /// <summary>
        /// ["<c>networks</c>"] Networks
        /// </summary>
        [JsonPropertyName("networks")]
        public BitMEXAssetNetwork[] Networks { get; set; } = Array.Empty<BitMEXAssetNetwork>();
    }

    /// <summary>
    /// Network info
    /// </summary>
    [SerializationModel]
    public record BitMEXAssetNetwork
    {
        /// <summary>
        /// ["<c>asset</c>"] Asset
        /// </summary>
        [JsonPropertyName("asset")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>tokenAddress</c>"] Token address
        /// </summary>
        [JsonPropertyName("tokenAddress")]
        public string? TokenAddress { get; set; }
        /// <summary>
        /// ["<c>depositEnabled</c>"] Deposit enabled
        /// </summary>
        [JsonPropertyName("depositEnabled")]
        public bool DepositEnabled { get; set; }
        /// <summary>
        /// ["<c>withdrawalEnabled</c>"] Withdrawal enabled
        /// </summary>
        [JsonPropertyName("withdrawalEnabled")]
        public bool WithdrawalEnabled { get; set; }
        /// <summary>
        /// ["<c>withdrawalFee</c>"] Withdrawal fee
        /// </summary>
        [JsonPropertyName("withdrawalFee")]
        public long? WithdrawalFee { get; set; }
        /// <summary>
        /// ["<c>minFee</c>"] Min fee
        /// </summary>
        [JsonPropertyName("minFee")]
        public long? MinFee { get; set; }
        /// <summary>
        /// ["<c>maxFee</c>"] Max fee
        /// </summary>
        [JsonPropertyName("maxFee")]
        public long? MaxFee { get; set; }
    }


}
