using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.Models.Football
{
    public class FootballMatchGroupInfo
    {
        public string Key => League != null ? $"{Country}. {League}" : $"{Country}";
        public string Country { get; set; }
        public string League { get; set; }
        public string GroupLogo { get; set; }
        public FootballMatchInfo[] Matches { get; set; }
        public bool IsExpanded { get; set; }
    }
}