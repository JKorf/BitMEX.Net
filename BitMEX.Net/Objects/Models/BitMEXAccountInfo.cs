using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Account info
    /// </summary>
    public record BitMEXAccountInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }
        /// <summary>
        /// Firstname
        /// </summary>
        [JsonPropertyName("firstname")]
        public string Firstname { get; set; } = string.Empty;
        /// <summary>
        /// Lastname
        /// </summary>
        [JsonPropertyName("lastname")]
        public string Lastname { get; set; } = string.Empty;
        /// <summary>
        /// Username
        /// </summary>
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;
        /// <summary>
        /// Account name
        /// </summary>
        [JsonPropertyName("accountName")]
        public string? AccountName { get; set; }
        /// <summary>
        /// Is user
        /// </summary>
        [JsonPropertyName("isUser")]
        public bool IsUser { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// Date of birth
        /// </summary>
        [JsonPropertyName("dateOfBirth")]
        public DateTime? DateOfBirth { get; set; }
        /// <summary>
        /// Phone
        /// </summary>
        [JsonPropertyName("phone")]
        public string? Phone { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>
        [JsonPropertyName("created")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Last updated
        /// </summary>
        [JsonPropertyName("lastUpdated")]
        public DateTime LastUpdated { get; set; }
        /// <summary>
        /// Preferences
        /// </summary>
        [JsonPropertyName("preferences")]
        public BitMEXAccountInfoPreferences Preferences { get; set; } = null!;
        /// <summary>
        /// TFA enabled
        /// </summary>
        [JsonPropertyName("TFAEnabled")]
        public string TFAEnabled { get; set; } = string.Empty;
        /// <summary>
        /// Affiliate id
        /// </summary>
        [JsonPropertyName("affiliateID")]
        public string? AffiliateId { get; set; }
        /// <summary>
        /// Country
        /// </summary>
        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;
        /// <summary>
        /// Geo-ip country
        /// </summary>
        [JsonPropertyName("geoipCountry")]
        public string GeoipCountry { get; set; } = string.Empty;
        /// <summary>
        /// Geo-ip region
        /// </summary>
        [JsonPropertyName("geoipRegion")]
        public string GeoipRegion { get; set; } = string.Empty;
        /// <summary>
        /// First trade timestamp
        /// </summary>
        [JsonPropertyName("firstTradeTimestamp")]
        public DateTime? FirstTradeTimestamp { get; set; }
        /// <summary>
        /// First deposit timestamp
        /// </summary>
        [JsonPropertyName("firstDepositTimestamp")]
        public DateTime? FirstDepositTimestamp { get; set; }
        /// <summary>
        /// Is elite
        /// </summary>
        [JsonPropertyName("isElite")]
        public bool IsElite { get; set; }
        /// <summary>
        /// Type
        /// </summary>
        [JsonPropertyName("typ")]
        public string? Type { get; set; }
    }

    /// <summary>
    /// Preferences
    /// </summary>
    public record BitMEXAccountInfoPreferences
    {
        /// <summary>
        /// Alert on liquidations
        /// </summary>
        [JsonPropertyName("alertOnLiquidations")]
        public bool AlertOnLiquidations { get; set; }
        /// <summary>
        /// Animations enabled
        /// </summary>
        [JsonPropertyName("animationsEnabled")]
        public bool AnimationsEnabled { get; set; }
        /// <summary>
        /// Announcements last seen
        /// </summary>
        [JsonPropertyName("announcementsLastSeen")]
        public DateTime? AnnouncementsLastSeen { get; set; }
        /// <summary>
        /// Bots advanced mode
        /// </summary>
        [JsonPropertyName("botsAdvancedMode")]
        public bool BotsAdvancedMode { get; set; }
        /// <summary>
        /// Bot videos hidden
        /// </summary>
        [JsonPropertyName("botVideosHidden")]
        public bool BotVideosHidden { get; set; }
        /// <summary>
        /// Chat channel id
        /// </summary>
        [JsonPropertyName("chatChannelID")]
        public decimal ChatChannelId { get; set; }
        /// <summary>
        /// Color theme
        /// </summary>
        [JsonPropertyName("colorTheme")]
        public string ColorTheme { get; set; } = string.Empty;
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Debug
        /// </summary>
        [JsonPropertyName("debug")]
        public bool Debug { get; set; }
        /// <summary>
        /// Disable emails
        /// </summary>
        [JsonPropertyName("disableEmails")]
        public IEnumerable<string>? DisableEmails { get; set; }
        /// <summary>
        /// Disable push
        /// </summary>
        [JsonPropertyName("disablePush")]
        public IEnumerable<string>? DisablePush { get; set; }
        /// <summary>
        /// Display corp enroll upsell
        /// </summary>
        [JsonPropertyName("displayCorpEnrollUpsell")]
        public bool DisplayCorpEnrollUpsell { get; set; }
        /// <summary>
        /// Equivalent asset
        /// </summary>
        [JsonPropertyName("equivalentCurrency")]
        public string EquivalentAsset { get; set; } = string.Empty;
        /// <summary>
        /// Features
        /// </summary>
        [JsonPropertyName("features")]
        public IEnumerable<string>? Features { get; set; } 
        /// <summary>
        /// Favourites
        /// </summary>
        [JsonPropertyName("favourites")]
        public IEnumerable<string>? Favourites { get; set; }
        /// <summary>
        /// Favourites assets
        /// </summary>
        [JsonPropertyName("favouritesAssets")]
        public IEnumerable<string>? FavouritesAssets { get; set; }
        /// <summary>
        /// Favourites ordered
        /// </summary>
        [JsonPropertyName("favouritesOrdered")]
        public IEnumerable<string>? FavouritesOrdered { get; set; }
        /// <summary>
        /// Favourite bots
        /// </summary>
        [JsonPropertyName("favouriteBots")]
        public IEnumerable<string>? FavouriteBots { get; set; }
        /// <summary>
        /// Favourite contracts
        /// </summary>
        [JsonPropertyName("favouriteContracts")]
        public IEnumerable<string>? FavouriteContracts { get; set; }
        /// <summary>
        /// Has set trading currencies
        /// </summary>
        [JsonPropertyName("hasSetTradingCurrencies")]
        public bool HasSetTradingCurrencies { get; set; }
        /// <summary>
        /// Hide confirm dialogs
        /// </summary>
        [JsonPropertyName("hideConfirmDialogs")]
        public IEnumerable<string>? HideConfirmDialogs { get; set; }
        /// <summary>
        /// Hide connection modal
        /// </summary>
        [JsonPropertyName("hideConnectionModal")]
        public bool HideConnectionModal { get; set; }
        /// <summary>
        /// Hide from leaderboard
        /// </summary>
        [JsonPropertyName("hideFromLeaderboard")]
        public bool HideFromLeaderboard { get; set; }
        /// <summary>
        /// Hide name from leaderboard
        /// </summary>
        [JsonPropertyName("hideNameFromLeaderboard")]
        public bool HideNameFromLeaderboard { get; set; }
        /// <summary>
        /// Hide profit and loss in guilds
        /// </summary>
        [JsonPropertyName("hidePnlInGuilds")]
        public bool HidePnlInGuilds { get; set; }
        /// <summary>
        /// Hide return on investment in guilds
        /// </summary>
        [JsonPropertyName("hideRoiInGuilds")]
        public bool HideRoiInGuilds { get; set; }
        /// <summary>
        /// Hide notifications
        /// </summary>
        [JsonPropertyName("hideNotifications")]
        public IEnumerable<string>? HideNotifications { get; set; }
        /// <summary>
        /// Hide phone confirm
        /// </summary>
        [JsonPropertyName("hidePhoneConfirm")]
        public bool HidePhoneConfirm { get; set; }
        /// <summary>
        /// Guides shown version
        /// </summary>
        [JsonPropertyName("guidesShownVersion")]
        public int GuidesShownVersion { get; set; }
        /// <summary>
        /// Is sensitive info visible
        /// </summary>
        [JsonPropertyName("isSensitiveInfoVisible")]
        public bool IsSensitiveInfoVisible { get; set; }
        /// <summary>
        /// Is wallet zero balance hidden
        /// </summary>
        [JsonPropertyName("isWalletZeroBalanceHidden")]
        public bool IsWalletZeroBalanceHidden { get; set; }
        /// <summary>
        /// Locale
        /// </summary>
        [JsonPropertyName("locale")]
        public string Locale { get; set; } = string.Empty;
        /// <summary>
        /// Locale set time
        /// </summary>
        [JsonPropertyName("localeSetTime")]
        public long? LocaleSetTime { get; set; }
        /// <summary>
        /// Margin profit and loss row
        /// </summary>
        [JsonPropertyName("marginPnlRow")]
        public string MarginPnlRow { get; set; } = string.Empty;
        /// <summary>
        /// Margin profit and loss row kind
        /// </summary>
        [JsonPropertyName("marginPnlRowKind")]
        public string MarginPnlRowKind { get; set; } = string.Empty;
        /// <summary>
        /// Mobile locale
        /// </summary>
        [JsonPropertyName("mobileLocale")]
        public string MobileLocale { get; set; } = string.Empty;
        /// <summary>
        /// Msgs seen
        /// </summary>
        [JsonPropertyName("msgsSeen")]
        public IEnumerable<string>? MessagesSeen { get; set; }
        /// <summary>
        /// Options beta
        /// </summary>
        [JsonPropertyName("optionsBeta")]
        public bool OptionsBeta { get; set; }
        /// <summary>
        /// Order book type
        /// </summary>
        [JsonPropertyName("orderBookType")]
        public string OrderBookType { get; set; } = string.Empty;
        /// <summary>
        /// Order clear immediate
        /// </summary>
        [JsonPropertyName("orderClearImmediate")]
        public bool OrderClearImmediate { get; set; }
        /// <summary>
        /// Order controls plus minus
        /// </summary>
        [JsonPropertyName("orderControlsPlusMinus")]
        public bool OrderControlsPlusMinus { get; set; }
        /// <summary>
        /// Order input type
        /// </summary>
        [JsonPropertyName("orderInputType")]
        public string OrderInputType { get; set; } = string.Empty;
        /// <summary>
        /// Platform layout
        /// </summary>
        [JsonPropertyName("platformLayout")]
        public string PlatformLayout { get; set; } = string.Empty;
        /// <summary>
        /// Selected fiat asset
        /// </summary>
        [JsonPropertyName("selectedFiatCurrency")]
        public string SelectedFiatAsset { get; set; } = string.Empty;
        /// <summary>
        /// Show chart bottom toolbar
        /// </summary>
        [JsonPropertyName("showChartBottomToolbar")]
        public bool ShowChartBottomToolbar { get; set; }
        /// <summary>
        /// Show locale numbers
        /// </summary>
        [JsonPropertyName("showLocaleNumbers")]
        public bool ShowLocaleNumbers { get; set; }
        /// <summary>
        /// Sounds
        /// </summary>
        [JsonPropertyName("sounds")]
        public IEnumerable<string>? Sounds { get; set; }
        /// <summary>
        /// Spacing preference
        /// </summary>
        [JsonPropertyName("spacingPreference")]
        public string SpacingPreference { get; set; } = string.Empty;
        /// <summary>
        /// Strict ip check
        /// </summary>
        [JsonPropertyName("strictIPCheck")]
        public bool StrictIPCheck { get; set; }
        /// <summary>
        /// Strict timeout
        /// </summary>
        [JsonPropertyName("strictTimeout")]
        public bool StrictTimeout { get; set; }
        /// <summary>
        /// Ticker group
        /// </summary>
        [JsonPropertyName("tickerGroup")]
        public string TickerGroup { get; set; } = string.Empty;
        /// <summary>
        /// Ticker pinned
        /// </summary>
        [JsonPropertyName("tickerPinned")]
        public bool TickerPinned { get; set; }
        /// <summary>
        /// Trade layout
        /// </summary>
        [JsonPropertyName("tradeLayout")]
        public string TradeLayout { get; set; } = string.Empty;
        /// <summary>
        /// User color
        /// </summary>
        [JsonPropertyName("userColor")]
        public string UserColor { get; set; } = string.Empty;
        /// <summary>
        /// Videos seen
        /// </summary>
        [JsonPropertyName("videosSeen")]
        public IEnumerable<string>? VideosSeen { get; set; }
    }
}
