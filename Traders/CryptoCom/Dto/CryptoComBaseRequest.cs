using TradingWebSocket.Adapter;

namespace Traders.CryptoCom.Dto
{
    internal abstract class CryptoComBaseRequest : ICryptoComBaseTransaction, ITransaction
    {
        private static long _id;
        protected CryptoComBaseRequest()
        {
            _id++;
            Id = _id;
        }
        public long Id { get; set; }
        public string Method { get; set; } = string.Empty;
    }
}
