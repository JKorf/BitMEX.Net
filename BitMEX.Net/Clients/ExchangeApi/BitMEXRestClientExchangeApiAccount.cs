using CryptoExchange.Net.Objects;
using BitMEX.Net.Clients.ExchangeApi;
using BitMEX.Net.Interfaces.Clients.ExchangeApi;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using BitMEX.Net.Objects.Models;
using System;

namespace BitMEX.Net.Clients.ExchangeApi
{
    /// <inheritdoc />
    internal class BitMEXRestClientExchangeApiAccount : IBitMEXRestClientExchangeApiAccount
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly BitMEXRestClientExchangeApi _baseClient;

        internal BitMEXRestClientExchangeApiAccount(BitMEXRestClientExchangeApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get User Events

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXUserEvent>>> GetUserEventsAsync(long? fromId = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("startId", fromId);
            parameters.AddOptional("count", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/userEvent", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            var result = await _baseClient.SendAsync<BitMEXUserEventWrapper>(request, parameters, ct).ConfigureAwait(false);
            return result.As<IEnumerable<BitMEXUserEvent>>(result.Data?.UserEvents);
        }

        #endregion

        #region Get Account Info

        /// <inheritdoc />
        public async Task<WebCallResult<BitMEXAccountInfo>> GetAccountInfoAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/user", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXAccountInfo>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Fees

        /// <inheritdoc />
        public async Task<WebCallResult<Dictionary<string, BitMEXTradeFee>>> GetFeesAsync(CancellationToken ct = default) 
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/user/commission", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<Dictionary<string, BitMEXTradeFee>>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Deposit Address

        /// <inheritdoc />
        public async Task<WebCallResult<Dictionary<string, BitMEXTradeFee>>> GetDepositAddressAsync(string asset, string network, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("currency", asset);
            parameters.Add("network", network);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/user/depositAddress", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<Dictionary<string, BitMEXTradeFee>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Deposit Address

        /// <inheritdoc />
        public async Task<WebCallResult<Dictionary<string, BitMEXTradeFee>>> GetTransferAccountsAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/user/getWalletTransferAccounts", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<Dictionary<string, BitMEXTradeFee>>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Margin Status

        /// <inheritdoc />
        public async Task<WebCallResult<BitMEXMarginStatus>> GetMarginStatusAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("symbol", symbol ?? "all");
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/user/margin", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXMarginStatus>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Quote Fill Ratio

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXFillRatio>>> GetQuoteFillRatioAsync(long? accountId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("targetAccountId", accountId?.ToString() ?? "*");
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/user/quoteFillRatio", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<IEnumerable<BitMEXFillRatio>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Quote Fill Ratio

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXValueRatio>>> GetQuoteValueRatioAsync(long accountId, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("targetAccountId", accountId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/user/quoteValueRatio", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<IEnumerable<BitMEXValueRatio>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Quote Fill Ratio

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXUsdVolume>>> GetTradingVolumeAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/user/tradingVolume", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<IEnumerable<BitMEXUsdVolume>>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Balances

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXBalance>>> GetBalancesAsync(string? asset = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("currency", asset ?? "all");
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/user/wallet", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<IEnumerable<BitMEXBalance>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Balance History

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXTransaction>>> GetBalanceHistoryAsync(string? asset = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("currency", asset ?? "all");
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/user/walletHistory", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<IEnumerable<BitMEXTransaction>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Balance Summary

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BitMEXBalanceSummary>>> GetBalanceSummaryAsync(string? asset = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("currency", asset ?? "all");
            parameters.AddOptionalMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/user/walletSummary", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<IEnumerable<BitMEXBalanceSummary>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Transfer

        /// <inheritdoc />
        public async Task<WebCallResult<BitMEXTransaction>> TransferAsync(string asset, long fromAccountId, long toAccountId, decimal quantity, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("currency", asset ?? "all");
            parameters.Add("fromAccountId", fromAccountId);
            parameters.Add("toAccountId", toAccountId);
            parameters.AddString("amount", quantity);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/user/walletSummary", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXTransaction>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Withdraw

        /// <inheritdoc />
        public async Task<WebCallResult<BitMEXTransaction>> WithdrawAsync(
            string asset,
            string network,
            decimal quantity,
            string? address = null,
            string? memo = null,
            string? addressId = null,
            long? targetUserId = null,
            decimal? fee = null,
            string? text = null,
            string? otpToken = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("currency", asset);
            parameters.Add("network", network);
            parameters.AddString("amount", quantity);
            parameters.AddOptional("otpToken", otpToken);
            parameters.AddOptional("address", address);
            parameters.AddOptional("memo", memo);
            parameters.AddOptional("addressId", addressId);
            parameters.AddOptional("targetUserId", targetUserId);
            parameters.AddOptionalString("fee", fee);
            parameters.AddOptional("text", text);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/user/requestWithdrawal", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXTransaction>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Cancel Withdrawal

        /// <inheritdoc />
        public async Task<WebCallResult> CancelWithdrawalAsync(string transactId, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("transactID", transactId);
            var request = _definitions.GetOrCreate(HttpMethod.Delete, "/user/withdrawal", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Set Isolated Margin

        /// <inheritdoc />
        public async Task<WebCallResult<BitMEXPosition>> SetIsolatedMarginAsync(string symbol, bool isolatedMarginEnabled, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("symbol", symbol);
            parameters.Add("enabled", isolatedMarginEnabled);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/position/isolate", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXPosition>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Set Risk Limit

        /// <inheritdoc />
        public async Task<WebCallResult<BitMEXPosition>> SetRiskLimitAsync(string symbol, int riskLimit, long? targetAccountId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("symbol", symbol);
            parameters.Add("riskLimit", riskLimit);
            parameters.AddOptional("targetAccountId", targetAccountId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/position/riskLimit", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXPosition>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Transfer Margin

        /// <inheritdoc />
        public async Task<WebCallResult<BitMEXPosition>> TransferMarginAsync(string symbol, decimal quantity, long? targetAccountId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("symbol", symbol);
            parameters.AddString("amount", quantity);
            parameters.AddOptional("targetAccountId", targetAccountId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/position/transferMargin", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXPosition>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion
    }
}
