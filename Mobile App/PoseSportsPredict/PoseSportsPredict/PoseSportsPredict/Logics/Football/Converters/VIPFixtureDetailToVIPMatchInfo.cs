using PosePacket.Service.Football.Models;
using PoseSportsPredict.Logics.View.Converters;
using PoseSportsPredict.Models.Football;
using Shiny;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoseSportsPredict.Logics.Football.Converters
{
    public class VIPFixtureDetailToVIPMatchInfo
    {
        public FootballVIPMatchInfo Convert(FootballVIPFixtureDetail[] vipFixtureDetails)
        {
            if (vipFixtureDetails == null
                || vipFixtureDetails.Length == 0)
                return null;

            var matchInfo = ShinyHost.Resolve<FixtureDetailToMatchInfo>().Convert(vipFixtureDetails.First());

            var returnValue = new FootballVIPMatchInfo()
            {
                Id = matchInfo.Id,
                League_CountryName = matchInfo.League_CountryName,
                League_CountryLogo = matchInfo.League_CountryLogo,
                LeagueName = matchInfo.LeagueName,
                LeagueLogo = matchInfo.LeagueLogo,
                LeagueType = matchInfo.LeagueType,

                HomeTeamId = matchInfo.HomeTeamId,
                HomeName = matchInfo.HomeName,
                HomeLogo = matchInfo.HomeLogo,
                Home_CountryName = matchInfo.Home_CountryName,
                HomeScore = matchInfo.HomeScore,

                AwayTeamId = matchInfo.AwayTeamId,
                AwayName = matchInfo.AwayName,
                AwayLogo = matchInfo.AwayLogo,
                Away_CountryName = matchInfo.Away_CountryName,
                AwayScore = matchInfo.AwayScore,

                MatchStatus = matchInfo.MatchStatus,
                MatchTime = matchInfo.MatchTime,
                Round = matchInfo.Round,

                IsAlarmed = matchInfo.IsAlarmed,
                IsBookmarked = matchInfo.IsBookmarked,

                IsPredicted = matchInfo.IsPredicted,
                IsRecommended = matchInfo.IsRecommended,
                MaxRating = matchInfo.MaxRating,
                HasOdds = matchInfo.HasOdds,
            };

            // parsing prediction
            var predTitleConverter = ShinyHost.Resolve<PredictionLabelToString>();
            returnValue.PredictionPicks = new List<FootballPredictionPickInfo>();
            foreach (var vipFixtureDetail in vipFixtureDetails)
            {
                var prediction = new FootballPredictionPickInfo
                {
                    Title = predTitleConverter.Convert(vipFixtureDetail.MainLabel, vipFixtureDetail.SubLabel),
                    Rate = vipFixtureDetail.Grade / 2.0,
                    IsHit = vipFixtureDetail.IsHit,
                    MainLabel = vipFixtureDetail.MainLabel,
                    SubLabel = vipFixtureDetail.SubLabel,
                };

                returnValue.PredictionPicks.Add(prediction);
            }

            returnValue.PredictionPicks = returnValue.PredictionPicks.OrderByDescending(elem => elem.Rate).ToList();

            return returnValue;
        }
    }
}