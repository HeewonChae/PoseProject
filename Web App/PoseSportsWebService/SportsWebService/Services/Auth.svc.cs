using LogicCore.Utility;
using MessagePack;
using PosePacket.Service.Auth;
using SportsWebService.Authentication;
using SportsWebService.Logics;
using SportsWebService.Utilities;
using System;
using System.IO;
using System.Security.Permissions;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Xml;

namespace SportsWebService.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class Auth : Contract.IAuth
    {
        public Stream P_PUBLISH_KEY()
        {
            var pub_key = Singleton.Get<CryptoFacade>().GetPub_Key();

            return pub_key.SerializeToStream();
        }

        public Stream P_E_TokenRefresh()
        {
            var signature = ServerContext.Current.Signature;
            var signatureIV = ServerContext.Current.SignatureIV;

            var result = Commands.Auth.P_E_TokenRefresh.Execute();

            return result.SerializeToStream(signature, signatureIV);
        }

        public async Task<Stream> P_E_CheckVaildOAuthUser(Stream e_stream)
        {
            var signature = ServerContext.Current.Signature;
            var signatureIV = ServerContext.Current.SignatureIV;

            var input = e_stream.StreamDeserialize<I_CheckVaildOAuthUser>(signature, signatureIV);
            var result = await Commands.Auth.P_E_CheckVaildOAuthUser.Execute(input);

            return result.SerializeToStream(signature, signatureIV);
        }

        public Stream P_E_Login(Stream e_stream)
        {
            var signature = ServerContext.Current.Signature;
            var signatureIV = ServerContext.Current.SignatureIV;

            var input = e_stream.StreamDeserialize<I_Login>(signature, signatureIV);
            var result = Commands.Auth.P_E_Login.Execute(input);

            return result.SerializeToStream(signature, signatureIV);
        }
    }
}