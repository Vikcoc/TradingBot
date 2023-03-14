using Dapper;
using Newtonsoft.Json.Linq;
using OWT.CryptoCom.Dto;
using System.Data;
using AiModel;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;
using System;

namespace OWT.CryptoCom.ResponseHandlers {
    public class TradingEthHandler2 : ICryptoComDtoExecutor {

        private readonly CryptoComBalanceDto _balanceDto;
        private readonly SecondPass _debouncePass;
        private readonly IDbConnection _connection;
        private readonly double _amountToTrade;

        public TradingEthHandler2(CryptoComBalanceDto balanceDto, SecondPass debouncePass, IDbConnection connection)
        {
            _balanceDto = balanceDto;
            _debouncePass = debouncePass;
            _connection = connection;
            _amountToTrade = 0.002;
        }

        public bool CanExecute(JObject dto) => _debouncePass;

        public async Task Execute(JObject dto, CryptoComMarketClient marketClient, CancellationToken token)
        {
            var curPrice = await _connection.QueryFirstAsync<dynamic>("SELECT TOP(1) [DateTime], [BestBid], [BestAsk], [Actual], [Low], [High], [Volume], [Change], [BigVolume] FROM [MarketStateSnaps] ORDER BY [DateTime] DESC");
            var date = (DateTime)curPrice.DateTime;
            var times = new object[] { date.Day, (int)date.DayOfWeek, date.Hour, date.Minute, date.Second, date.Millisecond };

            var predict = Test1.Predict(new Test1.ModelInput
            {
                Col0 = curPrice.DateTime.ToString(),
                Col1 = (float)curPrice.BestBid,
                Col2 = (float)curPrice.BestAsk,
                Col3 = (float)curPrice.Actual,
                Col4 = (float)curPrice.Low,
                Col5 = (float)curPrice.High,
                Col6 = (float)curPrice.Volume,
                Col7 = (float)curPrice.Change,
                Col8 = (float)curPrice.BigVolume,
                Col9 = date.Day,
                Col10 = (int)date.DayOfWeek,
                Col11 = date.Hour,
                Col12 = date.Minute,
                Col13 = date.Second,
                Col14 = date.Millisecond,
            });

            if ((double)((IDictionary<string, object>)curPrice)["Actual"] < predict.Col3 
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
            else if ((double)((IDictionary<string, object>)curPrice)["Actual"] > predict.Col3 && _amountToTrade < _balanceDto.Eth) {
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
