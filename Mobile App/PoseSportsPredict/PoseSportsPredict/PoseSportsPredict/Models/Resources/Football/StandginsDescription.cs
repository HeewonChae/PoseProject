using PoseSportsPredict.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoseSportsPredict.Models.Resources.Football
{
    public class StandginsDescription
    {
        public string Keyword { get; set; }
        public StandingsDescCategoryType Category { get; set; }

        public static readonly Dictionary<StandingsDescCategoryType, StandginsDescription[]> StandginsDescriptions = new Dictionary<StandingsDescCategoryType, StandginsDescription[]>();

        public static void Load(StandginsDescription[] standginsDescriptions)
        {
            var grouppingDatas = standginsDescriptions.GroupBy(elem => elem.Category);

            foreach (var grouppingData in grouppingDatas)
            {
                StandginsDescriptions.Add(grouppingData.Key, grouppingData.ToArray());
            }
        }

        public static StandingsDescCategoryType GetDescCategory(string description)
        {
            if (StandginsDescriptions.TryGetValue(StandingsDescCategoryType.Negative, out StandginsDescription[] negativeDescs))
            {
                foreach (var negativeDesc in negativeDescs)
                {
                    if (description.ToLower().Contains(negativeDesc.Keyword.ToLower()))
                        return StandingsDescCategoryType.Negative;
                }
            }

            if (StandginsDescriptions.TryGetValue(StandingsDescCategoryType.Positive, out StandginsDescription[] positiveDescs))
            {
                foreach (var positiveDesc in positiveDescs)
                {
                    if (description.ToLower().Contains(positiveDesc.Keyword.ToLower()))
                        return StandingsDescCategoryType.Positive;
                }
            }

            if (StandginsDescriptions.TryGetValue(StandingsDescCategoryType.Neutral, out StandginsDescription[] neutralDescs))
            {
                foreach (var neutralDesc in neutralDescs)
                {
                    if (description.ToLower().Contains(neutralDesc.Keyword.ToLower()))
                        return StandingsDescCategoryType.Neutral;
                }
            }

            return StandingsDescCategoryType.None;
        }
    }
}