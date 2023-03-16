using Dapper;
using Newtonsoft.Json.Linq;
using OWT.CryptoCom.Dto;
using System.Data;
using AiModel;
using Newtonsoft.Json;

namespace OWT.CryptoCom.ResponseHandlers {
    public class TradingEthHandler3 : ICryptoComDtoExecutor {
        private readonly CryptoComBalanceDto _balanceDto;
        private readonly SecondPass _debouncePass;
        private readonly IDbConnection _connection;
        private readonly double _amountToTrade;

        public TradingEthHandler3(CryptoComBalanceDto balanceDto, SecondPass debouncePass, IDbConnection connection)
        {
            _balanceDto = balanceDto;
            _debouncePass = debouncePass;
            _connection = connection;
            _amountToTrade = 0.002;
        }

        public bool CanExecute(JObject dto) => _debouncePass;

        public async Task Execute(JObject dto, CryptoComMarketClient marketClient, CancellationToken token)
        {
            var curPrice = await _connection.QueryFirstAsync<dynamic>("SELECT TOP(1) [DateTime], [BestBid], [BestAsk], [Actual], [Low], [High] FROM [MarketStateSnaps] ORDER BY [DateTime] DESC");
            var date = (DateTime)curPrice.DateTime;

            //Load sample data
            var sampleData = new Test2.ModelInput() {
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
            };

            //Load model and predict output
            var result = Test2.Predict(sampleData);
            if ((double)((IDictionary<string, object>)curPrice)["Actual"] < result.Score
                && _amountToTrade * (double)((IDictionary<string, object>)curPrice)["Actual"] < _balanceDto.Usd) {
                var trans = new CryptoComParamTransaction {
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
            else if ((double)((IDictionary<string, object>)curPrice)["Actual"] > result.Score && _amountToTrade < _balanceDto.Eth) {
                var trans = new CryptoComParamTransaction {
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
