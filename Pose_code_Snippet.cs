// FootballDB Code First
Enable-Migrations -ProjectName Repository.Mysql -ContextTypeName Repository.Mysql.Contexts.FootballDB -MigrationsDirectory FootballDBMigrations -Verbose
Add-Migration -Name ******** -ProjectName Repository.Mysql -ConfigurationTypeName Repository.Mysql.FootballDBMigrations.Configuration -Verbose
Update-database -ProjectName Repository.Mysql -ConfigurationTypeName Repository.Mysql.FootballDBMigrations.Configuration -Verbose

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
