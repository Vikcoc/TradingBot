using AiModel;
using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OWT.CryptoCom.Dto;
using System.Data;

namespace OWT.CryptoCom.ResponseHandlers
{
    public class TradingEthHandler4 : ICryptoComDtoExecutor
    {
        private readonly CryptoComBalanceDto _balanceDto;
        private readonly SecondPass _debouncePass;
        private readonly IDbConnection _connection;
        private readonly double _amountToTrade;
        private readonly ILogger<TradingEthHandler4> _logger;

        private const string SelectQuery = "SELECT TOP (1) a.[DateTime], a.[BestBid], a.[BestAsk], a.[Actual], a.[Low], a.[High], ( SELECT TOP (1) [Actual] FROM [dbo].[MarketStateSnaps] b WHERE DATEADD(second, -55, a.DateTime) < b.DateTime AND b.DateTime < DATEADD(second, -45, a.DateTime) ORDER BY b.DateTime DESC ) as Past50s, ( SELECT TOP (1) [Actual] FROM [dbo].[MarketStateSnaps] b WHERE DATEADD(second, -35, a.DateTime) < b.DateTime AND b.DateTime < DATEADD(second, -25, a.DateTime) ORDER BY b.DateTime DESC ) as Past30s, ( SELECT TOP (1) [Actual] FROM [dbo].[MarketStateSnaps] b WHERE DATEADD(second, -15, a.DateTime) < b.DateTime AND b.DateTime < DATEADD(second, -5, a.DateTime) ORDER BY b.DateTime DESC ) as Past10s FROM [dbo].[MarketStateSnaps] a ORDER BY a.DateTime DESC";

        public TradingEthHandler4(CryptoComBalanceDto balanceDto, SecondPass debouncePass, IDbConnection connection, ILogger<TradingEthHandler4> logger)
        {
            _balanceDto = balanceDto;
            _debouncePass = debouncePass;
            _connection = connection;
            _logger = logger;
            _amountToTrade = 0.002;
        }

        public bool CanExecute(JObject dto) => _debouncePass;

        public async Task Execute(JObject dto, CryptoComMarketClient marketClient, CancellationToken token)
        {
            var curPrice = await _connection.QueryFirstAsync<dynamic>(SelectQuery);
            var date = (DateTime)curPrice.DateTime;

            if(curPrice.Past10s == null || curPrice.Past30s == null || curPrice.Past50s == null)
                return;

            var sampleData = new Test3.ModelInput()
            {
                CurrentValue = (float)curPrice.Actual,
                BestBid = (float)curPrice.BestBid,
                BestAsk = (float)curPrice.BestAsk,
                LowOfTheDay = (float)curPrice.Low,
                HighOfTheDay = (float)curPrice.High,
                DayOfMonth = date.Day,
                DayOfWeek = date.DayOfWeek.ToString(),
                Hour = date.Hour,
                Minute = date.Minute,
                Second = date.Second,
                Millisecond = date.Millisecond,
                Past10s = (float)curPrice.Past10s,
                Past30s = (float)curPrice.Past30s,
                Past50s = (float)curPrice.Past50s
            };
            
            var result = Test3.Predict(sampleData);

            _logger.LogInformation("Tried to trade when price is: {0}, expected {1}, trying to buy: {2} from {3} Usd, trying to sell {4} from {5} Eth",
                (double)((IDictionary<string, object>)curPrice)["Actual"],
                result.Score,
                _amountToTrade * (double)((IDictionary<string, object>)curPrice)["Actual"],
                _balanceDto.Usd,
                _amountToTrade,
                _balanceDto.Eth);

            if ((double)((IDictionary<string, object>)curPrice)["Actual"] < result.Score
                && _amountToTrade * (double)((IDictionary<string, object>)curPrice)["Actual"] < _balanceDto.Usd)
            {
                var trans = new CryptoComParamTransaction
                {
                    Method = "private/create-order",
                    Params = new Dictionary<string, object>
                    {
                        { "instrument_name", "ETH_USDT" },
                        { "side", "BUY" },
                        { "type", "MARKET" },
                        { "quantity", _amountToTrade },
                    }
                };
                await marketClient.Send(JsonConvert.SerializeObject(trans), token);
            }
            else if ((double)((IDictionary<string, object>)curPrice)["Actual"] > result.Score && _amountToTrade < _balanceDto.Eth)
            {
                var trans = new CryptoComParamTransaction
                {
                    Method = "private/create-order",
                    Params = new Dictionary<string, object>
                    {
                        { "instrument_name", "ETH_USDT" },
                        { "side", "SELL" },
                        { "type", "MARKET" },
                        { "quantity", _amountToTrade },
                    }
                };
                await marketClient.Send(JsonConvert.SerializeObject(trans), token);
            }
        }
    }
}
