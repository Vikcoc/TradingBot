using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using TradingWebSocket.Socket;

namespace TradingWebSocket.Adapter
{
    public abstract class MarketAdapter : IMarketAdapter
    {
        private readonly ISocketAdapter _socketAdapter;
        private event Func<ITransaction, Task>? RequestCallback;

        protected MarketAdapter(ISocketAdapter socketAdapter)
        {
            _socketAdapter = socketAdapter;
        }

        public void AddResponseCallback<T>(Func<T, Task> callback) where T : IResponseDto
        {
            _socketAdapter.OnReceive += async s =>
            {
                if (T.CanJson(s))
                {
                    var res = JsonConvert.DeserializeObject<T>(s)!;
                    await callback(res);
                }
            };
        }

        public void AddRequestCallback<T>(Func<T, Task> callback) where T : ITransaction
        {
            RequestCallback += async s =>
            {
                if (s is T dto)
                    await callback(dto);
            };
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
