using System.Collections.Generic;
using Traders.Data;

namespace Traders.CryptoCom.Data
{
    public static class CryptoComTrades
    {
        public static Dictionary<Trades, string> Trades { get; }

        static CryptoComTrades()
        {
            Trades = new Dictionary<Trades, string>
            {
                { Traders.Data.Trades.BtcUsd, "BTC_USDT" }
            };
        }
    }
}
