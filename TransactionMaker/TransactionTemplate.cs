using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;

namespace TransactionMaker
{
    public class TransactionTemplate
    {
        private static long _id;
        private static readonly SemaphoreSlim Semaphore;

        static TransactionTemplate()
        {
            Semaphore = new SemaphoreSlim(1, 1);
        }

        public TransactionTemplate()
        {
            Params = new Dictionary<string, string>();
            Method = string.Empty;
            ApiKey = string.Empty;
            Signature = string.Empty;
            Nonce = DateTimeOffset.UtcNow.Subtract(TimeSpan.FromSeconds(20)).ToUnixTimeMilliseconds();
            Semaphore.Wait();
            Id = _id;
            _id++;
            Semaphore.Release();
        }
        [JsonProperty("params")]
        public Dictionary<string, string> Params { get; set; }
        [JsonProperty("method")]
        public string Method { get; set; }
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("nonce")]
        public long Nonce { get; set; }
        [JsonProperty("api_key")]
        public string ApiKey { get; set; }
        [JsonProperty("sig")]
        public string Signature { get; set; }
    }
}
