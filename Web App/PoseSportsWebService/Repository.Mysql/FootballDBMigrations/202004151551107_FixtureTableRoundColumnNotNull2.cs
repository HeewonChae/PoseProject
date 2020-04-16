namespace Repository.Mysql.FootballDBMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixtureTableRoundColumnNotNull2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("fixture", "round", c => c.String(nullable: false, maxLength: 64, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("fixture", "round", c => c.String(maxLength: 64, unicode: false));
        }
    }
}
