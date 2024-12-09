using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Symbol type
    /// </summary>
    public enum SymbolType
    {
        /// <summary>
        /// Perpetual Contracts
        /// </summary>
        [Map("FFWCSX")]
        PerpetualContract,
        /// <summary>
        /// Perpetual Contracts (FX underliers)
        /// </summary>
        [Map("FFWCSF")]
        PerpetualContractFX,
        /// <summary>
        /// Prediction Market
        /// </summary>
        [Map("FFICSX")]
        PredictionMarket,
        /// <summary>
        /// Unknown
        /// </summary>
        [Map("FXXXS")]
        Unknown,
        /// <summary>
        /// Spot
        /// </summary>
        [Map("IFXXXP")]
        Spot,
        /// <summary>
        /// Futures
        /// </summary>
        [Map("FFCCSX")]
        Futures,
        /// <summary>
        /// BitMEX Basket Index
        /// </summary>
        [Map("MRBXXX")]
        BitMEXBasketIndex,
        /// <summary>
        /// BitMEX Crypto Index
        /// </summary>
        [Map("MRCXXX")]
        BitMEXCryptoIndex,
        /// <summary>
        /// BitMEX FX Index
        /// </summary>
        [Map("MRFXXX")]
        BitMEXFXIndex,
        /// <summary>
        /// BitMEX Lending/Premium Index
        /// </summary>
        [Map("MRRXXX")]
        BitMEXLendingPremiumIndex,
        /// <summary>
        /// BitMEX Volatility Index
        /// </summary>
        [Map("MRIXXX")]
        BitMEXVolatilityIndex,
    }
}
