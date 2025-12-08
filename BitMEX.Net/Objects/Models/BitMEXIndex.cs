using CryptoExchange.Net.Converters.SystemTextJson;
using BitMEX.Net.Converters;
using BitMEX.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Index info
    /// </summary>
    [SerializationModel]
    public record BitMEXIndex
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Base asset
        /// </summary>
        [JsonPropertyName("rootSymbol")]
        public string BaseAsset { get; set; } = string.Empty;
        /// <summary>
        /// Status
        /// </summary>
        [JsonPropertyName("state")]
        public SymbolStatus Status { get; set; }
        /// <summary>
        /// Symbol type
        /// </summary>
        [JsonPropertyName("typ")]
        public SymbolType SymbolType { get; set; }        
        /// <summary>
        /// Underlying asset
        /// </summary>
        [JsonPropertyName("underlying")]
        public string Underlying { get; set; } = string.Empty;
        /// <summary>
        /// Quote asset
        /// </summary>
        [JsonPropertyName("quoteCurrency")]
        public string QuoteAsset { get; set; } = string.Empty;
        /// <summary>
        /// Underlying symbol
        /// </summary>
        [JsonPropertyName("underlyingSymbol")]
        public string UnderlyingSymbol { get; set; } = string.Empty;
        /// <summary>
        /// Reference
        /// </summary>
        [JsonPropertyName("reference")]
        public string Reference { get; set; } = string.Empty;
        /// <summary>
        /// Reference symbol
        /// </summary>
        [JsonPropertyName("referenceSymbol")]
        public string ReferenceSymbol { get; set; } = string.Empty;        
        /// <summary>
        /// Publish interval
        /// </summary>
        [JsonPropertyName("publishInterval")]
        [JsonConverter(typeof(IntervalConverter))]
        public TimeSpan? PublishInterval { get; set; }        
        /// <summary>
        /// Price step
        /// </summary>
        [JsonPropertyName("tickSize")]
        public decimal PriceStep { get; set; }        
        /// <summary>
        /// Is quanto
        /// </summary>
        [JsonPropertyName("isQuanto")]
        public bool IsQuanto { get; set; }
        /// <summary>
        /// Is inverse
        /// </summary>
        [JsonPropertyName("isInverse")]
        public bool IsInverse { get; set; }        
        /// <summary>
        /// Taxed
        /// </summary>
        [JsonPropertyName("taxed")]
        public bool Taxed { get; set; }
        /// <summary>
        /// Deleverage
        /// </summary>
        [JsonPropertyName("deleverage")]
        public bool Deleverage { get; set; }        
        /// <summary>
        /// Previous price24h
        /// </summary>
        [JsonPropertyName("prevPrice24h")]
        public decimal PrevPrice24h { get; set; }        
        /// <summary>
        /// Last price
        /// </summary>
        [JsonPropertyName("lastPrice")]
        public decimal? LastPrice { get; set; }
        /// <summary>
        /// Last change percentage
        /// </summary>
        [JsonPropertyName("lastChangePcnt")]
        public decimal? LastChangePcnt { get; set; }        
        /// <summary>
        /// Has liquidity
        /// </summary>
        [JsonPropertyName("hasLiquidity")]
        public bool HasLiquidity { get; set; }        
        /// <summary>
        /// Mark method
        /// </summary>
        [JsonPropertyName("markMethod")]
        public MarkMethod MarkMethod { get; set; }
        /// <summary>
        /// Mark price
        /// </summary>
        [JsonPropertyName("markPrice")]
        public decimal? MarkPrice { get; set; }        
        /// <summary>
        /// Instant pnl
        /// </summary>
        [JsonPropertyName("instantPnl")]
        public bool InstantPnl { get; set; }        
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Capped
        /// </summary>
        [JsonPropertyName("capped")]
        public bool Capped { get; set; }
    }
}
