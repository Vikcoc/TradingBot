using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace TransactionSigner
{
    public class TransactionSigner
    {
        private const string API_KEY = "YOUR_API_KEY";
        private const string API_SECRET = "YOUR_API_SECRET";
        private static string GetSign(Dictionary<string, object> request)
        {
            var parameters = (Dictionary<string, string>) request["Params"];

            // Ensure the params are alphabetically sorted by key
            var paramString = string.Join("", parameters.Keys.OrderBy(key => key).Select(key => key + parameters[key]));

            var sigPayload = Encoding.ASCII.GetBytes(request["method"].ToString() + request["id"] + API_KEY + paramString + request["nonce"]);

            var hash = new HMACSHA256(Encoding.ASCII.GetBytes(API_SECRET) );
            var computedHash = hash.ComputeHash(sigPayload);
            return BitConverter.ToString(computedHash).Replace("-", "");
        }
    }
}
