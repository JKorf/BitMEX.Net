using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// Book type
    /// </summary>
    public enum BookUpdateType
    {
        /// <summary>
        /// Snapshot
        /// </summary>
        [Map("partial")]
        Snapshot,
        /// <summary>
        /// Insert
        /// </summary>
        [Map("insert")]
        Insert,
        /// <summary>
        /// Update
        /// </summary>
        [Map("update")]
        Update,
        /// <summary>
        /// Delete
        /// </summary>
        [Map("delete")]
        Delete
    }
}
