using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitMEX.Net.Enums
{
    /// <summary>
    /// 
    /// </summary>
    public enum EventType
    {
        /// <summary>
        /// Api key created
        /// </summary>
        [Map("apiKeyCreated")]
        ApiKeyCreated,
        /// <summary>
        /// Deleverage execution
        /// </summary>
        [Map("deleverageExecution")]
        DeleverageExecution,
        /// <summary>
        /// Deposit confirmed
        /// </summary>
        [Map("depositConfirmed")]
        DepositConfirmed,
        /// <summary>
        /// Deposit pending
        /// </summary>
        [Map("depositPending")]
        DepositPending,
        /// <summary>
        /// Ban zero volume api user
        /// </summary>
        [Map("banZeroVolumeApiUser")]
        BanZeroVolumeApiUser,
        /// <summary>
        /// Liquidation order placed
        /// </summary>
        [Map("liquidationOrderPlaced")]
        LiquidationOrderPlaced,
        /// <summary>
        /// Login
        /// </summary>
        [Map("login")]
        Login,
        /// <summary>
        /// Existing account registration attempt
        /// </summary>
        [Map("existingAccountRegistrationAttempt")]
        ExistingAccountRegistrationAttempt,
        /// <summary>
        /// Password changed
        /// </summary>
        [Map("passwordChanged")]
        PasswordChanged,
        /// <summary>
        /// Position state liquidated
        /// </summary>
        [Map("positionStateLiquidated")]
        PositionStateLiquidated,
        /// <summary>
        /// Position state warning
        /// </summary>
        [Map("positionStateWarning")]
        PositionStateWarning,
        /// <summary>
        /// Reset password confirmed
        /// </summary>
        [Map("resetPasswordConfirmed")]
        ResetPasswordConfirmed,
        /// <summary>
        /// Reset password request
        /// </summary>
        [Map("resetPasswordRequest")]
        ResetPasswordRequest,
        /// <summary>
        /// Trading bot stopped
        /// </summary>
        [Map("tradingBotStopped")]
        TradingBotStopped,
        /// <summary>
        /// Transfer canceled
        /// </summary>
        [Map("transferCanceled")]
        TransferCanceled,
        /// <summary>
        /// Transfer completed
        /// </summary>
        [Map("transferCompleted")]
        TransferCompleted,
        /// <summary>
        /// Transfer received
        /// </summary>
        [Map("transferReceived")]
        TransferReceived,
        /// <summary>
        /// Transfer requested
        /// </summary>
        [Map("transferRequested")]
        TransferRequested,
        /// <summary>
        /// 2FA disabled
        /// </summary>
        [Map("twoFactorDisabled")]
        TwoFactorDisabled,
        /// <summary>
        /// 2FA enabled
        /// </summary>
        [Map("twoFactorEnabled")]
        TwoFactorEnabled,
        /// <summary>
        /// 2FA reset codes created
        /// </summary>
        [Map("twoFactorResetCodeCreated")]
        TwoFactorResetCodeCreated,
        /// <summary>
        /// Withdrawal canceled
        /// </summary>
        [Map("withdrawalCanceled")]
        WithdrawalCanceled,
        /// <summary>
        /// Withdrawal completed
        /// </summary>
        [Map("withdrawalCompleted")]
        WithdrawalCompleted,
        /// <summary>
        /// Withdrawal confirmed
        /// </summary>
        [Map("withdrawalConfirmed")]
        WithdrawalConfirmed,
        /// <summary>
        /// Withdrawal requested
        /// </summary>
        [Map("withdrawalRequested")]
        WithdrawalRequested,
        /// <summary>
        /// Address created
        /// </summary>
        [Map("addressCreated")]
        AddressCreated,
        /// <summary>
        /// Address removed
        /// </summary>
        [Map("addressRemoved")]
        AddressRemoved,
        /// <summary>
        /// Address verified
        /// </summary>
        [Map("addressVerified")]
        AddressVerified,
        /// <summary>
        /// Address skip confirm requested
        /// </summary>
        [Map("addressSkipConfirmRequested")]
        AddressSkipConfirmRequested,
        /// <summary>
        /// Address skip confirm verified
        /// </summary>
        [Map("addressSkipConfirmVerified")]
        AddressSkipConfirmVerified,
        /// <summary>
        /// Address cooldown updated
        /// </summary>
        [Map("addressCooldownUpdated")]
        AddressCooldownUpdated,
        /// <summary>
        /// Address config updated
        /// </summary>
        [Map("addressConfigUpdated")]
        AddressConfigUpdated,
        /// <summary>
        /// Verify
        /// </summary>
        [Map("verify")]
        Verify,
        /// <summary>
        /// Restricted account
        /// </summary>
        [Map("restrictedAccount")]
        RestrictedAccount,
        /// <summary>
        /// Unrestricted account
        /// </summary>
        [Map("unrestrictedAccount")]
        UnrestrictedAccount,
        /// <summary>
        /// Disabled account
        /// </summary>
        [Map("disabledAccount")]
        DisabledAccount,
        /// <summary>
        /// Enabled account
        /// </summary>
        [Map("enabledAccount")]
        EnabledAccount,
        /// <summary>
        /// Role mapping destory
        /// </summary>
        [Map("role:roleMappingDestroy")]
        RoleMappingDestroy,
        /// <summary>
        /// Role chat banned
        /// </summary>
        [Map("role:chatBanned")]
        RoleChatBanned,
        /// <summary>
        /// Role withdrawal banned
        /// </summary>
        [Map("role:withdrawalBanned")]
        RoleWithdrawalBanned,
        /// <summary>
        /// Role order banned
        /// </summary>
        [Map("role:orderBanned")]
        RoleOrderBanned,
        /// <summary>
        /// Role api banned
        /// </summary>
        [Map("role:apiBanned")]
        RoleApiBanned,
        /// <summary>
        /// Role restricted jurisdication privilege
        /// </summary>
        [Map("role:restrictedJurisdictionPrivilege")]
        RoleRestrictedJurisdictionPrivilege
    }

}
