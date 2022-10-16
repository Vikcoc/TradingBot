using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebSocketFlow.Dto
{
    public class TransactionWithParams : BaseTransactionDto
    {
        [JsonProperty("params")]
        public Dictionary<string, object> Params { get; set; } = new Dictionary<string, object>();
    }
}
