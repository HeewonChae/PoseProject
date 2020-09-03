using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PosePacket.Service.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PoseSportsPredict.Models.Resources.Common
{
    public class MembershipAdvantage
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public MemberRoleType MemberRoleType { get; set; }

        public int MatchBookmarkLimit { get; set; }
        public int TeamBookmarkLimit { get; set; }
        public int LeagueBookmarkLimit { get; set; }
        public bool IsBannerAdRemove { get; set; }
        public bool IsVideoAdRemove { get; set; }

        public static Dictionary<MemberRoleType, MembershipAdvantage> Advantages { get; } = new Dictionary<MemberRoleType, MembershipAdvantage>();

        public static void Load(params MembershipAdvantage[] advantages)
        {
            foreach (var advantage in advantages)
            {
                Debug.Assert(!Advantages.ContainsKey(advantage.MemberRoleType));

                Advantages.Add(advantage.MemberRoleType, advantage);
            }
        }

        public static bool TryGetValue(MemberRoleType memberRoleType, out MembershipAdvantage value)
        {
            return Advantages.TryGetValue(memberRoleType, out value);
        }
    }
}