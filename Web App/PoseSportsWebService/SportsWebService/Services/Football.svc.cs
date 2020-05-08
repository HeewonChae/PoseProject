using PosePacket.Service.Football;
using SportsWebService.Logics;
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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Football" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Football.svc or Football.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class Football : IFootball
    {
        [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        public Stream P_GET_FIXTURES_BY_DATE(Stream stream)
        {
            var input = stream.StreamDeserialize<I_GET_FIXTURES_BY_DATE>();
            var output = Commands.Football.P_GET_FIXTURES_BY_DATE.Execute(input);

            return output.SerializeToStream();
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        public Stream P_GET_FIXTURES_BY_INDEX(Stream stream)
        {
            var input = stream.StreamDeserialize<I_GET_FIXTURES_BY_INDEX>();
            var output = Commands.Football.P_GET_FIXTURES_BY_INDEX.Execute(input);

            return output.SerializeToStream();
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        public Stream P_GET_FIXTURES_BY_LEAGUE(Stream stream)
        {
            var input = stream.StreamDeserialize<I_GET_FIXTURES_BY_LEAGUE>();
            var output = Commands.Football.P_GET_FIXTURES_BY_LEAGUE.Execute(input);

            return output.SerializeToStream();
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        public Stream P_GET_FIXTURES_BY_TEAM(Stream stream)
        {
            var input = stream.StreamDeserialize<I_GET_FIXTURES_BY_TEAM>();
            var output = Commands.Football.P_GET_FIXTURES_BY_TEAM.Execute(input);

            return output.SerializeToStream();
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        public Stream P_GET_MATCH_OVERVIEW(Stream stream)
        {
            var input = stream.StreamDeserialize<I_GET_MATCH_OVERVIEW>();
            var output = Commands.Football.P_GET_MATCH_OVERVIEW.Execute(input);

            return output.SerializeToStream();
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        public Stream P_GET_LEAGUE_OVERVIEW(Stream stream)
        {
            var input = stream.StreamDeserialize<I_GET_LEAGUE_OVERVIEW>();

            var output = Commands.Football.P_GET_LEAGUE_OVERVIEW.Execute(input);

            return output.SerializeToStream();
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        public Stream P_GET_TEAM_OVERVIEW(Stream stream)
        {
            var input = stream.StreamDeserialize<I_GET_TEAM_OVERVIEW>();

            var output = Commands.Football.P_GET_TEAM_OVERVIEW.Execute(input);

            return output.SerializeToStream();
        }

        #region For Benchmark

        //[PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        //public O_GET_FIXTURES_BY_DATE P_GET_FIXTURES_BY_DATE_JSON(string i_json)
        //{
        //    var input = i_json.JsonDeserialize<I_GET_FIXTURES_BY_DATE>();

        //    return Commands.Football.P_GET_FIXTURES_BY_DATE.Execute(input);
        //}

        //[PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        //public Stream P_GET_FIXTURES_BY_DATE_MessagePack(Stream stream)
        //{
        //    var input = stream.StreamDeserialize<I_GET_FIXTURES_BY_DATE>();

        //    var output = Commands.Football.P_GET_FIXTURES_BY_DATE.Execute(input);

        //    return output.SerializeToStream();
        //}

        #endregion For Benchmark
    }
}