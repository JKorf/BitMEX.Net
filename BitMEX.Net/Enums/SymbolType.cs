using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Symbol type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<SymbolType>))]
    public enum SymbolType
    {
        /// <summary>
        /// ["<c>FFWCSX</c>"] Perpetual Contracts
        /// </summary>
        [Map("FFWCSX")]
        PerpetualContract,
        /// <summary>
        /// ["<c>FFWCSF</c>"] Perpetual Contracts (FX underliers)
        /// </summary>
        [Map("FFWCSF")]
        PerpetualContractFX,
        /// <summary>
        /// ["<c>FFICSX</c>"] Prediction Market
        /// </summary>
        [Map("FFICSX")]
        PredictionMarket,
        /// <summary>
        /// ["<c>FXXXS</c>"] Unknown
        /// </summary>
        [Map("FXXXS")]
        UnknownFXXXS,
        /// <summary>
        /// ["<c>FMXXS</c>"] Unknown
        /// </summary>
        [Map("FMXXS")]
        UnknownFMXXS,
        /// <summary>
        /// ["<c>IFXXXP</c>"] Spot
        /// </summary>
        [Map("IFXXXP")]
        Spot,
        /// <summary>
        /// ["<c>FFCCSX</c>"] Futures
        /// </summary>
        [Map("FFCCSX")]
        Futures,
        /// <summary>
        /// ["<c>MRBXXX</c>"] BitMEX Basket Index
        /// </summary>
        [Map("MRBXXX")]
        BitMEXBasketIndex,
        /// <summary>
        /// ["<c>MRCXXX</c>"] BitMEX Crypto Index
        /// </summary>
        [Map("MRCXXX")]
        BitMEXCryptoIndex,
        /// <summary>
        /// ["<c>MRFXXX</c>"] BitMEX FX Index
        /// </summary>
        [Map("MRFXXX")]
        BitMEXFXIndex,
        /// <summary>
        /// ["<c>MRRXXX</c>"] BitMEX Lending/Premium Index
        /// </summary>
        [Map("MRRXXX")]
        BitMEXLendingPremiumIndex,
        /// <summary>
        /// ["<c>MRIXXX</c>"] BitMEX Volatility Index
        /// </summary>
        [Map("MRIXXX")]
        BitMEXVolatilityIndex,
        /// <summary>
        /// ["<c>MRSXXX</c>"] Perpetual equity swap index
        /// </summary>
        [Map("MRSXXX")]
        PerpetualEquitySwapIndex,
        /// <summary>
        /// ["<c>FFSCSX</c>"] Perpetual equity swap
        /// </summary>
        [Map("FFSCSX")]
        PerpetualEquitySwap
    }
}
