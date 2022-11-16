using System;
using System.Threading.Tasks;

namespace TradingWebSocket.Adapter
{
    public interface IMarketAdapter
    {
        void AddResponseCallback<T>(Func<T, Task> callback) where T : IResponseDto;
        void AddRequestCallback<T>(Func<T, Task> callback) where T : ITransaction;
        Task ConnectAndListen();
        Task Disconnect();
        bool IsConnected { get; }
        Task Send(ITransaction dto);
    }
}
