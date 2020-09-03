namespace Repository.Mysql.PoseGlobalDBMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitiateGlobalDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "in_app_billing",
                c => new
                    {
                        trans_no = c.Long(nullable: false, identity: true),
                        user_no = c.Long(nullable: false),
                        store_type = c.String(nullable: false, maxLength: 16, unicode: false),
                        product_id = c.String(nullable: false, maxLength: 48, unicode: false),
                        order_id = c.String(maxLength: 64, unicode: false),
                        purchase_token = c.String(maxLength: 1024, unicode: false),
                        purchase_state = c.String(nullable: false, maxLength: 16, unicode: false),
                        upt_date = c.DateTime(nullable: false, precision: 0),
                        ipt_date = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.trans_no)                
                .Index(t => t.user_no, name: "IDX_USER_NO")
                .Index(t => t.product_id, name: "IDX_PRODUCT_ID")
                .Index(t => t.order_id, name: "IDX_ORDER_ID");
            
            CreateTable(
                "user_base",
                c => new
                    {
                        user_no = c.Long(nullable: false, identity: true),
                        platform_id = c.String(nullable: false, maxLength: 128, unicode: false),
                        platform_type = c.String(nullable: false, maxLength: 16, unicode: false),
                        platform_email = c.String(maxLength: 64, unicode: false),
                        last_login_date = c.DateTime(nullable: false, precision: 0),
                        ipt_date = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.user_no)                
                .Index(t => t.platform_id, unique: true, name: "IDX_PLATFORM_ID")
                .Index(t => t.platform_email, name: "IDX_PLATFORM_EMAIL");
            
            CreateTable(
                "user_role",
                c => new
                    {
                        user_no = c.Long(nullable: false),
                        role_type = c.String(nullable: false, maxLength: 16, unicode: false),
                        linked_trans_no = c.Long(nullable: false),
                        expire_time = c.DateTime(nullable: false, precision: 0),
                        upt_date = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.user_no)                ;
            
        }
        
        public override void Down()
        {
            DropIndex("user_base", "IDX_PLATFORM_EMAIL");
            DropIndex("user_base", "IDX_PLATFORM_ID");
            DropIndex("in_app_billing", "IDX_ORDER_ID");
            DropIndex("in_app_billing", "IDX_PRODUCT_ID");
            DropIndex("in_app_billing", "IDX_USER_NO");
            DropTable("user_role");
            DropTable("user_base");
            DropTable("in_app_billing");
        }
    }
}
