using System.Data;
using Dapper;
using Newtonsoft.Json.Linq;
using static OWT.MLModel;

namespace OWT.CryptoCom.ResponseHandlers;

public class TickerWithEstimationHandler : ICryptoComDtoExecutor
{
    private const string SelectQuery =
        "SELECT TOP (1) [Actual] AS Value FROM [MarketStateSnaps] ORDER BY [DateTime] DESC";

    private readonly IDbConnection _connection;
    private readonly ILogger<TickerWithEstimationHandler> _logger;

    public TickerWithEstimationHandler(ILogger<TickerWithEstimationHandler> logger, IDbConnection connection)
    {
        _logger = logger;
        _connection = connection;
    }

    public bool CanExecute(JObject dto)
    {
        return dto["method"]?.ToString() == "subscribe" && dto["result"]?["channel"]?.ToString() == "ticker";
    }

    public Task Execute(JObject dto, CryptoComMarketClient marketClient, CancellationToken token)
    {
        var val = dto["result"]?["data"]?.Values<JObject>().FirstOrDefault()["a"]?.Value<float>();
        var prev = _connection.Query<float?>(SelectQuery).FirstOrDefault();
        if (val == prev)
            return Task.CompletedTask;
        var req = new ModelInput
        {
            Col3 = val ?? 0
        };
        var res = Predict(req, 1);

        _logger.Log(req.Col3 < res.Col3.Last() ? LogLevel.Information : LogLevel.Warning,
            "Current: {0}, Prediction: {1}, PredictionLow: {2}, PredictionHigh: {3}", req.Col3, res.Col3.Last(),
            res.Col3_LB.Last(), res.Col3_UB.Last());
        return Task.CompletedTask;
    }
}