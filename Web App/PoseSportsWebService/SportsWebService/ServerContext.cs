using LogicCore.Utility;
using PosePacket.Header;
using SportsWebService.Authentication;
using SportsWebService.Logics;
using SportsWebService.Utilities;
using System.Net;
using System.ServiceModel;

namespace SportsWebService
{
    /// <summary>
    /// This class will act as a custom context, an extension to the OperationContext.
    /// This class holds all context information for our application.
    /// </summary>
    public class ServerContext : IExtension<OperationContext>
    {
        public const string CONTEXT_PROPERTY_NAME = "ServerContext";

        internal byte[] _eSignature;
        internal byte[] _eSignatureIV;
        internal byte[] _eCredentials;

        public PoseCredentials Credentials { get; private set; }

        #region Crypto

        private byte[] _signature;
        private byte[] _signatureIV;

        public byte[] Signature => _signature;
        public byte[] SignatureIV => _signatureIV;

        public void SetSignature(byte[] signature) => _signature = signature;

        public void SetSignatureIV(byte[] signatureIV) => _signatureIV = signatureIV;

        #endregion Crypto

        public static bool IsExistData => OperationContext.Current.IncomingMessageProperties.ContainsKey(CONTEXT_PROPERTY_NAME);

        public static ServerContext Current => OperationContext.Current.IncomingMessageProperties[CONTEXT_PROPERTY_NAME] as ServerContext;

        public ServerContext(PoseHeader header)
        {
            if (header == null)
                return;

            _eSignature = header.eSignature;
            _eSignatureIV = header.eSignatureIV;
            _eCredentials = header.eCredentials;
        }

        public PoseCredentials DecryptCredentials()
        {
            if (_eCredentials == null || _eCredentials.Length == 0)
            {
                if (_eSignature != null && _eSignature.Length >= 0
                && _eSignatureIV != null && _eSignatureIV.Length >= 0)
                {
                    byte[] signature = Singleton.Get<CryptoFacade>().Decrypt_RSA(_eSignature);
                    byte[] signatureIV = Singleton.Get<CryptoFacade>().Decrypt_RSA(_eSignatureIV);
                    SetSignature(signature);
                    SetSignatureIV(signatureIV);
                }
                return Credentials = PoseCredentials.Default;
            }

            try
            {
                byte[] signature = Singleton.Get<CryptoFacade>().Decrypt_RSA(_eSignature);
                byte[] signatureIV = Singleton.Get<CryptoFacade>().Decrypt_RSA(_eSignatureIV);
                byte[] credentials = Singleton.Get<CryptoFacade>().Decrypt_RSA(_eCredentials);

                Credentials = PoseCredentials.Deserialize(credentials);
                SetSignature(signature);
                SetSignatureIV(signatureIV);
            }
            catch
            {
                ErrorHandler.OccurException(HttpStatusCode.Unauthorized);
            }

            return Credentials;
        }

        #region For MessageHeader Behavior

        public void Attach(OperationContext owner)
        {
            owner.IncomingMessageProperties.Add(CONTEXT_PROPERTY_NAME, this);
        }

        public void Detach(OperationContext owner)
        {
            owner.IncomingMessageProperties.Remove(CONTEXT_PROPERTY_NAME);
        }

        #endregion For MessageHeader Behavior
    }
}