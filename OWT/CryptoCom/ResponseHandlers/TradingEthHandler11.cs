using AiModel;
using DataExporter.Scripts;
using Newtonsoft.Json.Linq;
using OWT.CryptoCom.Deciders;
using OWT.CryptoCom.Dto;
using System.Data;
using Dapper;

namespace OWT.CryptoCom.ResponseHandlers
{
    public class TradingEthHandler11 : ICryptoComDtoExecutor
    {
        private readonly CryptoComBalanceDto _balanceDto;
        private readonly SecondPass _debouncePass;
        private readonly IDbConnection _connection;
        private readonly double _amountToTrade;
        private readonly ILogger<TradingEthHandler8> _logger;
        private readonly CryptoComPurchaseDecider _purchaseDecider;

        private const string SelectQuery = "SELECT TOP (51) a.[DateTime], a.[Actual] FROM [dbo].[MarketStateSnaps] a ORDER BY a.[DateTime] DESC";

        public TradingEthHandler11(CryptoComBalanceDto balanceDto, SecondPass debouncePass, IDbConnection connection, ILogger<TradingEthHandler8> logger, CryptoComPurchaseDecider purchaseDecider)
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
            var curPrice = await _connection.QueryAsync<Script9.DbData>(SelectQuery);

            var cur = (float)curPrice.First().Actual;

            var sampleData = new Averages.ModelInput()
            {
                Col0 = (float)curPrice.Skip(1).Average(x => x.Actual),
                Col1 = cur,
            };

            var result = Averages.Predict(sampleData);

            if (Math.Abs(result.Score - cur) < 0.3)
            {
                _logger.LogInformation("Cannot trade from price: {0} with expected: {1} and difference: {2}", cur,
                    result.Score, result.Score - cur);
                return;
            }

            _logger.LogInformation("Tried to trade when price is: {0}, expected {1}, trying to buy: {2} from {3} Usd, trying to sell {4} from {5} Eth",
                cur,
                result.Score,
                _amountToTrade * cur,
                _balanceDto.Usd,
                _amountToTrade,
                _balanceDto.Eth);

            if (cur < result.Score
                && _amountToTrade * cur + 0.01 < _balanceDto.Usd)
            {
                await _purchaseDecider.Buy(marketClient, token);
            }
            else if (cur > result.Score && _amountToTrade < _balanceDto.Eth)
            {
                await _purchaseDecider.Sell(marketClient, token);
            }
        }
    }
}
