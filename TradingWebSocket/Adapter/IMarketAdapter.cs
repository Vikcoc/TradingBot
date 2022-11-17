using System;
using System.Threading.Tasks;

namespace TradingWebSocket.Adapter
{
    public interface IMarketAdapter
    {
        public void AddSocketResponse<T>(IResponseFactory<T> factory) where T : IResponseDto;
        public void RemoveSocketResponse<T>(IResponseFactory<T> factory) where T : IResponseDto;
        Task ConnectAndListen();
        Task Disconnect();
        bool IsConnected { get; }
        Task Send(ITransaction dto);
    }
}
