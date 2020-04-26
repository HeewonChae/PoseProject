using PosePacket.Service.Football;
using PoseSportsPredict.Models.Football;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.Football.Converters
{
    public class FootballMatchStatisticsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                throw new ArgumentException("parameter");

            FootballMatchStatistics returnValue = null;

            var curMatch = parameter as FootballMatchInfo;
            if (value is O_GET_MATCH_OVERVIEW matchOverviewInfo)
            {
                returnValue = new FootballMatchStatistics();

                var homeMatches = matchOverviewInfo.League_HomeRecentFixtures.Take(6);
                var homeSideMatches = matchOverviewInfo.League_HomeRecentFixtures.Where(elem => elem.HomeTeam.Id == curMatch.HomeTeamId).Take(3);
                var awayMatches = matchOverviewInfo.Leauge_AwayRecentFixtures.Take(6);
                var awaySideMatches = matchOverviewInfo.Leauge_AwayRecentFixtures.Where(elem => elem.AwayTeam.Id == curMatch.AwayTeamId).Take(3);

                // Recent Record
                returnValue.HomeRecentRecord = new FootballMatchStatistics.RecordInfo();
                foreach (var match in homeMatches)
                {
                    if (match.HomeTeamScore > match.AwayTeamScore)
                    {
                        if (match.HomeTeam.Id == curMatch.HomeTeamId)
                            returnValue.HomeRecentRecord.Win++;
                        else
                            returnValue.HomeRecentRecord.Lose++;
                    }
                    else if (match.HomeTeamScore == match.AwayTeamScore)
                    {
                        returnValue.HomeRecentRecord.Darw++;
                    }
                    else
                    {
                        if (match.HomeTeam.Id == curMatch.HomeTeamId)
                            returnValue.HomeRecentRecord.Lose++;
                        else
                            returnValue.HomeRecentRecord.Win++;
                    }
                }

                returnValue.AwayRecentRecord = new FootballMatchStatistics.RecordInfo();
                foreach (var match in awayMatches)
                {
                    if (match.HomeTeamScore > match.AwayTeamScore)
                    {
                        if (match.HomeTeam.Id == curMatch.AwayTeamId)
                            returnValue.AwayRecentRecord.Win++;
                        else
                            returnValue.AwayRecentRecord.Lose++;
                    }
                    else if (match.HomeTeamScore == match.AwayTeamScore)
                    {
                        returnValue.AwayRecentRecord.Darw++;
                    }
                    else
                    {
                        if (match.HomeTeam.Id == curMatch.AwayTeamId)
                            returnValue.AwayRecentRecord.Lose++;
                        else
                            returnValue.AwayRecentRecord.Win++;
                    }
                }

                returnValue.TotalRecordPoints = returnValue.HomeRecentRecord.Points + returnValue.AwayRecentRecord.Points;

                // Goal For
                int homeGF = 0;
                foreach (var match in homeMatches)
                {
                    if (match.HomeTeam.Id == curMatch.HomeTeamId)
                    {
                        homeGF += match.HomeTeamScore;
                    }
                    else
                    {
                        homeGF += match.AwayTeamScore;
                    }
                }
                returnValue.HomeGoalFor = new FootballMatchStatistics.BaseInfo
                {
                    Value = homeGF,
                    Avg = homeMatches.Count() == 0 ? 0 : Math.Round((double)homeGF / homeMatches.Count(), 2),
                };

                int awayGF = 0;
                foreach (var match in awayMatches)
                {
                    if (match.HomeTeam.Id == curMatch.AwayTeamId)
                    {
                        awayGF += match.HomeTeamScore;
                    }
                    else
                    {
                        awayGF += match.AwayTeamScore;
                    }
                }
                returnValue.AwayGoalFor = new FootballMatchStatistics.BaseInfo
                {
                    Value = awayGF,
                    Avg = awayMatches.Count() == 0 ? 0 : Math.Round((double)awayGF / awayMatches.Count(), 2),
                };

                returnValue.TotalGoalFor = returnValue.HomeGoalFor.Avg + returnValue.AwayGoalFor.Avg;

                // Goal Against
                int homeGA = 0;
                foreach (var match in homeMatches)
                {
                    if (match.HomeTeam.Id == curMatch.HomeTeamId)
                    {
                        homeGA += match.AwayTeamScore;
                    }
                    else
                    {
                        homeGA += match.HomeTeamScore;
                    }
                }
                returnValue.HomeGoalAgainst = new FootballMatchStatistics.BaseInfo
                {
                    Value = homeGA,
                    Avg = homeMatches.Count() == 0 ? 0 : Math.Round((double)homeGA / homeMatches.Count(), 2),
                };

                int awayGA = 0;
                foreach (var match in awayMatches)
                {
                    if (match.HomeTeam.Id == curMatch.AwayTeamId)
                    {
                        awayGA += match.AwayTeamScore;
                    }
                    else
                    {
                        awayGA += match.HomeTeamScore;
                    }
                }
                returnValue.AwayGoalAgainst = new FootballMatchStatistics.BaseInfo
                {
                    Value = awayGA,
                    Avg = awayMatches.Count() == 0 ? 0 : Math.Round((double)awayGA / awayMatches.Count(), 2),
                };

                returnValue.TotalGoalAgainst = returnValue.HomeGoalAgainst.Avg + returnValue.AwayGoalAgainst.Avg;

                // H/A Goal For
                int homeSideGF = 0;
                foreach (var match in homeSideMatches)
                {
                    if (match.HomeTeam.Id == curMatch.HomeTeamId)
                    {
                        homeSideGF += match.HomeTeamScore;
                    }
                    else
                    {
                        homeSideGF += match.AwayTeamScore;
                    }
                }
                returnValue.HomeSideGoalFor = new FootballMatchStatistics.BaseInfo
                {
                    Value = homeSideGF,
                    Avg = homeSideMatches.Count() == 0 ? 0 : Math.Round((double)homeSideGF / homeSideMatches.Count(), 2),
                };

                int awaySideGF = 0;
                foreach (var match in awaySideMatches)
                {
                    if (match.HomeTeam.Id == curMatch.AwayTeamId)
                    {
                        awaySideGF += match.HomeTeamScore;
                    }
                    else
                    {
                        awaySideGF += match.AwayTeamScore;
                    }
                }
                returnValue.AwaySideGoalFor = new FootballMatchStatistics.BaseInfo
                {
                    Value = awaySideGF,
                    Avg = awaySideMatches.Count() == 0 ? 0 : Math.Round((double)awaySideGF / awaySideMatches.Count(), 2),
                };

                returnValue.TotalSideGoalFor = returnValue.HomeSideGoalFor.Avg + returnValue.AwaySideGoalFor.Avg;

                // H/A Goal Against
                int homeSideGA = 0;
                foreach (var match in homeSideMatches)
                {
                    if (match.HomeTeam.Id == curMatch.HomeTeamId)
                    {
                        homeSideGA += match.AwayTeamScore;
                    }
                    else
                    {
                        homeSideGA += match.HomeTeamScore;
                    }
                }
                returnValue.HomeSideGoalAgainst = new FootballMatchStatistics.BaseInfo
                {
                    Value = homeSideGA,
                    Avg = homeSideMatches.Count() == 0 ? 0 : Math.Round((double)homeSideGA / homeSideMatches.Count(), 2),
                };

                int awaySideGA = 0;
                foreach (var match in awaySideMatches)
                {
                    if (match.HomeTeam.Id == curMatch.AwayTeamId)
                    {
                        awaySideGA += match.AwayTeamScore;
                    }
                    else
                    {
                        awaySideGA += match.HomeTeamScore;
                    }
                }
                returnValue.AwaySideGoalAgainst = new FootballMatchStatistics.BaseInfo
                {
                    Value = awaySideGA,
                    Avg = awaySideMatches.Count() == 0 ? 0 : Math.Round((double)awaySideGA / awaySideMatches.Count(), 2),
                };

                returnValue.TotalSideGoalAgainst = returnValue.HomeSideGoalAgainst.Avg + returnValue.AwaySideGoalAgainst.Avg;

                // Rest Period
                returnValue.HomeRestPeriod = homeMatches.Count() == 0 ? 0 : (curMatch.MatchTime - matchOverviewInfo.HomeRecentFixtures.First().MatchTime.ToLocalTime()).Days;
                returnValue.AwayRestPeriod = awayMatches.Count() == 0 ? 0 : (curMatch.MatchTime - matchOverviewInfo.AwayRecentFixtures.First().MatchTime.ToLocalTime()).Days;
                returnValue.TotalRestPeriod = returnValue.HomeRestPeriod + returnValue.AwayRestPeriod;
            }

            return returnValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value != 0 ? true : false;
        }
    }
}