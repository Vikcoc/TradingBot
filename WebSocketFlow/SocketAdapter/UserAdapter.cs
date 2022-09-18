using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebSocketFlow.Dto;
using WebSocketFlow.Extra;

namespace WebSocketFlow.SocketAdapter
{
    public class UserAdapter : MarketAdapter, IUserAdapter
    {
        private readonly string _apiKey;
        private readonly string _secretKey;
        public UserAdapter(ISocketAdapter socketAdapter, string apiKey, string secretKey) : base(socketAdapter)
        {
            _apiKey = apiKey;
            _secretKey = secretKey;
        }

        protected override string SocketEndpoint => "wss://stream.crypto.com/v2/user";

        public async Task ConnectListenAndAuthenticate()
        {
            await ConnectAndListen();
            await Send(GetAuthenticationCredentials());
        }

        private SignedTransactionDto GetAuthenticationCredentials()
        {
            var trans = new SignedTransactionDto
            {
                Method = Methods.Authenticate,
                ApiKey = _apiKey,
                Nonce = DateTimeOffset.UtcNow.Subtract(TimeSpan.FromSeconds(20)).ToUnixTimeMilliseconds()
            };

            var sigPayload = Encoding.ASCII.GetBytes(trans.Method + trans.Id + trans.ApiKey + trans.Nonce);

            var hash = new HMACSHA256(Encoding.ASCII.GetBytes(_secretKey));
            var computedHash = hash.ComputeHash(sigPayload);
            trans.Signature = BitConverter.ToString(computedHash).Replace("-", string.Empty);

            return trans;
        }
    }
}
