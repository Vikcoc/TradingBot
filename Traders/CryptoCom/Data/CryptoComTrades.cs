using System.Collections.Generic;
using TradingWebSocket.BaseTrader;

namespace Traders.CryptoCom.Data
{
    public static class CryptoComTrades
    {
        public static Dictionary<Trades, CryptoComTradeStrings> Trades { get; }

        static CryptoComTrades()
        {
            Trades = new Dictionary<Trades, CryptoComTradeStrings>
            {
                { TradingWebSocket.BaseTrader.Trades.BtcUsd,
                    new CryptoComTradeStrings
                    {
                        FirstCurrency = "BTC",
                        SecondCurrency = "USDT",
                        Trade = "BTC_USDT"
                    }
                },
                { TradingWebSocket.BaseTrader.Trades.EthUsd,
                    new CryptoComTradeStrings
                    {
                        FirstCurrency = "ETH",
                        SecondCurrency = "USDT",
                        Trade = "ETH_USDT"
                    }
                }
            };
        }

        public class CryptoComTradeStrings
        {
            public string Trade { get; set;} = string.Empty;
            public string FirstCurrency { get; set;} = string.Empty;
            public string SecondCurrency { get; set;} = string.Empty;
        }
    }
}
