using PosePacket.Header;
using System;

namespace WebServiceShare.ServiceContext
{
    public class ClientContext
    {
        private static PoseHeader _header = new PoseHeader();
        public static byte[] eSignature { get; set; } = new byte[0];
        public static byte[] eSignatureIV { get; set; } = new byte[0];
        public static byte[] eCredentials { get; set; } = new byte[0];
        public static DateTime TokenExpireIn { get; set; } = DateTime.UtcNow;

        public static PoseHeader MakeHeader()
        {
            _header.eSignature = eSignature;
            _header.eSignatureIV = eSignatureIV;
            _header.eCredentials = eCredentials;

            return _header;
        }

        public static void SetCredentialsFrom(string token)
        {
            eCredentials = System.Convert.FromBase64String(token);
        }
    }
}