using System;
using System.Threading.Tasks;
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

        private Trades _trade;

        public Trades Trade => _trade;

        private Task SeePublicPrice()
        {
            return Task.CompletedTask;
        }

        public CryptoComTrader(CryptoComMarketAdapter marketAdapter, CryptoComUserAdapter userAdapter)
        {
            MarketAdapter = marketAdapter;
            UserAdapter = userAdapter;
        }

        public async Task Configure(Trades trade)
        {
            if (_trade != default)
                throw new NotSupportedException("Cannot reconfigure " + this.GetType().Name);
            _trade = trade;
            MarketAdapter.AddResponseCallback<>();
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
