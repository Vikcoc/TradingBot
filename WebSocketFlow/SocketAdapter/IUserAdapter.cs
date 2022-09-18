using System.Threading.Tasks;

namespace WebSocketFlow.SocketAdapter
{
    public interface IUserAdapter : IMarketAdapter
    {
        public Task ConnectListenAndAuthenticate();
    }
}
