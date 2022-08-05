using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;

namespace TransactionMaker
{
    public class TransactionTemplate : TransactionTemplateLite
    {
        public TransactionTemplate()
        {

            ApiKey = string.Empty;
            Signature = string.Empty;
        }
        [JsonProperty("api_key")]
        public string ApiKey { get; set; }
        [JsonProperty("sig")]
        public string Signature { get; set; }
    }
}
