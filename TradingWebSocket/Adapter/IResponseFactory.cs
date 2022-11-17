using System;
using System.Threading.Tasks;

namespace TradingWebSocket.Adapter
{
    public interface IResponseFactory<out T> where T : IResponseDto
    {
        Task DeserializeObjectAndAct(string obj);
        event Func<T, Task> OnValidObject;
    }
}
