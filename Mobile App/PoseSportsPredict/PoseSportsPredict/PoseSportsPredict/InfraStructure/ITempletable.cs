using PoseSportsPredict.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.InfraStructure
{
    public interface ITempletable
    {
        MatchGroupType GroupType { get; set; }
        AdsBannerType AdsBannerType { get; set; }
    }
}