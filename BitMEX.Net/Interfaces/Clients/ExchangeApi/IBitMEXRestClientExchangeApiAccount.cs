using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using BitMEX.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using System;

namespace BitMEX.Net.Interfaces.Clients.ExchangeApi
{
    /// <summary>
    /// BitMEX Exchange account endpoints. Account endpoints include balance info, withdraw/deposit info and requesting and account settings
    /// </summary>
    public interface IBitMEXRestClientExchangeApiAccount
    {
        /// <summary>
        /// Get user events
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/UserEvent/UserEvent_get" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/userEvent
        /// </para>
        /// </summary>
        /// <param name="fromId">["<c>startId</c>"] From id</param>
        /// <param name="limit">["<c>count</c>"] Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXUserEvent[]>> GetUserEventsAsync(long? fromId = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get account info
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/User/User_get" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/user
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXAccountInfo>> GetAccountInfoAsync(CancellationToken ct = default);

        /// <summary>
        /// Get user trading fees
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/User/User_getCommission" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/user/commission
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<Dictionary<string, BitMEXTradeFee>>> GetFeesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get deposit address
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/User/User_getDepositAddress" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/user/depositAddress
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>currency</c>"] Asset name</param>
        /// <param name="network">["<c>network</c>"] Network</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<string>> GetDepositAddressAsync(string asset, string network, CancellationToken ct = default);

        /// <summary>
        /// Get user margin status
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/User/User_getMargin" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/user/margin
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>currency</c>"] Filter by asset</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXMarginStatus[]>> GetMarginStatusAsync(string? asset = null, CancellationToken ct = default);

        /// <summary>
        /// Get user order quote fill ratio
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/User/User_getQuoteFillRatio" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/user/quoteFillRatio
        /// </para>
        /// </summary>
        /// <param name="accountId">["<c>targetAccountId</c>"] Account id</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXFillRatio[]>> GetQuoteFillRatioAsync(long? accountId = null, CancellationToken ct = default);

        /// <summary>
        /// Get user order quote value ratio
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/User/User_getQuoteValueRatio" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/user/quoteValueRatio
        /// </para>
        /// </summary>
        /// <param name="accountId">["<c>targetAccountId</c>"] Account id</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXValueRatio[]>> GetQuoteValueRatioAsync(long accountId, CancellationToken ct = default);

        /// <summary>
        /// Get user 30 days average USD trading volume
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/User/User_getTradingVolume" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/user/tradingVolume
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXUsdVolume[]>> GetTradingVolumeAsync(CancellationToken ct = default);

        /// <summary>
        /// Get user balances
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/User/User_getWallet" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/user/wallet
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>currency</c>"] Filter by asset</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXBalance[]>> GetBalancesAsync(string? asset = null, CancellationToken ct = default);

        /// <summary>
        /// Get balance history
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/User/User_getWalletHistory" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/user/walletHistory
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>currency</c>"] Filter by asset</param>
        /// <param name="targetAccountId">["<c>targetAccountId</c>"] Filter by account id</param>
        /// <param name="reverse">["<c>reverse</c>"] Reverse result order</param>
        /// <param name="offset">["<c>start</c>"] Result offset</param>
        /// <param name="limit">["<c>count</c>"] Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXTransaction[]>> GetBalanceHistoryAsync(string? asset = null,
            long? targetAccountId = null,
            bool? reverse = null,
            int? offset = null,
            int? limit = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get wallet summary
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/User/User_getWalletSummary" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/user/walletSummary
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>currency</c>"] Filter by asset</param>
        /// <param name="startTime">["<c>startTime</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>endTime</c>"] Filter by end time</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BitMEXBalanceSummary[]>> GetBalanceSummaryAsync(string? asset = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default);

        /// <summary>
        /// Transfer funds between accounts
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/User/User_walletTransfer" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/user/walletSummary
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>currency</c>"] Asset</param>
        /// <param name="fromAccountId">["<c>fromAccountId</c>"] From account id</param>
        /// <param name="toAccountId">["<c>toAccountId</c>"] To account id</param>
        /// <param name="quantity">["<c>amount</c>"] Quantity</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BitMEXTransaction>> TransferAsync(string asset, long fromAccountId, long toAccountId, long quantity, CancellationToken ct = default);

