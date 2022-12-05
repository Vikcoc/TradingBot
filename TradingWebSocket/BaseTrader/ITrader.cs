using System;
using System.Threading.Tasks;

namespace TradingWebSocket.BaseTrader
{
    /// <summary>
    /// Basic trader interface to allow for development of algorithms
    /// </summary>
    public interface ITrader
    {
        /// <summary>
        /// Event to take action on the exchange rate update
        /// </summary>
        public event Func<double, Task> PriceUpdate;
        /// <summary>
        /// Event to take action on the available funds to buy update
        /// </summary>
        public event Func<double, Task> BuyAvailableUpdate; //todo change names to reflect which currency it refers to
        /// <summary>
        /// Event to take action on the available funds to sell rate update
        /// </summary>
        public event Func<double, Task> SellAvailableUpdate;
        /// <summary>
        /// Get the current exchange rate
        /// </summary>
        public double Price { get; }
        /// <summary>
        /// Get the current available funds to buy
        /// </summary>
        public double BuyAvailable { get; }
        /// <summary>
        /// Get the current available funds to sell
        /// </summary>
        public double SellAvailable { get; }
        /// <summary>
        /// Add transaction to buy first currency
        /// </summary>
        /// <param name="amount"> How much of the first currency to buy </param>
        /// <returns></returns>
        public Task<bool> Buy(double amount);//todo change names to reflect which currency it refers to 
        /// <summary>
        /// Add transaction to sell first currency
        /// </summary>
        /// <param name="amount"> How much of the first currency to sell </param>
        /// <returns></returns>
        public Task<bool> Sell(double amount);
        /// <summary>
        /// What instrument are trades being performed on
        /// </summary>
        public Trades Trade { get; }
    }
}
