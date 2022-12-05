namespace TradingWebSocket.Adapter
{
    /// <summary>
    /// Interface that represents a possible message from trading server
    /// </summary>
    public interface IResponseDto
    {
        /// <summary>
        /// Check if the server message is of this type 
        /// </summary>
        /// <param name="json"> The message to be checked </param>
        /// <returns></returns>
        public static abstract bool CanJson(string json);
    }
}
