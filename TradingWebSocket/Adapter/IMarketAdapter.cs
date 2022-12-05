using System.Threading.Tasks;

namespace TradingWebSocket.Adapter
{
    /// <summary>
    /// Interface to separate the trading server communication channel onto relevant branches  
    /// </summary>
    public interface IMarketAdapter
    {
        /// <summary>
        /// Add factory for a type of message
        /// </summary>
        /// <typeparam name="T"> The type of message </typeparam>
        /// <param name="factory"> The message factory </param>
        public void AddSocketResponse<T>(IResponseFactory<T> factory) where T : IResponseDto;
        /// <summary>
        /// Remove factory for a type of message
        /// </summary>
        /// <typeparam name="T"> The type of message </typeparam>
        /// <param name="factory"> The message factory </param>
        public void RemoveSocketResponse<T>(IResponseFactory<T> factory) where T : IResponseDto;
        /// <summary>
        /// Connect to trading server and listen for it's information
        /// </summary>
        /// <returns></returns>
        Task ConnectAndListen();
        /// <summary>
        /// Disconnect from server
        /// </summary>
        /// <returns></returns>
        Task Disconnect();
        /// <summary>
        /// Check if connected to server
        /// </summary>
        bool IsConnected { get; }
        /// <summary>
        /// Send message to server
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task Send(ITransaction dto);
    }
}
