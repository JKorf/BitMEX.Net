using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// 
    /// </summary>
    [JsonConverter(typeof(EnumConverter<EventType>))]
    public enum EventType
    {
        /// <summary>
        /// ["<c>apiKeyCreated</c>"] Api key created
        /// </summary>
        [Map("apiKeyCreated")]
        ApiKeyCreated,
        /// <summary>
        /// ["<c>deleverageExecution</c>"] Deleverage execution
        /// </summary>
        [Map("deleverageExecution")]
        DeleverageExecution,
        /// <summary>
        /// ["<c>depositConfirmed</c>"] Deposit confirmed
        /// </summary>
        [Map("depositConfirmed")]
        DepositConfirmed,
        /// <summary>
        /// ["<c>depositPending</c>"] Deposit pending
        /// </summary>
        [Map("depositPending")]
        DepositPending,
        /// <summary>
        /// ["<c>banZeroVolumeApiUser</c>"] Ban zero volume api user
        /// </summary>
        [Map("banZeroVolumeApiUser")]
        BanZeroVolumeApiUser,
        /// <summary>
        /// ["<c>liquidationOrderPlaced</c>"] Liquidation order placed
        /// </summary>
        [Map("liquidationOrderPlaced")]
        LiquidationOrderPlaced,
        /// <summary>
        /// ["<c>login</c>"] Login
        /// </summary>
        [Map("login")]
        Login,
        /// <summary>
        /// ["<c>existingAccountRegistrationAttempt</c>"] Existing account registration attempt
        /// </summary>
        [Map("existingAccountRegistrationAttempt")]
        ExistingAccountRegistrationAttempt,
        /// <summary>
        /// ["<c>passwordChanged</c>"] Password changed
        /// </summary>
        [Map("passwordChanged")]
        PasswordChanged,
        /// <summary>
        /// ["<c>positionStateLiquidated</c>"] Position state liquidated
        /// </summary>
        [Map("positionStateLiquidated")]
        PositionStateLiquidated,
        /// <summary>
        /// ["<c>positionStateWarning</c>"] Position state warning
        /// </summary>
        [Map("positionStateWarning")]
        PositionStateWarning,
        /// <summary>
        /// ["<c>resetPasswordConfirmed</c>"] Reset password confirmed
        /// </summary>
        [Map("resetPasswordConfirmed")]
        ResetPasswordConfirmed,
        /// <summary>
        /// ["<c>resetPasswordRequest</c>"] Reset password request
        /// </summary>
        [Map("resetPasswordRequest")]
        ResetPasswordRequest,
        /// <summary>
        /// ["<c>tradingBotStopped</c>"] Trading bot stopped
        /// </summary>
        [Map("tradingBotStopped")]
        TradingBotStopped,
        /// <summary>
        /// ["<c>transferCanceled</c>"] Transfer canceled
        /// </summary>
        [Map("transferCanceled")]
        TransferCanceled,
        /// <summary>
        /// ["<c>transferCompleted</c>"] Transfer completed
        /// </summary>
        [Map("transferCompleted")]
        TransferCompleted,
        /// <summary>
        /// ["<c>transferReceived</c>"] Transfer received
        /// </summary>
        [Map("transferReceived")]
        TransferReceived,
        /// <summary>
        /// ["<c>transferRequested</c>"] Transfer requested
        /// </summary>
        [Map("transferRequested")]
        TransferRequested,
        /// <summary>
        /// ["<c>twoFactorDisabled</c>"] 2FA disabled
        /// </summary>
        [Map("twoFactorDisabled")]
        TwoFactorDisabled,
        /// <summary>
        /// ["<c>twoFactorEnabled</c>"] 2FA enabled
        /// </summary>
        [Map("twoFactorEnabled")]
        TwoFactorEnabled,
        /// <summary>
        /// ["<c>twoFactorResetCodeCreated</c>"] 2FA reset codes created
        /// </summary>
        [Map("twoFactorResetCodeCreated")]
        TwoFactorResetCodeCreated,
        /// <summary>
        /// ["<c>withdrawalCanceled</c>"] Withdrawal canceled
        /// </summary>
        [Map("withdrawalCanceled")]
        WithdrawalCanceled,
        /// <summary>
        /// ["<c>withdrawalCompleted</c>"] Withdrawal completed
        /// </summary>
        [Map("withdrawalCompleted")]
        WithdrawalCompleted,
        /// <summary>
        /// ["<c>withdrawalConfirmed</c>"] Withdrawal confirmed
        /// </summary>
        [Map("withdrawalConfirmed")]
        WithdrawalConfirmed,
        /// <summary>
        /// ["<c>withdrawalRequested</c>"] Withdrawal requested
        /// </summary>
        [Map("withdrawalRequested")]
        WithdrawalRequested,
        /// <summary>
        /// ["<c>addressCreated</c>"] Address created
        /// </summary>
        [Map("addressCreated")]
        AddressCreated,
        /// <summary>
        /// ["<c>addressRemoved</c>"] Address removed
        /// </summary>
        [Map("addressRemoved")]
        AddressRemoved,
        /// <summary>
        /// ["<c>addressVerified</c>"] Address verified
        /// </summary>
        [Map("addressVerified")]
        AddressVerified,
        /// <summary>
        /// ["<c>addressSkipConfirmRequested</c>"] Address skip confirm requested
        /// </summary>
        [Map("addressSkipConfirmRequested")]
        AddressSkipConfirmRequested,
        /// <summary>
        /// ["<c>addressSkipConfirmVerified</c>"] Address skip confirm verified
        /// </summary>
        [Map("addressSkipConfirmVerified")]
        AddressSkipConfirmVerified,
        /// <summary>
        /// ["<c>addressCooldownUpdated</c>"] Address cooldown updated
        /// </summary>
        [Map("addressCooldownUpdated")]
        AddressCooldownUpdated,
        /// <summary>
        /// ["<c>addressConfigUpdated</c>"] Address config updated
        /// </summary>
        [Map("addressConfigUpdated")]
        AddressConfigUpdated,
        /// <summary>
        /// ["<c>verify</c>"] Verify
        /// </summary>
        [Map("verify")]
        Verify,
        /// <summary>
        /// ["<c>restrictedAccount</c>"] Restricted account
        /// </summary>
        [Map("restrictedAccount")]
        RestrictedAccount,
        /// <summary>
        /// ["<c>unrestrictedAccount</c>"] Unrestricted account
        /// </summary>
        [Map("unrestrictedAccount")]
        UnrestrictedAccount,
        /// <summary>
        /// ["<c>disabledAccount</c>"] Disabled account
        /// </summary>
        [Map("disabledAccount")]
        DisabledAccount,
        /// <summary>
        /// ["<c>enabledAccount</c>"] Enabled account
        /// </summary>
        [Map("enabledAccount")]
        EnabledAccount,
        /// <summary>
        /// ["<c>role:roleMappingDestroy</c>"] Role mapping destory
        /// </summary>
        [Map("role:roleMappingDestroy")]
        RoleMappingDestroy,
        /// <summary>
        /// ["<c>role:chatBanned</c>"] Role chat banned
        /// </summary>
        [Map("role:chatBanned")]
        RoleChatBanned,
        /// <summary>
        /// ["<c>role:withdrawalBanned</c>"] Role withdrawal banned
        /// </summary>
        [Map("role:withdrawalBanned")]
        RoleWithdrawalBanned,
        /// <summary>
        /// ["<c>role:orderBanned</c>"] Role order banned
        /// </summary>
        [Map("role:orderBanned")]
        RoleOrderBanned,
        /// <summary>
        /// ["<c>role:apiBanned</c>"] Role api banned
        /// </summary>
        [Map("role:apiBanned")]
        RoleApiBanned,
        /// <summary>
        /// ["<c>role:restrictedJurisdictionPrivilege</c>"] Role restricted jurisdication privilege
        /// </summary>
        [Map("role:restrictedJurisdictionPrivilege")]
        RoleRestrictedJurisdictionPrivilege
    }

}
