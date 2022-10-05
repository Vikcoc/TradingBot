using WebSocketFlow.DtoInterfaces;

namespace WebSocketFlow
{
    public interface IRequestDto
    {
        public IBaseTransactionDto ToTransactionDto();
    }
}
