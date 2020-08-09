using PosePacket.Service.Football.Models;
using PosePacket.Service.Football.Models.Enums;
using PoseSportsPredict.Models.Football;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.Football.Converters
{
    public class OddsDetailToOddsInfo
    {
        public FootballOddsInfo Convert(FootballOddsDetail oddsDetail)
        {
            if (oddsDetail == null)
                throw new ArgumentException("oddsDetail");

            var returnValue = new FootballOddsInfo
            {
                FixtureId = oddsDetail.FixtureId,
                BookmakerType = oddsDetail.BoomakerType,
                OddsLabelType = oddsDetail.OddsLabelType,
                WinOdds = Math.Round(oddsDetail.Odds1, 2),
                DrawOdds = Math.Round(oddsDetail.Odds2, 2),
                LoseOdds = Math.Round(oddsDetail.Odds3, 2),
            };

            returnValue.RefundRate = 100.0 / ((100.0 / returnValue.WinOdds) + (100.0 / returnValue.DrawOdds) + (100.0 / returnValue.LoseOdds));

            SetBookmakerImageAndColor(returnValue);

            return returnValue;
        }

        public void SetBookmakerImageAndColor(FootballOddsInfo oddsInfo)
        {
            switch (oddsInfo.BookmakerType)
            {
                case FootballBookmakerType.TenBet:
                    oddsInfo.BookmakerImageUrl = "img_10Bet.png";
                    oddsInfo.BookmakerColor = Color.FromHex("#ffffff");
                    break;

                case FootballBookmakerType.Marathonbet:
                    oddsInfo.BookmakerImageUrl = "img_Marathonbet.png";
                    oddsInfo.BookmakerColor = Color.FromHex("#ffffff");
                    break;

                case FootballBookmakerType.Betfair:
                    oddsInfo.BookmakerImageUrl = "img_betfair.png";
                    oddsInfo.BookmakerColor = Color.FromHex("#ffffff");
                    break;

                case FootballBookmakerType.Bwin:
                    oddsInfo.BookmakerImageUrl = "img_bwin.png";
                    oddsInfo.BookmakerColor = Color.FromHex("#ffffff");
                    break;

                case FootballBookmakerType.WilliamHill:
                    oddsInfo.BookmakerImageUrl = "img_williamhill.jpg";
                    oddsInfo.BookmakerColor = Color.FromHex("#ffffff");
                    break;

                case FootballBookmakerType.Bet365:
                    oddsInfo.BookmakerImageUrl = "img_bet365.png";
                    oddsInfo.BookmakerColor = Color.FromHex("#ffffff");
                    break;

                case FootballBookmakerType.Dafabet:
                    oddsInfo.BookmakerImageUrl = "img_dafabet.png";
                    oddsInfo.BookmakerColor = Color.FromHex("#ffffff");
                    break;

                case FootballBookmakerType.Ladbrokes:
                    oddsInfo.BookmakerImageUrl = "img_ladbrokes.png";
                    oddsInfo.BookmakerColor = Color.FromHex("#ec1f25");
                    break;

                case FootballBookmakerType.OnexBet:
                    oddsInfo.BookmakerImageUrl = "img_1xbet.png";
                    oddsInfo.BookmakerColor = Color.FromHex("#1a5685");
                    break;

                case FootballBookmakerType.Interwetten:
                    oddsInfo.BookmakerImageUrl = "img_Interwetten.png";
                    oddsInfo.BookmakerColor = Color.FromHex("#ffffff");
                    break;

                default:
                    throw new Exception("Undefined BookmakerType");
            }
        }
    }
}