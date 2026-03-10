using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Transaction type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TransactionType>))]
    public enum TransactionType
    {
        /// <summary>
        /// ["<c>Deposit</c>"] Deposit
        /// </summary>
        [Map("Deposit")]
        Deposit,
        /// <summary>
        /// ["<c>Withdrawal</c>"] Withdrawal
        /// </summary>
        [Map("Withdrawal")]
        Withdrawal
    }
}
