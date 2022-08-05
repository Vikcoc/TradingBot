using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace TransactionMaker
{
    public class TransactionTemplateLite
    {
        private static long _id;
        private static readonly SemaphoreSlim Semaphore;

        static TransactionTemplateLite()
        {
            Semaphore = new SemaphoreSlim(1, 1);
        }

        public TransactionTemplateLite()
        {
            Params = new Dictionary<string, string>();
            Method = string.Empty;
            Nonce = DateTimeOffset.UtcNow.Subtract(TimeSpan.FromSeconds(20)).ToUnixTimeMilliseconds();
            Semaphore.Wait();
            Id = _id;
            _id++;
            Semaphore.Release();
        }

        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("method")]
        public string Method { get; set; }
        [JsonProperty("params")]
        public Dictionary<string, string> Params { get; set; }

        [JsonProperty("nonce")]
        public long Nonce { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this).Replace("\\", "").Replace("\"[", "[").Replace("]\"", "]");
        }
    }
}
