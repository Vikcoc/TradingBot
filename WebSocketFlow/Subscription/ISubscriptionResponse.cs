using Newtonsoft.Json;

namespace WebSocketFlow.Subscription
{
    public interface ISubscriptionResponse<T> : IResponseDto where T : IResponseDto
    {
        [JsonProperty("result")]
        public T? Result { get; set; }
    }
}
