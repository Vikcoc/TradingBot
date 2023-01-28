using NewCallback.EndpointDefinitions;
using WebSocketService.Interfaces;

namespace NewCallback.TestingClasses
{
    public class SocketMessagesConsumer2 : INotificationConsumer<INotification<string>, string>
    {
        private readonly ILogger<SocketMessagesConsumer2> _logger;
        private readonly ScopedGuid _scope;

        public SocketMessagesConsumer2(ILogger<SocketMessagesConsumer2> logger, ScopedGuid scope)
        {
            _logger = logger;
            _scope = scope;
        }
        
        public Task ProcessNotification(INotification<string> notification, CancellationToken token)
        {
            _logger.LogWarning("Consumer number 2 running at: {time} with scope: {scope}", notification.Info, _scope.Id);
            return Task.CompletedTask;
        }
    }
}
