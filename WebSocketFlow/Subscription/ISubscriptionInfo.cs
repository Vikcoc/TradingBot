using Newtonsoft.Json;
using System.Collections.Generic;

namespace WebSocketFlow.Subscription
{
    public interface ISubscriptionInfo<T> where T : ISubscriptionData
    {
        [JsonProperty("instrument_name")]
        public string InstrumentName { get; set; }
        [JsonProperty("subscription")]
        public string Subscription { get; set; }
        [JsonProperty("channel")]
        public string Channel { get; set; }
        [JsonProperty("data")]
        public List<T>? Data { get; set; }
    }
}
