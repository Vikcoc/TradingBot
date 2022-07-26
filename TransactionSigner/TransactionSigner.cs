using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TransactionMaker;

namespace TransactionSigner
{
    public class TransactionSigner
    {
        private const string API_KEY = "YOUR_API_KEY";
        private const string API_SECRET = "YOUR_API_SECRET";
        private static string GetSign(TransactionTemplate request)
        {
            var parameters = request.Params;

            // Ensure the params are alphabetically sorted by key
            var paramString = string.Join("", parameters.Keys.OrderBy(key => key).Select(key => key + parameters[key]));

            var sigPayload = Encoding.ASCII.GetBytes(request.Method + request.Id + API_KEY + paramString + request.Nonce);

            var hash = new HMACSHA256(Encoding.ASCII.GetBytes(API_SECRET) );
            var computedHash = hash.ComputeHash(sigPayload);
            return BitConverter.ToString(computedHash).Replace("-", "");
        }
    }
}
