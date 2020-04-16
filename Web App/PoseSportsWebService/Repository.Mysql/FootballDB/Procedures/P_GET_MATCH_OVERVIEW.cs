using Repository.Mysql.Dapper;
using Repository.Mysql.FootballDB.OutputModels;
using Repository.Mysql.FootballDB.Tables;
using Repository.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.FootballDB.Procedures
{
    public class P_GET_MATCH_OVERVIEW : MysqlQuery<P_GET_MATCH_OVERVIEW.Input, P_GET_MATCH_OVERVIEW.Output>
    {
        public string FixturesQueryString;
        public string LeagueFixturesQueryString;
        public string StandingsQueryString;

        public struct Input
        {
            public int FixtureId { get; set; }
        }

        public class Output
        {
            public IEnumerable<DB_FootballFixtureDetail> HomeRecntFixtures { get; set; }
            public IEnumerable<DB_FootballFixtureDetail> AwayRecentFixtures { get; set; }

            public IEnumerable<DB_FootballFixtureDetail> League_HomeRecentFixtures { get; set; }
            public IEnumerable<DB_FootballFixtureDetail> League_AwayRecentFixtures { get; set; }

            public IEnumerable<DB_FootballStandingsDetail> StandingsDetails { get; set; }

            public int Result { get; set; }
        }

        public override void OnAlloc()
        {
            base.OnAlloc();
        }

        public override void OnFree()
        {
            base.OnFree();
        }

        public override void BindParameters()
        {
            var sb_fixture = new StringBuilder();
            sb_fixture.Append($"SELECT c.{nameof(Country.name)} as {nameof(DB_FootballFixtureDetail.CountryName)}, c.{nameof(Country.logo)} as {nameof(DB_FootballFixtureDetail.CountryLogo)}, ");
            sb_fixture.Append($"l.{nameof(League.name)} as {nameof(DB_FootballFixtureDetail.LeagueName)}, l.{nameof(League.logo)} as {nameof(DB_FootballFixtureDetail.LeagueLogo)}, f.{nameof(Fixture.round)} as {nameof(DB_FootballFixtureDetail.Round)},");
            sb_fixture.Append($"ht.{nameof(League.id)} as {nameof(DB_FootballFixtureDetail.HomeTeamId)}, ht.{nameof(Team.name)} as {nameof(DB_FootballFixtureDetail.HomeTeamName)}, ht.{nameof(Team.logo)} as {nameof(DB_FootballFixtureDetail.HomeTeamLogo)}, ");
            sb_fixture.Append($"at.{nameof(League.id)} as {nameof(DB_FootballFixtureDetail.AwayTeamId)}, at.{nameof(Team.name)} as {nameof(DB_FootballFixtureDetail.AwayTeamName)}, at.{nameof(Team.logo)} as {nameof(DB_FootballFixtureDetail.AwayTeamLogo)}, ");
            sb_fixture.Append($"f.{nameof(Fixture.home_score)} as {nameof(DB_FootballFixtureDetail.HomeTeamScore)}, f.{nameof(Fixture.away_score)} as {nameof(DB_FootballFixtureDetail.AwayTeamScore)}, ");
            sb_fixture.Append($"f.{nameof(Fixture.id)} as {nameof(DB_FootballFixtureDetail.FixtureId)}, f.{nameof(Fixture.status)} as {nameof(DB_FootballFixtureDetail.MatchStatus)}, ");
            sb_fixture.Append($"f.{nameof(Fixture.match_time)} as {nameof(DB_FootballFixtureDetail.MatchTime)}, l.{nameof(League.type)} as {nameof(DB_FootballFixtureDetail.LeagueType)} ");
            sb_fixture.Append("FROM fixture as f ");
            sb_fixture.Append($"INNER JOIN league as l on f.{nameof(Fixture.league_id)} = l.{nameof(League.id)} ");
            sb_fixture.Append($"INNER JOIN country as c on l.{nameof(League.country_name)} = c.{nameof(Country.name)} ");
            sb_fixture.Append($"INNER JOIN team as ht on f.{nameof(Fixture.home_team_id)} = ht.{nameof(Team.id)} ");
            sb_fixture.Append($"INNER JOIN team as at on f.{nameof(Fixture.away_team_id)} = at.{nameof(Team.id)} ");

            FixturesQueryString = $"{sb_fixture} WHERE (f.{nameof(Fixture.home_team_id)} = @TeamId OR f.{nameof(Fixture.away_team_id)} = @TeamId) AND f.{nameof(Fixture.match_time)} < @MatchTime ORDER BY f.{nameof(Fixture.match_time)} DESC LIMIT 6;";
            LeagueFixturesQueryString = $"{sb_fixture} WHERE f.{nameof(Fixture.league_id)} = @LeagueId AND (f.{nameof(Fixture.home_team_id)} = @TeamId OR f.{nameof(Fixture.away_team_id)} = @TeamId) AND f.{nameof(Fixture.match_time)} < @MatchTime ORDER BY f.{nameof(Fixture.match_time)} DESC LIMIT 15;";

            var sb_standings = new StringBuilder();
            sb_standings.Append($"SELECT l.{nameof(League.name)} as {nameof(DB_FootballStandingsDetail.LeagueName)}, l.{nameof(League.logo)} as {nameof(DB_FootballStandingsDetail.LeagueLogo)}, l.{nameof(League.type)} as {nameof(DB_FootballStandingsDetail.LeagueType)}, ");
            sb_standings.Append($"c.{nameof(Country.name)} as {nameof(DB_FootballStandingsDetail.CountryName)}, c.{nameof(Country.logo)} as {nameof(DB_FootballStandingsDetail.CountryLogo)}, ");
            sb_standings.Append($"t.{nameof(Team.id)} as {nameof(DB_FootballStandingsDetail.TeamId)}, t.{nameof(Team.name)} as {nameof(DB_FootballStandingsDetail.TeamName)}, t.{nameof(Team.logo)} as {nameof(DB_FootballStandingsDetail.TeamLogo)}, ");
            sb_standings.Append($"s.{nameof(Standings.rank)} as '{nameof(DB_FootballStandingsDetail.Rank)}', s.{nameof(Standings.points)} as {nameof(DB_FootballStandingsDetail.Points)}, s.{nameof(Standings.group)} as '{nameof(DB_FootballStandingsDetail.Group)}', ");
            sb_standings.Append($"s.{nameof(Standings.description)} as '{nameof(DB_FootballStandingsDetail.Description)}', s.{nameof(Standings.played)} as {nameof(DB_FootballStandingsDetail.Played)}, s.{nameof(Standings.win)} as {nameof(DB_FootballStandingsDetail.Win)}, ");
            sb_standings.Append($"s.{nameof(Standings.draw)} as {nameof(DB_FootballStandingsDetail.Draw)}, s.{nameof(Standings.lose)} as {nameof(DB_FootballStandingsDetail.Lose)}, s.{nameof(Standings.goals_for)} as {nameof(DB_FootballStandingsDetail.GoalFor)}, ");
            sb_standings.Append($"s.{nameof(Standings.goals_against)} as {nameof(DB_FootballStandingsDetail.GoalAgainst)}, s.{nameof(Standings.forme)} as {nameof(DB_FootballStandingsDetail.Form)} ");
            sb_standings.Append("FROM standings as s ");
            sb_standings.Append($"INNER JOIN league as l on s.{nameof(Standings.league_id)} = l.{nameof(League.id)} ");
            sb_standings.Append($"INNER JOIN country as c on l.{nameof(League.country_name)} = c.{nameof(Country.name)} ");
            sb_standings.Append($"INNER JOIN team as t on s.{nameof(Standings.team_id)} = t.{nameof(Team.id)} ");

            StandingsQueryString = $"{sb_standings} WHERE s.{nameof(Standings.league_id)} = @LeagueId AND s.{nameof(Standings.team_id)} IN (@HomeTeamId, @AwayTeamId);";
        }

        public override P_GET_MATCH_OVERVIEW.Output OnQuery()
        {
            _output = new Output();

            DapperFacade.DoWithDBContext(
                    null,
                    (Contexts.FootballDB footballDB) =>
                    {
                        var fixture = footballDB.Query<Fixture>($"SELECT * FROM fixture WHERE id={_input.FixtureId}").FirstOrDefault();
                        if (fixture == null)
                        {
                            _output.Result = 1;
                            return;
                        }

                        _output.HomeRecntFixtures = footballDB.Query<DB_FootballFixtureDetail>(FixturesQueryString, new { TeamId = fixture.home_team_id, MatchTime = fixture.match_time });
                        _output.AwayRecentFixtures = footballDB.Query<DB_FootballFixtureDetail>(FixturesQueryString, new { TeamId = fixture.away_team_id, MatchTime = fixture.match_time });

                        _output.League_HomeRecentFixtures = footballDB.Query<DB_FootballFixtureDetail>(LeagueFixturesQueryString, new { LeagueId = fixture.league_id, TeamId = fixture.home_team_id, MatchTime = fixture.match_time });
                        _output.League_AwayRecentFixtures = footballDB.Query<DB_FootballFixtureDetail>(LeagueFixturesQueryString, new { LeagueId = fixture.league_id, TeamId = fixture.away_team_id, MatchTime = fixture.match_time });

                        _output.StandingsDetails = footballDB.Query<DB_FootballStandingsDetail>(StandingsQueryString, new { LeagueId = fixture.league_id, HomeTeamId = fixture.home_team_id, AwayTeamId = fixture.away_team_id });

                        _output.Result = 0;
                    },
                    this.OnError);

            return _output;
        }

        public override void OnError(EntityStatus entityStatus)
        {
            base.OnError(entityStatus);

            // Error Control
        }
    }
}