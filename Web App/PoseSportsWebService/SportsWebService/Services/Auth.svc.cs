using LogicCore.Utility;
using PosePacket.Service.Auth;
using SportsWebService.Authentication;
using SportsWebService.Utilities;
using System;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace SportsWebService.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Auth : Contract.IAuth
    {
        public string P_PUBLISHKEY()
        {
            return Singleton.Get<CryptoFacade>().GetPub_Key();
        }

        public async Task<string> P_E_CheckVaildOAuthUser(string e_json)
        {
            var signature = ServerContext.Current.Signature;
            var signatureIV = ServerContext.Current.SignatureIV;

            var input = Singleton.Get<CryptoFacade>()
                            .Decrypt_AES(e_json, signature, signatureIV)
                            .JsonDeserialize<I_CheckVaildOAuthUser>();

            var result = await Commands.Auth.P_E_CheckVaildOAuthUser.Execute(input);

            return Singleton.Get<CryptoFacade>().Encrypt_AES(result.JsonSerialize(), signature, signatureIV);
        }

        public async Task<string> P_E_Login(string e_json)
        {
            var signature = ServerContext.Current.Signature;
            var signatureIV = ServerContext.Current.SignatureIV;

            var input = Singleton.Get<CryptoFacade>()
                            .Decrypt_AES(e_json, signature, signatureIV)
                            .JsonDeserialize<I_Login>();

            var result = await Commands.Auth.P_E_Login.Execute(input);

            return Singleton.Get<CryptoFacade>().Encrypt_AES(result.JsonSerialize(), signature, signatureIV);
        }

        public string P_E_TokenRefresh()
        {
            var result = Commands.Auth.P_E_TokenRefresh.Execute();

            return Singleton.Get<CryptoFacade>().Encrypt_AES(result.JsonSerialize()
                , ServerContext.Current.Signature, ServerContext.Current.SignatureIV);
        }
    }
}