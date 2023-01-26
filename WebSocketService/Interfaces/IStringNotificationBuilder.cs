using MediatR;

namespace WebSocketService.Interfaces
{
    public interface IStringNotificationBuilder
    {
        INotification MakeNotification(string message);
    }
}
