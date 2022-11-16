using System;
using System.Threading.Tasks;

namespace TradingWebSocket.Socket
{
    public interface ISocketAdapter
    {
        public Task Connect(string path);
        public Task Disconnect();
        public Task StartListening();
        public bool IsConnected { get; }
        public event Func<string, Task> OnReceive;
        public Task Send(string dto);
    }
}
