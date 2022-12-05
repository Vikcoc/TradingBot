using System;
using System.Threading.Tasks;

namespace TradingWebSocket.Socket
{
    /// <summary>
    /// Interface to implement for making socket connections
    /// </summary>
    public interface ISocketAdapter
    {
        /// <summary>
        /// Connect to server
        /// </summary>
        /// <param name="path"> Url of server </param>
        /// <returns></returns>
        public Task Connect(string path);
        /// <summary>
        /// Disconnect from server
        /// </summary>
        /// <returns></returns>
        public Task Disconnect();
        /// <summary>
        /// Listen for messages from server
        /// </summary>
        /// <returns></returns>
        public Task StartListening();
        /// <summary>
        /// Check if socket is connected
        /// </summary>
        public bool IsConnected { get; }
        /// <summary>
        /// Event for taking action on the received message
        /// </summary>
        public event Func<string, Task> OnReceive;
        /// <summary>
        /// Send message to server
        /// </summary>
        /// <param name="dto">The message as a string representing text</param>
        /// <returns></returns>
        public Task Send(string dto);
    }
}
