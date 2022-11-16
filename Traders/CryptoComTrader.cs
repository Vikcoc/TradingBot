using System;
using System.Threading.Tasks;
using Traders.Data;
using TradingWebSocket.Adapter;
using TradingWebSocket.BaseTrader;

namespace Traders
{
    public class CryptoComTrader : ITrader
    {
        protected readonly IMarketAdapter MarketAdapter;
        protected readonly IUserAdapter UserAdapter;

        private Trades _trade;

        public Trades Trade => _trade;

        private Task SeePublicPrice()
        {
            return Task.CompletedTask;
        }

        public CryptoComTrader(IMarketAdapter marketAdapter, IUserAdapter userAdapter)
        {
            MarketAdapter = marketAdapter;
            UserAdapter = userAdapter;
        }

        //public async Task Configure(Trades trade)
        //{
        //    _trade = trade;
        //    MarketAdapter.AddResponseCallback();
        //}

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
