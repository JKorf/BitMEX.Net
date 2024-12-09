﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Exchange stats
    /// </summary>
    public record BitMEXExchangeStat
    {
        /// <summary>
        /// Root symbol
        /// </summary>
        [JsonPropertyName("rootSymbol")]
        public string RootSymbol { get; set; } = string.Empty;
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string? Asset { get; set; }
        /// <summary>
        /// Volume 24 hours
        /// </summary>
        [JsonPropertyName("volume24h")]
        public decimal Volume24h { get; set; }
        /// <summary>
        /// Turnover 24 hours
        /// </summary>
        [JsonPropertyName("turnover24h")]
        public decimal Turnover24h { get; set; }
        /// <summary>
        /// Open interest
        /// </summary>
        [JsonPropertyName("openInterest")]
        public decimal OpenInterest { get; set; }
        /// <summary>
        /// Open value
        /// </summary>
        [JsonPropertyName("openValue")]
        public decimal OpenValue { get; set; }
    }


}
