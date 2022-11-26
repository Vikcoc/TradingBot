using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Traders.CryptoCom.Data;
using Traders.Data;
using TradingWebSocket.Adapter;

namespace Traders.CryptoCom.Dto
{
    public class CryptoComSubscriptionResponseFactory<T> : IResponseFactory<CryptoComSubscriptionResponse<T>> where T : IResponseDto
    {
        private readonly Trades _trade;
        protected Dictionary<Trades, string> Trades => CryptoComTrades.Trades;


        public CryptoComSubscriptionResponseFactory(Trades trade)
        {
            _trade = trade;
        }

        public async Task DeserializeObjectAndAct(string obj)
        {
            try
            {
                if (CryptoComSubscriptionResponse<T>.CanJson(obj) && obj.Contains(Trades[_trade]))
                {
                    var res = JsonConvert.DeserializeObject<CryptoComSubscriptionResponse<T>>(obj)!;
                    if (OnValidObject != null)
                        await OnValidObject(res);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public event Func<CryptoComSubscriptionResponse<T>, Task>? OnValidObject;
    }
}
