using System.Collections;

namespace WebSocketService.Interfaces
{
    public interface INotificationConsumer<in T, TC> where T : INotification<TC>
    {
        Task ProcessNotification(T notification, CancellationToken token);
    }
}
