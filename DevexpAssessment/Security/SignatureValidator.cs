using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DevexpAssessment.Security
{
    public class SignatureValidator
    {
        private const string _secret = "mySecret";

        public static bool Validate(string message, string signature)
        {
            var keyBytes = Encoding.UTF8.GetBytes(_secret);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            using var hmac = new HMACSHA256(keyBytes);
            var hash = hmac.ComputeHash(messageBytes);
            var computedSignature = Convert.ToHexString(hash).ToLowerInvariant();

            return computedSignature == signature.ToLowerInvariant();
        }
    }
}
