using System.Collections.Generic;
using WebSocketFlow.Extra;

namespace WebSocketFlow.Subscription.Response
{
    public class BtcSubscriptionDto<T> : IResponseDto, ISubscriptionInfo<T> where T : ISubscriptionData
    {
        public static bool CanJson(string json) => json.Contains(T.Type + Exchanges.BtcUsd);
        public string InstrumentName { get; set; } = string.Empty;
        public string Subscription { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
        public List<T>? Data { get; set; }
    }
}
