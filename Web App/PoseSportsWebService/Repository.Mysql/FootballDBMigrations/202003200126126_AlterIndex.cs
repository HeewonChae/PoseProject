namespace Repository.Mysql.FootballDBMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterIndex : DbMigration
    {
        public override void Up()
        {
            DropIndex("league", "IDX_LEAGUE_NAME");
            DropIndex("team", "IDX_TEAM_NAME");
            CreateIndex("league", new[] { "name", "country_name", "type" }, name: "IDX_LEAGUE_COUNTRY_TYPE");
            CreateIndex("team", new[] { "name", "country_name" }, name: "IDX_NAME_COUNTRY");
        }
        
        public override void Down()
        {
            DropIndex("team", "IDX_NAME_COUNTRY");
            DropIndex("league", "IDX_LEAGUE_COUNTRY_TYPE");
            CreateIndex("team", "name", name: "IDX_TEAM_NAME");
            CreateIndex("league", "name", name: "IDX_LEAGUE_NAME");
        }
    }
}
