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
        /// ["<c>symbol</c>"] Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>rootSymbol</c>"] Base asset
        /// </summary>
        [JsonPropertyName("rootSymbol")]
        public string BaseAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>instrumentID</c>"] Instrument id
        /// </summary>
        [JsonPropertyName("instrumentID")]
        public long? InstrumentId { get; set; }
        /// <summary>
        /// ["<c>state</c>"] Status
        /// </summary>
        [JsonPropertyName("state")]
        public SymbolStatus Status { get; set; }
        /// <summary>
        /// ["<c>typ</c>"] Symbol type
        /// </summary>
        [JsonPropertyName("typ")]
        public SymbolType SymbolType { get; set; }        
        /// <summary>
        /// ["<c>underlying</c>"] Underlying asset
        /// </summary>
        [JsonPropertyName("underlying")]
        public string Underlying { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>quoteCurrency</c>"] Quote asset
        /// </summary>
        [JsonPropertyName("quoteCurrency")]
        public string QuoteAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>underlyingSymbol</c>"] Underlying symbol
        /// </summary>
        [JsonPropertyName("underlyingSymbol")]
        public string UnderlyingSymbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>reference</c>"] Reference
        /// </summary>
        [JsonPropertyName("reference")]
        public string Reference { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>referenceSymbol</c>"] Reference symbol
        /// </summary>
        [JsonPropertyName("referenceSymbol")]
        public string ReferenceSymbol { get; set; } = string.Empty;        
        /// <summary>
        /// ["<c>publishInterval</c>"] Publish interval
        /// </summary>
        [JsonPropertyName("publishInterval")]
        [JsonConverter(typeof(IntervalConverter))]
        public TimeSpan? PublishInterval { get; set; }        
        /// <summary>
        /// ["<c>tickSize</c>"] Price step
        /// </summary>
        [JsonPropertyName("tickSize")]
        public decimal PriceStep { get; set; }        
        /// <summary>
        /// ["<c>isQuanto</c>"] Is quanto
        /// </summary>
        [JsonPropertyName("isQuanto")]
        public bool IsQuanto { get; set; }
        /// <summary>
        /// ["<c>isInverse</c>"] Is inverse
        /// </summary>
        [JsonPropertyName("isInverse")]
        public bool IsInverse { get; set; }        
        /// <summary>
        /// ["<c>taxed</c>"] Taxed
        /// </summary>
        [JsonPropertyName("taxed")]
        public bool Taxed { get; set; }
        /// <summary>
        /// ["<c>deleverage</c>"] Deleverage
        /// </summary>
        [JsonPropertyName("deleverage")]
        public bool Deleverage { get; set; }        
        /// <summary>
        /// ["<c>prevPrice24h</c>"] Previous price24h
        /// </summary>
        [JsonPropertyName("prevPrice24h")]
        public decimal PrevPrice24h { get; set; }        
        /// <summary>
        /// ["<c>lastPrice</c>"] Last price
        /// </summary>
        [JsonPropertyName("lastPrice")]
        public decimal? LastPrice { get; set; }
        /// <summary>
        /// ["<c>lastChangePcnt</c>"] Last change percentage
        /// </summary>
        [JsonPropertyName("lastChangePcnt")]
        public decimal? LastChangePcnt { get; set; }        
        /// <summary>
        /// ["<c>hasLiquidity</c>"] Has liquidity
        /// </summary>
        [JsonPropertyName("hasLiquidity")]
        public bool HasLiquidity { get; set; }        
        /// <summary>
        /// ["<c>markMethod</c>"] Mark method
        /// </summary>
        [JsonPropertyName("markMethod")]
        public MarkMethod MarkMethod { get; set; }
        /// <summary>
        /// ["<c>markPrice</c>"] Mark price
        /// </summary>
        [JsonPropertyName("markPrice")]
        public decimal? MarkPrice { get; set; }        
        /// <summary>
        /// ["<c>instantPnl</c>"] Instant pnl
        /// </summary>
        [JsonPropertyName("instantPnl")]
        public bool InstantPnl { get; set; }        
        /// <summary>
        /// ["<c>timestamp</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>capped</c>"] Capped
        /// </summary>
        [JsonPropertyName("capped")]
        public bool Capped { get; set; }
        /// <summary>
        /// ["<c>minPrice</c>"] Min price
        /// </summary>
        [JsonPropertyName("minPrice")]
        public decimal MinPrice { get; set; }
        /// <summary>
        /// Last tick direction
        /// </summary>
        [JsonPropertyName("lastTickDirection")]
        public TickDirection LastTickDirection { get; set; }
        /// <summary>
        /// ["<c>openValue</c>"] Open value
        /// </summary>
        [JsonPropertyName("openValue")]
        public decimal OpenValue { get; set; }
        /// <summary>
        /// ["<c>calcInterval</c>"] Calc interval
        /// </summary>
        [JsonPropertyName("calcInterval")]
        [JsonConverter(typeof(IntervalConverter))]
        public TimeSpan? CalcInterval { get; set; }
        /// <summary>
        /// ["<c>publishTime</c>"] Publish time
        /// </summary>
        [JsonPropertyName("publishTime")]
        public DateTime? PublishTime { get; set; }
        /// <summary>
        /// ["<c>openInterest</c>"] Open interest
        /// </summary>
        [JsonPropertyName("openInterest")]
        public long? OpenInterest { get; set; }
    }
}
