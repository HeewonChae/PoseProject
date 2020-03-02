namespace Repository.Mysql.PoseGlobalDBMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitUserBase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "user_base",
                c => new
                    {
                        user_no = c.Long(nullable: false, identity: true),
                        platform_id = c.String(nullable: false, maxLength: 128, unicode: false),
                        platform_type = c.Int(nullable: false, storeType: "mediumint"),
                        last_login_date = c.DateTime(nullable: false, precision: 0),
                        ipt_date = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.user_no)                
                .Index(t => t.platform_id, unique: true, name: "IDX_PLATFORM_ID");
            
        }
        
        public override void Down()
        {
            DropIndex("user_base", "IDX_PLATFORM_ID");
            DropTable("user_base");
        }
    }
}
