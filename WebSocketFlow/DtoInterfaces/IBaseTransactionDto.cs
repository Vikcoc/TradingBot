﻿using Newtonsoft.Json;

namespace WebSocketFlow.DtoInterfaces
{
    public interface IBaseTransactionDto
    {
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("method")]
        public string Method { get; set; }
    }
}
