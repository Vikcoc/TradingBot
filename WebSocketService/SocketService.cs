using System.Net.WebSockets;
using System.Text;
using WebSocketService.Interfaces;

namespace WebSocketService
{
    internal class SocketService : BackgroundService, ISocketService
    {
        private readonly ClientWebSocket _clientWebSocket;
        private readonly string _url;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SocketService> _logger;

        public SocketService(ClientWebSocket clientWebSocket, string url, IServiceProvider serviceProvider, ILogger<SocketService> logger)
        {
            _clientWebSocket = clientWebSocket;
            _url = url;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Try connecting to {url}", _url);
            await _clientWebSocket.ConnectAsync(new Uri(_url), stoppingToken);
            _logger.LogInformation("Connected to {url}", _url);
            var buffer = new ArraySegment<byte>(new byte[2048]);
            await using var objectStream = new MemoryStream();
            var streamReader = new StreamReader(objectStream);
            while (!stoppingToken.IsCancellationRequested)
            {
                objectStream.SetLength(0);
                var receiveResult = await _clientWebSocket.ReceiveAsync(buffer, stoppingToken);
                while (!receiveResult.EndOfMessage && !stoppingToken.IsCancellationRequested)
                {
                    await objectStream.WriteAsync(buffer, stoppingToken);
                    receiveResult = await _clientWebSocket.ReceiveAsync(buffer, stoppingToken);
                }

                await objectStream.WriteAsync(buffer.Array!, 0, receiveResult.Count, stoppingToken);
                objectStream.Seek(0, SeekOrigin.Begin);
                var responseString = await streamReader.ReadToEndAsync(stoppingToken);
                
                if (string.IsNullOrWhiteSpace(responseString) || stoppingToken.IsCancellationRequested) 
                    continue;
                
                using var scope = _serviceProvider.CreateScope();
                
                var notification = scope.ServiceProvider.GetRequiredService<INotification<string>>();
                notification.Info = responseString;

                await notification.Notify(stoppingToken);
            }
            _logger.LogInformation("Stopped connection {url}", _url);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning("Stopping connection {url}", _url);
            await _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, WebSocketCloseStatus.NormalClosure.ToString(), cancellationToken);
            await base.StopAsync(cancellationToken);
        }

        public async Task Send(string message, CancellationToken cancellationToken)
        {
            await _clientWebSocket.SendAsync(Encoding.ASCII.GetBytes(message), WebSocketMessageType.Text, true, cancellationToken);
        }
    }
}
