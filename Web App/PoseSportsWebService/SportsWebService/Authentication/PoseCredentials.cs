using LogicCore.Utility;
using PosePacket;
using SportsWebService.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Security.Principal;

namespace SportsWebService.Authentication
{
    public class PoseCredentials : IIdentity
    {
        public static readonly long TOKEN_EXPIRE_IN = (long)TimeSpan.FromHours(3).TotalMilliseconds; // 3시간
        public static readonly PoseCredentials Default = new PoseCredentials();

        public static class RowCode
        {
            [Description("Not authenticated credentials")]
            public const int Not_Authenticated_Credentials = ServiceErrorCode.Authenticate.Credentials + 1;
        }

        #region IIdentity

        private bool _isAuthentication;

        public string Name => UserNo.ToString();
        public string AuthenticationType => "Pose_Authenticaion";

        public bool IsAuthenticated
        {
            get
            {
                if (!_isAuthentication)
                    ErrorHandler.OccurException(RowCode.Not_Authenticated_Credentials);

                return _isAuthentication;
            }

            set { _isAuthentication = value; }
        }

        #endregion IIdentity

        private long _userNo;
        private long _expireTime;
        private int _serviceRoleType;

        public long UserNo => _userNo;
        public long ExpireTime => _expireTime;
        public int ServiceRoleType => _serviceRoleType;

        public void SetUserNo(long userNo) => _userNo = userNo;

        public void RefreshExpireTime() => _expireTime = LogicTime.TIME() + PoseCredentials.TOKEN_EXPIRE_IN;

        public void SetServiceRoleType(int serviceRoleType) => _serviceRoleType = serviceRoleType;

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

                // CertifiedTime
                buffer.AddRange(BitConverter.GetBytes(credentials._serviceRoleType));
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

                // CertifiedTime
                credentials._serviceRoleType = BitConverter.ToInt32(buffer, curPosition);
                curPosition += 4;

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