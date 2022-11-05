using Newtonsoft.Json;
using System.Collections.Generic;
using WebSocketFlow.Subscription.Response.SubscriptionData;

namespace WebSocketFlow.Subscription.Response
{
    public class UserBalanceResponseDto : IResponseDto
    {
        [JsonProperty("subscription")]
        public string Subscription { get; set; } = string.Empty;
        [JsonProperty("channel")]
        public string Channel { get; set; } = string.Empty;
        [JsonProperty("data")]
        public List<BalanceDataDto>? Data { get; set; }
        public static bool CanJson(string json) => json.Contains("user.balance");
    }
}
