using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocketTests;
using TransactionMaker;

namespace ClientApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly TransactionSigner.TransactionSigner _signer;
        private readonly HttpClient _client;

        public WeatherForecastController(IConfiguration configuration, TransactionSigner.TransactionSigner signer, HttpClient client)
        {
            _configuration = configuration;
            _signer = signer;
            _client = client;
        }

        [HttpGet("GetMills")]
        public long GetMillis()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        [HttpGet("GetSample")]
        public async Task<string> GetSample()
        {
            var temp = new TransactionTemplate
            {
                Method = "private/get-account-summary",
                ApiKey = _configuration["ApiKey"],
                Params = new Dictionary<string, string>
                {
                    //{"currency", "USDT"},
                    {"currency", "BTC"}
                }
            };
            temp.Signature = _signer.GetSign(temp);
            var bod = JsonConvert.SerializeObject(temp);
            Console.WriteLine(bod);
            var req = new StringContent(bod);
            req.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var res = await _client.PostAsync(temp.Method, req);
            return await res.Content.ReadAsStringAsync();
        }

        [HttpGet("connect")]
        public IActionResult Connect()
        {
            Class1.StartClient();
            return Ok();
        }

        [HttpGet("connectCrypto")]
        public IActionResult ConnectCrypto()
        {
            Class1.Web5(_configuration["ApiKey"], _signer);
            return Ok();
        }
        [HttpPost("connectCrypto")]
        public IActionResult ConnectCrypto(string arg)
        {
            Class1.Web4(arg, _configuration["ApiKey"], _signer);
            return Ok();
        }

        [HttpGet("disconnectCrypto")]
        public IActionResult DisconnectCrypto()
        {
            Class1.StopForCrypto();
            return Ok();
        }

        [HttpGet("connectCryptoMarkets")]
        public IActionResult ConnectCryptoMarkets()
        {
            Class1.Web6();
            return Ok();
        }
    }
}