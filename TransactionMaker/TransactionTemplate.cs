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
            Nonce = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            Semaphore.Wait();
            Id = _id;
            _id++;
            Semaphore.Release();
        }

        public Dictionary<string, string> Params { get; set; }
        public string Method { get; set; }
        public long Id { get; set; }
        public long Nonce { get; set; }
    }
}
