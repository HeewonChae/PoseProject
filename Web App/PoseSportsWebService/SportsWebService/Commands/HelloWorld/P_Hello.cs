using PosePacket;
using PosePacket.Service.HelloWorld;
using SportsWebService.Infrastructure;
using SportsWebService.Utilities;
using System.ComponentModel;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace SportsWebService.Commands.HelloWorld
{
    [WebModelType(InputType = typeof(I_Hello), OutputType = typeof(O_Hello))]
    public static class P_Hello
    {
        public class RowCode
        {
            [Description("Invalid name value")]
            public const int InvalidInputValue = ServiceErrorCode.WebMethod_HelloWorld.P_Hello + 1;
        }

        public static O_Hello Execute(I_Hello input)
        {
            if (string.IsNullOrEmpty(input.Name))
                ErrorHandler.OccurException(RowCode.InvalidInputValue);

            var remoteEndPoint = ((RemoteEndpointMessageProperty)OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name]);

            var output = new O_Hello()
            {
                Name = input.Name,
                Address = $"{remoteEndPoint.Address}:{remoteEndPoint.Port}",
            };

            return output;
        }
    }
}