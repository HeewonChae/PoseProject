using PacketBuilder.CodeDom;
using System;
using System.Linq;

namespace PacketBuilder
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			// Web Service Proxy
			var webServiceAssembly = typeof(SportsWebService.Global).Assembly;

			foreach (var definedType in webServiceAssembly.DefinedTypes)
			{
				var attrData = definedType.CustomAttributes.Where(elem => elem.AttributeType.Name.Equals("ServiceContractAttribute")).FirstOrDefault();
				if (attrData != null)
				{
					string serviceName = (string)attrData.NamedArguments[0].TypedValue.Value;
					ProxyBuilder proxyBuilder = new ProxyBuilder(definedType, serviceName, "PosePacket.Proxy");
					proxyBuilder.AddFields();
					proxyBuilder.AddProperties();
					proxyBuilder.GenerateCSharpCode();
				}
			}

			// ErrorCode Message
			ErrorCodeDescriptionBuilder errorCodeBuilder = new ErrorCodeDescriptionBuilder(webServiceAssembly);
			errorCodeBuilder.ParseErrorCode();
			errorCodeBuilder.GenerateJsonFile();

			Console.WriteLine("Complete!!!");
			Console.ReadLine();
		}
	}
}