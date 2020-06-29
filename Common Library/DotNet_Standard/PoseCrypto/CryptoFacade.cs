using PoseCrypto._AES;
using PoseCrypto._Hash;
using PoseCrypto._RSA;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PoseCrypto
{
    public class CryptoFacade
    {
        #region Singleton Pattern

        private static CryptoFacade _instance;

        public static CryptoFacade Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CryptoFacade();

                return _instance;
            }
        }

        #endregion Singleton Pattern

        private readonly Cryptography<RijndaelManaged> _AESCrypto;

        private RSA _RSACrypto;
        private RSAOAEPKeyExchangeFormatter _RSAFormatter;
        private SHA_256 _SHA256;

        public SHA_256 SHA_256 => _SHA256;

        private CryptoFacade()
        {
            _AESCrypto = new Cryptography<RijndaelManaged>();
            _SHA256 = new SHA_256();
        }

        public void RSA_FromXmlString(string pub_key)
        {
            _RSACrypto = RSACryptoProvider.FromXmlString(pub_key);
            _RSAFormatter = new RSAOAEPKeyExchangeFormatter(_RSACrypto);
        }

        public byte[] GetEncryptedSignature() => Encrypt_RSA(_AESCrypto.Key);

        public byte[] GetEncryptedSignatureIV() => Encrypt_RSA(_AESCrypto.IV);

        #region AES

        public string Encrypt_AES(string input)
        {
            return _AESCrypto.Encrypt(input);
        }

        public byte[] Encrypt_AES(byte[] input)
        {
            return _AESCrypto.Encrypt(input);
        }

        public string Decrypt_AES(string input)
        {
            return _AESCrypto.Decrypt(input);
        }

        public byte[] Decrypt_AES(byte[] input)
        {
            return _AESCrypto.Decrypt(input);
        }

        #endregion AES

        #region RSA

        public byte[] Encrypt_RSA(byte[] input)
        {
            return _RSAFormatter.CreateKeyExchange(input, typeof(RijndaelManaged));
        }

        #endregion RSA
    }
}