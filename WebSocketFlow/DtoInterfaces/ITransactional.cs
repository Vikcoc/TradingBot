using Newtonsoft.Json;

namespace WebSocketFlow.DtoInterfaces
{
    public interface ITransactional
    {
        public string GetBody()
        {
            return JsonConvert.SerializeObject(this);
        }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }
    }
}
