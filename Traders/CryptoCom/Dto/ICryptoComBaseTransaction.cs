using Newtonsoft.Json;

namespace Traders.CryptoCom.Dto
{
    public interface ICryptoComBaseTransaction
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("method")]
        public string Method { get; set; }
    }
}
