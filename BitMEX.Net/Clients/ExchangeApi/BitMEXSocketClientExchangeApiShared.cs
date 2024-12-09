using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Text;
using BitMEX.Net.Interfaces.Clients.ExchangeApi;

namespace BitMEX.Net.Clients.ExchangeApi
{
    internal partial class BitMEXSocketClientExchangeApi : IBitMEXSocketClientExchangeApiShared
    {
        public string Exchange => "BitMEX";

        public TradingMode[] SupportedTradingModes => new[] { TradingMode.Spot };

        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();
    }
}
