using System;
using System.Threading.Tasks;
using Traders.CryptoCom.Dto;
using TradingWebSocket.Adapter;
using TradingWebSocket.Socket;

namespace Traders.CryptoCom.Socket
{
    public class CryptoComMarketAdapter : MarketAdapter
    {
        private readonly IResponseFactory<HeartbeatTransaction> _rf;
        public CryptoComMarketAdapter(ISocketAdapter socketAdapter) : base(socketAdapter)
        {
            _rf = new CryptoComResponseFactory<HeartbeatTransaction>();
            _rf.OnValidObject += async res =>
            {
                res.Method = CryptoComMethods.HeartbeatRequest;
                await Send(res);
            };
            AddSocketResponse(_rf);
        }

        protected override string SocketEndpoint { get; set; } = CryptoComMethods.Market;

        internal class HeartbeatTransaction : IResponseDto, ICryptoComBaseTransaction, ITransaction
        {
            public static bool CanJson(string json) => json.Contains(CryptoComMethods.HeartbeatResponse);
            public long Id { get; set; }
            public string Method { get; set; } = string.Empty;
        }
    }
}
