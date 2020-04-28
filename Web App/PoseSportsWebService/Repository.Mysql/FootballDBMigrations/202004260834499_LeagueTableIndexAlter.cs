namespace Repository.Mysql.FootballDBMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LeagueTableIndexAlter : DbMigration
    {
        public override void Up()
        {
            DropIndex("league", "IDX_LEAGUE_COUNTRY_TYPE");
            DropIndex("league", "IDX_COUNTRY_NAME");
            DropIndex("league", "IDX_SEASON_END");
            AlterColumn("league", "name", c => c.String(nullable: false, maxLength: 64, unicode: false));
            AlterColumn("league", "type", c => c.String(nullable: false, maxLength: 8, unicode: false));
            AlterColumn("league", "logo", c => c.String(nullable: false, maxLength: 100, unicode: false));
            CreateIndex("league", new[] { "country_name", "name" }, name: "IDX_COUNTRY_LEAGUE");
        }
        
        public override void Down()
        {
            DropIndex("league", "IDX_COUNTRY_LEAGUE");
            AlterColumn("league", "logo", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("league", "type", c => c.String(maxLength: 8, unicode: false));
            AlterColumn("league", "name", c => c.String(maxLength: 64, unicode: false));
            CreateIndex("league", "season_end", name: "IDX_SEASON_END");
            CreateIndex("league", "country_name", name: "IDX_COUNTRY_NAME");
            CreateIndex("league", new[] { "name", "country_name", "type" }, name: "IDX_LEAGUE_COUNTRY_TYPE");
        }
    }
}
