using LogicCore.Converter;
using LogicCore.Utility;
using PosePacket.Service.Football.Models;
using Repository.Mysql.FootballDB.OutputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsWebService.Logics.Converters
{
    public class FootballTeamDetailConverter : IValueConverter<FootballTeamDetail>, Singleton.INode
    {
        public FootballTeamDetail Convert(object value, params object[] parameters)
        {
            var result = new FootballTeamDetail();

            if (value is DB_FootballTeamDetail db_teamDetail)
            {
                result.Id = db_teamDetail.Id;
                result.Name = db_teamDetail.Name;
                result.Logo = db_teamDetail.Logo;
                result.CountryName = db_teamDetail.CountryName;
            }
            return result;
        }
    }
}