using System.Threading;
using WebSocketFlow.DtoInterfaces;

namespace WebSocketFlow.Dto
{
    public class BaseTransactionDto : IBaseTransactionDto
    {
        private static long _id;
        private static readonly SemaphoreSlim Semaphore;

        static BaseTransactionDto()
        {
            Semaphore = new SemaphoreSlim(1, 1);
        }

        public BaseTransactionDto()
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
