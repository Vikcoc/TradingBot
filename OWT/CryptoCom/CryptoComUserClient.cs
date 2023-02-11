using Newtonsoft.Json;
using OWT.CryptoCom.Dto;
using OWT.SocketClient;
using System.Security.Cryptography;
using System.Text;

namespace OWT.CryptoCom
{
    public class CryptoComUserClient : CryptoComMarketClient
    {
        private readonly string _apiKey;
        private readonly string _secretKey;


        public CryptoComUserClient(ISocketClient socketClient, string apiKey, string secretKey) : base(socketClient)
        {
            _apiKey = apiKey;
            _secretKey = secretKey;
        }

        protected override string SocketEndpoint => "wss://stream.crypto.com/v2/user";

        public async Task Authenticate(CancellationToken token)
        {
            var trans = new CryptoComSignedTransaction
            {
                Method = "public/auth",
                ApiKey = _apiKey,
                Nonce = DateTimeOffset.UtcNow.Subtract(TimeSpan.FromSeconds(20)).ToUnixTimeMilliseconds()
            };

            var sigPayload = Encoding.ASCII.GetBytes(trans.Method + trans.Id + trans.ApiKey + trans.Nonce);

            var hash = new HMACSHA256(Encoding.ASCII.GetBytes(_secretKey));
            var computedHash = hash.ComputeHash(sigPayload);
            trans.Signature = BitConverter.ToString(computedHash).Replace("-", string.Empty);

            await Send(JsonConvert.SerializeObject(trans), token);
        }

    }
}
