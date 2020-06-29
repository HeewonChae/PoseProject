using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PoseCrypto._Hash
{
    public class SHA_256
    {
        public string ComputeHash(string input)
        {
            using (SHA256 sha1Hash = SHA256.Create())
            {
                //From String to byte array
                byte[] sourceBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha1Hash.ComputeHash(sourceBytes);

                return string.Join(
                    string.Empty,
                    hashBytes.Select(x => x.ToString("x2")));
            }
        }
    }
}