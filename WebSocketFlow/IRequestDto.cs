namespace WebSocketFlow
{
    public interface IRequestDto
    {
        public IBaseTransactionDto ToTransactionDto();
    }
}
