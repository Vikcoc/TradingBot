using System.Collections.Generic;
using WebSocketFlow.Dto;
using WebSocketFlow.Extra;

namespace WebSocketFlow.Subscription.Request
{
    public class SubscriptionRequestDto : IRequestDto
    {
        public ICollection<string> Channels { get; set; } = new List<string>();
        public IBaseTransactionDto ToTransactionDto()
        {
            return new TransactionWithParams
            {
                Method = Methods.Subscribe,
                Params = new Dictionary<string, object>
                {
                    {"channels", Channels}
                }
            };
        }
    }
}
