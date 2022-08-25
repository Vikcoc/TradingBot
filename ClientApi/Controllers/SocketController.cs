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
        [HttpGet]
        public async Task<IActionResult> Debug()
        {
            var x = new SocketAdapter();
            await x.Connect();
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
    }
}
