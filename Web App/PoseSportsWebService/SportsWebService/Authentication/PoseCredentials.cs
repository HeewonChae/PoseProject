using LogicCore.Utility;
using PosePacket;
using SportsWebService.Utilities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Principal;

namespace SportsWebService.Authentication
{
    public class PoseCredentials : IIdentity
    {
        public const int TOKEN_EXPIRE_IN = 60 * 60 * 1000; // 1시간
        public static readonly PoseCredentials Default = new PoseCredentials();

        #region IIdentity

        private bool _isAuthentication;

        public string Name => UserNo.ToString();
        public string AuthenticationType => "Pose_Authenticaion";

        public bool IsAuthenticated
        {
            get
            {
                if (!_isAuthentication)
                    ErrorHandler.OccurException(ServiceErrorCode.Authenticate.CredentialsExpire);

                return _isAuthentication;
            }

            set { _isAuthentication = value; }
        }

        #endregion IIdentity

        private long _userNo;
        private long _expireTime;

        public long UserNo => _userNo;
        public long ExpireTime => _expireTime;

        public void SetUserNo(long userNo) => _userNo = userNo;

        public void RefreshExpireTime() => _expireTime = LogicTime.TIME() + PoseCredentials.TOKEN_EXPIRE_IN;

        #region Serialize Methods

        public static byte[] Serialize(PoseCredentials credentials)
        {
            List<byte> buffer = new List<byte>();

            try
            {
                // SessionID
                buffer.AddRange(BitConverter.GetBytes(credentials._userNo));

                // CertifiedTime
                buffer.AddRange(BitConverter.GetBytes(credentials._expireTime));
            }
            catch (Exception)
            {
                ErrorHandler.OccurException(HttpStatusCode.Unauthorized);
            }

            return buffer.ToArray();
        }

        public static PoseCredentials Deserialize(byte[] buffer)
        {
            PoseCredentials credentials = new PoseCredentials();

            try
            {
                int curPosition = 0;

                // SessionID
                credentials._userNo = BitConverter.ToInt64(buffer, curPosition);
                curPosition += 8;

                // CertifiedTime
                credentials._expireTime = BitConverter.ToInt64(buffer, curPosition);
                curPosition += 8;

                if (buffer.Length != curPosition)
                    throw new Exception();
            }
            catch (Exception)
            {
                ErrorHandler.OccurException(HttpStatusCode.Unauthorized);
            }

            return credentials;
        }

        #endregion Serialize Methods
    }
}