namespace Repository.Mysql.FootballDBMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterLogoStringLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("country", "flag", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("league", "logo", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("team", "logo", c => c.String(maxLength: 100, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("team", "logo", c => c.String(maxLength: 64, unicode: false));
            AlterColumn("league", "logo", c => c.String(maxLength: 64, unicode: false));
            AlterColumn("country", "flag", c => c.String(maxLength: 64, unicode: false));
        }
    }
}
