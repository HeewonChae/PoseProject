namespace Repository.Mysql.FootballDBMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterFixtureIndex : DbMigration
    {
        public override void Up()
        {
            CreateIndex("fixture", new[] { "league_id", "away_team_id" }, name: "IDX_LEAGUE_AWAY");
            CreateIndex("fixture", new[] { "league_id", "home_team_id" }, name: "IDX_LEAGUE_HOME");
        }
        
        public override void Down()
        {
            DropIndex("fixture", "IDX_LEAGUE_HOME");
            DropIndex("fixture", "IDX_LEAGUE_AWAY");
        }
    }
}
