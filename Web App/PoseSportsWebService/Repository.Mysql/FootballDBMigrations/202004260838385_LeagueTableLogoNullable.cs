namespace Repository.Mysql.FootballDBMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LeagueTableLogoNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("league", "logo", c => c.String(maxLength: 100, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("league", "logo", c => c.String(nullable: false, maxLength: 100, unicode: false));
        }
    }
}
