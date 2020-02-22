using PosePacket.Header;

namespace WebServiceShare.ServiceContext
{
	public class ClientContext
	{
		public static byte[] eSignature { get; set; } = new byte[0];
		public static byte[] eSignatureIV { get; set; } = new byte[0];
		public static byte[] eCredentials { get; set; } = new byte[0];

		public static PoseHeader CopyTo(PoseHeader header)
		{
			header.eSignature = eSignature;
			header.eSignatureIV = eSignatureIV;
			header.eCredentials = eCredentials;

			return header;
		}
	}
}