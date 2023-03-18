using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OWT.CryptoCom.Deciders;
using OWT.CryptoCom.Dto;
using OWT.CryptoCom.ResponseHandlers;

namespace OWT.CryptoCom.BackgroundService;

public class CryptoComUserCollector : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly CryptoComUserClient _userClient;

    public CryptoComUserCollector(IServiceProvider serviceProvider, CryptoComUserClient userClient)
    {
        _serviceProvider = serviceProvider;
        _userClient = userClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _userClient.Connect(stoppingToken);
        await Task.Delay(1000, stoppingToken);
        await _userClient.Authenticate(stoppingToken);
        await Task.Delay(1000, stoppingToken);
        var trans = new CryptoComParamTransaction
        {
            Method = "subscribe",
            Params = new Dictionary<string, object>
            {
                { "channels", new[] { "user.balance" } }
            }
        };
        await _userClient.Send(JsonConvert.SerializeObject(trans), stoppingToken);
        var tim = DateTime.UtcNow;
        while (!stoppingToken.IsCancellationRequested)
        {
            var dto = await _userClient.Receive(stoppingToken);
            using (var scope = _serviceProvider.CreateScope())
            {
                var nTim = DateTime.UtcNow;
                var sPass = scope.ServiceProvider.GetRequiredService<SecondPass>();
                sPass.Passed = (nTim - tim) >= TimeSpan.FromSeconds(5);
                if (sPass)
                    tim = nTim;

                var decider = scope.ServiceProvider.GetRequiredService<CryptoComUserDtoDecider>();
                var val = await decider.Execute(JsonConvert.DeserializeObject<JObject>(dto), _userClient,
                    stoppingToken);

                //var toWrite = scope.ServiceProvider.GetRequiredService<BalanceHandler>();
                //if(toWrite.CanExecute(JsonConvert.DeserializeObject<JObject>(dto)))
                //    continue;
                //Console.WriteLine(dto);
            }
        }
    }
}