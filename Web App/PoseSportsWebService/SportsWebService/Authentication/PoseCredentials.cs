using LogicCore.Utility;
using PosePacket;
using SportsWebService.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Security.Principal;
using System.Text;

namespace SportsWebService.Authentication
{
	public class PoseCredentials : IIdentity
	{
		public const int TOKEN_EXPIRE_IN = 60 * 60 * 1000; // 1시간
		public static readonly PoseCredentials Default = new PoseCredentials();

		#region IIdentity Implement

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

		#endregion IIdentity Implement

		private long _userNo;
		private long _expireTick;

		public long UserNo => _userNo;
		public long ExpireTime => _expireTick;

		public PoseCredentials()
		{
			_isAuthentication = false;
			_userNo = 0;
			_expireTick = PoseCredentials.TOKEN_EXPIRE_IN;
		}

		#region Static Methods

		public static byte[] Serialize(PoseCredentials credentials)
		{
			List<byte> buffer = new List<byte>(1024);

			try
			{
				// SessionID
				buffer.AddRange(BitConverter.GetBytes(credentials._userNo));

				// CertifiedTime
				buffer.AddRange(BitConverter.GetBytes(credentials._expireTick));
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
				credentials._expireTick = BitConverter.ToInt64(buffer, curPosition);
				curPosition += 8;
			}
			catch (Exception)
			{
				ErrorHandler.OccurException(HttpStatusCode.Unauthorized);
			}

			return credentials;
		}

		#endregion Static Methods
	}
}