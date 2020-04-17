using PosePacket.Service.HelloWorld;
using SportsWebService.Utilities;
using System.Security.Permissions;
using System.ServiceModel;

namespace SportsWebService.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class HelloWorld : Contract.IHelloWorld
    {
        [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        public O_Hello P_Hello(string i_json)
        {
            var input = i_json.JsonDeserialize<I_Hello>();

            return Commands.HelloWorld.P_Hello.Execute(input);
        }
    }
}