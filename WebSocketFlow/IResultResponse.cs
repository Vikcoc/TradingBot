using Newtonsoft.Json;

namespace WebSocketFlow
{
    public interface IResultResponse<T> : IResponseDto where T : IResponseDto
    {
        [JsonProperty("result")]
        public T? Result { get; set; }
    }
}
