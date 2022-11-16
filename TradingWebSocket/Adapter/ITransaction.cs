using Newtonsoft.Json;

namespace TradingWebSocket.Adapter
{
    public interface ITransaction
    {
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
