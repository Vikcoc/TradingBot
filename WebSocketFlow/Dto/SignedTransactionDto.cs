using Newtonsoft.Json;
using WebSocketFlow.DtoInterfaces;

namespace WebSocketFlow.Dto
{
    public class SignedTransactionDto : BaseTransactionDto, IRequestDto
    {
        [JsonProperty("api_key")]
        public string ApiKey { get; set; } = string.Empty;
        [JsonProperty("sig")]
        public string Signature { get; set; } = string.Empty;
        [JsonProperty("nonce")]
        public long Nonce { get; set; }

        public IBaseTransactionDto ToTransactionDto() => this;
    }
}
