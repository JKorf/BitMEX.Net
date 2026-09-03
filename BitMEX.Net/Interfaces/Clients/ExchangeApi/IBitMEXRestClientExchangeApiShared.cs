using CryptoExchange.Net.SharedApis;

namespace BitMEX.Net.Interfaces.Clients.ExchangeApi
{
    /// <summary>
    /// Shared interface for Exchange rest API usage
    /// </summary>
    public interface IBitMEXRestClientExchangeApiShared :
        IAssetsRestClient,
        IBalanceRestClient,
        IDepositRestClient,
        IFeeRestClient,
        IKlineRestClient,
        IOrderBookRestClient,
        IRecentTradeRestClient,
        ITradeHistoryRestClient,
        IWithdrawalRestClient,
        IWithdrawRestClient,
        ISpotSymbolRestClient,
        ISpotTickerRestClient,
        ISpotOrderRestClient,
        IFundingRateRestClient,
        IFuturesSymbolRestClient,
        IFuturesTickerRestClient,
        ILeverageRestClient,
        IOpenInterestRestClient,
        IFuturesOrderRestClient,
        ISpotOrderClientIdRestClient,
        IFuturesOrderClientIdRestClient,
        IFuturesTriggerOrderRestClient,
        ISpotTriggerOrderRestClient,
        IFuturesTpSlRestClient,
        IBookTickerRestClient
    {
    }

    /// <summary>
    /// Shared API interface. Shared APIs provide a common,
    /// exchange-independent contract for accessing functionality across different
    /// exchange client libraries.
    /// </summary>
    public interface IBitMEXRestClientExchangeSharedApi :
        IGetAssetRest,
        IGetAllAssetsRest,
        IGetBalancesRest,
        IGetDepositAddressesRest,
        IGetDepositHistoryRest,
        IGetKlinesRest,
        IGetOrderBookRest,
        IGetRecentTradesRest,
        IGetTradeHistoryRest,
        IGetWithdrawalHistoryRest,
        IWithdrawRest,
        IGetSpotSymbolsRest,
        IGetSpotTickerRest,
        IGetAllSpotTickersRest,
        IPlaceSpotOrderRest,
        IGetSpotOrderRest,
        IGetOpenSpotOrdersRest,
        IGetClosedSpotOrdersRest,
        IGetSpotOrderTradesRest,
        IGetSpotUserTradeHistoryRest,
        ICancelSpotOrderRest,
        IGetFundingRateHistoryRest,
        IGetFuturesSymbolsRest,
        IGetFuturesTickerRest,
        IGetAllFuturesTickersRest,
        ISetLeverageRest,
        IGetLeverageRest,
        IGetOpenInterestRest,
        IPlaceFuturesOrderRest,
        IGetFuturesOrderRest,
        IGetOpenFuturesOrdersRest,
        IGetClosedFuturesOrdersRest,
        ICancelFuturesOrderRest,
        IClosePositionRest,
        IGetFuturesOrderTradesRest,
        IGetFuturesUserTradeHistoryRest,
        ICancelFuturesOrderByClientOrderIdRest,
        IGetFuturesOrderByClientOrderIdRest,
        IGetPositionsRest,
        IPlaceFuturesTriggerOrderRest,
        IGetFuturesTriggerOrderRest,
        ICancelFuturesTriggerOrderRest,
        IGetFeesRest,
        ISetFuturesTpSlRest,
        ICancelFuturesTpSlRest,
        IGetBookTickerRest,
        IPlaceSpotTriggerOrderRest,
        IGetSpotTriggerOrderRest,
        ICancelSpotTriggerOrderRest,
        ICancelSpotOrderByClientOrderIdRest
    { }
}
