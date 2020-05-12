using PoseSportsPredict.Models.Football;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoseSportsPredict.Logics.Football.Converters
{
    public class MatchInfoToGoalLineChartData
    {
        public (List<FootballGoalLineChartData> goalFors, List<FootballGoalLineChartData> goalAgainsts) Convert(List<FootballMatchInfo> matches, short teamId, int dataCnt)
        {
            var goalFors = new List<FootballGoalLineChartData>();
            var goalAgainsts = new List<FootballGoalLineChartData>();

            var orderedMatches = matches.OrderBy(elem => elem.MatchTime).Take(dataCnt);

            foreach (var match in orderedMatches)
            {
                goalFors.Add(new FootballGoalLineChartData
                {
                    FixtureId = match.Id,
                    MatchTime = match.MatchTime,
                    Score = match.HomeTeamId == teamId ? match.HomeScore : match.AwayScore,
                });

                goalAgainsts.Add(new FootballGoalLineChartData
                {
                    FixtureId = match.Id,
                    MatchTime = match.MatchTime,
                    Score = match.HomeTeamId == teamId ? match.AwayScore : match.HomeScore,
                });
            }

            return (goalFors, goalAgainsts);
        }
    }
}