namespace WebSocketService
{
    public interface ISocketService : IHostedService, IDisposable
    {
        Task Send(string message, CancellationToken cancellationToken);
    }
}
