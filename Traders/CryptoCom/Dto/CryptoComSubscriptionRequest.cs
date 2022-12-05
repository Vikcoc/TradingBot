using Newtonsoft.Json;
using System.Collections.Generic;
using Traders.CryptoCom.Data;
using TradingWebSocket.Adapter;

namespace Traders.CryptoCom.Dto
{
    internal class CryptoComSubscriptionRequest : ITransaction
    {
        public List<string> Channels { get; set; } = new ();

        public string ToJson()
        {
            return JsonConvert.SerializeObject(new CryptoComParamRequest
            {
                Method = CryptoComMethods.Subscribe,
                Params = new Dictionary<string, object>
                {
                    {"channels", Channels}
                }
            });
        }
    }
}
