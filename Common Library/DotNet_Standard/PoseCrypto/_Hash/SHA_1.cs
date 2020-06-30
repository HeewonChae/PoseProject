using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PoseCrypto._Hash
{
    public class SHA_1
    {
        private string _salt = string.Empty;

        public void SetSalt(string salt)
        {
            _salt = salt;
        }

        public string ComputeHash(string input)
        {
            byte[] hashBytes;

            using (SHA1 sha1Hash = SHA1.Create())
            {
                //From String to byte array
                byte[] sourceBytes = Encoding.UTF8.GetBytes(string.Concat(input, _salt));
                hashBytes = sha1Hash.ComputeHash(sourceBytes);
            }

            return string.Join(
                    string.Empty,
                    hashBytes.Select(x => x.ToString("x2")));
        }
    }
}