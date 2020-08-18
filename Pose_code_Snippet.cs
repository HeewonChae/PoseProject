// FootballDB Code First
Enable-Migrations -ProjectName Repository.Mysql -ContextTypeName Repository.Mysql.Contexts.FootballDB -MigrationsDirectory FootballDBMigrations -Verbose
Add-Migration -Name ******** -ProjectName Repository.Mysql -ConfigurationTypeName Repository.Mysql.FootballDBMigrations.Configuration -Verbose
Update-database -ProjectName Repository.Mysql -ConfigurationTypeName Repository.Mysql.FootballDBMigrations.Configuration -Verbose

// PoseGlobalDB Code First
enable-Migrations -ProjectName Repository.Mysql -ContextTypeName Repository.Mysql.Contexts.PoseGlobalDB -MigrationsDirectory PoseGlobalDBMigrations -Verbose
Add-Migration -Name ******** -ProjectName Repository.Mysql -ConfigurationTypeName Repository.Mysql.PoseGlobalDBMigrations.Configuration -Verbose
Update-database -ProjectName Repository.Mysql -ConfigurationTypeName Repository.Mysql.PoseGlobalDBMigrations.Configuration -Verbose

// DB Safe update
SET SQL_SAFE_UPDATES =0;

// DB Procedure
public class P_EXECUTE_QUERY : MysqlQuery<T_in, T_out>
{
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
        // if you need Binding Parameters, write here
    }

    public override int OnQuery()
    {
        DapperFacade.DoWithDBContext(
                null, 
                (Contexts.FootballDB footballDB) =>
                {
                    if (!string.IsNullOrEmpty(_input))
                        _output = footballDB.ExecuteQuery(_input);
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

// Command
[WebModelType(InputType = typeof(), OutputType = typeof())]
public static class P_Command
{
    public static class RowCode
    {
        [Description("Invalid input")]
        public const int Invalid_Input = ServiceErrorCode.WebMethod.P_Command + 1;
    }

    public static output Execute(input input)
    {
        if (input == null)
            ErrorHandler.OccurException(Invalid_Input);
        return outpout;
    }
}

// Find Invalid Team
https://rapidapi.com/api-sports/api/api-football
SELECT l.id, l.country_name, l.name, f.home_team_id, t.name 
FROM footballdb.fixture as f
INNER JOIN footballdb.league as l ON f.league_id = l.id
INNER JOIN footballdb.team as t ON f.home_team_id = t.id
where f.league_id=855 and home_team_id not in (SELECT team_id FROM footballdb.standings where league_id=855 group by team_id)
group by home_team_id;
 
SELECT * FROM footballdb.team where country_name="Azerbaidjan";
SELECT * FROM footballdb.league where country_name="Azerbaidjan";
SELECT * FROM footballdb.standings where league_id=905 order by `group`, `rank`;

// Select LeagueCoverage
SELECT l.id, l.name, l.type, l.country_name, lc.standings, lc.predictions, lc.odds, lc.players, lc.lineups, lc.fixture_statistics, l.logo FROM footballdb.league as l
inner join footballdb.league_coverage as lc on l.id = lc.league_id
where lc.predictions = 1 and l.is_current = 1
group by l.name, l.country_name order by l.country_name;

// select completed prediction
SELECT f.id, l.country_name, l.name as league, ht.name as home_team, at.name as away_team, f.match_time, f.home_score, f.away_score,
p.main_label, p.sub_label, p.value1, p.value2, p.value3, p.grade, p.is_recommended, p.is_hit FROM footballdb.fixture as f
inner join footballdb.league as l on f.league_id = l.id
inner join footballdb.team as ht on f.home_team_id = ht.id
inner join footballdb.team as at on f.away_team_id = at.id
left join footballdb.prediction as p on f.id = p.fixture_id
where f.is_predicted = 1 
and  f.match_time between '20200809' and '20200815'
and f.is_completed = 1 
and p.is_recommended = 1;