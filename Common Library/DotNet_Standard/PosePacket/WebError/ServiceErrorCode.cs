namespace PosePacket
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
            public const int CredentialsExpire = Root.Authenticate + 1;
        }

        public static class WebService
        {
            public const int HelloWorld = Root.WebService + 01000000;
            public const int Auth = Root.WebService + 02000000;

            public const int Max = Root.Max + 99000000;
        };

        public static class Database
        {
            public const int FootballDB = Root.Mysql + 10000000;

            public const int Max = Root.Max + 90000000;
        }

        public static class Redis
        {
        }

        public static class WebMethod_Auth
        {
            public const int P_E_CheckVaildOAuthUser = WebService.Auth + 0001000;

            public const int P_E_Login = WebService.Auth + 0002000;

            public const int P_E_TokenRefresh = WebService.Auth + 0003000;
        }

        public static class WebMethod_HelloWorld
        {
            public const int P_Hello = WebService.HelloWorld + 0001000;
        }

        public static class Procedure
        {
        }
    }
}