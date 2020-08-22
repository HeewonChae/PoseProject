using PosePacket.Service.Football.Models;
using PosePacket.Service.Football.Models.Enums;
using PoseSportsPredict.Models.Football;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoseSportsPredict.Logics.Football.Converters
{
    public class PredictionDetailsToPredictionGroup
    {
        public Dictionary<FootballPredictionType, FootballPredictionGroup> Convert(FootballPredictionDetail[] predictionDetails)
        {
            if (predictionDetails == null)
                throw new ArgumentException("predictionDetails");

            List<FootballPredictionInfo> predictions = new List<FootballPredictionInfo>();
            foreach (var predictionDetail in predictionDetails)
            {
                var prediction = new FootballPredictionInfo
                {
                    MainLabel = predictionDetail.MainLabel,
                    SubLabel = predictionDetail.SubLabel,
                    Value1 = predictionDetail.Value1,
                    Value2 = predictionDetail.Value2,
                    Value3 = predictionDetail.Value3,
                    Value4 = predictionDetail.Value4,
                    Grade = predictionDetail.Grade,
                    IsHit = predictionDetail.IsHit,
                    IsRecommended = predictionDetail.IsRecommended,
                };

                predictions.Add(prediction);
            }

            var matchId = predictionDetails[0].FixtureId;
            var returnValue = new Dictionary<FootballPredictionType, FootballPredictionGroup>();

            var finalScorePredictions = predictions.Where(elem => elem.MainLabel == FootballPredictionType.Final_Score).ToArray();
            var fianlScoreGroup = new FootballPredictionGroup()
            {
                MainLabel = FootballPredictionType.Final_Score,
                MatchId = matchId,
                IsRecommand = finalScorePredictions.FirstOrDefault(elem => elem.IsRecommended) != null,
                Rate = finalScorePredictions.Max(elem => elem.Grade) / 2.0,
                Predictions = finalScorePredictions,
            };

            var matchWinnerPredictions = predictions.Where(elem => elem.MainLabel == FootballPredictionType.Match_Winner).ToArray();
            var matchWinnerGroup = new FootballPredictionGroup()
            {
                MainLabel = FootballPredictionType.Match_Winner,
                MatchId = matchId,
                IsRecommand = matchWinnerPredictions.FirstOrDefault(elem => elem.IsRecommended) != null,
                Rate = matchWinnerPredictions.Max(elem => elem.Grade) / 2.0,
                Predictions = matchWinnerPredictions,
            };

            var bothToScorePredictions = predictions.Where(elem => elem.MainLabel == FootballPredictionType.Both_Teams_to_Score).ToArray();
            var bothToScoreGroup = new FootballPredictionGroup()
            {
                MainLabel = FootballPredictionType.Both_Teams_to_Score,
                MatchId = matchId,
                IsRecommand = bothToScorePredictions.FirstOrDefault(elem => elem.IsRecommended) != null,
                Rate = bothToScorePredictions.Max(elem => elem.Grade) / 2.0,
                Predictions = bothToScorePredictions,
            };

            var underOverPredictions = predictions.Where(elem => elem.MainLabel == FootballPredictionType.Under_Over).ToArray();
            var underOverGroup = new FootballPredictionGroup()
            {
                MainLabel = FootballPredictionType.Under_Over,
                MatchId = matchId,
                IsRecommand = underOverPredictions.FirstOrDefault(elem => elem.IsRecommended) != null,
                Rate = underOverPredictions.Max(elem => elem.Grade) / 2.0,
                Predictions = underOverPredictions,
            };

            returnValue.Add(FootballPredictionType.Final_Score, fianlScoreGroup);
            returnValue.Add(FootballPredictionType.Match_Winner, matchWinnerGroup);
            returnValue.Add(FootballPredictionType.Both_Teams_to_Score, bothToScoreGroup);
            returnValue.Add(FootballPredictionType.Under_Over, underOverGroup);

            return returnValue;
        }
    }
}