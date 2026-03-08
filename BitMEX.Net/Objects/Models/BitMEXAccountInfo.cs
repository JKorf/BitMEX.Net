using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Objects.Models
{
    /// <summary>
    /// Account info
    /// </summary>
    [SerializationModel]
    public record BitMEXAccountInfo
    {
        /// <summary>
        /// ["<c>id</c>"] Id
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }
        /// <summary>
        /// ["<c>username</c>"] Username
        /// </summary>
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>accountName</c>"] Account name
        /// </summary>
        [JsonPropertyName("accountName")]
        public string? AccountName { get; set; }
        /// <summary>
        /// ["<c>isUser</c>"] Is user
        /// </summary>
        [JsonPropertyName("isUser")]
        public bool IsUser { get; set; }
        /// <summary>
        /// ["<c>created</c>"] CreateTime
        /// </summary>
        [JsonPropertyName("created")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// ["<c>lastUpdated</c>"] Last updated
        /// </summary>
        [JsonPropertyName("lastUpdated")]
        public DateTime LastUpdated { get; set; }
        /// <summary>
        /// ["<c>preferences</c>"] Preferences
        /// </summary>
        [JsonPropertyName("preferences")]
        public BitMEXAccountInfoPreferences Preferences { get; set; } = null!;
        /// <summary>
        /// ["<c>TFAEnabled</c>"] TFA enabled
        /// </summary>
        [JsonPropertyName("TFAEnabled")]
        public string TFAEnabled { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>affiliateID</c>"] Affiliate id
        /// </summary>
        [JsonPropertyName("affiliateID")]
        public string? AffiliateId { get; set; }
        /// <summary>
        /// ["<c>firstTradeTimestamp</c>"] First trade timestamp
        /// </summary>
        [JsonPropertyName("firstTradeTimestamp")]
        public DateTime? FirstTradeTimestamp { get; set; }
        /// <summary>
        /// ["<c>firstDepositTimestamp</c>"] First deposit timestamp
        /// </summary>
        [JsonPropertyName("firstDepositTimestamp")]
        public DateTime? FirstDepositTimestamp { get; set; }
        /// <summary>
        /// ["<c>isElite</c>"] Is elite
        /// </summary>
        [JsonPropertyName("isElite")]
        public bool IsElite { get; set; }
        /// <summary>
        /// ["<c>typ</c>"] Type
        /// </summary>
        [JsonPropertyName("typ")]
        public string? Type { get; set; }
    }

    /// <summary>
    /// Preferences
    /// </summary>
    [SerializationModel]
    public record BitMEXAccountInfoPreferences
    {
        /// <summary>
        /// ["<c>alertOnLiquidations</c>"] Alert on liquidations
        /// </summary>
        [JsonPropertyName("alertOnLiquidations")]
        public bool AlertOnLiquidations { get; set; }
        /// <summary>
        /// ["<c>animationsEnabled</c>"] Animations enabled
        /// </summary>
        [JsonPropertyName("animationsEnabled")]
        public bool AnimationsEnabled { get; set; }
        /// <summary>
        /// ["<c>announcementsLastSeen</c>"] Announcements last seen
        /// </summary>
        [JsonPropertyName("announcementsLastSeen")]
        public DateTime? AnnouncementsLastSeen { get; set; }
        /// <summary>
        /// ["<c>botsAdvancedMode</c>"] Bots advanced mode
        /// </summary>
        [JsonPropertyName("botsAdvancedMode")]
        public bool BotsAdvancedMode { get; set; }
        /// <summary>
        /// ["<c>botVideosHidden</c>"] Bot videos hidden
        /// </summary>
        [JsonPropertyName("botVideosHidden")]
        public bool BotVideosHidden { get; set; }
        /// <summary>
        /// ["<c>chatChannelID</c>"] Chat channel id
        /// </summary>
        [JsonPropertyName("chatChannelID")]
        public decimal ChatChannelId { get; set; }
        /// <summary>
        /// ["<c>colorTheme</c>"] Color theme
        /// </summary>
        [JsonPropertyName("colorTheme")]
        public string ColorTheme { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>currency</c>"] Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>debug</c>"] Debug
        /// </summary>
        [JsonPropertyName("debug")]
        public bool Debug { get; set; }
        /// <summary>
        /// ["<c>disableEmails</c>"] Disable emails
        /// </summary>
        [JsonPropertyName("disableEmails")]
        public string[]? DisableEmails { get; set; }
        /// <summary>
        /// ["<c>disablePush</c>"] Disable push
        /// </summary>
        [JsonPropertyName("disablePush")]
        public string[]? DisablePush { get; set; }
        /// <summary>
        /// ["<c>displayCorpEnrollUpsell</c>"] Display corp enroll upsell
        /// </summary>
        [JsonPropertyName("displayCorpEnrollUpsell")]
        public bool DisplayCorpEnrollUpsell { get; set; }
        /// <summary>
        /// ["<c>equivalentCurrency</c>"] Equivalent asset
        /// </summary>
        [JsonPropertyName("equivalentCurrency")]
        public string EquivalentAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>features</c>"] Features
        /// </summary>
        [JsonPropertyName("features")]
        public string[]? Features { get; set; } 
        /// <summary>
        /// ["<c>favourites</c>"] Favourites
        /// </summary>
        [JsonPropertyName("favourites")]
        public string[]? Favourites { get; set; }
        /// <summary>
        /// ["<c>favouritesAssets</c>"] Favourites assets
        /// </summary>
        [JsonPropertyName("favouritesAssets")]
        public string[]? FavouritesAssets { get; set; }
        /// <summary>
        /// ["<c>favouritesOrdered</c>"] Favourites ordered
        /// </summary>
        [JsonPropertyName("favouritesOrdered")]
        public string[]? FavouritesOrdered { get; set; }
        /// <summary>
        /// ["<c>favouriteBots</c>"] Favourite bots
        /// </summary>
        [JsonPropertyName("favouriteBots")]
        public string[]? FavouriteBots { get; set; }
        /// <summary>
        /// ["<c>favouriteContracts</c>"] Favourite contracts
        /// </summary>
        [JsonPropertyName("favouriteContracts")]
        public string[]? FavouriteContracts { get; set; }
        /// <summary>
        /// ["<c>hasSetTradingCurrencies</c>"] Has set trading currencies
        /// </summary>
        [JsonPropertyName("hasSetTradingCurrencies")]
        public bool HasSetTradingCurrencies { get; set; }
        /// <summary>
        /// ["<c>hideConfirmDialogs</c>"] Hide confirm dialogs
        /// </summary>
        [JsonPropertyName("hideConfirmDialogs")]
        public string[]? HideConfirmDialogs { get; set; }
        /// <summary>
        /// ["<c>hideConnectionModal</c>"] Hide connection modal
        /// </summary>
        [JsonPropertyName("hideConnectionModal")]
        public bool HideConnectionModal { get; set; }
        /// <summary>
        /// ["<c>hideFromLeaderboard</c>"] Hide from leaderboard
        /// </summary>
        [JsonPropertyName("hideFromLeaderboard")]
        public bool HideFromLeaderboard { get; set; }
        /// <summary>
        /// ["<c>hideNameFromLeaderboard</c>"] Hide name from leaderboard
        /// </summary>
        [JsonPropertyName("hideNameFromLeaderboard")]
        public bool HideNameFromLeaderboard { get; set; }
        /// <summary>
        /// ["<c>hidePnlInGuilds</c>"] Hide profit and loss in guilds
        /// </summary>
        [JsonPropertyName("hidePnlInGuilds")]
        public bool HidePnlInGuilds { get; set; }
        /// <summary>
        /// ["<c>hideRoiInGuilds</c>"] Hide return on investment in guilds
        /// </summary>
        [JsonPropertyName("hideRoiInGuilds")]
        public bool HideRoiInGuilds { get; set; }
        /// <summary>
        /// ["<c>hideNotifications</c>"] Hide notifications
        /// </summary>
        [JsonPropertyName("hideNotifications")]
        public string[]? HideNotifications { get; set; }
        /// <summary>
        /// ["<c>hidePhoneConfirm</c>"] Hide phone confirm
        /// </summary>
        [JsonPropertyName("hidePhoneConfirm")]
        public bool HidePhoneConfirm { get; set; }
        /// <summary>
        /// ["<c>guidesShownVersion</c>"] Guides shown version
        /// </summary>
        [JsonPropertyName("guidesShownVersion")]
        public int GuidesShownVersion { get; set; }
        /// <summary>
        /// ["<c>isSensitiveInfoVisible</c>"] Is sensitive info visible
        /// </summary>
        [JsonPropertyName("isSensitiveInfoVisible")]
        public bool IsSensitiveInfoVisible { get; set; }
        /// <summary>
        /// ["<c>isWalletZeroBalanceHidden</c>"] Is wallet zero balance hidden
        /// </summary>
        [JsonPropertyName("isWalletZeroBalanceHidden")]
        public bool IsWalletZeroBalanceHidden { get; set; }
        /// <summary>
        /// ["<c>locale</c>"] Locale
        /// </summary>
        [JsonPropertyName("locale")]
        public string Locale { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>localeSetTime</c>"] Locale set time
        /// </summary>
        [JsonPropertyName("localeSetTime")]
        public long? LocaleSetTime { get; set; }
        /// <summary>
        /// ["<c>marginPnlRow</c>"] Margin profit and loss row
        /// </summary>
        [JsonPropertyName("marginPnlRow")]
        public string MarginPnlRow { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>marginPnlRowKind</c>"] Margin profit and loss row kind
        /// </summary>
        [JsonPropertyName("marginPnlRowKind")]
        public string MarginPnlRowKind { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>mobileLocale</c>"] Mobile locale
        /// </summary>
        [JsonPropertyName("mobileLocale")]
        public string MobileLocale { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>msgsSeen</c>"] Msgs seen
        /// </summary>
        [JsonPropertyName("msgsSeen")]
        public string[]? MessagesSeen { get; set; }
        /// <summary>
        /// ["<c>optionsBeta</c>"] Options beta
        /// </summary>
        [JsonPropertyName("optionsBeta")]
        public bool OptionsBeta { get; set; }
        /// <summary>
        /// ["<c>orderBookType</c>"] Order book type
        /// </summary>
        [JsonPropertyName("orderBookType")]
        public string OrderBookType { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>orderClearImmediate</c>"] Order clear immediate
        /// </summary>
        [JsonPropertyName("orderClearImmediate")]
        public bool OrderClearImmediate { get; set; }
        /// <summary>
        /// ["<c>orderControlsPlusMinus</c>"] Order controls plus minus
        /// </summary>
        [JsonPropertyName("orderControlsPlusMinus")]
        public bool OrderControlsPlusMinus { get; set; }
        /// <summary>
        /// ["<c>orderInputType</c>"] Order input type
        /// </summary>
        [JsonPropertyName("orderInputType")]
        public string OrderInputType { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>platformLayout</c>"] Platform layout
        /// </summary>
        [JsonPropertyName("platformLayout")]
        public string PlatformLayout { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>selectedFiatCurrency</c>"] Selected fiat asset
        /// </summary>
        [JsonPropertyName("selectedFiatCurrency")]
        public string SelectedFiatAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>showChartBottomToolbar</c>"] Show chart bottom toolbar
        /// </summary>
        [JsonPropertyName("showChartBottomToolbar")]
        public bool ShowChartBottomToolbar { get; set; }
        /// <summary>
        /// ["<c>showLocaleNumbers</c>"] Show locale numbers
        /// </summary>
        [JsonPropertyName("showLocaleNumbers")]
        public bool ShowLocaleNumbers { get; set; }
        /// <summary>
        /// ["<c>sounds</c>"] Sounds
        /// </summary>
        [JsonPropertyName("sounds")]
        public string[]? Sounds { get; set; }
        /// <summary>
        /// ["<c>spacingPreference</c>"] Spacing preference
        /// </summary>
        [JsonPropertyName("spacingPreference")]
        public string SpacingPreference { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>strictIPCheck</c>"] Strict ip check
        /// </summary>
        [JsonPropertyName("strictIPCheck")]
        public bool StrictIPCheck { get; set; }
        /// <summary>
        /// ["<c>strictTimeout</c>"] Strict timeout
        /// </summary>
        [JsonPropertyName("strictTimeout")]
        public bool StrictTimeout { get; set; }
        /// <summary>
        /// ["<c>tickerGroup</c>"] Ticker group
        /// </summary>
        [JsonPropertyName("tickerGroup")]
        public string TickerGroup { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>tickerPinned</c>"] Ticker pinned
        /// </summary>
        [JsonPropertyName("tickerPinned")]
        public bool TickerPinned { get; set; }
        /// <summary>
        /// ["<c>tradeLayout</c>"] Trade layout
        /// </summary>
        [JsonPropertyName("tradeLayout")]
        public string TradeLayout { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>userColor</c>"] User color
        /// </summary>
        [JsonPropertyName("userColor")]
        public string UserColor { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>videosSeen</c>"] Videos seen
        /// </summary>
        [JsonPropertyName("videosSeen")]
        public string[]? VideosSeen { get; set; }
    }
}
