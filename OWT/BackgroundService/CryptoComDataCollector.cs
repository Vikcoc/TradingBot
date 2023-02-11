using OWT.CryptoCom;

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
            await _marketClient.Send("", stoppingToken);
            while (!stoppingToken.IsCancellationRequested)
            {
                var dto = await _marketClient.Receive(stoppingToken);
                Console.WriteLine(dto);
            }
        }
    }
}
