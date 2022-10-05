using WebSocketFlow.Extra;

namespace WebSocketFlow.Subscription.Response
{
    public class SubscriptionResponseDto<T, TU> : ISubscriptionResponse<T> where T : IResponseDto, ISubscriptionInfo<TU> where TU : ISubscriptionData
    {
        public static bool CanJson(string json) => json.Contains(Methods.Subscribe) && T.CanJson(json);

        public long Id { get; set; }
        public string Method { get; set; } = Methods.Subscribe;
        public T? Result { get; set; }
    }
}
