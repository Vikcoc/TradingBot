using Microsoft.AspNetCore.Mvc;
using WebSocketFlow.Extra;
using WebSocketFlow.SocketAdapter;
using WebSocketFlow.Subscription.Request;
using WebSocketFlow.Subscription.Response;
using WebSocketFlow.Subscription.Response.SubscriptionData;

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
            _adapter.AddResponseCallback((Func<SubscriptionResponseDto<BtcSubscriptionDto<TickerSubscriptionDataDto>, TickerSubscriptionDataDto>, Task>)(call =>
            {
                Console.WriteLine(call.Result?.Data?.First().PartChange);
                return Task.CompletedTask;
            }));
            _adapter.AddResponseCallback((Func<SubscriptionResponseDto<BtcSubscriptionDto<TradeSubscriptionDataDto>, TradeSubscriptionDataDto>, Task>)(call =>
            {
                Console.WriteLine(call.Result?.Data?.First().Action);
                return Task.CompletedTask;
            }));
            await _adapter.Send(new SubscriptionRequestDto
            {
                Channels = new List<string>
                {
                    Tickers.BtcUsd,
                    Trades.BtcUsd
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
