using LogicCore.File;
using Newtonsoft.Json;
using PosePacket.Service.Football.Models.Enums;
using PredictionBackTesting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredictionBackTesting.Commands
{
    using db_table = Repository.Mysql.FootballDB.Tables;

    public static class SaveJsonPredictionPercentage
    {
        private readonly static string rootDir = ".\\HitPercentage\\";

        public static void Execute(Dictionary<FootballPredictionType, List<db_table.PredictionBackTesting>> dic_predictions)
        {
            var result = new List<PredictionHitInfo>();

            foreach (var dic_prediction in dic_predictions)
            {
                var hitPercentageInfo = new PredictionHitInfo();
                hitPercentageInfo.PredictionType = dic_prediction.Key.ToString();
                hitPercentageInfo.SubLabelType = string.Empty;
                hitPercentageInfo.TotalCount = dic_prediction.Value.Count;
                hitPercentageInfo.TotalHitCount = dic_prediction.Value.Where(elem => elem.is_hit).Count();
                hitPercentageInfo.RecommendedCount = dic_prediction.Value.Where(elem => elem.is_recommended).Count();
                hitPercentageInfo.RecommendedHitCount = dic_prediction.Value.Where(elem => elem.is_recommended && elem.is_hit).Count();

                hitPercentageInfo.TotalHitPercentage = ((double)hitPercentageInfo.TotalHitCount / (double)hitPercentageInfo.TotalCount) * 100;
                hitPercentageInfo.RecommendedHitPercentage = ((double)hitPercentageInfo.RecommendedHitCount / (double)hitPercentageInfo.RecommendedCount) * 100;

                result.Add(hitPercentageInfo);
            }

            // under over sub label
            result.AddRange(
                GetUnderOverSubLaPercentage(dic_predictions[FootballPredictionType.Under_Over].ToArray()));

            var serializeString = JsonConvert.SerializeObject(result, Formatting.Indented);
            FileFacade.MakeSimpleTextFile(rootDir, $"{DateTime.Now.ToString("yyyyMMdd_HHmm")}.json", serializeString);
        }

        private static PredictionHitInfo[] GetUnderOverSubLaPercentage(db_table.PredictionBackTesting[] predictions)
        {
            var result = new List<PredictionHitInfo>();

            var over_1_5 = predictions.Where(elem => elem.sub_label == 2);
            var over_1_5_hitPercentage = new PredictionHitInfo();
            over_1_5_hitPercentage.PredictionType = FootballPredictionType.Under_Over.ToString();
            over_1_5_hitPercentage.SubLabelType = FootballUnderOverType.OVER_1_5.ToString();
            over_1_5_hitPercentage.TotalCount = over_1_5.Count();
            over_1_5_hitPercentage.TotalHitCount = over_1_5.Where(elem => elem.is_hit).Count();
            over_1_5_hitPercentage.RecommendedCount = over_1_5.Where(elem => elem.is_recommended).Count();
            over_1_5_hitPercentage.RecommendedHitCount = over_1_5.Where(elem => elem.is_recommended && elem.is_hit).Count();
            over_1_5_hitPercentage.TotalHitPercentage = ((double)over_1_5_hitPercentage.TotalHitCount / (double)over_1_5_hitPercentage.TotalCount) * 100;
            over_1_5_hitPercentage.RecommendedHitPercentage = ((double)over_1_5_hitPercentage.RecommendedHitCount / (double)over_1_5_hitPercentage.RecommendedCount) * 100;
            result.Add(over_1_5_hitPercentage);

            var under_2_5 = predictions.Where(elem => elem.sub_label == 3);
            var under_2_5_hitPercentage = new PredictionHitInfo();
            under_2_5_hitPercentage.PredictionType = FootballPredictionType.Under_Over.ToString();
            under_2_5_hitPercentage.SubLabelType = FootballUnderOverType.UNDER_2_5.ToString();
            under_2_5_hitPercentage.TotalCount = under_2_5.Count();
            under_2_5_hitPercentage.TotalHitCount = under_2_5.Where(elem => elem.is_hit).Count();
            under_2_5_hitPercentage.RecommendedCount = under_2_5.Where(elem => elem.is_recommended).Count();
            under_2_5_hitPercentage.RecommendedHitCount = under_2_5.Where(elem => elem.is_recommended && elem.is_hit).Count();
            under_2_5_hitPercentage.TotalHitPercentage = ((double)under_2_5_hitPercentage.TotalHitCount / (double)under_2_5_hitPercentage.TotalCount) * 100;
            under_2_5_hitPercentage.RecommendedHitPercentage = ((double)under_2_5_hitPercentage.RecommendedHitCount / (double)under_2_5_hitPercentage.RecommendedCount) * 100;
            result.Add(under_2_5_hitPercentage);

            var over_2_5 = predictions.Where(elem => elem.sub_label == 4);
            var over_2_5_hitPercentage = new PredictionHitInfo();
            over_2_5_hitPercentage.PredictionType = FootballPredictionType.Under_Over.ToString();
            over_2_5_hitPercentage.SubLabelType = FootballUnderOverType.OVER_2_5.ToString();
            over_2_5_hitPercentage.TotalCount = over_2_5.Count();
            over_2_5_hitPercentage.TotalHitCount = over_2_5.Where(elem => elem.is_hit).Count();
            over_2_5_hitPercentage.RecommendedCount = over_2_5.Where(elem => elem.is_recommended).Count();
            over_2_5_hitPercentage.RecommendedHitCount = over_2_5.Where(elem => elem.is_recommended && elem.is_hit).Count();
            over_2_5_hitPercentage.TotalHitPercentage = ((double)over_2_5_hitPercentage.TotalHitCount / (double)over_2_5_hitPercentage.TotalCount) * 100;
            over_2_5_hitPercentage.RecommendedHitPercentage = ((double)over_2_5_hitPercentage.RecommendedHitCount / (double)over_2_5_hitPercentage.RecommendedCount) * 100;
            result.Add(over_2_5_hitPercentage);

            var under_3_5 = predictions.Where(elem => elem.sub_label == 5);
            var under_3_5_hitPercentage = new PredictionHitInfo();
            under_3_5_hitPercentage.PredictionType = FootballPredictionType.Under_Over.ToString();
            under_3_5_hitPercentage.SubLabelType = FootballUnderOverType.UNDER_3_5.ToString();
            under_3_5_hitPercentage.TotalCount = under_3_5.Count();
            under_3_5_hitPercentage.TotalHitCount = under_3_5.Where(elem => elem.is_hit).Count();
            under_3_5_hitPercentage.RecommendedCount = under_3_5.Where(elem => elem.is_recommended).Count();
            under_3_5_hitPercentage.RecommendedHitCount = under_3_5.Where(elem => elem.is_recommended && elem.is_hit).Count();
            under_3_5_hitPercentage.TotalHitPercentage = ((double)under_3_5_hitPercentage.TotalHitCount / (double)under_3_5_hitPercentage.TotalCount) * 100;
            under_3_5_hitPercentage.RecommendedHitPercentage = ((double)under_3_5_hitPercentage.RecommendedHitCount / (double)under_3_5_hitPercentage.RecommendedCount) * 100;
            result.Add(under_3_5_hitPercentage);

            return result.ToArray();
        }
    }
}