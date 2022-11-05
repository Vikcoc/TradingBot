using Newtonsoft.Json;

namespace WebSocketFlow.Subscription.Response.SubscriptionData
{
    public class BalanceDataDto
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
    }
}
