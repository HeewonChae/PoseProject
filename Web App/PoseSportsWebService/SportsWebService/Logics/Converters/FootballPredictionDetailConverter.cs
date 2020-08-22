using LogicCore.Converter;
using LogicCore.Utility;
using PosePacket.Service.Football.Models;
using PosePacket.Service.Football.Models.Enums;
using Repository.Mysql.FootballDB.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsWebService.Logics.Converters
{
    public class FootballPredictionDetailConverter : IValueConverter<FootballPredictionDetail>, Singleton.INode
    {
        public FootballPredictionDetail Convert(object value, params object[] parameters)
        {
            var result = new FootballPredictionDetail();

            if (value is Prediction db_prediction)
            {
                result.FixtureId = db_prediction.fixture_id;
                result.Seq = (byte)db_prediction.pred_seq;

                EnumConverter.TryParseEnum<FootballPredictionType>(db_prediction.main_label, out FootballPredictionType mainLabelType);
                result.MainLabel = mainLabelType;
                result.SubLabel = (byte)db_prediction.sub_label;

                result.Value1 = db_prediction.value1;
                result.Value2 = db_prediction.value2;
                result.Value3 = db_prediction.value3;
                result.Value4 = db_prediction.value4;

                result.Grade = (byte)db_prediction.grade;
                result.IsRecommended = db_prediction.is_recommended;
                result.IsHit = db_prediction.is_hit;
            }
            return result;
        }
    }
}