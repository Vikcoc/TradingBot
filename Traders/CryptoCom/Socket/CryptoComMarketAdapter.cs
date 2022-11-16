using System;
using System.Threading.Tasks;
using TradingWebSocket.Adapter;
using TradingWebSocket.Socket;

namespace Traders.CryptoCom.Socket
{
    public class CryptoComMarketAdapter : MarketAdapter
    {
        public CryptoComMarketAdapter(ISocketAdapter socketAdapter) : base(socketAdapter)
        {
        }

        protected override string SocketEndpoint { get; set; } = CryptoComMethods.Market;

        public override Task ConnectAndListen()
        {
            AddResponseCallback<HeartbeatResponse>(async res =>
            {
                res.Method = CryptoComMethods.HeartbeatRequest;
                await Send(res);
            });
            return base.ConnectAndListen();
        }

        internal class HeartbeatResponse : IResponseDto, ICryptoComBaseResponse, ITransaction
        {
            public static bool CanJson(string json) => json.Contains(CryptoComMethods.HeartbeatResponse);
            public long Id { get; set; }
            public string Method { get; set; } = string.Empty;
        }
    }
}
