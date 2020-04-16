namespace Repository.Mysql.FootballDBMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteStandingsIndex : DbMigration
    {
        public override void Up()
        {
            DropIndex("standings", "IDX_LEAGUE_ID");
            DropIndex("standings", "IDX_LEAGUE_TEAM");
        }
        
        public override void Down()
        {
            CreateIndex("standings", new[] { "league_id", "team_id" }, name: "IDX_LEAGUE_TEAM");
            CreateIndex("standings", "league_id", name: "IDX_LEAGUE_ID");
        }
    }
}
