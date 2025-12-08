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
        UnknownFXXXS,
        /// <summary>
        /// Unknown
        /// </summary>
        [Map("FMXXS")]
        UnknownFMXXS,
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
        /// <summary>
        /// Unknown
        /// </summary>
        [Map("MRSXXX")]
        UnknownMRSXXX
    }
}
