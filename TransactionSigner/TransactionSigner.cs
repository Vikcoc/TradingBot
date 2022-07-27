using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TransactionMaker;

namespace TransactionSigner
{
    public class TransactionSigner
    {
        private readonly string _apiKey;
        private readonly string _apiSecret;

        public TransactionSigner(string apiKey, string apiSecret)
        {
            _apiKey = apiKey;
            _apiSecret = apiSecret;
        }

        public string GetSign(TransactionTemplate request)
        {
            var parameters = request.Params;

            // Ensure the params are alphabetically sorted by key
            var paramString = string.Join("", parameters.Keys.OrderBy(key => key).Select(key => key + parameters[key]));

            var sigPayload = Encoding.ASCII.GetBytes(request.Method + request.Id + _apiKey + paramString + request.Nonce);

            var hash = new HMACSHA256(Encoding.ASCII.GetBytes(_apiSecret) );
            var computedHash = hash.ComputeHash(sigPayload);
            return BitConverter.ToString(computedHash).Replace("-", "");
        }
    }
}
