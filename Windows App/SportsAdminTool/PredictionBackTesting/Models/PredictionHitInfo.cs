using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredictionBackTesting.Models
{
    public class PredictionHitInfo
    {
        public string PredictionType { get; set; }
        public int TotalCount { get; set; }
        public int TotalHitCount { get; set; }
        public double TotalHitPercentage { get; set; }
        public int RecommendedCount { get; set; }
        public int RecommendedHitCount { get; set; }
        public double RecommendedHitPercentage { get; set; }
    }
}