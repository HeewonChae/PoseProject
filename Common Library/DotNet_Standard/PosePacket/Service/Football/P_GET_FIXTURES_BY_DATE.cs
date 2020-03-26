using PosePacket.Service.Football.Models;
using System;
using System.Collections.Generic;

namespace PosePacket.Service.Football
{
    public class I_GET_FIXTURES_BY_DATE
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class O_GET_FIXTURES_BY_DATE
    {
        public List<FootballFixtureDetail> Fixtures { get; set; }
    }
}