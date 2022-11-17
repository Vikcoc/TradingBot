using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using TradingWebSocket.Adapter;

namespace Traders.CryptoCom
{
    public class CryptoComResponseFactory<T> : IResponseFactory<T> where T : IResponseDto
    {
        public async Task DeserializeObjectAndAct(string obj)
        {
            if (T.CanJson(obj))
            {
                var res = JsonConvert.DeserializeObject<T>(obj)!;
                if(OnValidObject != null)
                    await OnValidObject(res);
            }
        }

        public event Func<T, Task>? OnValidObject;
    }
}
