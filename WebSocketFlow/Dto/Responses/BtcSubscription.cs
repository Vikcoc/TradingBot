using Newtonsoft.Json;
using System.Collections.Generic;
using WebSocketFlow.DtoInterfaces;
using WebSocketFlow.Extra;

namespace WebSocketFlow.Dto.Responses
{
    public class BtcSubscription : IResponseDto
    {
        [JsonProperty("instrument_name")]
        public string InstrumentName { get; set; } = string.Empty;
        [JsonProperty("subscription")]
        public string Subscription { get; set; } = string.Empty;
        [JsonProperty("channel")]
        public string Channel { get; set; } = string.Empty;
        [JsonProperty("data")]
        public List<SubscriptionDataDto>? Data { get; set; }

        public static bool CanJson(string json) => json.Contains(Tickers.BtcUsd);
    }
}
