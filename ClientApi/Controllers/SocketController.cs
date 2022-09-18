using Microsoft.AspNetCore.Mvc;
using WebSocketFlow.Dto;
using WebSocketFlow.Extra;
using WebSocketFlow.SocketAdapter;

namespace ClientApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocketController : ControllerBase
    {
        private readonly IMarketAdapter _adapter;
        private readonly IUserAdapter _adapter2;


        public SocketController(IMarketAdapter adapter, IUserAdapter adapter2)
        {
            _adapter = adapter;
            _adapter2 = adapter2;
        }

        [HttpGet("StartListening")]
        public async Task<IActionResult>Debug2()
        {
            await _adapter.ConnectAndListen();
            await _adapter.Send(new TickerRequestDto
            {
                Tickers = new List<string>
                {
                    Tickers.BtcUsd
                }
            });
            return Ok();
        }

        [HttpGet("StopListening")]
        public async Task<IActionResult> Debug3()
        {
            await _adapter.Disconnect();
            return Ok();
        }

        [HttpGet("UserStartListening")]
        public async Task<IActionResult> Debug4()
        {
            await _adapter2.ConnectListenAndAuthenticate();
            return Ok();
        }

        [HttpGet("UserStopListening")]
        public async Task<IActionResult> Debug5()
        {
            await _adapter2.Disconnect();
            return Ok();
        }
    }
}
