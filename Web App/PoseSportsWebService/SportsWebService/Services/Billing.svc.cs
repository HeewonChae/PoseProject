using PosePacket.Service.Billing;
using SportsWebService.Services.Contract;
using SportsWebService.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SportsWebService.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Billing" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Billing.svc or Billing.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class Billing : IBilling
    {
        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = "Regular")]
        public Stream P_E_INSERT_IN_APP_BILLING_BY_GOOGLE(Stream e_stream)
        {
            var signature = ServerContext.Current.Signature;
            var signatureIV = ServerContext.Current.SignatureIV;
            var userNo = ServerContext.Current.Credentials.UserNo;

            var input = e_stream.StreamDeserialize<I_E_INSERT_IN_APP_BILLING_BY_GOOGLE>(signature, signatureIV);
            var result = Commands.Billing.P_E_INSERT_IN_APP_BILLING_BY_GOOGLE.Execute(input, userNo);

            return result.SerializeToStream(signature, signatureIV);
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = "Regular")]
        public async Task<Stream> P_E_UPDATE_IN_APP_BILLING_BY_GOOGLE(Stream e_stream)
        {
            var signature = ServerContext.Current.Signature;
            var signatureIV = ServerContext.Current.SignatureIV;
            var userNo = ServerContext.Current.Credentials.UserNo;

            var input = e_stream.StreamDeserialize<I_E_UPDATE_IN_APP_BILLING_BY_GOOGLE>(signature, signatureIV);
            var result = await Commands.Billing.P_E_UPDATE_IN_APP_BILLING_BY_GOOGLE.Execute(input, userNo);

            return result.SerializeToStream(signature, signatureIV);
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = "Regular")]
        public async Task<Stream> P_E_CHECK_MEMBERSHIP_BY_GOOGLE(Stream e_stream)
        {
            var signature = ServerContext.Current.Signature;
            var signatureIV = ServerContext.Current.SignatureIV;
            var userNo = ServerContext.Current.Credentials.UserNo;
            var serviceRoleType = ServerContext.Current.Credentials.ServiceRoleType;

            var input = e_stream.StreamDeserialize<I_E_CHECK_MEMBERSHIP_BY_GOOGLE>(signature, signatureIV);
            var result = await Commands.Billing.P_E_CHECK_MEMBERSHIP_BY_GOOGLE.Execute(input, userNo, serviceRoleType);

            return result.SerializeToStream(signature, signatureIV);
        }
    }
}