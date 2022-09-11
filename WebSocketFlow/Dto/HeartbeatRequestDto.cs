using WebSocketFlow.DtoInterfaces;
using WebSocketFlow.Extra;

namespace WebSocketFlow.Dto
{
    public class HeartbeatRequestDto : IRequestDto
    {
        public long Id { get; set; }
        public IBaseTransactionDto ToTransactionDto()
        {
            return new BaseTransactionDto
            {
                Id = Id,
                Method = Methods.HeartbeatRequest
            };
        }
    }
}
