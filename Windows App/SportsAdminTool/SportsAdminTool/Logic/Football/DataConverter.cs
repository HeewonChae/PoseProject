using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FootballDB = Repository.Mysql.FootballDB;
using AppModel = SportsAdminTool.Model;

namespace SportsAdminTool.Logic.Football
{
	public static class DataConverter
	{
		public static bool CovertAppModelToDbModel(int fixtureID, short homeTeamID, short awayTeamID, AppModel.Football.FixtureStatistic appModel, out FootballDB.Table.FixtureStatistic[] dbModel)
		{
			dbModel = new FootballDB.Table.FixtureStatistic[2]
				{
					new FootballDB.Table.FixtureStatistic(),
					new FootballDB.Table.FixtureStatistic() 
				};

			dbModel[0].fixture_id = fixtureID;
			dbModel[1].fixture_id = fixtureID;

			dbModel[0].team_id = homeTeamID;
			dbModel[1].team_id = awayTeamID;

			dbModel[0].shots_total = string.IsNullOrEmpty(appModel.TotalShorts?.Home) ? (short)0 : short.Parse(appModel.TotalShorts?.Home);
			dbModel[1].shots_total = string.IsNullOrEmpty(appModel.TotalShorts?.Away) ? (short)0 : short.Parse(appModel.TotalShorts?.Away);

			dbModel[0].shots_on_goal = string.IsNullOrEmpty(appModel.ShotsOnGoal?.Home) ? (short)0 : short.Parse(appModel.ShotsOnGoal.Home);
			dbModel[1].shots_on_goal = string.IsNullOrEmpty(appModel.ShotsOnGoal?.Away) ? (short)0 : short.Parse(appModel.ShotsOnGoal.Away);

			dbModel[0].shots_off_goal = string.IsNullOrEmpty(appModel.ShotsOffGoal?.Home) ? (short)0 : short.Parse(appModel.ShotsOffGoal.Home);
			dbModel[1].shots_off_goal = string.IsNullOrEmpty(appModel.ShotsOffGoal?.Away) ? (short)0 : short.Parse(appModel.ShotsOffGoal.Away);

			dbModel[0].shots_blocked = string.IsNullOrEmpty(appModel.BlockedShots?.Home) ? (short)0 : short.Parse(appModel.BlockedShots.Home);
			dbModel[1].shots_blocked = string.IsNullOrEmpty(appModel.BlockedShots?.Away) ? (short)0 : short.Parse(appModel.BlockedShots.Away);

			dbModel[0].shots_inside_box = string.IsNullOrEmpty(appModel.ShotsInsideBox?.Home) ? (short)0 : short.Parse(appModel.ShotsInsideBox.Home);
			dbModel[1].shots_inside_box = string.IsNullOrEmpty(appModel.ShotsInsideBox?.Away) ? (short)0 : short.Parse(appModel.ShotsInsideBox.Away);

			dbModel[0].shots_outside_box = string.IsNullOrEmpty(appModel.ShotsOutsideBox?.Home) ? (short)0 : short.Parse(appModel.ShotsOutsideBox.Home);
			dbModel[1].shots_outside_box = string.IsNullOrEmpty(appModel.ShotsOutsideBox?.Away) ? (short)0 : short.Parse(appModel.ShotsOutsideBox.Away);

			dbModel[0].goalkeeper_saves = string.IsNullOrEmpty(appModel.GoalkeeperSaves?.Home) ? (short)0 : short.Parse(appModel.GoalkeeperSaves.Home);
			dbModel[1].goalkeeper_saves = string.IsNullOrEmpty(appModel.GoalkeeperSaves?.Away) ? (short)0 : short.Parse(appModel.GoalkeeperSaves.Away);

			dbModel[0].fouls = string.IsNullOrEmpty(appModel.Fouls?.Home) ? (short)0 : short.Parse(appModel.Fouls.Home);
			dbModel[1].fouls = string.IsNullOrEmpty(appModel.Fouls?.Away) ? (short)0 : short.Parse(appModel.Fouls.Away);

			dbModel[0].corner_kicks = string.IsNullOrEmpty(appModel.CornerKicks?.Home) ? (short)0 : short.Parse(appModel.CornerKicks.Home);
			dbModel[1].corner_kicks = string.IsNullOrEmpty(appModel.CornerKicks?.Away) ? (short)0 : short.Parse(appModel.CornerKicks.Away);

			dbModel[0].offsides = string.IsNullOrEmpty(appModel.Offsides?.Home) ? (short)0 : short.Parse(appModel.Offsides.Home);
			dbModel[1].offsides = string.IsNullOrEmpty(appModel.Offsides?.Away) ? (short)0 : short.Parse(appModel.Offsides.Away);

			dbModel[0].yellow_cards = string.IsNullOrEmpty(appModel.YellowCards?.Home) ? (short)0 : short.Parse(appModel.YellowCards.Home);
			dbModel[1].yellow_cards = string.IsNullOrEmpty(appModel.YellowCards?.Away) ? (short)0 : short.Parse(appModel.YellowCards.Away);

			dbModel[0].red_cards = string.IsNullOrEmpty(appModel.RedCards?.Home) ? (short)0 : short.Parse(appModel.RedCards.Home);
			dbModel[1].red_cards = string.IsNullOrEmpty(appModel.RedCards?.Away) ? (short)0 : short.Parse(appModel.RedCards.Away);

			dbModel[0].ball_possessions = string.IsNullOrEmpty(appModel.BallPossession?.Home) ? (short)0 : short.Parse(appModel.BallPossession.Home.Replace("%", "").Trim());
			dbModel[1].ball_possessions = string.IsNullOrEmpty(appModel.BallPossession?.Away) ? (short)0 : short.Parse(appModel.BallPossession.Away.Replace("%", "").Trim());

			dbModel[0].passes_total = string.IsNullOrEmpty(appModel.TotalPasses?.Home) ? (short)0 : short.Parse(appModel.TotalPasses.Home);
			dbModel[1].passes_total = string.IsNullOrEmpty(appModel.TotalPasses?.Away) ? (short)0 : short.Parse(appModel.TotalPasses.Away);

			dbModel[0].passes_accurate = string.IsNullOrEmpty(appModel.AccuratePasses?.Home) ? (short)0 : short.Parse(appModel.AccuratePasses.Home);
			dbModel[1].passes_accurate = string.IsNullOrEmpty(appModel.AccuratePasses?.Away) ? (short)0 : short.Parse(appModel.AccuratePasses.Away);

			return true;
		}
	}
}
