using MediatR;

namespace WebSocketService.DefaultImplementations
{
    public class StringNotification : INotification
    {
        public StringNotification(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}
