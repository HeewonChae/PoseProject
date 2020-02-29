using PosePacket.Header;
using SportsWebService.Authentication;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;
using System.Xml.Serialization;

namespace SportsWebService.WebBehavior.Common.HeaderProcess
{
	public class PoseHeaderMessage : MessageHeader
	{
		public PoseHeader CustomData { get; set; }

		public PoseHeaderMessage()
		{
		}

		public PoseHeaderMessage(PoseHeader customData)
		{
			CustomData = customData;
		}

		public override string Name
		{
			get { return (PoseHeader.HEADER_NAME); }
		}

		public override string Namespace
		{
			get { return (PoseHeader.HEADER_NAMESPACE); }
		}

		public static void ReadHeader(Message request)
		{
			HttpRequestMessageProperty properties = request.Properties["httpRequest"] as HttpRequestMessageProperty;
			var headerString = properties.Headers.Get(PoseHeader.HEADER_NAME);

			PoseHeader headerData = null;
			if (!string.IsNullOrEmpty(headerString))
				headerData = PoseHeader.ParseFromBase64(headerString);

			ServerContext serverContext = new ServerContext(headerData);
			serverContext.Attach(OperationContext.Current);

			#region Legacy Code

			//Int32 headerPosition = request.Headers.FindHeader(HEADER_NAME,"");
			//if (headerPosition == -1)
			//{
			//    serverContext = new ServerContext();
			//    serverContext.Attach(OperationContext.Current);
			//    return;
			//}

			//XmlNode[] content = request.Headers.GetHeader<XmlNode[]>(headerPosition);

			//string text = content[0].InnerText;

			//XmlSerializer deserializer = new XmlSerializer(typeof(PoseHeader));
			//TextReader textReader = new StringReader(text);
			//PoseHeader customData = (PoseHeader)deserializer.Deserialize(textReader);
			//textReader.Close();

			//serverContext = new ServerContext(customData);
			//serverContext.Attach(OperationContext.Current);

			#endregion Legacy Code

			return;
		}

		protected override void OnWriteHeaderContents(
			XmlDictionaryWriter writer, MessageVersion messageVersion)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(PoseHeader));
			StringWriter textWriter = new StringWriter();
			serializer.Serialize(textWriter, CustomData);
			textWriter.Close();

			string text = textWriter.ToString();

			writer.WriteElementString(PoseHeader.HEADER_NAME, PoseHeader.HEADER_NAMESPACE, text.Trim());
		}
	}
}