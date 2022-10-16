using System.Collections.Generic;
using System.Linq;
using WebSocketFlow.Dto;
using WebSocketFlow.Extra;

namespace WebSocketFlow.SpotTrading
{
    public class AccountSummaryRequestDto : IRequestDto
    {
        public string Currency { get; set; } = string.Empty;
        public IBaseTransactionDto ToTransactionDto()
        {
            return new TransactionWithParams
            {
                Method = Methods.AccountSummary,
                Params = new Dictionary<string, object>
                {
                    {"currency", Currency}
                }
            };
        }
    }
}
