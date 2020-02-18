using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pose_sports_statistics.Logic.Football
{
	public enum BookmakerType
	{
		_10Bet = 1,
		_Marathonbet,
		_Betfair,
		_Pinnacle,
		_Sport_Betting_Odds,
		_Bwin,
		_William_Hill,
		_Bet365,
		_Dafabet,
		_Max_,
	}

	public enum BetLabelType
	{
		_Match_Winner = 1,
		_Home_Away,
		_Second_Half_Winner,
		_Asian_Handicap,
		_Goals_Over_Under,
		_Goals_Over_Under_First_Half,
		_Max_,
	}

	public enum BetValueNameType
	{
		Home = 1,
		Draw,
		Away,
		_Max_
	}
}