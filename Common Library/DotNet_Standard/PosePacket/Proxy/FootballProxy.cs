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
    }
}
