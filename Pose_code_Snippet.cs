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
SELECT l.id, l.country_name, l.name, f.home_team_id, t.name FROM footballdb.fixture as f
INNER JOIN footballdb.league as l ON f.league_id = l.id
INNER JOIN footballdb.team as t ON f.home_team_id = t.id
where league_id= group by home_team_id;
SELECT * FROM footballdb.league where country_name="";
SELECT f.id as fixtureId, f.league_id as league_id, f.home_team_id as home_team_id, ht.name as home_team_name
, f.away_team_id as away_team_id, at.name as away_team_name, ht.country_name
FROM footballdb.fixture as f
inner join footballdb.team as ht on f.home_team_id = ht.id
inner join footballdb.team as at on f.away_team_id = at.id
where f.league_id=;