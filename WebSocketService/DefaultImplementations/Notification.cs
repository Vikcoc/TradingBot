using WebSocketService.Interfaces;

namespace WebSocketService.DefaultImplementations
{
    public class Notification<T> : INotification<T>
    {
        private readonly INotificationConsumer<INotification<T>, T>[] _consumers;

        public Notification(INotificationConsumer<INotification<T>, T>[] consumers)
        {
            _consumers = consumers;
        }

        public T? Info { get; set; }
        public async Task Notify(CancellationToken token)
        {
            if (Info != null)
                foreach (var consumer in _consumers)
                    await consumer.ProcessNotification(this, token);
        }
    }
}
