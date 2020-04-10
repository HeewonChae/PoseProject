using PosePacket.Service.Football;
using SportsWebService.Services.Contract;
using SportsWebService.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SportsWebService.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Football" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Football.svc or Football.svc.cs at the Solution Explorer and start debugging.
    public class Football : IFootball
    {
        [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        public O_GET_FIXTURES_BY_DATE P_GET_FIXTURES_BY_DATE(string i_json)
        {
            var input = i_json.JsonDeserialize<I_GET_FIXTURES_BY_DATE>();

            return Commands.Football.P_GET_FIXTURES_BY_DATE.Execute(input);
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        public O_GET_FIXTURES_BY_INDEX P_GET_FIXTURES_BY_INDEX(string i_json)
        {
            var input = i_json.JsonDeserialize<I_GET_FIXTURES_BY_INDEX>();

            return Commands.Football.P_GET_FIXTURES_BY_INDEX.Execute(input);
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        public async Task<O_GET_MATCH_OVERVIEW> P_GET_MATCH_OVERVIEW(string i_json)
        {
            var input = i_json.JsonDeserialize<I_GET_MATCH_OVERVIEW>();

            return await Commands.Football.P_GET_MATCH_OVERVIEW.Execute(input);
        }
    }
}