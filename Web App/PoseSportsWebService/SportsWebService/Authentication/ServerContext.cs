using PosePacket.Header;
using System.ServiceModel;

namespace SportsWebService.Authentication
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

		// Get the current one from the extensions that are added to OperationContext.
		public static ServerContext Current
		{
			get
			{
				return OperationContext.Current.IncomingMessageProperties[CONTEXT_PROPERTY_NAME] as ServerContext;
			}
		}

		public static bool IsExistData => OperationContext.Current.IncomingMessageProperties.ContainsKey(CONTEXT_PROPERTY_NAME);

		public ServerContext(PoseHeader header)
		{
			if (header == null)
				return;

			_eSignature = header.eSignature;
			_eSignatureIV = header.eSignatureIV;
			_eCredentials = header.eCredentials;
		}

		public PoseCredentials CreateCredentials()
		{
			return Credentials = new PoseCredentials();
		}

		public PoseCredentials DecryptCredentials()
		{
			if (_eCredentials == null || _eCredentials.Length == 0)
			{
				Credentials = PoseCredentials.Default;
				return Credentials;
			}

			return Credentials = PoseCredentials.Deserialize(_eCredentials);
		}

		public byte[] EncryptCredentials()
		{
			return _eCredentials = PoseCredentials.Serialize(Credentials);
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