        /// <summary>
        /// Create a new withdrawal request
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/User/User_requestWithdrawal" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/user/requestWithdrawal
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>currency</c>"] Asset</param>
        /// <param name="network">["<c>network</c>"] Network</param>
        /// <param name="quantity">["<c>amount</c>"] Quantity</param>
        /// <param name="address">["<c>address</c>"] Target address. One of address, addressId or targetUserId should be provided</param>
        /// <param name="memo">["<c>memo</c>"] Target address memo</param>
        /// <param name="addressId">["<c>addressId</c>"] Address id. One of address, addressId or targetUserId should be provided</param>
        /// <param name="targetUserId">["<c>targetUserId</c>"] Target user id. One of address, addressId or targetUserId should be provided</param>
        /// <param name="fee">["<c>fee</c>"] Network fee for Bitcoin withdrawals. If not specified, a default value will be calculated based on Bitcoin network conditions</param>
        /// <param name="text">["<c>text</c>"] Client text</param>
        /// <param name="otpToken">["<c>otpToken</c>"] 2FA token. Required for all external withdrawals unless the destination is a saved address with skip2FA configured.</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXTransaction>> WithdrawAsync(
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
            CancellationToken ct = default);

        /// <summary>
        /// Cancel a pending withdrawal request
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/User/User_cancelPendingWithdrawal" /><br />
        /// Endpoint:<br />
        /// DELETE /api/v1/user/withdrawal
        /// </para>
        /// </summary>
        /// <param name="transactId">["<c>transactID</c>"] Transaction id</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult> CancelWithdrawalAsync(string transactId, CancellationToken ct = default);

        /// <summary>
        /// Set isolated margin enabled for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Position/Position_isolateMargin" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/position/isolate
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Symbol</param>
        /// <param name="isolatedMarginEnabled">["<c>enabled</c>"] Isolated margin enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXPosition>> SetIsolatedMarginAsync(string symbol, bool isolatedMarginEnabled, CancellationToken ct = default);

        /// <summary>
        /// Update risk limit for a position
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Position/Position_updateRiskLimit" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/position/riskLimit
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Symbol</param>
        /// <param name="riskLimit">["<c>riskLimit</c>"] Risk limit</param>
        /// <param name="targetAccountId">["<c>targetAccountId</c>"] Target account id</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXPosition>> SetRiskLimitAsync(string symbol, int riskLimit, long? targetAccountId = null, CancellationToken ct = default);

        /// <summary>
        /// Add or remove margin to an isolated margin position
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Position/Position_transferIsolatedMargin" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/position/transferMargin
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Symbol</param>
        /// <param name="quantity">["<c>amount</c>"] Quantity to add or remove</param>
        /// <param name="targetAccountId">["<c>targetAccountId</c>"] Target account id</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXPosition>> TransferMarginAsync(string symbol, long quantity, long? targetAccountId = null, CancellationToken ct = default);

        /// <summary>
        /// Get saved addresses
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Address/Address_get" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/address
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXAddress[]>> GetSavedAddressesAsync(CancellationToken ct = default);

        /// <summary>
        /// Add a saved address
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#!/Address/Address_new" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/address
        /// </para>
        /// </summary>
        /// <param name="currency">["<c>currency</c>"] Currency which the asset is for</param>
        /// <param name="network">["<c>network</c>"] Network of the address</param>
        /// <param name="address">["<c>address</c>"] Address</param>
        /// <param name="name">["<c>name</c>"] Address name</param>
        /// <param name="note">["<c>note</c>"] Address note</param>
        /// <param name="skipConfirm">["<c>skipConfirm</c>"] Skip confirm</param>
        /// <param name="skip2FA">["<c>skip2FA</c>"] Skip 2FA</param>
        /// <param name="memo">["<c>memo</c>"] Memo</param>
        /// <param name="otpToken">["<c>otpToken</c>"] OTP token</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXAddress>> AddSavedAddressAsync(
            string currency,
            string network,
            string address,
            string name,
            string? note = null,
            bool? skipConfirm = null,
            bool? skip2FA = null,
            string? memo = null,
            string? otpToken = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get address book config
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#/AddressConfig" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/addressConfig
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXAddressBookConfig>> GetAddressBookSettingsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get API key info
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.bitmex.com/api/explorer/#/APIKey" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/apiKey
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXApiKey[]>> GetApiKeyInfoAsync(CancellationToken ct = default);
    }
}
