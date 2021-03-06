﻿using System.Diagnostics;
using System.Security.Cryptography;

namespace PoseCrypto._RSA
{
    public class RSACryptoProvider
    {
        public static RSACryptoServiceProvider Get_RSA(string keyContainerName = null)
        {
            return Create_RSA(keyContainerName);
        }

        public static RSACryptoServiceProvider FromXmlString(string pub_key)
        {
            if (pub_key == null)
                return null;

            var RSAProvider = Create_RSA();
            RSAProvider.FromXmlString(pub_key);

            return RSAProvider;
        }

        public static string ToXmlString(string keyContainerName = null)
        {
            var provider = Create_RSA(keyContainerName);

            if (provider != null)
                return provider.ToXmlString(false);

            return null;
        }

        private static RSACryptoServiceProvider Create_RSA(string keyContainerName = null)
        {
            try
            {
                CspParameters cp = new CspParameters();
                if (string.IsNullOrEmpty(keyContainerName))
                {
                    cp.Flags |= CspProviderFlags.UseDefaultKeyContainer;
                }
                else
                {
                    cp.KeyContainerName = keyContainerName;
                    cp.Flags |= CspProviderFlags.UseMachineKeyStore;
                }

                return new RSACryptoServiceProvider(cp);
            }
            catch (CryptographicException ex)
            {
                Debug.Assert(false, $"RSA Create Exception: {ex.Message}");

                return null;
            }
        }
    }
}