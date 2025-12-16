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
}
