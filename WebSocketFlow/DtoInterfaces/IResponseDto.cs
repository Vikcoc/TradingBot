using Newtonsoft.Json;

namespace WebSocketFlow.DtoInterfaces
{
    public interface IResponseDto
    {
        public static abstract string ClassMethod { get; }

        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("method")]
        public string Method { get; set; }
    }
}
