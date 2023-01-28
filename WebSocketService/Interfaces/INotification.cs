namespace WebSocketService.Interfaces
{
    public interface INotification<T>
    {
        T? Info { get; set; }
        Task Notify(CancellationToken token);
    }
}
