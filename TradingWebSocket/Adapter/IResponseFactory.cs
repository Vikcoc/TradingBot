using System;
using System.Threading.Tasks;

namespace TradingWebSocket.Adapter
{
    /// <summary>
    /// Factory for server messages of certain type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IResponseFactory<out T> where T : IResponseDto
    {
        /// <summary>
        /// Method to turn server message into usable object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        Task DeserializeObjectAndAct(string obj);
        /// <summary>
        /// Event with the message type
        /// </summary>
        event Func<T, Task> OnValidObject;
    }
}
