using Algorithms;
using Microsoft.AspNetCore.Mvc;
using TradingWebSocket.BaseTrader;
using Timer = System.Timers.Timer;

namespace ClientApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlgorithmController : ControllerBase
    {
        private readonly ITrader _trader;
        private readonly SimpleAlgorithm2 _algorithm;
        private Timer _clsTimer;

        public AlgorithmController(ITrader trader, SimpleAlgorithm2 algorithm)
        {
            _trader = trader;
            _algorithm = algorithm;
            
        }

        [HttpGet("Start")]
        public async Task<IActionResult> Start()
        {
            _trader.PriceUpdate += d =>
            {
                Console.WriteLine("Price: {0}, Buy: {1}, Sell: {2}", d, _trader.BuyAvailable, _trader.SellAvailable);
                return Task.CompletedTask;
            };
            //_trader.BuyAvailableUpdate += d =>
            //{
            //    Console.WriteLine("Buy: " + d);
            //    return Task.CompletedTask;
            //};
            //_trader.SellAvailableUpdate += d =>
            //{
            //    Console.WriteLine("Sell: " + d);
            //    return Task.CompletedTask;
            //};
            await _algorithm.StartAlgorithm();
            _clsTimer = new Timer(200000);
            _clsTimer.Elapsed += (sender, args) =>
            {
                Console.Clear();
            };
            _clsTimer.Start();
            return Ok();
        }
    }
}
