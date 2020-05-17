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
    public class FootballOddsDetailConverter : IValueConverter<FootballOddsDetail>, Singleton.INode
    {
        public FootballOddsDetail Convert(object value, params object[] parameters)
        {
            var result = new FootballOddsDetail();

            if (value is Odds db_odds)
            {
                result.FixtureId = db_odds.fixture_id;

                db_odds.bookmaker_type.TryParseEnum(out BookmakerType bookmakerType);
                result.BoomakerType = bookmakerType;

                db_odds.label_type.TryParseEnum(out OddsLabelType oddsLabelType);
                result.OddsLabelType = oddsLabelType;

                SetOdds(db_odds.subtitle_1, db_odds.odds_1, result);
                SetOdds(db_odds.subtitle_2, db_odds.odds_2, result);
                SetOdds(db_odds.subtitle_3, db_odds.odds_3, result);
            }

            return result;
        }

        public void SetOdds(string subTitle, float odds, FootballOddsDetail oddsDetail)
        {
            switch (subTitle)
            {
                case "Home":
                    oddsDetail.Odds1 = odds;
                    break;

                case "Draw":
                    oddsDetail.Odds2 = odds;
                    break;

                case "Away":
                    oddsDetail.Odds3 = odds;
                    break;
            }
        }
    }
}