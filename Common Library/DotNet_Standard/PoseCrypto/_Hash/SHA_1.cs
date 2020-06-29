using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PoseCrypto._Hash
{
    public class SHA_1
    {
        public string ComputeHash(string input)
        {
            using (SHA1 sha1Hash = SHA1.Create())
            {
                //From String to byte array
                byte[] sourceBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha1Hash.ComputeHash(sourceBytes);
                return BitConverter.ToString(hashBytes).Replace("-", String.Empty);
            }
        }
    }
}