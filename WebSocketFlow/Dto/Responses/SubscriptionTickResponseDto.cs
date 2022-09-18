using Newtonsoft.Json;
using WebSocketFlow.DtoInterfaces;
using WebSocketFlow.Extra;

namespace WebSocketFlow.Dto.Responses
{
    public class SubscriptionTickResponseDto : IResponseDto
    {
        public static string ClassMethod => Methods.Subscribe;
        public long Id { get; set; }
        public string Method { get; set; } = ClassMethod;
        [JsonProperty("result")]
        public SubscriptionInfoDto? Result { get; set; }
    }
}
