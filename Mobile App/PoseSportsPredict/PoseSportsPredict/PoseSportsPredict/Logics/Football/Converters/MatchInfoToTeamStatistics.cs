using PoseSportsPredict.Models.Football;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoseSportsPredict.Logics.Football.Converters
{
    public class MatchInfoToTeamStatistics
    {
        public FootballTeamStatistics Convert(IEnumerable<FootballMatchInfo> matches, short teamdId, int lastPlayCnt, int campPlayCnt)
        {
            if (matches == null)
                throw new ArgumentException("fixtures");

            FootballTeamStatistics returnValue = new FootballTeamStatistics();

            matches = matches
                .Where(elem => elem.HomeTeamId == teamdId || elem.AwayTeamId == teamdId)
                .OrderByDescending(elem => elem.MatchTime);

            var Matches = matches.Take(lastPlayCnt);
            var homeCampMatches = matches.Where(elem => elem.HomeTeamId == teamdId).Take(campPlayCnt);
            var awayCampMatches = matches.Where(elem => elem.AwayTeamId == teamdId).Take(campPlayCnt);

            // Recent Record
            returnValue.RecentRecord = new FootballTeamStatistics.RecordInfo();
            foreach (var match in Matches)
            {
                if (match.HomeScore == match.AwayScore)
                {
                    returnValue.RecentRecord.Draw++;
                }
                else if (match.HomeScore > match.AwayScore)
                {
                    if (match.HomeTeamId == teamdId)
                        returnValue.RecentRecord.Win++;
                    else
                        returnValue.RecentRecord.Lose++;
                }
                else
                {
                    if (match.HomeTeamId == teamdId)
                        returnValue.RecentRecord.Lose++;
                    else
                        returnValue.RecentRecord.Win++;
                }
            }

            // Total Goal
            int totalGF = 0;
            int totalGA = 0;
            foreach (var match in Matches)
            {
                totalGF += match.HomeTeamId == teamdId ? match.HomeScore : match.AwayScore;
                totalGA += match.HomeTeamId == teamdId ? match.AwayScore : match.HomeScore;
            }

            returnValue.TotalGoalFor = new FootballTeamStatistics.BaseInfo
            {
                Value = totalGF,
                Avg = Matches.Count() == 0 ? 0 : Math.Round((double)totalGF / Matches.Count(), 2),
            };

            returnValue.TotalGoalAgainst = new FootballTeamStatistics.BaseInfo
            {
                Value = totalGA,
                Avg = Matches.Count() == 0 ? 0 : Math.Round((double)totalGA / Matches.Count(), 2),
            };

            //HomeCampGoal
            totalGF = 0;
            totalGA = 0;
            foreach (var match in homeCampMatches)
            {
                totalGF += match.HomeTeamId == teamdId ? match.HomeScore : match.AwayScore;
                totalGA += match.HomeTeamId == teamdId ? match.AwayScore : match.HomeScore;
            }

            returnValue.HomeGoalFor = new FootballTeamStatistics.BaseInfo
            {
                Value = totalGF,
                Avg = homeCampMatches.Count() == 0 ? 0 : Math.Round((double)totalGF / homeCampMatches.Count(), 2),
            };

            returnValue.HoemGoalAgainst = new FootballTeamStatistics.BaseInfo
            {
                Value = totalGA,
                Avg = homeCampMatches.Count() == 0 ? 0 : Math.Round((double)totalGA / homeCampMatches.Count(), 2),
            };

            //AwayCampGoal
            totalGF = 0;
            totalGA = 0;
            foreach (var match in awayCampMatches)
            {
                totalGF += match.HomeTeamId == teamdId ? match.HomeScore : match.AwayScore;
                totalGA += match.HomeTeamId == teamdId ? match.AwayScore : match.HomeScore;
            }

            returnValue.AwayGoalFor = new FootballTeamStatistics.BaseInfo
            {
                Value = totalGF,
                Avg = awayCampMatches.Count() == 0 ? 0 : Math.Round((double)totalGF / awayCampMatches.Count(), 2),
            };

            returnValue.AwayGoalAgainst = new FootballTeamStatistics.BaseInfo
            {
                Value = totalGA,
                Avg = awayCampMatches.Count() == 0 ? 0 : Math.Round((double)totalGA / awayCampMatches.Count(), 2),
            };

            return returnValue;
        }
    }
}