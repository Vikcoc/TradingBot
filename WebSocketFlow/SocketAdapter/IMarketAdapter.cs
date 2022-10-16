using System.Threading.Tasks;
using System;

namespace WebSocketFlow.SocketAdapter
{
    public interface IMarketAdapter
    {
        void AddResponseCallback<T>(Func<T, Task> callback) where T : IResponseDto;
        void AddRequestCallback<T>(Func<T, Task> callback) where T : IRequestDto;
        Task ConnectAndListen();
        Task Disconnect();
        bool IsConnected { get; }
        Task Send(IRequestDto dto);
    }
}
