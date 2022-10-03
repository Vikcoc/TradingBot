using Newtonsoft.Json;
using WebSocketFlow.DtoInterfaces;
using WebSocketFlow.Extra;

namespace WebSocketFlow.Dto.Responses
{
    public class SubscriptionTickResponseDto : ISubscriptionResponse<BtcSubscription>
    {
        public static bool CanJson(string json) => json.Contains(Methods.Subscribe) && BtcSubscription.CanJson(json);

        public long Id { get; set; }
        public string Method { get; set; } = Methods.Subscribe;
        [JsonProperty("result")]
        public BtcSubscription? Result { get; set; }
    }
}
