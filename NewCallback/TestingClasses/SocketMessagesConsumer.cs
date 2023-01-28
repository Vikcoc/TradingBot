﻿using NewCallback.EndpointDefinitions;
using WebSocketService.Interfaces;

namespace NewCallback.TestingClasses
{
    public class SocketMessagesConsumer : INotificationConsumer<INotification<string>, string>
    {
        private readonly ILogger<SocketMessagesConsumer> _logger;
        private readonly ScopedGuid _scope;

        public SocketMessagesConsumer(ILogger<SocketMessagesConsumer> logger, ScopedGuid scope)
        {
            _logger = logger;
            _scope = scope;
        }
        
        public Task ProcessNotification(INotification<string> notification, CancellationToken token)
        {
            _logger.LogInformation("Consumer running at: {time} with scope: {scope}", notification.Info, _scope.Id);
            return Task.CompletedTask;
        }
    }
}
