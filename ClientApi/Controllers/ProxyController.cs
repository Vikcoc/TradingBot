using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TraderProxy;
using TradingWebSocket.BaseTrader;

namespace ClientApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProxyController : ControllerBase
    {
        private readonly ProxyTrader _trader;

        public ProxyController(ProxyTrader trader)
        {
            _trader = trader;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await _trader.Start(Trades.EthUsd);
            return Ok();
        }
    }
}
