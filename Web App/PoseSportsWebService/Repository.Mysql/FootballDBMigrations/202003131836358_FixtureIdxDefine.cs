namespace Repository.Mysql.FootballDBMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixtureIdxDefine : DbMigration
    {
        public override void Up()
        {
            DropIndex("fixture", "IDX_PREDICTED");
            CreateIndex("fixture", "event_date", name: "IDX_EVENT_DATE");
            CreateIndex("fixture", new[] { "is_predicted", "event_date" }, name: "IDX_PREDICTED_DATE");
        }
        
        public override void Down()
        {
            DropIndex("fixture", "IDX_PREDICTED_DATE");
            DropIndex("fixture", "IDX_EVENT_DATE");
            CreateIndex("fixture", "is_predicted", name: "IDX_PREDICTED");
        }
    }
}
