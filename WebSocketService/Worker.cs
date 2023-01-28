using WebSocketService.Interfaces;

namespace WebSocketService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;


        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                using (var scope = _serviceProvider.CreateScope())
                {
                    var notification = scope.ServiceProvider.GetRequiredService<INotification<string>>();
                    notification.Info = DateTimeOffset.Now.ToString();

                    await notification.Notify(stoppingToken);
                }
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}