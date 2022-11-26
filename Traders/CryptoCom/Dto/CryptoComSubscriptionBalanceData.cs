using Newtonsoft.Json;
using Traders.CryptoCom.Data;
using TradingWebSocket.Adapter;

namespace Traders.CryptoCom.Dto
{
    internal class CryptoComSubscriptionBalanceData : IResponseDto
    {
        [JsonProperty("currency")]
        public string Currency { get; set; } = string.Empty;
        [JsonProperty("balance")]
        public double Balance { get; set; }
        [JsonProperty("available")]
        public double Available { get; set; }
        [JsonProperty("order")]
        public double Order { get; set; }
        [JsonProperty("stake")]
        public double Stake { get; set; }

        public static bool CanJson(string json) => json.Contains(CryptoComMethods.Balance);
    }
}
