using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using WebSocketFlow.Dto.Requests;
using WebSocketFlow.Dto.Responses;
using WebSocketFlow.DtoInterfaces;

namespace WebSocketFlow.SocketAdapter
{
    public class MarketAdapter : IMarketAdapter
    {
        private readonly ISocketAdapter _socketAdapter;

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
        public Task Send(IRequestDto dto) => _socketAdapter.Send(dto.ToTransactionDto().ToJson());
    }
}
