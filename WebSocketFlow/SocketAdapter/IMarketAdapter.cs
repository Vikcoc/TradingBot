using System.Threading.Tasks;
using System;
using WebSocketFlow.DtoInterfaces;

namespace WebSocketFlow.SocketAdapter
{
    public interface IMarketAdapter
    {
        public void AddResponseCallback<T>(Func<T, Task> callback) where T : IResponseDto;
        public Task ConnectAndListen();
        public Task Disconnect();
        public bool IsConnected { get; }
        public Task Send(IRequestDto dto);
    }
}
