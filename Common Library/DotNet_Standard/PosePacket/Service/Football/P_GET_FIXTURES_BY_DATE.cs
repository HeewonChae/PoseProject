using System;
using System.Collections.Generic;
using System.Text;

namespace PosePacket.Service.Football
{
    public class I_GET_FIXTURES_BY_DATE
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class O_GET_FIXTURES_BY_DATE
    {
        public class FixtureInfo
        {
            public class DataInfo
            {
                public string Name { get; set; }
                public string Logo { get; set; }
            }

            public DataInfo Country { get; set; }
            public DataInfo League { get; set; }
            public DataInfo HomeTeam { get; set; }
            public DataInfo AwayTeam { get; set; }
            public string MatchStatus { get; set; }
            public DateTime MatchTime { get; set; }
        }

        public List<FixtureInfo> Fixtures { get; set; }
    }
}