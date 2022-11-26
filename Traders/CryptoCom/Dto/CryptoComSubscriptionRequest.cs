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
            return JsonConvert.SerializeObject(new CryptoComSubscriptionRequestHidden
            {
                Params = new Dictionary<string, object>
                {
                    {"channels", Channels}
                }
            });
        }

        internal class CryptoComSubscriptionRequestHidden : CryptoComBaseRequest
        {
            public CryptoComSubscriptionRequestHidden()
            {
                Method = CryptoComMethods.Subscribe;
            }

            [JsonProperty("params")]
            public Dictionary<string, object>? Params { get; set; }
        }
    }
}
