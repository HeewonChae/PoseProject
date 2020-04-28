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
    public class FootballLeagueDetailConverter : IValueConverter<FootballLeagueDetail>, Singleton.INode
    {
        public FootballLeagueDetail Convert(object value, params object[] parameters)
        {
            var result = new FootballLeagueDetail();

            if (value is DB_FootballLeagueDetail db_leagueDetail)
            {
                db_leagueDetail.LeagueType.TryParseEnum(out FootballLeagueType leagueType);

                result.Country = new FootballCountryDetail
                {
                    Name = db_leagueDetail.CountryName,
                    Logo = db_leagueDetail.CountryLogo,
                };

                result.Name = db_leagueDetail.Name;
                result.Logo = db_leagueDetail.Logo;
                result.LeagueType = leagueType;
                result.SeasonStartDate = db_leagueDetail.SeasonStartDate;
                result.SeasonEndDate = db_leagueDetail.SeasonEndDate;
            }
            return result;
        }
    }
}