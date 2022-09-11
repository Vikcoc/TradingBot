using System;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebSocketFlow.Dto;
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
                if (s.Contains(T.ClassMethod))
                {
                    var res = JsonConvert.DeserializeObject<T>(s)!;
                    await callback(res);
                }
            };
        }

        public async Task ConnectAndListen()
        {
            await _socketAdapter.Connect("wss://stream.crypto.com/v2/market");
            AddResponseCallback((Func<HeartbeatResponseDto, Task>)(async x => await Send(new HeartbeatRequestDto{Id = x.Id})) );
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
