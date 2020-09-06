using PosePacket.Header;
using System;

namespace WebServiceShare.ServiceContext
{
    public class ClientContext
    {
        #region Header

        private static PoseHeader _header;

        public static string Token
        {
            get
            {
                if (_header == null)
                    _header = new PoseHeader();

                _header.eSignature = eSignature;
                _header.eSignatureIV = eSignatureIV;
                _header.eCredentials = eCredentials;

                return _header.ToString();
            }
        }

        #endregion Header

        public static byte[] eSignature { get; set; } = new byte[0];
        public static byte[] eSignatureIV { get; set; } = new byte[0];
        public static byte[] eCredentials { get; set; } = new byte[0];
        public static long UserNo { get; set; } = 0;
        public static DateTime TokenExpireIn { get; set; } = DateTime.MinValue;
        public static DateTime LastLoginTime { get; set; } = DateTime.MinValue;

        public static void SetCredentialsFrom(byte[] token)
        {
            eCredentials = token;
        }
    }
}