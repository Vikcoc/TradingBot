using AiModel;
using DataExporter.Scripts;
using Newtonsoft.Json.Linq;
using OWT.CryptoCom.Deciders;
using OWT.CryptoCom.Dto;
using System.Data;
using Dapper;

namespace OWT.CryptoCom.ResponseHandlers
{
    public class TradingEthHandler9 : ICryptoComDtoExecutor
    {
        private readonly CryptoComBalanceDto _balanceDto;
        private readonly SecondPass _debouncePass;
        private readonly IDbConnection _connection;
        private readonly double _amountToTrade;
        private readonly ILogger<TradingEthHandler8> _logger;
        private readonly CryptoComPurchaseDecider _purchaseDecider;

        private const string SelectQuery = "SELECT TOP (1) a.[DateTime], a.[Actual], ( SELECT TOP (1) [Actual] FROM [dbo].[MarketStateSnaps] b WHERE DATEADD(second, -55, a.DateTime) < b.DateTime AND b.DateTime < DATEADD(second, -45, a.DateTime) ORDER BY b.DateTime DESC ) as Past50s, ( SELECT TOP (1) [Actual] FROM [dbo].[MarketStateSnaps] b WHERE DATEADD(second, -35, a.DateTime) < b.DateTime AND b.DateTime < DATEADD(second, -25, a.DateTime) ORDER BY b.DateTime DESC ) as Past30s, ( SELECT TOP (1) [Actual] FROM [dbo].[MarketStateSnaps] b WHERE DATEADD(second, -15, a.DateTime) < b.DateTime AND b.DateTime < DATEADD(second, -5, a.DateTime) ORDER BY b.DateTime DESC ) as Past10s FROM [dbo].[MarketStateSnaps] a ORDER BY a.[DateTime] DESC";

        public TradingEthHandler9(CryptoComBalanceDto balanceDto, SecondPass debouncePass, IDbConnection connection, ILogger<TradingEthHandler8> logger, CryptoComPurchaseDecider purchaseDecider)
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
            var curPrice = await _connection.QueryFirstAsync<Script8.Script8DbData>(SelectQuery);

            if (curPrice.Past10s == null || curPrice.Past30s == null || curPrice.Past50s == null)
                return;

            var sampleData = new MinMaxNorm.ModelInput()
            {
                Col0 = MinMaxNorm.Normalize((float)curPrice.Actual),
                Col1 = MinMaxNorm.Normalize((float)curPrice.Past50s),
                Col2 = MinMaxNorm.Normalize((float)curPrice.Past30s),
                Col3 = MinMaxNorm.Normalize((float)curPrice.Past10s)
            };

            var result = MinMaxNorm.Predict(sampleData);

            _logger.LogInformation("Tried to trade when price is: {0}, normalized: {1}, expected {2}, trying to buy: {3} from {4} Usd, trying to sell {5} from {6} Eth",
                curPrice.Actual,
                MinMaxNorm.Normalize((float)curPrice.Actual),
                result.Score,
                _amountToTrade * (double)curPrice.Actual,
                _balanceDto.Usd,
                _amountToTrade,
                _balanceDto.Eth);

            if (MinMaxNorm.Normalize((float)curPrice.Actual) > result.Score
                                     && _amountToTrade * (double)curPrice.Actual < _balanceDto.Usd)
            {
                await _purchaseDecider.Buy(marketClient, token, _amountToTrade);
            }
            else if (MinMaxNorm.Normalize((float)curPrice.Actual) < result.Score && _amountToTrade < _balanceDto.Eth)
            {
                await _purchaseDecider.Sell(marketClient, token, _amountToTrade);
            }
        }
    }
}
