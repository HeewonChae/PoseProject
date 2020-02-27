using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidAPI.Models.Football.Enums
{
	public enum FixtureStatusType
	{
		_NONE_,
		TBD, // Time To Be Defined
		NS, // Not Started
		FH, // First Half, Kick Off
		HT, // Halftime
		SH, // Second Half, 2nd Half Started
		ET, // Extra Time
		P, // Penalty In Progress
		FT, // Match Finished
		AET, // Match Finished After Extra Time
		PEN, // Match Finished After Penalty
		BT, // Break Time (in Extra Time)
		SUSP, // Match Suspended
		INT, // Match Interrupted
		PST, // Match Postponed
		CANC, // Match Cancelled
		ABD, // Match Abandoned
		AWD, // Technical Loss
		WO, // WalkOver
		_MAX_
	}

	public enum BookmakerType
	{ 
		_NONE_,
		TenBet,
		Marathonbet,
		Betfair,
		Pinnacle,
		Sport_Betting_Odds,
		Bwin,
		William_Hill,
		Bet365,
		Dafabet,
		Ladbrokes,
		OnexBet,
		BetFred,
		_MAX_
	}

	public enum OddsLabelType
	{
		_NONE_,
		Match_Winner = 1,
		Goals_Over_Under = 5,
		Both_Teams_Score = 8,
		Double_Chance = 12,
		_MAX_
	}

}
