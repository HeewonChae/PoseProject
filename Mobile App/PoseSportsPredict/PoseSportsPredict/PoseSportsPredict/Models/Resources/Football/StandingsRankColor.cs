using Newtonsoft.Json;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Utilities.JsonConverter;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.Models.Resources.Football
{
    public class StandingsRankColor
    {
        public StandingsDescCategoryType Category { get; set; }

        [JsonConverter(typeof(JsonColorConverter))]
        public Color[] Colors { get; set; }

        public static readonly Dictionary<StandingsDescCategoryType, Color[]> StandingsRankColors = new Dictionary<StandingsDescCategoryType, Color[]>();

        public static void Load(StandingsRankColor[] standingsRankColors)
        {
            foreach (var standingsRankColor in standingsRankColors)
            {
                StandingsRankColors.Add(standingsRankColor.Category, standingsRankColor.Colors);
            }
        }

        public static Color GetRankColor(StandingsDescCategoryType category, int index)
        {
            if (StandingsRankColors.ContainsKey(category))
            {
                var colors = StandingsRankColors[category];
                return colors[index % colors.Length]; // Loopping
            }

            return Color.Transparent;
        }
    }
}