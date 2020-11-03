using LogicCore.Converter;
using LogicCore.Utility;
using PosePacket.Service.Football.Models;
using PosePacket.Service.Football.Models.Enums;
using Repository.Mysql.FootballDB.OutputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsWebService.Logics.Converters
{
    public class FootballVIPFixtureDetailConverter : IValueConverter<FootballVIPFixtureDetail>, Singleton.INode
    {
        public FootballVIPFixtureDetail Convert(object value, params object[] parameters)
        {
            var result = new FootballVIPFixtureDetail();

            if (value is DB_FootballVIPFixtureDetail db_vipFixtureDetail)
            {
                // 기존 fixtureDetail정보
                var fixtureDetail = Singleton.Get<FootballFixtureDetailConverter>().Convert(db_vipFixtureDetail);
                result.League = fixtureDetail.League;
                result.HomeTeam = fixtureDetail.HomeTeam;
                result.AwayTeam = fixtureDetail.AwayTeam;
                result.Round = fixtureDetail.Round;
                result.FixtureId = fixtureDetail.FixtureId;
                result.MatchStatus = fixtureDetail.MatchStatus;
                result.MatchTime = fixtureDetail.MatchTime;
                result.HomeTeamScore = fixtureDetail.HomeTeamScore;
                result.AwayTeamScore = fixtureDetail.AwayTeamScore;
                result.IsPredicted = fixtureDetail.IsPredicted;
                result.IsRecommended = fixtureDetail.IsRecommended;
                result.MaxGrade = fixtureDetail.MaxGrade;
                result.HasOdds = fixtureDetail.HasOdds;

                EnumConverter.TryParseEnum(db_vipFixtureDetail.MainLabel, out FootballPredictionType mainLabelType);
                result.MainLabel = mainLabelType;
                result.SubLabel = (byte)db_vipFixtureDetail.SubLabel;
                result.Grade = (byte)db_vipFixtureDetail.Grade;
                result.IsHit = db_vipFixtureDetail.IsHit;
            }

            return result;
        }
    }
}