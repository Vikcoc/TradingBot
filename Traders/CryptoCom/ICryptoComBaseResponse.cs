using Newtonsoft.Json;

namespace Traders.CryptoCom
{
    public interface ICryptoComBaseResponse
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("method")]
        public string Method { get; set; }
    }
}
