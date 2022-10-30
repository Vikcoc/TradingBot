using WebSocketFlow.Dto;
using WebSocketFlow.Extra;

namespace WebSocketFlow.Subscription.Request
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
