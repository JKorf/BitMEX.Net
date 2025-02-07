using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.OrderBook;
using Microsoft.Extensions.Logging;
using BitMEX.Net.Clients;
using BitMEX.Net.Interfaces.Clients;
using BitMEX.Net.Objects.Options;
using BitMEX.Net.Objects.Models;
using System.Linq;
using BitMEX.Net.ExtensionMethods;
using CryptoExchange.Net.Interfaces;

namespace BitMEX.Net.SymbolOrderBooks
{
    /// <summary>
    /// Implementation for a synchronized order book. After calling Start the order book will sync itself and keep up to date with new data. It will automatically try to reconnect and resync in case of a lost/interrupted connection.
    /// Make sure to check the State property to see if the order book is synced.
    /// </summary>
    public class BitMEXExchangeSymbolOrderBook : SymbolOrderBook
    {
        private readonly bool _adjustQuantities;
        private readonly bool _clientOwner;
        private readonly IBitMEXSocketClient _socketClient;
        private readonly TimeSpan _initialDataTimeout;

        /// <summary>
        /// Create a new order book instance
        /// </summary>
        /// <param name="symbol">The symbol the order book is for</param>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public BitMEXExchangeSymbolOrderBook(string symbol, Action<BitMEXOrderBookOptions>? optionsDelegate = null)
            : this(symbol, optionsDelegate, null, null)
        {
            _clientOwner = true;
        }

        /// <summary>
        /// Create a new order book instance
        /// </summary>
        /// <param name="symbol">The symbol the order book is for</param>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        /// <param name="logger">Logger</param>
        /// <param name="socketClient">Socket client instance</param>
        public BitMEXExchangeSymbolOrderBook(
            string symbol,
            Action<BitMEXOrderBookOptions>? optionsDelegate,
            ILoggerFactory? logger,
            IBitMEXSocketClient? socketClient) : base(logger, "BitMEX", "Exchange", symbol)
        {
            var options = BitMEXOrderBookOptions.Default.Copy();
            if (optionsDelegate != null)
                optionsDelegate(options);
            Initialize(options);

            _adjustQuantities = options.AdjustQuantities;
            _strictLevels = false;
            _sequencesAreConsecutive = options?.Limit == null;

            Levels = options?.Limit;
            _initialDataTimeout = options?.InitialDataTimeout ?? TimeSpan.FromSeconds(10);
            _clientOwner = socketClient == null;
            _socketClient = socketClient ?? new BitMEXSocketClient();
        }

        /// <inheritdoc />
        protected override async Task<CallResult<UpdateSubscription>> DoStartAsync(CancellationToken ct)
        {
            var symbolResult = await BitMEXUtils.UpdateSymbolInfoAsync(ct).ConfigureAwait(false);
            if (!symbolResult)
                return new CallResult<UpdateSubscription>(symbolResult.Error!);

            var result = await _socketClient.ExchangeApi.SubscribeToIncrementalOrderBookUpdatesAsync(Symbol, Levels == 25 ? Enums.IncrementalBookLimit.Top25 : Enums.IncrementalBookLimit.Full, DataHandler, ct).ConfigureAwait(false);
            if (!result)
                return result;

            if (ct.IsCancellationRequested)
            {
                await result.Data.CloseAsync().ConfigureAwait(false);
                return result.AsError<UpdateSubscription>(new CancellationRequestedError());
            }
            Status = OrderBookStatus.Syncing;

            var setResult = await WaitForSetOrderBookAsync(_initialDataTimeout, ct).ConfigureAwait(false);
            return setResult ? result : new CallResult<UpdateSubscription>(setResult.Error!);
        }

        private void DataHandler(DataEvent<BitMEXOrderBookIncrementalUpdate> @event)
        {
            if (_adjustQuantities)
            {
                foreach (var item in @event.Data.Entries)
                    item.Quantity = ((long)item.Quantity).ToSharedSymbolQuantity(Symbol);
            }

            if (@event.UpdateType == SocketUpdateType.Snapshot)
                SetInitialOrderBook(DateTime.UtcNow.Ticks, @event.Data.Entries.Where(x => x.Side == Enums.OrderSide.Buy), @event.Data.Entries.Where(x => x.Side == Enums.OrderSide.Sell));
            else
                UpdateOrderBook(DateTime.UtcNow.Ticks, @event.Data.Entries.Where(x => x.Side == Enums.OrderSide.Buy), @event.Data.Entries.Where(x => x.Side == Enums.OrderSide.Sell));
        }

        /// <inheritdoc />
        protected override async Task<CallResult<bool>> DoResyncAsync(CancellationToken ct)
        {
            return await WaitForSetOrderBookAsync(_initialDataTimeout, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (_clientOwner)
                _socketClient?.Dispose();

            base.Dispose(disposing);
        }
    }
}
