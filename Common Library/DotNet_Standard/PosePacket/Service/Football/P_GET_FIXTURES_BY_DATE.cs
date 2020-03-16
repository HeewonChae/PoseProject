using PosePacket.Service.Football.Models;
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
        public List<FixtureDetail> Fixtures { get; set; }
    }
}