//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PosePacket.Proxy
{
    using System;
    
    
    public class FootballProxy
    {
        
        private static string _serviceUrl = "Services/Football.svc";
        
        private static string _P_GET_FIXTURES_BY_DATE = "GetFixturesByDate";
        
        private static string _P_GET_FIXTURES_BY_INDEX = "GetFixturesByIndex";
        
        private static string _P_GET_FIXTURES_BY_LEAGUE = "GetFixturesByLeague";
        
        private static string _P_GET_FIXTURES_BY_TEAM = "GetFixturesByTeam";
        
        private static string _P_GET_MATCH_OVERVIEW = "GetMatchOverview";
        
        private static string _P_GET_MATCH_H2H = "GetMatchH2H";
        
        private static string _P_GET_LEAGUE_OVERVIEW = "GetLeagueOverview";
        
        private static string _P_GET_TEAM_OVERVIEW = "GetTeamOverview";
        
        // Service base url
        public static string ServiceUrl
        {
            get
            {
                return _serviceUrl;
            }
        }
        
        // MethodType: POST, Segment: GetFixturesByDate 
        // InputType: PosePacket.Service.Football.I_GET_FIXTURES_BY_DATE 
        // OutputType: PosePacket.Service.Football.O_GET_FIXTURES_BY_DATE
        public static string P_GET_FIXTURES_BY_DATE
        {
            get
            {
                return _P_GET_FIXTURES_BY_DATE;
            }
        }
        
        // MethodType: POST, Segment: GetFixturesByIndex 
        // InputType: PosePacket.Service.Football.I_GET_FIXTURES_BY_INDEX 
        // OutputType: PosePacket.Service.Football.O_GET_FIXTURES_BY_INDEX
        public static string P_GET_FIXTURES_BY_INDEX
        {
            get
            {
                return _P_GET_FIXTURES_BY_INDEX;
            }
        }
        
        // MethodType: POST, Segment: GetFixturesByLeague 
        // InputType: PosePacket.Service.Football.I_GET_FIXTURES_BY_LEAGUE 
        // OutputType: PosePacket.Service.Football.O_GET_FIXTURES_BY_LEAGUE
        public static string P_GET_FIXTURES_BY_LEAGUE
        {
            get
            {
                return _P_GET_FIXTURES_BY_LEAGUE;
            }
        }
        
        // MethodType: POST, Segment: GetFixturesByTeam 
        // InputType: PosePacket.Service.Football.I_GET_FIXTURES_BY_TEAM 
        // OutputType: PosePacket.Service.Football.O_GET_FIXTURES_BY_TEAM
        public static string P_GET_FIXTURES_BY_TEAM
        {
            get
            {
                return _P_GET_FIXTURES_BY_TEAM;
            }
        }
        
        // MethodType: POST, Segment: GetMatchOverview 
        // InputType: PosePacket.Service.Football.I_GET_MATCH_OVERVIEW 
        // OutputType: PosePacket.Service.Football.O_GET_MATCH_OVERVIEW
        public static string P_GET_MATCH_OVERVIEW
        {
            get
            {
                return _P_GET_MATCH_OVERVIEW;
            }
        }
        
        // MethodType: POST, Segment: GetMatchH2H 
        // InputType: PosePacket.Service.Football.I_GET_MATCH_H2H 
        // OutputType: PosePacket.Service.Football.O_GET_MATCH_H2H
        public static string P_GET_MATCH_H2H
        {
            get
            {
                return _P_GET_MATCH_H2H;
            }
        }
        
        // MethodType: POST, Segment: GetLeagueOverview 
        // InputType: PosePacket.Service.Football.I_GET_LEAGUE_OVERVIEW 
        // OutputType: PosePacket.Service.Football.O_GET_LEAGUE_OVERVIEW
        public static string P_GET_LEAGUE_OVERVIEW
        {
            get
            {
                return _P_GET_LEAGUE_OVERVIEW;
            }
        }
        
        // MethodType: POST, Segment: GetTeamOverview 
        // InputType: PosePacket.Service.Football.I_GET_TEAM_OVERVIEW 
        // OutputType: PosePacket.Service.Football.O_GET_TEAM_OVERVIEW
        public static string P_GET_TEAM_OVERVIEW
        {
            get
            {
                return _P_GET_TEAM_OVERVIEW;
            }
        }
    }
}
