using LogicCore.Utility;
using Pose_sports_statistics.Logic.RapidAPI;
using Repository.Data.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.ModelBinding;
using System.Web.UI.WebControls;

using WebFormModel = Pose_sports_statistics.Models;

namespace Pose_sports_statistics.Controls
{
	public partial class FootballPrediction : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (IsPostBack)
				return;
		}

		public WebFormModel.FootballPrediction GetFootballPrediction([RouteData] int fixtureID)
		{
			//TODO FixtureDetail 버전으로 바꾸기..
			var predictions = Singleton.Get<RedisCacheManager>()
			.Get<IList<WebFormModel.FootballPrediction>>
			(
				() => RequestLoader.FootballPredictionByFixtureID(fixtureID),
				RequestLoader.Locker_FootballPredictionByFixtureID,
				DateTime.Now.AddDays(1),
				RedisKeyMaker.FootballPredictionByFixtureID(fixtureID)
			);

			return predictions.FirstOrDefault();
		}

		protected void Form_Prediction_DataBound(object sender, EventArgs e)
		{
			if (form_prediction.Row != null)
			{
				var dataItem = this.form_prediction.DataItem as WebFormModel.FootballPrediction;

				// 승,무,패 확률
				var lbl_winPG = this.form_prediction.Row.FindControl("lbl_winPG") as Label;
				var lbl_drawPG = this.form_prediction.Row.FindControl("lbl_drawPG") as Label;
				var lbl_losePG = this.form_prediction.Row.FindControl("lbl_losePG") as Label;

				lbl_winPG.Text = $"홈 승: {dataItem.WinningPG.Home}";
				lbl_drawPG.Text = $"무승부: {dataItem.WinningPG.Draw}";
				lbl_losePG.Text = $"원정 승: {dataItem.WinningPG.Away}";

				// 결론
				var lbl_MatchWinner = this.form_prediction.Row.FindControl("lbl_MatchWinner") as Label;
				var lbl_UnderOver = this.form_prediction.Row.FindControl("lbl_UnderOver") as Label;
				var lbl_Advice = this.form_prediction.Row.FindControl("lbl_Advice") as Label;

				lbl_MatchWinner.Text = $"승자: {dataItem.MatchWinner}";
				lbl_UnderOver.Text = $"언더&오버: {dataItem.UnderOver}";
				lbl_Advice.Text = $"총평: {dataItem.Advice}";
			}
		}
	}
}