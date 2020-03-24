namespace Repository.Mysql.FootballDBMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeStandingsTableName : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "standing", newName: "standings");
        }
        
        public override void Down()
        {
            RenameTable(name: "standings", newName: "standing");
        }
    }
}
