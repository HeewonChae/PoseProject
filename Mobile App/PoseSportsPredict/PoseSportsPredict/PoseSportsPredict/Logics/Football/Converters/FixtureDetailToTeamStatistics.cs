using PosePacket.Service.Football;
using PosePacket.Service.Football.Models;
using PoseSportsPredict.Models.Football;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.Football.Converters
{
    public class FixtureDetailToTeamStatistics
    {
        public FootballTeamStatistics Convert(IEnumerable<FootballFixtureDetail> fixtures, short teamdId, int lastPlayCnt, int campPlayCnt)
        {
            if (fixtures == null)
                throw new ArgumentException("fixtures");

            FootballTeamStatistics returnValue = new FootballTeamStatistics();

            fixtures = fixtures
                .Where(elem => elem.HomeTeam.Id == teamdId || elem.AwayTeam.Id == teamdId)
                .OrderByDescending(elem => elem.MatchTime);

            var Matches = fixtures.Take(lastPlayCnt);
            var homeCampMatches = fixtures.Where(elem => elem.HomeTeam.Id == teamdId).Take(campPlayCnt);
            var awayCampMatches = fixtures.Where(elem => elem.AwayTeam.Id == teamdId).Take(campPlayCnt);

            // Recent Record
            returnValue.RecentRecord = new FootballTeamStatistics.RecordInfo();
            foreach (var match in Matches)
            {
                if (match.HomeTeamScore == match.AwayTeamScore)
                {
                    returnValue.RecentRecord.Darw++;
                }
                else if (match.HomeTeamScore > match.AwayTeamScore)
                {
                    if (match.HomeTeam.Id == teamdId)
                        returnValue.RecentRecord.Win++;
                    else
                        returnValue.RecentRecord.Lose++;
                }
                else
                {
                    if (match.HomeTeam.Id == teamdId)
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
                totalGF += match.HomeTeam.Id == teamdId ? match.HomeTeamScore : match.AwayTeamScore;
                totalGA += match.HomeTeam.Id == teamdId ? match.AwayTeamScore : match.HomeTeamScore;
            }

            returnValue.TotalGoalFor = new FootballTeamStatistics.BaseInfo
            {
                Value = totalGF,
                Avg = Matches.Count() == 0 ? 0 : Math.Round((double)totalGF / Matches.Count(), 2),
            };

            returnValue.TotalAgainst = new FootballTeamStatistics.BaseInfo
            {
                Value = totalGA,
                Avg = Matches.Count() == 0 ? 0 : Math.Round((double)totalGA / Matches.Count(), 2),
            };

            //HomeCampGoal
            totalGF = 0;
            totalGA = 0;
            foreach (var match in homeCampMatches)
            {
                totalGF += match.HomeTeam.Id == teamdId ? match.HomeTeamScore : match.AwayTeamScore;
                totalGA += match.HomeTeam.Id == teamdId ? match.AwayTeamScore : match.HomeTeamScore;
            }

            returnValue.HomeGoalFor = new FootballTeamStatistics.BaseInfo
            {
                Value = totalGF,
                Avg = Matches.Count() == 0 ? 0 : Math.Round((double)totalGF / Matches.Count(), 2),
            };

            returnValue.HoemGoalAgainst = new FootballTeamStatistics.BaseInfo
            {
                Value = totalGA,
                Avg = Matches.Count() == 0 ? 0 : Math.Round((double)totalGA / Matches.Count(), 2),
            };

            //AwayCampGoal
            totalGF = 0;
            totalGA = 0;
            foreach (var match in awayCampMatches)
            {
                totalGF += match.HomeTeam.Id == teamdId ? match.HomeTeamScore : match.AwayTeamScore;
                totalGA += match.HomeTeam.Id == teamdId ? match.AwayTeamScore : match.HomeTeamScore;
            }

            returnValue.AwayGoalFor = new FootballTeamStatistics.BaseInfo
            {
                Value = totalGF,
                Avg = Matches.Count() == 0 ? 0 : Math.Round((double)totalGF / Matches.Count(), 2),
            };

            returnValue.AwayGoalAgainst = new FootballTeamStatistics.BaseInfo
            {
                Value = totalGA,
                Avg = Matches.Count() == 0 ? 0 : Math.Round((double)totalGA / Matches.Count(), 2),
            };

            return returnValue;
        }
    }
}