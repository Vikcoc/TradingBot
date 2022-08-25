namespace WebSocketFlow.DtoInterfaces
{
    public interface IHeartbeat : ITransactional
    {
        public bool IsValid();
    }
}
