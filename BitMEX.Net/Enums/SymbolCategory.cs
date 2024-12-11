using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Category
    /// </summary>
    public enum SymbolCategory
    {
        /// <summary>
        /// Contracts
        /// </summary>
        [Map("CONTRACTS")]
        Contracts,
        /// <summary>
        /// Indices
        /// </summary>
        [Map("INDICES")]
        Indices,
        /// <summary>
        /// Derivatives
        /// </summary>
        [Map("DERIVATIVES")]
        Derivatives,
        /// <summary>
        /// Spot
        /// </summary>
        [Map("SPOT")]
        Spot
    }
}
