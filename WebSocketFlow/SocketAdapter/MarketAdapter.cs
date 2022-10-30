using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using WebSocketFlow.Subscription.Request;
using WebSocketFlow.Subscription.Response;

namespace WebSocketFlow.SocketAdapter
{
    public class MarketAdapter : IMarketAdapter
    {
        private readonly ISocketAdapter _socketAdapter;
        private event Func<IRequestDto, Task>? RequestCallback;

        public MarketAdapter(ISocketAdapter socketAdapter)
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

        public void AddRequestCallback<T>(Func<T, Task> callback) where T : IRequestDto
        {
            RequestCallback += async s =>
            {
                if (s is T dto)
                    await callback(dto);
            };
        }

        protected virtual string SocketEndpoint => "wss://stream.crypto.com/v2/market";

        public async Task ConnectAndListen()
        {
            AddResponseCallback((Func<HeartbeatResponseDto, Task>)(async x => await Send(new HeartbeatRequestDto { Id = x.Id })));
            await _socketAdapter.Connect(SocketEndpoint);
#pragma warning disable CS4014
            _socketAdapter.StartListening();
#pragma warning restore CS4014
            await Task.Delay(1000);
        }

        public Task Disconnect() => _socketAdapter.Disconnect();

        public bool IsConnected => _socketAdapter.IsConnected;
        public async Task Send(IRequestDto dto)
        {
            if (RequestCallback != null)
                await RequestCallback(dto);
            await _socketAdapter.Send(dto.ToTransactionDto().ToJson());
        }
    }
}
