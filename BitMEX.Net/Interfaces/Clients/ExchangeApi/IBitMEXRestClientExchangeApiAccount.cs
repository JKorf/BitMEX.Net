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
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/UserEvent/UserEvent_get" /></para>
        /// </summary>
        /// <param name="fromId">From id</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<BitMEXUserEvent>>> GetUserEventsAsync(long? fromId = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get account info
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/User/User_get" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXAccountInfo>> GetAccountInfoAsync(CancellationToken ct = default);

        /// <summary>
        /// Get user trading fees
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/User/User_getCommission" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<Dictionary<string, BitMEXTradeFee>>> GetFeesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get deposit address
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/User/User_getDepositAddress" /></para>
        /// </summary>
        /// <param name="asset">Asset name</param>
        /// <param name="network">Network</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<string>> GetDepositAddressAsync(string asset, string network, CancellationToken ct = default);

        /// <summary>
        /// Get transfer accounts
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/User/User_getWalletTransferAccounts" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<BitMEXTransaction>>> GetTransferAccountsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get user margin status
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/User/User_getMargin" /></para>
        /// </summary>
        /// <param name="symbol">Filter by symbol</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXMarginStatus>> GetMarginStatusAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Get user order quote fill ratio
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/User/User_getQuoteFillRatio" /></para>
        /// </summary>
        /// <param name="accountId">Account id</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<BitMEXFillRatio>>> GetQuoteFillRatioAsync(long? accountId = null, CancellationToken ct = default);

        /// <summary>
        /// Get user order quote value ratio
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/User/User_getQuoteValueRatio" /></para>
        /// </summary>
        /// <param name="accountId">Account id</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<BitMEXValueRatio>>> GetQuoteValueRatioAsync(long accountId, CancellationToken ct = default);

        /// <summary>
        /// Get user 30 days average USD trading volume
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/User/User_getTradingVolume" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<BitMEXUsdVolume>>> GetTradingVolumeAsync(CancellationToken ct = default);

        /// <summary>
        /// Get user balances
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/User/User_getWallet" /></para>
        /// </summary>
        /// <param name="asset">Filter by asset</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<BitMEXBalance>>> GetBalancesAsync(string? asset = null, CancellationToken ct = default);

        /// <summary>
        /// Get balance history
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/User/User_getWalletHistory" /></para>
        /// </summary>
        /// <param name="asset">Filter by asset</param>
        /// <param name="targetAccountId">Filter by account id</param>
        /// <param name="reverse">Reverse result order</param>
        /// <param name="offset">Result offset</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<BitMEXTransaction>>> GetBalanceHistoryAsync(string? asset = null,
            long? targetAccountId = null,
            bool? reverse = null,
            int? offset = null,
            int? limit = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get wallet summary
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/User/User_getWalletSummary" /></para>
        /// </summary>
        /// <param name="asset">Filter by asset</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<IEnumerable<BitMEXBalanceSummary>>> GetBalanceSummaryAsync(string? asset = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default);

        /// <summary>
        /// Transfer funds between accounts
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/User/User_walletTransfer" /></para>
        /// </summary>
        /// <param name="asset">Asset</param>
        /// <param name="fromAccountId">From account id</param>
        /// <param name="toAccountId">To account id</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BitMEXTransaction>> TransferAsync(string asset, long fromAccountId, long toAccountId, long quantity, CancellationToken ct = default);

        /// <summary>
        /// Create a new withdrawal request
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/User/User_requestWithdrawal" /></para>
        /// </summary>
        /// <param name="asset">Asset</param>
        /// <param name="network">Network</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="address">Target address. One of address, addressId or targetUserId should be provided</param>
        /// <param name="memo">Target address memo</param>
        /// <param name="addressId">Address id. One of address, addressId or targetUserId should be provided</param>
        /// <param name="targetUserId">Target user id. One of address, addressId or targetUserId should be provided</param>
        /// <param name="fee">Network fee for Bitcoin withdrawals. If not specified, a default value will be calculated based on Bitcoin network conditions</param>
        /// <param name="text">Client text</param>
        /// <param name="otpToken">2FA token. Required for all external withdrawals unless the destination is a saved address with skip2FA configured.</param>
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
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/User/User_cancelPendingWithdrawal" /></para>
        /// </summary>
        /// <param name="transactId">Transaction id</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult> CancelWithdrawalAsync(string transactId, CancellationToken ct = default);

        /// <summary>
        /// Set isolated margin enabled for a symbol
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Position/Position_isolateMargin" /></para>
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="isolatedMarginEnabled">Isolated margin enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXPosition>> SetIsolatedMarginAsync(string symbol, bool isolatedMarginEnabled, CancellationToken ct = default);

        /// <summary>
        /// Update risk limit for a position
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Position/Position_updateRiskLimit" /></para>
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="riskLimit">Risk limit</param>
        /// <param name="targetAccountId">Target account id</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXPosition>> SetRiskLimitAsync(string symbol, int riskLimit, long? targetAccountId = null, CancellationToken ct = default);

        /// <summary>
        /// Add or remove margin to an isolated margin position
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Position/Position_transferIsolatedMargin" /></para>
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="quantity">Quantity to add or remove</param>
        /// <param name="targetAccountId">Target account id</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXPosition>> TransferMarginAsync(string symbol, long quantity, long? targetAccountId = null, CancellationToken ct = default);

        /// <summary>
        /// Get saved addresses
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Address/Address_get" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<BitMEXAddress>>> GetSavedAddressesAsync(CancellationToken ct = default);

        /// <summary>
        /// Add a saved address
        /// <para><a href="https://www.bitmex.com/api/explorer/#!/Address/Address_new" /></para>
        /// </summary>
        /// <param name="currency">Currency which the asset is for</param>
        /// <param name="network">Network of the address</param>
        /// <param name="address">Address</param>
        /// <param name="name">Address name</param>
        /// <param name="note">Address note</param>
        /// <param name="skipConfirm">Skip confirm</param>
        /// <param name="skip2FA">Skip 2FA</param>
        /// <param name="memo">Memo</param>
        /// <param name="otpToken">OTP token</param>
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
        /// <para><a href="https://www.bitmex.com/api/explorer/#/AddressConfig" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BitMEXAddressBookConfig>> GetAddressBookSettingsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get API key info
        /// <para><a href="https://www.bitmex.com/api/explorer/#/APIKey" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<BitMEXApiKey>>> GetApiKeyInfoAsync(CancellationToken ct = default);
    }
}
