using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using TradingWebSocket.Socket;

namespace TradingWebSocket.Adapter
{
    public abstract class MarketAdapter : IMarketAdapter
    {
        private readonly ISocketAdapter _socketAdapter;
        private event Func<ITransaction, Task>? RequestCallback;

        public void AddSocketResponse<T>(IResponseFactory<T> factory) where T : IResponseDto => _socketAdapter.OnReceive += factory.DeserializeObjectAndAct;
        public void RemoveSocketResponse<T>(IResponseFactory<T> factory) where T : IResponseDto => _socketAdapter.OnReceive -= factory.DeserializeObjectAndAct;

        protected MarketAdapter(ISocketAdapter socketAdapter)
        {
            _socketAdapter = socketAdapter;
        }

        protected abstract string SocketEndpoint { get; set; }

        public virtual async Task ConnectAndListen()
        {
            await _socketAdapter.Connect(SocketEndpoint);
#pragma warning disable CS4014
            _socketAdapter.StartListening();
#pragma warning restore CS4014
            await Task.Delay(1000);
        }

        public Task Disconnect() => _socketAdapter.Disconnect();

        public bool IsConnected => _socketAdapter.IsConnected;

        public async Task Send(ITransaction dto)
        {
            if (RequestCallback != null)
                await RequestCallback(dto);
            await _socketAdapter.Send(dto.ToJson());
        }
    }
}
