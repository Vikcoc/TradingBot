using System.Data;
using System.Data.SqlClient;
using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OWT.CryptoCom.Dto;
using static OWT.MLModel;

namespace OWT.CryptoCom.ResponseHandlers {
    public class TradingEthHandler : ICryptoComDtoExecutor
    {
        private readonly CryptoComBalanceDto _balanceDto;
        private readonly SecondPass _debouncePass;
        private readonly IDbConnection _connection;
        private readonly double _amountToTrade;



        public TradingEthHandler(CryptoComBalanceDto balanceDto, SecondPass debouncePass, IDbConnection connection)
        {
            _balanceDto = balanceDto;
            _debouncePass = debouncePass;
            _connection = connection;
            _amountToTrade = 0.001;
        }

        public bool CanExecute(JObject dto) => true;

        public async Task Execute(JObject dto, CryptoComMarketClient marketClient, CancellationToken token)
        {
            if(!_debouncePass)
                return;

            var curPrice = await _connection.QueryFirstAsync<float>("SELECT TOP (1) [Actual] as Value FROM [TradingBot].[dbo].[MarketStateSnaps] order by [DateTime] desc");
            var req = new ModelInput {
                Col3 = curPrice
            };
            var predict = Predict(req, 1);
            
            if (curPrice < predict.Col3.Average() && _amountToTrade * curPrice < _balanceDto.Usd)
            {
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
            else if (curPrice > predict.Col3.Average() && _amountToTrade < _balanceDto.Eth)
            {
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
