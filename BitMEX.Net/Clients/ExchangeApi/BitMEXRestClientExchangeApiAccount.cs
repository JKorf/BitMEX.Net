using CryptoExchange.Net.Objects;
using CryptoExchange.Net;
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
        public async Task<WebCallResult<BitMEXUserEvent[]>> GetUserEventsAsync(long? fromId = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("startId", fromId);
            parameters.AddOptional("count", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/userEvent", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            var result = await _baseClient.SendAsync<BitMEXUserEventWrapper>(request, parameters, ct).ConfigureAwait(false);
            return result.As<BitMEXUserEvent[]>(result.Data?.UserEvents);
        }

        #endregion

        #region Get Account Info

        /// <inheritdoc />
        public async Task<WebCallResult<BitMEXAccountInfo>> GetAccountInfoAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/user", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXAccountInfo>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Fees

        /// <inheritdoc />
        public async Task<WebCallResult<Dictionary<string, BitMEXTradeFee>>> GetFeesAsync(CancellationToken ct = default) 
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/user/commission", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<Dictionary<string, BitMEXTradeFee>>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Deposit Address

        /// <inheritdoc />
        public async Task<WebCallResult<string>> GetDepositAddressAsync(string asset, string network, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("currency", asset);
            parameters.Add("network", network);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/user/depositAddress", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<string>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        // Doesn't ever seem to return anything
//        #region Get Transfer Accounts

//        /// <inheritdoc />
//        public async Task<WebCallResult<BitMEXTransaction[]>> GetTransferAccountsAsync(CancellationToken ct = default)
//        {
//            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/user/getWalletTransferAccounts", BitMEXExchange.RateLimiter.BitMEX, 1, true);
//            return await _baseClient.SendAsync<BitMEXTransaction[]>(request, null, ct).ConfigureAwait(false);
//        }

//        #endregion

        #region Get Margin Status

        /// <inheritdoc />
        public async Task<WebCallResult<BitMEXMarginStatus>> GetMarginStatusAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("symbol", symbol ?? "all");
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/user/margin", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXMarginStatus>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Quote Fill Ratio

        /// <inheritdoc />
        public async Task<WebCallResult<BitMEXFillRatio[]>> GetQuoteFillRatioAsync(long? accountId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("targetAccountId", accountId?.ToString() ?? "*");
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/user/quoteFillRatio", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXFillRatio[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Quote Value Ratio

        /// <inheritdoc />
        public async Task<WebCallResult<BitMEXValueRatio[]>> GetQuoteValueRatioAsync(long accountId, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("targetAccountId", accountId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/user/quoteValueRatio", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXValueRatio[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Trading Volume

        /// <inheritdoc />
        public async Task<WebCallResult<BitMEXUsdVolume[]>> GetTradingVolumeAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/user/tradingVolume", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXUsdVolume[]>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Balances

        /// <inheritdoc />
        public async Task<WebCallResult<BitMEXBalance[]>> GetBalancesAsync(string? asset = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("currency", asset ?? "all");
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/user/wallet", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXBalance[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Balance History

        /// <inheritdoc />
        public async Task<WebCallResult<BitMEXTransaction[]>> GetBalanceHistoryAsync(
            string? asset = null,
            long? targetAccountId = null,
            bool? reverse = null,
            int? offset = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("currency", asset ?? "all");
            parameters.AddOptional("targetAccountId", targetAccountId);
            parameters.AddOptional("reverse", reverse);
            parameters.AddOptional("offset", offset);
            parameters.AddOptional("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/user/walletHistory", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXTransaction[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Balance Summary

        /// <inheritdoc />
        public async Task<WebCallResult<BitMEXBalanceSummary[]>> GetBalanceSummaryAsync(string? asset = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("currency", asset ?? "all");
            parameters.AddOptional("startTime", startTime?.ToRfc3339String());
            parameters.AddOptional("endTime", endTime?.ToRfc3339String());
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/user/walletSummary", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXBalanceSummary[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Transfer

        /// <inheritdoc />
        public async Task<WebCallResult<BitMEXTransaction>> TransferAsync(string asset, long fromAccountId, long toAccountId, long quantity, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("currency", asset ?? "all");
            parameters.Add("fromAccountId", fromAccountId);
            parameters.Add("toAccountId", toAccountId);
            parameters.Add("amount", quantity);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "api/v1/user/walletSummary", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXTransaction>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Withdraw

        /// <inheritdoc />
        public async Task<WebCallResult<BitMEXTransaction>> WithdrawAsync(
            string asset,
            string network,
            long quantity,
            string? address = null,
            string? memo = null,
            string? addressId = null,
            long? targetUserId = null,
            long? fee = null,
            string? text = null,
            string? otpToken = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("currency", asset);
            parameters.Add("network", network);
            parameters.Add("amount", quantity);
            parameters.AddOptional("otpToken", otpToken);
            parameters.AddOptional("address", address);
            parameters.AddOptional("memo", memo);
            parameters.AddOptional("addressId", addressId);
            parameters.AddOptional("targetUserId", targetUserId);
            parameters.AddOptionalString("fee", fee);
            parameters.AddOptional("text", text);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "api/v1/user/requestWithdrawal", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXTransaction>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Cancel Withdrawal

        /// <inheritdoc />
        public async Task<WebCallResult> CancelWithdrawalAsync(string transactId, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("transactID", transactId);
            var request = _definitions.GetOrCreate(HttpMethod.Delete, "api/v1/user/withdrawal", BitMEXExchange.RateLimiter.BitMEX, 1, true);
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
            var request = _definitions.GetOrCreate(HttpMethod.Post, "api/v1/position/isolate", BitMEXExchange.RateLimiter.BitMEX, 1, true);
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
            var request = _definitions.GetOrCreate(HttpMethod.Post, "api/v1/position/riskLimit", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXPosition>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Transfer Margin

        /// <inheritdoc />
        public async Task<WebCallResult<BitMEXPosition>> TransferMarginAsync(string symbol, long quantity, long? targetAccountId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("symbol", symbol);
            parameters.Add("amount", quantity);
            parameters.AddOptional("targetAccountId", targetAccountId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "api/v1/position/transferMargin", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXPosition>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Saved Addresses

        /// <inheritdoc />
        public async Task<WebCallResult<BitMEXAddress[]>> GetSavedAddressesAsync(CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/address", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXAddress[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Add Saved Addresses

        /// <inheritdoc />
        public async Task<WebCallResult<BitMEXAddress>> AddSavedAddressAsync(
            string currency,
            string network,
            string address,
            string name,
            string? note = null,
            bool? skipConfirm = null,
            bool? skip2FA = null,
            string? memo = null,
            string? otpToken = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("currency", currency);
            parameters.Add("network", network);
            parameters.Add("address", address);
            parameters.Add("name", name);
            parameters.AddOptional("note", note);
            parameters.AddOptional("skipConfirm", skipConfirm);
            parameters.AddOptional("skip2FA", skip2FA);
            parameters.AddOptional("memo", memo);
            parameters.AddOptional("otpToken", otpToken);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "api/v1/address", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXAddress>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Address Book Settings

        /// <inheritdoc />
        public async Task<WebCallResult<BitMEXAddressBookConfig>> GetAddressBookSettingsAsync(CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/addressConfig", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXAddressBookConfig>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Api Key Info

        /// <inheritdoc />
        public async Task<WebCallResult<BitMEXApiKey[]>> GetApiKeyInfoAsync(CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/apiKey", BitMEXExchange.RateLimiter.BitMEX, 1, true);
            return await _baseClient.SendAsync<BitMEXApiKey[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion
    }
}
