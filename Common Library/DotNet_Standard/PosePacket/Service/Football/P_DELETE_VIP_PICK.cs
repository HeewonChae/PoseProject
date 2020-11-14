using MessagePack;
using PosePacket.Service.Football.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PosePacket.Service.Football
{
    [MessagePackObject]
    public class I_DELETE_VIP_PICK
    {
        [Key(0)]
        public int FixtureId { get; set; }

        [Key(1)]
        public FootballPredictionType MainLabel { get; set; }

        [Key(2)]
        public byte SubLabel { get; set; }
    }

    [MessagePackObject]
    public class O_DELETE_VIP_PICK
    {
        [Key(0)]
        public bool IsSuccess { get; set; }
    }
}