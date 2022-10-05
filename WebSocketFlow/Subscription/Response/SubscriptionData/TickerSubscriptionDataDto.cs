using Newtonsoft.Json;
using WebSocketFlow.Extra;

namespace WebSocketFlow.Subscription.Response.SubscriptionData
{
    public class TickerSubscriptionDataDto : ISubscriptionData
    {
        [JsonProperty("i")]
        public string InstrumentName { get; set; } = string.Empty;

        [JsonProperty("b")]
        public double? BestBid { get; set; }

        [JsonProperty("k")]
        public double? BestAsk { get; set; }

        [JsonProperty("a")]
        public double? Actual { get; set; }

        [JsonProperty("l")]
        public double? Low { get; set; }

        [JsonProperty("h")]
        public double? High { get; set; }

        [JsonProperty("v")]
        public long Volume { get; set; }

        [JsonProperty("c")]
        public double? Change { get; set; }

        [JsonProperty("t")]
        public long Timestamp { get; set; }

        [JsonProperty("vv")]
        public long BigVolume { get; set; }

        [JsonProperty("pc")]
        public double? PartChange { get; set; }

        public static string Type => Exchanges.Ticker;
    }
}
