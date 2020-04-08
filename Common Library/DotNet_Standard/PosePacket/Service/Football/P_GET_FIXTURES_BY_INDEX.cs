using PosePacket.Service.Football.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PosePacket.Service.Football
{
    public class I_GET_FIXTURES_BY_INDEX
    {
        public int[] FixtureIds { get; set; }
    }

    public class O_GET_FIXTURES_BY_INDEX
    {
        public List<FootballFixtureDetail> Fixtures { get; set; }
    }
}