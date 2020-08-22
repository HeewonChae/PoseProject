namespace Repository.Mysql.FootballDBMigrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddPredictionBackTestingTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "prediction_back_testing",
                c => new
                    {
                        fixture_id = c.Int(nullable: false),
                        pred_seq = c.Int(nullable: false, storeType: "mediumint"),
                        main_label = c.Int(nullable: false, storeType: "mediumint"),
                        sub_label = c.Int(nullable: false, storeType: "mediumint"),
                        value1 = c.Int(nullable: false, defaultValueSql: "0",
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        value2 = c.Int(nullable: false, defaultValueSql: "0",
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        value3 = c.Int(nullable: false, defaultValueSql: "0",
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        value4 = c.Int(nullable: false, defaultValueSql: "0",
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        grade = c.Int(nullable: false, storeType: "mediumint"),
                        is_recommended = c.Boolean(nullable: false),
                        is_hit = c.Boolean(nullable: false),
                        upt_time = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => new { t.fixture_id, t.pred_seq })                
                .Index(t => new { t.fixture_id, t.main_label }, name: "IDX_ID_MAIN_LABEL");
            
        }
        
        public override void Down()
        {
            DropIndex("prediction_back_testing", "IDX_ID_MAIN_LABEL");
            DropTable("prediction_back_testing");
        }
    }
}
