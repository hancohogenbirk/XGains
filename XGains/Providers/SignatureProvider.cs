using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace XGains.Providers
{
    public class SignatureProvider : ISignatureProvider
    {
        public string SignRequest(string endpoint, ref Dictionary<string, string> urlParameters, string base64EncodedSecret)
        {
            var nonce = DateTime.UtcNow.Ticks;
            urlParameters.Add(nameof(nonce), nonce.ToString());
            urlParameters = urlParameters
                .Reverse()
                .ToDictionary(x => x.Key, x => x.Value);

            var stringParameters = string.Join(
                "&",
                urlParameters.Select(x => $"{x.Key}={x.Value}"));
            var hashData = $"{nonce}{stringParameters}";

            var pathBytes = Encoding.UTF8.GetBytes(endpoint);
            var bytes = pathBytes.Concat(ComputeSha256Hash(hashData)).ToArray();
            var base64DecodedSecret = Convert.FromBase64String(base64EncodedSecret);
            var sha512HashedData = new HMACSHA512(base64DecodedSecret).ComputeHash(bytes);

            return Convert.ToBase64String(sha512HashedData);
        }

        private static byte[] ComputeSha256Hash(string rawData)
        {
            using SHA256 sha256Hash = SHA256.Create();
            return sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
        }
    }
}
