using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OWT.CryptoCom.Deciders;
using OWT.CryptoCom.Dto;

namespace OWT.CryptoCom.BackgroundService;

public class CryptoComDataCollector : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly CryptoComMarketClient _marketClient;
    private readonly IServiceProvider _serviceProvider;

    public CryptoComDataCollector(CryptoComMarketClient marketClient, IServiceProvider serviceProvider)
    {
        _marketClient = marketClient;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _marketClient.Connect(stoppingToken);
        Debug.WriteLine("Connected");
        Console.WriteLine("Connected");
        var trans = new CryptoComParamTransaction
        {
            Method = "subscribe",
            Params = new Dictionary<string, object>
            {
                { "channels", new[] { "ticker.ETH_USDT" } }
            }
        };
        await _marketClient.Send(JsonConvert.SerializeObject(trans), stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
        {
            var dto = await _marketClient.Receive(stoppingToken);
            using (var scope = _serviceProvider.CreateScope())
            {
                var decider = scope.ServiceProvider.GetRequiredService<CryptoComMarketDtoDecider>();
                var val = decider.Execute(JsonConvert.DeserializeObject<JObject>(dto), _marketClient,
                    stoppingToken);
                //Console.WriteLine("{0} handlers have been used", val);
            }
            //Console.WriteLine(dto);
        }
    }

    public async Task Send(string dto, CancellationToken stoppingToken)
    {
        await _marketClient.Send(dto, stoppingToken);
    }
}