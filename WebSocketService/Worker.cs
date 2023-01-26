using MediatR;
using WebSocketService.Interfaces;

namespace WebSocketService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMediator _mediator;
        private readonly IStringNotificationBuilder _notificationBuilder;

        public Worker(ILogger<Worker> logger, IMediator mediator, IStringNotificationBuilder notificationBuilder)
        {
            _logger = logger;
            _mediator = mediator;
            _notificationBuilder = notificationBuilder;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await _mediator.Publish(_notificationBuilder.MakeNotification(DateTimeOffset.Now.ToString()), stoppingToken);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}