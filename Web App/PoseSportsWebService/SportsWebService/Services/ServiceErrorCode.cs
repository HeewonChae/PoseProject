namespace SportsWebService.Services
{
    public static class ServiceErrorCode
    {
        public static class Root
        {
            public const int Authenticate = 100000000;
            public const int WebService = 200000000;
            public const int Mysql = 300000000;
            public const int Redis = 400000000;

            public const int Max = 900000000;
        };

        public static class Authenticate
        {
            public const int Credentials = Root.Authenticate + 01000000;
            public const int Principal = Root.Authenticate + 02000000;
        }

        public static class WebService
        {
            public const int HelloWorld = Root.WebService + 01000000;
            public const int Auth = Root.WebService + 02000000;
            public const int Football = Root.WebService + 03000000;
            public const int Billing = Root.WebService + 04000000;

            public const int Max = Root.Max + 99000000;
        };

        public static class Database
        {
            public const int GlobalDB = Root.Mysql + 01000000;
            public const int FootballDB = Root.Mysql + 02000000;

            public const int Max = Root.Max + 90000000;
        }

        public static class Redis
        {
        }

        public static class WebMethod_Auth
        {
            public const int P_E_CheckVaildOAuthUser = WebService.Auth + 001000;
            public const int P_E_Login = WebService.Auth + 002000;
            public const int P_E_TokenRefresh = WebService.Auth + 003000;
        }

        public static class WebMethod_Billing
        {
            public const int P_E_INSERT_IN_APP_BILLING_BY_GOOGLE = WebService.Billing + 001000;
            public const int P_E_UPDATE_IN_APP_BILLING_BY_GOOGLE = WebService.Billing + 002000;
            public const int P_E_CHECK_MEMBERSHIP_BY_GOOGLE = WebService.Billing + 003000;
        }

        public static class WebMethod_Football
        {
            public const int P_GET_FIXTURES_BY_DATE = WebService.Football + 001000;
            public const int P_GET_FIXTURES_BY_INDEX = WebService.Football + 002000;
            public const int P_GET_MATCH_OVERVIEW = WebService.Football + 003000;
            public const int P_GET_LEAGUE_OVERVIEW = WebService.Football + 004000;
            public const int P_GET_FIXTURES_BY_LEAGUE = WebService.Football + 005000;
            public const int P_GET_FIXTURES_BY_TEAM = WebService.Football + 006000;
            public const int P_GET_TEAM_OVERVIEW = WebService.Football + 007000;
            public const int P_GET_MATCH_H2H = WebService.Football + 008000;
            public const int P_GET_MATCH_ODDS = WebService.Football + 009000;
            public const int P_GET_MATCH_PREDICTIONS = WebService.Football + 010000;
        }

        public static class WebMethod_HelloWorld
        {
            public const int P_Hello = WebService.HelloWorld + 001000;
        }

        public static class StoredProcedure_Global
        {
            public const int P_E_CheckVaildOAuthUser = Database.GlobalDB + 001000;
            public const int P_E_Login = Database.GlobalDB + 002000;
            public const int P_INSERT_IN_APP_BILLING = Database.GlobalDB + 003000;
            public const int P_UPDATE_IN_APP_BILLING = Database.GlobalDB + 004000;
            public const int P_SELECT_LINKED_BILLING = Database.GlobalDB + 005000;
            public const int P_UPDATE_USER_ROLE = Database.GlobalDB + 006000;
        }

        public static class StoredProcedure_Football
        {
            public const int P_GET_FIXTURES_BY_DATE = Database.FootballDB + 001000;
            public const int P_GET_FIXTURES_BY_INDEX = Database.FootballDB + 002000;
            public const int P_GET_MATCH_OVERVIEW = Database.FootballDB + 003000;
            public const int P_GET_LEAGUE_OVERVIEW = Database.FootballDB + 004000;
            public const int P_GET_FIXTURES_BY_LEAGUE = Database.FootballDB + 005000;
            public const int P_GET_FIXTURES_BY_TEAM = Database.FootballDB + 006000;
            public const int P_GET_TEAM_OVERVIEW = Database.FootballDB + 007000;
            public const int P_GET_MATCH_H2H = Database.FootballDB + 008000;
            public const int P_GET_MATCH_ODDS = Database.FootballDB + 009000;
            public const int P_GET_MATCH_PREDICTIONS = Database.FootballDB + 010000;
            public const int P_GET_MATCH_VIP = Database.FootballDB + 011000;
            public const int P_GET_MATCH_VIP_HISTORY = Database.FootballDB + 012000;
        }

        public static class Procedure
        {
        }
    }
}