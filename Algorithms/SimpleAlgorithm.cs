using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using TradingWebSocket.BaseTrader;
using Timer = System.Timers.Timer;

namespace Algorithms
{
    public class SimpleAlgorithm
    {
        private readonly ITrader _trader;
        private readonly Timer _calibrateTimer;
        private readonly Timer _tradeTimer;
        private readonly Queue<double> _priceData;
        private double _buyThreshold;
        private double _sellThreshold;
        private readonly double _amountToTrade;

        public SimpleAlgorithm(ITrader trader)
        {
            _trader = trader;
            
            _calibrateTimer = new Timer(60000);
            _calibrateTimer.Elapsed += OnCalibrateTimerElapsed;
            _calibrateTimer.Elapsed += StartTradeTimer;

            _tradeTimer = new Timer(5000);
            _tradeTimer.Elapsed += OnTradeTimerElapsed;

            _priceData = new Queue<double>(800);

            _trader.PriceUpdate += OnPriceUpdate;

            _amountToTrade = 0.00001;
        }

        public void StartAlgorithm()
        {
            _tradeTimer.Start();
            _calibrateTimer.Start();
        }

        public void StopAlgorithm()
        {

            _calibrateTimer.Stop();
            _tradeTimer.Stop();
        }

        private void StartTradeTimer(object? sender, ElapsedEventArgs e)
        {
            _calibrateTimer.Elapsed -= StartTradeTimer;
            _trader.PriceUpdate += OnPriceUpdate2;
            _tradeTimer.Start();
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

        private async void OnTradeTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            if (_trader.Price >= _buyThreshold)
            {
                await _trader.Buy(_amountToTrade);
            }
            else if (_trader.Price <= _sellThreshold)
            {
                await _trader.Sell(_amountToTrade);
            }
        }

        private void OnCalibrateTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            // Calculate the 90th percentile of the collected data
            var highPercentile = CalculatePercentile(70);
            var lowPercentile = CalculatePercentile(30);

            // Initialize the threshold values using the calculated percentile
            _buyThreshold = lowPercentile;
            _sellThreshold = highPercentile;
        }

        private double CalculatePercentile(int percentile)
        {
            // Sort the collected data
            var dataArray = _priceData.ToArray();
            var data = dataArray.OrderBy(x => x);

            // Calculate the index of the desired percentile value
            var index = (percentile / 100.0) * dataArray.Length;

            // Return the value at the calculated index
            return data.ElementAt((int)index);
        }
    }
}