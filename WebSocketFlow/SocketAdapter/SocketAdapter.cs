using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketFlow.SocketAdapter
{
    public class SocketAdapter : ISocketAdapter
    {
        private readonly ClientWebSocket _clientWebSocket;
        public SocketAdapter()
        {
            _clientWebSocket = new ClientWebSocket();
        }
        public async Task Connect(string path = "wss://stream.crypto.com/v2/user")
        {
            if (IsConnected)
                throw new NotImplementedException();
            await _clientWebSocket.ConnectAsync(new Uri(path), CancellationToken.None);
        }

        public async Task Disconnect()
        {
            await _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                WebSocketCloseStatus.NormalClosure.ToString(), CancellationToken.None);
        }

        public async Task StartListening()
        {
            var buffer = new ArraySegment<byte>(new byte[2048]);
            await using var objectStream = new MemoryStream();
            var streamReader = new StreamReader(objectStream);
            while (IsConnected)
            {
                objectStream.SetLength(0);
                var receiveResult = await _clientWebSocket.ReceiveAsync(buffer, CancellationToken.None);
                while (!receiveResult.EndOfMessage)
                {
                    await objectStream.WriteAsync(buffer);
                    receiveResult = await _clientWebSocket.ReceiveAsync(buffer, CancellationToken.None);
                }

                await objectStream.WriteAsync(buffer.Array, 0, receiveResult.Count);
                objectStream.Seek(0, SeekOrigin.Begin);
                var responseString = await streamReader.ReadToEndAsync();
                if (!string.IsNullOrWhiteSpace(responseString) && OnReceive != null)
                    await OnReceive(responseString);
            }
        }

        public bool IsConnected => _clientWebSocket.State == WebSocketState.Open;
        public event Func<string, Task>? OnReceive;
        public async Task Send(string dto)
        {
            await _clientWebSocket.SendAsync(Encoding.ASCII.GetBytes(dto), WebSocketMessageType.Text, true,
                CancellationToken.None);
        }
    }
}
