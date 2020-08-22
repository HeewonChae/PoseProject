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
                hitPercentageInfo.TotalCount = dic_prediction.Value.Count;
                hitPercentageInfo.TotalHitCount = dic_prediction.Value.Where(elem => elem.is_hit).Count();
                hitPercentageInfo.RecommendedCount = dic_prediction.Value.Where(elem => elem.is_recommended).Count();
                hitPercentageInfo.RecommendedHitCount = dic_prediction.Value.Where(elem => elem.is_recommended && elem.is_hit).Count();

                hitPercentageInfo.TotalHitPercentage = ((double)hitPercentageInfo.TotalHitCount / (double)hitPercentageInfo.TotalCount) * 100;
                hitPercentageInfo.RecommendedHitPercentage = ((double)hitPercentageInfo.RecommendedHitCount / (double)hitPercentageInfo.RecommendedCount) * 100;

                result.Add(hitPercentageInfo);
            }

            var serializeString = JsonConvert.SerializeObject(result, Formatting.Indented);
            FileFacade.MakeSimpleTextFile(rootDir, $"{DateTime.Now.ToString("yyyyMMdd_HHmm")}.json", serializeString);
        }
    }
}