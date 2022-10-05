using Newtonsoft.Json;
using WebSocketFlow.Extra;

namespace WebSocketFlow.Subscription.Response.SubscriptionData
{
    public class TradeSubscriptionDataDto : ISubscriptionData
    {
        [JsonProperty("i")]
        public string InstrumentName { get; set; } = string.Empty;

        [JsonProperty("dataTime")]
        public long DataTime { get; set; }

        [JsonProperty("d")]
        public long TradeId { get; set; }

        [JsonProperty("s")]
        public string Action { get; set; } = string.Empty;

        [JsonProperty("p")]
        public double Price { get; set; }

        [JsonProperty("q")]
        public double Quantity { get; set; }

        [JsonProperty("t")]
        public long TradeTime { get; set; }

        public static string Type => Exchanges.Trade;
    }
}
