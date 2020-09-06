using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.Models.Football
{
    public class FootballMatchGroupInfo
    {
        public string Key => League != null ?
            string.IsNullOrEmpty(GroupSubString) ? $"{Country}. {League}" : $"{Country}. {League} - {GroupSubString}"
            : string.IsNullOrEmpty(GroupSubString) ? $"{Country}" : $"{GroupSubString}";

        public string Country { get; set; }
        public string League { get; set; }
        public string GroupLogo { get; set; }
        public string GroupSubString { get; set; }
        public FootballMatchInfo[] Matches { get; set; }
        public bool IsExpanded { get; set; }
    }
}