using PosePacket;
using SportsWebService.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web;

namespace SportsWebService.Authentication
{
	public class PoseCredentials : IIdentity
	{
		public static readonly TimeSpan EffectiveTime = TimeSpan.FromMinutes(10); // 10분
		public static readonly PoseCredentials Default = new PoseCredentials();

		#region IIdentity Implement
		private bool _isAuthentication;

		public string Name => SessionID;
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
		#endregion

		private string _sessionId; // TODO: 구글 토큰이나.. 단말기 고유번호로 변경..
		private long _certifiedTime;

		public string SessionID => _sessionId;
		public long CertifiedTime => _certifiedTime;
		
		public PoseCredentials()
		{
			_isAuthentication = false;
			_sessionId = Guid.NewGuid().ToString();
			_certifiedTime = LogicCore.Utility.LogicTime.TIME();
		}

		#region Credentials Serializer
		public static byte[] Serialize(PoseCredentials credentials)
		{
			List<byte> buffer = new List<byte>(1024);

			try
			{
				byte[] strBytes = null;

				// SessionID
				strBytes = Encoding.UTF8.GetBytes(credentials._sessionId);
				buffer.AddRange(BitConverter.GetBytes(strBytes.Length));
				buffer.AddRange(strBytes);

				// CertifiedTime
				buffer.AddRange(BitConverter.GetBytes(credentials._certifiedTime));
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
				int strSize = 0;

				// SessionID
				strSize = BitConverter.ToInt32(buffer, curPosition);
				curPosition += 4;
				credentials._sessionId = Encoding.UTF8.GetString(buffer, curPosition, strSize);
				curPosition += strSize;

				// CertifiedTime
				credentials._certifiedTime = BitConverter.ToInt64(buffer, curPosition);
				curPosition += 8;
			}
			catch (Exception)
			{
				ErrorHandler.OccurException(HttpStatusCode.Unauthorized);
			}

			return credentials;
		}
		#endregion
	}
}