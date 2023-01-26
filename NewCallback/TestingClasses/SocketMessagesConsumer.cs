using MediatR;
using NewCallback.EndpointDefinitions;
using WebSocketService.DefaultImplementations;

namespace NewCallback.TestingClasses
{
    public class SocketMessagesConsumer : INotificationHandler<StringNotification>
    {
        private readonly ILogger<SocketMessagesConsumer> _logger;
        private readonly ScopedGuid _scope;

        public SocketMessagesConsumer(ILogger<SocketMessagesConsumer> logger, ScopedGuid scope)
        {
            _logger = logger;
            _scope = scope;
        }

        public Task Handle(StringNotification notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Consumer running at: {time} with scope: {scope}", notification.Message, _scope);
            return Task.CompletedTask;
        }
    }
}
