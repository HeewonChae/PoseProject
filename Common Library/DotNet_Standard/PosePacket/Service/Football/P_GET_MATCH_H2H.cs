using MessagePack;
using PosePacket.Service.Football.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PosePacket.Service.Football
{
    [MessagePackObject]
    public class I_GET_MATCH_H2H
    {
        [Key(0)]
        public int FixtureId { get; set; }

        [Key(1)]
        public short HomeTeamId { get; set; }

        [Key(2)]
        public short AwayTeamId { get; set; }
    }

    [MessagePackObject]
    public class O_GET_MATCH_H2H
    {
        [Key(0)]
        public FootballFixtureDetail[] H2HFixtures { get; set; }
    }
}