using Newtonsoft.Json;

namespace TradingWebSocket.Adapter
{
    /// <summary>
    /// Message that can be transformed into text for sending to trading server
    /// </summary>
    public interface ITransaction
    {
        /// <summary>
        /// Transforms current object into string message
        /// </summary>
        /// <returns> String representation of this object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
