using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebSocketFlow.Dto;
using WebSocketFlow.DtoInterfaces;
using WebSocketFlow.SocketAdapter;

namespace ClientApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocketController : ControllerBase
    {
        private readonly MarketAdapter _adapter= new MarketAdapter(new SocketAdapter());


        [HttpGet]
        public async Task<IActionResult> Debug()
        {
            var x = new SocketAdapter();
            await x.Connect("wss://stream.crypto.com/v2/user");
            await Task.Delay(1000);
            x.OnReceive += async a =>
            {
                Console.WriteLine(a);
                var res = JsonConvert.DeserializeObject<Heartbeat>(a)!;
                if(res.IsValid())
                {
                    res.Method = "public/respond-heartbeat";
                    await x.Send(((ITransactional)res).GetBody());
                }
            };
            await x.StartListening();
            return Ok();
        }

        [HttpGet("StartListening")]
        public async Task<IActionResult>Debug2()
        {
            await _adapter.ConnectAndListen();
            return Ok();
        }

        [HttpGet("StopListening")]
        public async Task<IActionResult> Debug3()
        {
            await _adapter.Disconnect();
            return Ok();
        }
    }
}
