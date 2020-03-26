namespace Repository.Mysql.FootballDBMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTeamTableNullCheck : DbMigration
    {
        public override void Up()
        {
            DropIndex("team", "IDX_NAME_COUNTRY");
            AlterColumn("team", "name", c => c.String(nullable: false, maxLength: 64, unicode: false));
            AlterColumn("team", "logo", c => c.String(nullable: false, maxLength: 100, unicode: false));
            CreateIndex("team", new[] { "name", "country_name" }, name: "IDX_NAME_COUNTRY");
        }
        
        public override void Down()
        {
            DropIndex("team", "IDX_NAME_COUNTRY");
            AlterColumn("team", "logo", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("team", "name", c => c.String(maxLength: 64, unicode: false));
            CreateIndex("team", new[] { "name", "country_name" }, name: "IDX_NAME_COUNTRY");
        }
    }
}
