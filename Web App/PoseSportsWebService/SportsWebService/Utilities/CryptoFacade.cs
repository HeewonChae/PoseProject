using LogicCore.Utility;
using PoseCrypto._AES;
using PoseCrypto._Hash;
using PoseCrypto._RSA;
using SportsWebService.Authentication;
using SportsWebService.Logics;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace SportsWebService.Utilities
{
    public class CryptoFacade : Singleton.INode
    {
        private readonly RSACryptoServiceProvider _RSACrypto;
        private readonly Cryptography<RijndaelManaged> _AESCrypto;
        private readonly RSAOAEPKeyExchangeFormatter _RSAFormatter;
        private readonly RSAOAEPKeyExchangeDeformatter _RSADeformatter;
        private readonly SHA_256 _SHA256;

        public SHA_256 SHA_256 => _SHA256;

        public CryptoFacade()
        {
            string keyContainerName = ConfigurationManager.AppSettings["RSAKeyContainer"];
            _RSACrypto = RSACryptoProvider.Get_RSA(keyContainerName);
            _AESCrypto = new Cryptography<RijndaelManaged>();
            _RSAFormatter = new RSAOAEPKeyExchangeFormatter(_RSACrypto);
            _RSADeformatter = new RSAOAEPKeyExchangeDeformatter(_RSACrypto);

            string hashSalt = ConfigurationManager.AppSettings["Hash_Salt"];
            _SHA256 = new SHA_256();
            _SHA256.SetSalt(hashSalt);
        }

        #region AES

        public string Encrypt_AES(string input, byte[] signature, byte[] signatureIV)
        {
            if (string.IsNullOrEmpty(input)
                || signature == null
                || signatureIV == null)
                ErrorHandler.OccurException(System.Net.HttpStatusCode.BadRequest);

            return _AESCrypto.Encrypt(input, signature, signatureIV);
        }

        public byte[] Encrypt_AES(byte[] input, byte[] signature, byte[] signatureIV)
        {
            if (input == null || input.Length == 0
                || signature == null
                || signatureIV == null)
                ErrorHandler.OccurException(System.Net.HttpStatusCode.BadRequest);

            return _AESCrypto.Encrypt(input, signature, signatureIV);
        }

        public string Decrypt_AES(string input, byte[] signature, byte[] signatureIV)
        {
            if (string.IsNullOrEmpty(input)
                || signature == null
                || signatureIV == null)
                ErrorHandler.OccurException(System.Net.HttpStatusCode.BadRequest);

            return _AESCrypto.Decrypt(input, signature, signatureIV);
        }

        public byte[] Decrypt_AES(byte[] input, byte[] signature, byte[] signatureIV)
        {
            if (input == null || input.Length == 0
                || signature == null
                || signatureIV == null)
                ErrorHandler.OccurException(System.Net.HttpStatusCode.BadRequest);

            return _AESCrypto.Decrypt(input, signature, signatureIV);
        }

        #endregion AES

        #region RSA

        public string GetPub_Key()
        {
            return _RSACrypto.ToXmlString(false);
        }

        public byte[] Encrypt_RSA(byte[] input)
        {
            if (input == null || input.Length == 0)
                ErrorHandler.OccurException(System.Net.HttpStatusCode.BadRequest);

            return _RSAFormatter.CreateKeyExchange(input, typeof(RijndaelManaged));
        }

        public byte[] Decrypt_RSA(byte[] input)
        {
            if (input == null || input.Length == 0)
                ErrorHandler.OccurException(System.Net.HttpStatusCode.BadRequest);

            return _RSADeformatter.DecryptKeyExchange(input);
        }

        #endregion RSA
    }
}