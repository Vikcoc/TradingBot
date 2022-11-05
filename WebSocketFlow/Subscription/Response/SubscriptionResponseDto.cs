using WebSocketFlow.Extra;

namespace WebSocketFlow.Subscription.Response
{
    public class SubscriptionResponseDto<T> : IResultResponse<T> where T : IResponseDto
    {
        public static bool CanJson(string json) => json.Contains(Methods.Subscribe) && T.CanJson(json);

        public long Id { get; set; }
        public string Method { get; set; } = Methods.Subscribe;
        public T? Result { get; set; }
    }
}
