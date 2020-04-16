namespace Repository.Mysql.FootballDBMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StandingsTableIndexAlter : DbMigration
    {
        public override void Up()
        {
            DropIndex("standings", "IDX_TEAM_ID");
            CreateIndex("standings", new[] { "league_id", "team_id" }, name: "IDX_LEAGUE_TEAM");
        }
        
        public override void Down()
        {
            DropIndex("standings", "IDX_LEAGUE_TEAM");
            CreateIndex("standings", "team_id", name: "IDX_TEAM_ID");
        }
    }
}
