using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using TradingWebSocket.BaseTrader;

namespace OpenAi
{
    public class TradingAlgorithm
    {
        private readonly ITrader _trader;
        private readonly Timer _timer;
        private readonly Queue<double> _priceData;
        private double _currentPrice;
        private double _buyThreshold;
        private double _sellThreshold;
        private double _amountToTrade;

        public TradingAlgorithm(ITrader trader)
        {
            _trader = trader;
            _trader.PriceUpdate += OnPriceUpdate;
            _timer = new Timer(60000); // Wait for one minute before setting the threshold values
            _timer.Elapsed += OnTimerElapsed;
            _priceData = new Queue<double>(400);
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
            _trader.PriceUpdate -= OnPriceUpdate;
        }

        private Task OnPriceUpdate(double price)
        {
            _currentPrice = price;
            _priceData.Enqueue(price);
            return Task.CompletedTask;
        }

        //private async Task OnPriceUpdate(double price)
        //{
        //    _currentPrice = price;

        //    if (price >= _buyThreshold)
        //    {
        //        await _trader.Buy(_amountToTrade);
        //    }
        //    else if (price <= _sellThreshold)
        //    {
        //        await _trader.Sell(_amountToTrade);
        //    }
        //}


        private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            // Calculate the 90th percentile of the collected data
            var highPercentile = CalculatePercentile(70);
            var lowPercentile = CalculatePercentile(30);

            // Initialize the threshold values using the calculated percentile
            _buyThreshold = lowPercentile;
            _sellThreshold = highPercentile;
            _amountToTrade = 1;
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