using System.Collections.Generic;
using WebSocketFlow.DtoInterfaces;
using WebSocketFlow.Extra;

namespace WebSocketFlow.Dto
{
    public class TickerRequestDto : IRequestDto
    {
        public ICollection<string> Tickers { get; set; } = new List<string>();
        public IBaseTransactionDto ToTransactionDto()
        {
            return new TransactionWithParams
            {
                Method = Methods.Subscribe,
                Params = new Dictionary<string, object>
                {
                    {"channels", Tickers}   
                }
            };
        }
    }
}
