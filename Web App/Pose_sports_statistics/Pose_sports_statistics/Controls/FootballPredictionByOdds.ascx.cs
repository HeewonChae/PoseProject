using LogicCore;
using Pose_sports_statistics.Logic.Football;
using Pose_sports_statistics.Logic.RapidAPI;
using RapidAPI.Models.Football.Enums;
using Repository.Data.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebFormModel = Pose_sports_statistics.Models;

namespace Pose_sports_statistics.Controls
{
	public partial class FootballPredictionByOdds : System.Web.UI.UserControl
	{
		public class PredictionByOdds
		{
			public string BookMakerName { get; set; }
			public float HomeOdds { get; set; }
			public float DrawOdds { get; set; }
			public float AwayOdds { get; set; }
			public double RefundRate { get; set; }
			public double PredictionPG_Home { get; set; }
			public double PredictionPG_Draw { get; set; }
			public double PredictionPG_Away { get; set; }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (IsPostBack)
				return;
		}

		public IQueryable<dynamic> GetPredictionByOdds([RouteData] int fixtureID)
		{
			var oddsInfo = RequestLoader.FootballOddsByFixtureID(fixtureID);

			List<PredictionByOdds> list_predictionByOdds = new List<PredictionByOdds>();

			if (oddsInfo != null && oddsInfo.Bookmakers.Length > 0)
			{
				foreach (var bookMaker in oddsInfo.Bookmakers)
				{
					var MatchWinnerInfo = bookMaker.BetInfos.Where(elem => elem.LabelType == OddsLabelType.Match_Winner).FirstOrDefault();
					if (MatchWinnerInfo == null)
						continue;

					try
					{
						double refundRate = 100.0 / ((100.0 / MatchWinnerInfo.BetValues[0].Odds) + (100.0 / MatchWinnerInfo.BetValues[1].Odds) + (100.0 / MatchWinnerInfo.BetValues[2].Odds)) * 100;
						double predictionPG_Home = refundRate / MatchWinnerInfo.BetValues[0].Odds;
						double predictionPG_Draw = refundRate / MatchWinnerInfo.BetValues[1].Odds;
						double predictionPG_Away = refundRate / MatchWinnerInfo.BetValues[2].Odds;

						var predictionByOdds = new PredictionByOdds
						{
							BookMakerName = bookMaker.BookmakerName,
							HomeOdds = MatchWinnerInfo.BetValues[0].Odds,
							DrawOdds = MatchWinnerInfo.BetValues[1].Odds,
							AwayOdds = MatchWinnerInfo.BetValues[2].Odds,
							RefundRate = refundRate,
							PredictionPG_Home = predictionPG_Home,
							PredictionPG_Draw = predictionPG_Draw,
							PredictionPG_Away = predictionPG_Away
						};

						list_predictionByOdds.Add(predictionByOdds);
					}
					catch (Exception e)
					{
						break;
					}
				}
			}

			return list_predictionByOdds.AsQueryable();
		}
	}
}