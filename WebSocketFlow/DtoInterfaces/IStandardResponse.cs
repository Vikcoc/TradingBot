using Newtonsoft.Json;

namespace WebSocketFlow.DtoInterfaces
{
    public interface IStandardResponse
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("method")]
        public string Method { get; set; }
    }
}
