using Newtonsoft.Json;
using OWT.CryptoCom;
using OWT.CryptoCom.Dto;

namespace OWT.BackgroundService
{
    public class CryptoComDataCollector : Microsoft.Extensions.Hosting.BackgroundService
    {
        private readonly CryptoComMarketClient _marketClient;

        public CryptoComDataCollector(CryptoComMarketClient marketClient)
        {
            _marketClient = marketClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _marketClient.Connect(stoppingToken);
            var trans = new CryptoComParamTransaction
            {
                Method = "subscribe",
                Params = new Dictionary<string, object>
                {
                    {"channels", new string[] {"ticker.ETH_USDT"}}
                }
            };
            await _marketClient.Send(JsonConvert.SerializeObject(trans), stoppingToken);
            while (!stoppingToken.IsCancellationRequested)
            {
                var dto = await _marketClient.Receive(stoppingToken);
                Console.WriteLine(dto);
            }
        }
    }
}
