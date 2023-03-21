using AiModel;
using Dapper;
using DataExporter.Scripts;
using Newtonsoft.Json.Linq;
using OWT.CryptoCom.Deciders;
using OWT.CryptoCom.Dto;
using System.Data;

namespace OWT.CryptoCom.ResponseHandlers
{
    public class TradingEthHandler8 : ICryptoComDtoExecutor
    {
        private readonly CryptoComBalanceDto _balanceDto;
        private readonly SecondPass _debouncePass;
        private readonly IDbConnection _connection;
        private readonly double _amountToTrade;
        private readonly ILogger<TradingEthHandler8> _logger;
        private readonly CryptoComPurchaseDecider _purchaseDecider;

        private const string SelectQuery = "SELECT TOP (1) a.[DateTime], a.[Actual], ( SELECT TOP (1) [Actual] FROM [dbo].[MarketStateSnaps] b WHERE DATEADD(second, -55, a.DateTime) < b.DateTime AND b.DateTime < DATEADD(second, -45, a.DateTime) ORDER BY b.DateTime DESC ) as Past50s, ( SELECT TOP (1) [Actual] FROM [dbo].[MarketStateSnaps] b WHERE DATEADD(second, -35, a.DateTime) < b.DateTime AND b.DateTime < DATEADD(second, -25, a.DateTime) ORDER BY b.DateTime DESC ) as Past30s, ( SELECT TOP (1) [Actual] FROM [dbo].[MarketStateSnaps] b WHERE DATEADD(second, -15, a.DateTime) < b.DateTime AND b.DateTime < DATEADD(second, -5, a.DateTime) ORDER BY b.DateTime DESC ) as Past10s FROM [dbo].[MarketStateSnaps] a ORDER BY a.[DateTime] DESC";

        public TradingEthHandler8(CryptoComBalanceDto balanceDto, SecondPass debouncePass, IDbConnection connection, ILogger<TradingEthHandler8> logger, CryptoComPurchaseDecider purchaseDecider)
        {
            _balanceDto = balanceDto;
            _debouncePass = debouncePass;
            _connection = connection;
            _logger = logger;
            _purchaseDecider = purchaseDecider;
            _amountToTrade = 0.001;
        }

        public bool CanExecute(JObject dto) => _debouncePass;

        public async Task Execute(JObject dto, CryptoComMarketClient marketClient, CancellationToken token)
        {
            var curPrice = await _connection.QueryFirstAsync<Script7.Script7DbData>(SelectQuery);

            if (curPrice.Past10s == null || curPrice.Past30s == null || curPrice.Past50s == null)
                return;

            var act = (float)curPrice.Actual;

            var sampleData = new Model7.ModelInput()
            {
                Col0 = (float)curPrice.Past50s - act,
                Col1 = (float)curPrice.Past30s - act,
                Col2 = (float)curPrice.Past10s - act,
                Col4 = curPrice.DateTime.Minute
            };

            var result = Model7.Predict(sampleData);

            _logger.LogInformation("Tried to trade when price is: {0}, expected {1}, trying to buy: {2} from {3} Usd, trying to sell {4} from {5} Eth",
                curPrice.Actual,
                result.Score,
                _amountToTrade * (double)curPrice.Actual,
                _balanceDto.Usd,
                _amountToTrade,
                _balanceDto.Eth);

            if (0 < result.Score
                && _amountToTrade * (double)curPrice.Actual < _balanceDto.Usd)
            {
                await _purchaseDecider.Buy(marketClient, token);
            }
            else if (0 > result.Score && _amountToTrade < _balanceDto.Eth)
            {
                await _purchaseDecider.Sell(marketClient, token);
            }
        }
    }
}
