using CryptoExchange.Net.Converters.SystemTextJson;
using BitMEX.Net.Enums;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Incremental order book update
    /// </summary>
    [SerializationModel]
    public record BitMEXOrderBookIncrementalUpdate
    {
        /// <summary>
        /// Action
        /// </summary>
        public BookUpdateType Action { get; set; }

        /// <summary>
        /// Entries
        /// </summary>
        public BitMEXOrderBookEntry[] Entries { get; set; } = [];
    }
}
