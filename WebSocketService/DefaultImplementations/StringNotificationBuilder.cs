using MediatR;
using WebSocketService.Interfaces;

namespace WebSocketService.DefaultImplementations
{
    public class StringNotificationBuilder : IStringNotificationBuilder
    {
        public INotification MakeNotification(string message)
        {
            return new StringNotification(message);
        }
    }
}
