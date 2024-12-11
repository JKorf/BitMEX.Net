using BitMEX.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Incremental order book update
    /// </summary>
    public record BitMEXOrderBookIncrementalUpdate
    {
        /// <summary>
        /// Action
        /// </summary>
        public BookUpdateType Action { get; set; }

        /// <summary>
        /// Entries
        /// </summary>
        public IEnumerable<BitMEXOrderBookEntry> Entries { get; set; } = [];
    }
}
