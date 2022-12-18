using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TradingWebSocket.Socket
{
    /// <summary>
    /// Wrapper over WebSocket that implements interface
    /// </summary>
    public class SocketAdapter : ISocketAdapter
    {
        private readonly ClientWebSocket _clientWebSocket;
        public SocketAdapter()
        {
            _clientWebSocket = new ClientWebSocket();
        }
        /// <summary>
        /// Connect the WebSocket to server
        /// </summary>
        /// <param name="path"> Url to WebSocket server </param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Connect(string path)
        {
            if (IsConnected)
                throw new NotImplementedException();
            await _clientWebSocket.ConnectAsync(new Uri(path), CancellationToken.None);
        }

        /// <summary>
        /// Disconnect from WebSocket server
        /// </summary>
        /// <returns></returns>
        public async Task Disconnect()
        {
            await _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                WebSocketCloseStatus.NormalClosure.ToString(), CancellationToken.None);
        }

        /// <summary>
        /// Listen to websocket server and send messages trough event
        /// </summary>
        /// <returns></returns>
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

                await objectStream.WriteAsync(buffer.Array!, 0, receiveResult.Count, CancellationToken.None);
                objectStream.Seek(0, SeekOrigin.Begin);
                var responseString = await streamReader.ReadToEndAsync();
                //Console.WriteLine("Received: " + responseString);
                if (!string.IsNullOrWhiteSpace(responseString) && OnReceive != null)
                    await OnReceive(responseString);
            }
        }

        /// <summary>
        /// Check if socket is connected
        /// </summary>
        public bool IsConnected => _clientWebSocket.State == WebSocketState.Open;
        /// <summary>
        /// Event with the received message
        /// </summary>
        public event Func<string, Task>? OnReceive;

        /// <summary>
        /// Send string message to server
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task Send(string dto)
        {
            //Console.WriteLine("Send: " + dto);
            await _clientWebSocket.SendAsync(Encoding.ASCII.GetBytes(dto), WebSocketMessageType.Text, true,
                CancellationToken.None);
        }
    }
}
