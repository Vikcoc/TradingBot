using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Traders.CryptoCom.Data;
using Traders.CryptoCom.Dto;
using Traders.CryptoCom.Socket;
using Traders.Data;
using TradingWebSocket.Adapter;
using TradingWebSocket.BaseTrader;

namespace Traders.CryptoCom
{
    public class CryptoComTrader : ITrader
    {
        protected readonly CryptoComMarketAdapter MarketAdapter;
        protected readonly CryptoComUserAdapter UserAdapter;

        public Trades Trade { get; private set; }

        public CryptoComTrader(CryptoComMarketAdapter marketAdapter, CryptoComUserAdapter userAdapter)
        {
            MarketAdapter = marketAdapter;
            UserAdapter = userAdapter;
        }

        public async Task Start(Trades trade)
        {
            if (Trade != default)
                throw new NotSupportedException("Cannot reconfigure " + this.GetType().Name);
            Trade = trade;
            var tickerFactory = new CryptoComSubscriptionResponseFactory<CryptoComSubscriptionTickerData>(Trade);
            tickerFactory.OnValidObject += async response =>
            {
                if (PriceUpdate != null && response.Result != null)
                {
                    foreach (var data in response.Result.Where(data => data.Actual != null))
                    {
                        await PriceUpdate(data.Actual!.Value);
                    }
                }
            };
            MarketAdapter.AddSocketResponse(tickerFactory);
            await MarketAdapter.Send(new CryptoComSubscriptionRequest
            {
                Channels = new List<string>
                {
                    CryptoComMethods.Ticker + "." + CryptoComTrades.Trades[trade]
                }
            });
        }

        public event Func<double, Task>? PriceUpdate;
        public event Func<double, Task>? BuyAvailableUpdate;
        public event Func<double, Task>? SellAvailableUpdate;
        public double Price { get; set; }
        public double BuyAvailable { get; set; }
        public double SellAvailable { get; set; }
        public Task<bool> Buy(double amount)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Sell(double amount)
        {
            throw new NotImplementedException();
        }
    }
}
