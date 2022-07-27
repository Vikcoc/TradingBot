using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public async Task<HttpResponseMessage> GetSample()
        {
            var temp = new TransactionTemplate
            {
                Method = "private/get-currency-networks",
                ApiKey = _configuration["ApiKey"]
            };
            temp.Signature = _signer.GetSign(temp);
            var res = await _client.PostAsync(temp.Method, new StringContent(JsonConvert.SerializeObject(temp)));
            return res;
        }
    }
}