using WebSocketFlow.DtoInterfaces;

namespace WebSocketFlow.Dto
{
    public class BaseTransactionDto : IBaseTransactionDto
    {
        public long Id { get; set; }
        public string Method { get; set; } = "";
    }
}
