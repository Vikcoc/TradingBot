using System.Collections.Generic;
using Newtonsoft.Json;
using Traders.CryptoCom.Data;
using TradingWebSocket.Adapter;

namespace Traders.CryptoCom.Dto
{
    public class CryptoComSubscriptionResponse<T> : ICryptoComBaseTransaction, IResponseDto where T : IResponseDto
    {
        public static bool CanJson(string json) => json.Contains(CryptoComMethods.Subscribe) && T.CanJson(json);

        public long Id { get; set; }
        public string Method { get; set; } = string.Empty;
        [JsonProperty("result")]
        public CryptoComSubscriptionResult<T>? Result { get; set; }
    }
}
