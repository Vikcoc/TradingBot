using System;
using System.Threading.Tasks;

namespace TradingWebSocket.BaseTrader
{
    public interface ITrader
    {
        public event Func<double, Task> PriceUpdate;
        public event Func<double, Task> BuyAvailableUpdate;
        public event Func<double, Task> SellAvailableUpdate;
        public double Price { get; set; }
        public double BuyAvailable { get; set; }
        public double SellAvailable { get; set; }
        public Task<bool> Buy(double amount);
        public Task<bool> Sell(double amount);
    }
}
