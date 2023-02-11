﻿namespace OWT.CryptoCom.Dto
{
    public class CryptoComBaseTransaction
    {
        private static long _id;
        private static readonly SemaphoreSlim Semaphore;

        static CryptoComBaseTransaction()
        {
            Semaphore = new SemaphoreSlim(1, 1);
        }

        public CryptoComBaseTransaction()
        {
            Semaphore.Wait();
            _id++;
            Id = _id;
            Semaphore.Release();
        }

        public long Id { get; set; }
        public string Method { get; set; } = string.Empty;
    }
}
