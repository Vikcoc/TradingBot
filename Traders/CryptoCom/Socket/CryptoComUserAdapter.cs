using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Traders.CryptoCom.Data;
using Traders.CryptoCom.Dto;
using TradingWebSocket.Socket;

namespace Traders.CryptoCom.Socket
{
    public class CryptoComUserAdapter : CryptoComMarketAdapter
    {
        private readonly string _apiKey;
        private readonly string _secretKey;
        public CryptoComUserAdapter(ISocketAdapter socketAdapter, string apiKey, string secretKey) : base(socketAdapter)
        {
            _apiKey = apiKey;
            _secretKey = secretKey;
        }

        protected override string SocketEndpoint { get; set; } = CryptoComMethods.User;

        public override async Task ConnectAndListen()
        {
            await base.ConnectAndListen();
            await this.Send(GetAuthenticationCredentials());
        }


        private SignedTransactionDto GetAuthenticationCredentials()
        {
            var trans = new SignedTransactionDto
            {
                Method = CryptoComMethods.Authenticate,
                ApiKey = _apiKey,
                Nonce = DateTimeOffset.UtcNow.Subtract(TimeSpan.FromSeconds(20)).ToUnixTimeMilliseconds()
            };

            var sigPayload = Encoding.ASCII.GetBytes(trans.Method + trans.Id + trans.ApiKey + trans.Nonce);

            var hash = new HMACSHA256(Encoding.ASCII.GetBytes(_secretKey));
            var computedHash = hash.ComputeHash(sigPayload);
            trans.Signature = BitConverter.ToString(computedHash).Replace("-", string.Empty);

            return trans;
        }

        internal class SignedTransactionDto : CryptoComBaseRequest
        {
            [JsonProperty("api_key")]
            public string ApiKey { get; set; } = string.Empty;
            [JsonProperty("sig")]
            public string Signature { get; set; } = string.Empty;
            [JsonProperty("nonce")]
            public long Nonce { get; set; }
        }
    }
}
