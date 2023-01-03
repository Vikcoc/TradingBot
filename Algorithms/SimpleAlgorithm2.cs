using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using TradingWebSocket.BaseTrader;

namespace Algorithms
{
    public class SimpleAlgorithm2
    {
        private readonly ITrader _trader;
        private readonly Timer _calibrateTimer;
        private readonly Timer _tradeTimer;
        private readonly Queue<double> _priceData;
        private readonly double _amountToTrade;

        public SimpleAlgorithm2(ITrader trader)
        {
            _trader = trader;
            _amountToTrade = 0.001;

            _calibrateTimer = new Timer(60000);
            _calibrateTimer.Elapsed += StartTradeTimer;

            _tradeTimer = new Timer(10000);
            _tradeTimer.Elapsed += OnTradeTimerElapsed;

            _priceData = new Queue<double>(800);
        }

        public async Task StartAlgorithm()
        {
            _trader.PriceUpdate += OnPriceUpdate;
            await _trader.Start(Trades.EthUsd);
            _calibrateTimer.Start();
        }

        public void StopAlgorithm()
        {
            _trader.PriceUpdate -= OnPriceUpdate;
            _calibrateTimer.Stop();
            _tradeTimer.Stop();
        }

        private Task OnPriceUpdate(double price)
        {
            _priceData.Enqueue(price);
            return Task.CompletedTask;
        }

        private Task OnPriceUpdate2(double price)
        {
            _priceData.Dequeue();
            return Task.CompletedTask;
        }

        private (double low, double high) CalculatePercentile(int low, int high)
        {
            // Sort the collected data
            var dataArray = _priceData.ToArray();
            var data = dataArray.OrderBy(x => x);

            // Calculate the index of the desired percentile value
            var indexL = (low / 100.0) * dataArray.Length;
            var indexH = (high / 100.0) * dataArray.Length;

            // Return the value at the calculated index
            return (data.ElementAt((int)indexL), data.ElementAt((int)indexH));
        }

        private async void OnTradeTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            var (lowPercentile, highPercentile) = CalculatePercentile(30, 70);

            Console.WriteLine("Low Price: {0}, High Price: {1}", lowPercentile, highPercentile);

            Console.WriteLine("Try trade");
            if (_trader.Price > lowPercentile && _amountToTrade * _trader.Price < _trader.SellAvailable)
            {
                //var x = _amountToTrade * _trader.Price > _trader.SellAvailable;
                Console.WriteLine("Try buy {0}", _amountToTrade);
                await _trader.Buy(_amountToTrade);
            }
            else if (_trader.Price < highPercentile && _trader.BuyAvailable > _amountToTrade)
            {
                //var y = _trader.BuyAvailable > _amountToTrade;
                Console.WriteLine("Try sell {0}", _amountToTrade);
                await _trader.Sell(_amountToTrade);
            }
        }

        private void StartTradeTimer(object? sender, ElapsedEventArgs e)
        {
            _calibrateTimer.Stop();
            _tradeTimer.Start();
            _trader.PriceUpdate += OnPriceUpdate2;
        }
    }
}
